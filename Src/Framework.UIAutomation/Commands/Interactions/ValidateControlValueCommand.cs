namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;

    [CanParse(@"^ValidateControlValue\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateControlValueCommand : InteractionCommand
    {
        private readonly ScriptRunner scriptRunner;
        public ValidateControlValueCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter, ScriptRunner scriptRunner)
            : base(executionParameters, uiAdapter)
        {
            this.scriptRunner = scriptRunner;
        }

        # region execute method
        public override void Execute(TestRunContext context)
        {
           
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                Validation validation;

                try
                {
                    var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];

                    SetLogicalFieldValue(context, this.logicalFieldValue.ToString());
                    TryTransformAsCSharpScript();


                    validation = this.ValidateExpectedValue(controlDefinition, context);

                    var testFinding = new TestFinding
                    {
                        FlowIdentifier = context.FlowIdentifier,
                        DataIdentifier = context.Iteration,
                        Action = string.Format("Validate control value for" +
                                               " {0} as {1}", this.logicalFieldName, this.logicalFieldValue),
                        ActualResult = validation.Actual,
                        ExpectedResult = validation.Expected,
                        Value =
                            "Was: " + this.uiAdapter.GetElementValue(controlDefinition) + "\n" +
                            GetMatchedExpectationsMessage(validation),
                        TestResult = validation.Succeeds
                            ? TestResult.Pass
                            : TestResult.Fail
                    };
                    context.TestFindings.Add(testFinding);


                }
                catch (Exception ex)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new WorkflowFailedException("failed to run the validate control value command", ex);
                }

                if (!validation.Succeeds)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new ExpectationFailedException(
                        string.Format("{0} was expected to contain {1}, but some values did not match.",
                            this.logicalFieldName, validation.Expected));
                }
            }
        }

        private static string GetMatchedExpectationsMessage(Validation validation)
        {
            return (validation.Succeeds
                ? "Expectations matched."
                : string.Format("Matched expectations: {0}", 
                    ((string)validation.Actual).IsNullOrEmpty() 
                        ? "None" 
                        : validation.Actual));
        }
        private void TryTransformAsCSharpScript()
        {
            var regex = new Regex(@"^=C#\((?<code>.+?)\)$");
            var match = regex.Match(this.logicalFieldValue);
            if (match.Success)
            {
                var code = match.Groups["code"].Value;
                this.logicalFieldValue = this.scriptRunner.EvaluateAsCSharp(code).ToString();
            }
        }

        private Validation ValidateExpectedValue(ControlDefinition controlDefinition, TestRunContext context)
        {
            return TryMatchToolTip(controlDefinition)
                   ?? TryMatchMasked(controlDefinition)
                   ?? TryHandleDropDown(controlDefinition, context)
                   ?? TryHandleTable(controlDefinition, context)
                   ?? ValidateInequalityForAnyControl(controlDefinition) 
                   ?? ValidateValuesInControl(controlDefinition, context)
                   //?? ValidateMultipleValues(controlDefinition)
                   ?? HandleAnyOtherControlType(controlDefinition, context);
        }

        private void SetLogicalFieldValue(TestRunContext context, string value)
        {
            //if (value.StartsWith("{") && value.EndsWith("}"))
            //{
            //    this.logicalFieldValue = context.CapturedValues[value];
            //}
            var cFormat = Regex.Matches(value, @"{[A-Za-z]|^[A-Za-z][0-9]\-}");

            if (cFormat.Count > 0)
            {
                string[] values = value.Split(' ');

                for (int i = 0; i < values.Length; i++)
                {
                    var mFormat = Regex.Matches(values[i], @"\{.*?\}");
                    foreach (var item in mFormat)
                    {
                        if (context.CapturedValues.ContainsKey(item.ToString().ToUpper()))
                        {
                            values[i] = values[i].ToUpper().Replace(item.ToString().ToUpper(), context.CapturedValues[item.ToString().ToUpper()]);
                        }

                        else
                        {
                            string[] val = this.logicalFieldValue.Split(' ');
                            values[i] = val[i];
                        }
                    }
                }
                this.logicalFieldValue = string.Join(" ", values);
            }
        }

        private Validation TryMatchToolTip(ControlDefinition controlDefinition)
        {
            var regex = new Regex(@"^tooltip\s*=\s*(?<expected>.+?)$", RegexOptions.IgnoreCase);
            var match = regex.Match(this.logicalFieldValue);

            return match.Success
                ? new Validation
                {
                    Expected = match.Groups["expected"].Value,
                    Actual = this.uiAdapter.GetAttributeValue(controlDefinition, "HelpText")
                }
                : null;
        }

        private Validation TryMatchMasked(ControlDefinition controlDefinition)
        {
            var regex = new Regex(@"^masked\s*=\s*(?<expected>.+?)$", RegexOptions.IgnoreCase);
            var match = regex.Match(this.logicalFieldValue);

            return match.Success
                ? new Validation
                {
                    Expected = match.Groups["expected"].Value,
                    Actual = this.uiAdapter.GetAttributeValue(controlDefinition, "LabeledBy")
                }
                : null;
        }

        private Validation TryHandleDropDown(ControlDefinition controlDefinition, TestRunContext context)
        {
            return controlDefinition.TagName.Equals("select", StringComparison.OrdinalIgnoreCase)
                ? ValidateNoDuplicationsInList(controlDefinition) ?? ValidateItemValuesInList(controlDefinition, context)
                : null;
        }

        private Validation ValidateItemValuesInList(ControlDefinition controlDefinition, TestRunContext context)
        {
            var regex = new Regex(@"^(?<inequality>NE=)?\s*(?<item>.+?)(?:;;(?<item>.+?))*$", RegexOptions.IgnoreCase);
            var match = regex.Match(this.logicalFieldValue);

            if (!match.Success) return null;

            var validateThatValuesExists = !match.Groups["inequality"].Success;
            var expectedValues = match.Groups["item"].Captures.Cast<Capture>().Select(c => c.Value).ToArray();
            var expectedList=expectedValues.Select(val =>
                        val.StartsWith("{") && val.EndsWith("}")
                            ? context.CapturedValues[val.ToUpper()]
                            : val
                    ).ToArray();
            var actualValues = this.uiAdapter.GetDropDownOptions(controlDefinition);
            var matchedExpectations = validateThatValuesExists
                ? actualValues.Intersect(expectedList)
                : expectedList.Except(actualValues);

            return new Validation
            {
                Expected = expectedList.Aggregate((a, i) => a + ", " + i),
                Actual = matchedExpectations.Aggregate((a, i) => a + ", " + i)
            };
        }
       

        private Validation ValidateNoDuplicationsInList(ControlDefinition controlDefinition)
        {
            if (!this.logicalFieldValue.Equals("ValidateDuplicates", StringComparison.OrdinalIgnoreCase)) return null;

            var dropDownOptions = this.uiAdapter.GetDropDownOptions(controlDefinition);
            var duplicates = (dropDownOptions
                .GroupBy(x => x)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)).ToArray();
            return new Validation
            {
                Expected = string.Empty,
                Actual = duplicates.Any() ? duplicates.Aggregate((a, i) => a + ", " + i) : string.Empty
            };
        }

        private Validation ValidateValuesInControl(ControlDefinition controlDefinition, TestRunContext context)
        {
            if (!this.logicalFieldValue.Contains("~"))  return null;
            var expectedValues =
                this.logicalFieldValue
                    .Split(new[] { '~' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(val =>
                        val.StartsWith("{") && val.EndsWith("}")
                            ? context.CapturedValues[val.ToUpper()]
                            : val
                    );



            var actualValue = this.uiAdapter.GetElementValue(controlDefinition);
            var matchedExpectations = expectedValues.Where(actualValue.Contains).ToArray();

            return new Validation
            {
                Expected = expectedValues.Aggregate((a, i) => a + ", " + i),
                Actual = matchedExpectations.Any() ? matchedExpectations.Aggregate((a, i) => a + ", " + i) : string.Empty
            };
        }

        //FIx 12/6
        private Validation ValidateMultipleValues(ControlDefinition controlDefinition)
        {            
            if (!this.logicalFieldValue.Contains("/")) return null;
            var expectedValues =
                this.logicalFieldValue.ToLower()
                    .Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            var actualValue = this.uiAdapter.GetElementValue(controlDefinition);
            var matchedExpectations = expectedValues.Where(actualValue.ToLower().Contains).ToArray();
            if (matchedExpectations.Length > 0)
            {
                return new Validation
                {
                    Expected = matchedExpectations.Any() ? matchedExpectations.Aggregate((a, i) => a + ", " + i) : string.Empty,
                    Actual = matchedExpectations.Any() ? matchedExpectations.Aggregate((a, i) => a + ", " + i) : string.Empty
                };
            }
            else
            {
                return new Validation
                {
                    Expected = expectedValues.Aggregate((a, i) => a + ", " + i),
                    Actual = matchedExpectations.Any() ? matchedExpectations.Aggregate((a, i) => a + ", " + i) : string.Empty
                };
            }

        }

        private Validation HandleAnyOtherControlType(ControlDefinition controlDefinition, TestRunContext context)
        {
            var regex = new Regex(@"^{(?<variableName>).+?}$");
            var matchVariable = regex.Match(this.logicalFieldValue);

            return new Validation
            {

                Actual = this.uiAdapter.GetElementValue(controlDefinition),
                Expected = matchVariable.Success
                    ? context.CapturedValues[this.logicalFieldValue.ToUpper()]
                    : this.logicalFieldValue
            };
        }

        private Validation TryHandleTable(ControlDefinition controlDefinition, TestRunContext context)
        {
           
            if (!controlDefinition.TagName.Equals("table", StringComparison.OrdinalIgnoreCase)) return null;
            var regex = new Regex(@"^(?<row>\d+)-(?<column>\d+)#(?<expected>.+?)$");
            var match = regex.Match(this.logicalFieldValue);
            var row = int.Parse(match.Groups["row"].Value);
            var column = int.Parse(match.Groups["column"].Value);
            return new Validation
            {
                Expected =ExpectedValue( match.Groups["expected"].Value, context),
                Actual = this.uiAdapter.GetTableCellValue(controlDefinition, row, column)
            };
        }
        private string  ExpectedValue(string value, TestRunContext context)
        {
            value = TryTransformAsCSharpScript(value);

            var regex = new Regex(@"\{(?<variableName>.+)\}");
            var matchVariable = regex.Match(value);
            if ((matchVariable.Success))
            {
                if (value.StartsWith("~") || value.StartsWith("$"))
                {
                    return context.CapturedValues[value.Split('~')[1].ToUpper()]; 
                }
                return context.CapturedValues[value.ToUpper()];
            }
            return value;

        }

        private string TryTransformAsCSharpScript(string value)
        {
            var csharpregex = new Regex(@"^=C#\((?<code>.+?)\)$");
            var match = csharpregex.Match(value);
            if (match.Success)
            {
                var code = match.Groups["code"].Value;
                value = this.scriptRunner.EvaluateAsCSharp(code).ToString();
            }
            return value;
        }
        private Validation ValidateInequalityForAnyControl(ControlDefinition controlDefinition)
        {
            var regex = new Regex(@"^(?<inequality>NE=)+\s*(?<item>.+?)(?:~(?<item>.+?))*$", RegexOptions.IgnoreCase);
            var match = regex.Match(this.logicalFieldValue);
            if (!match.Success) return null;
            var expectedValues = match.Groups["item"].Captures.Cast<Capture>().Select(c => c.Value).ToArray();
            var actualValues = this.uiAdapter.GetElementValue(controlDefinition);
            var matchedExpectations = expectedValues.Where(actualValues.Contains).ToArray();
            return new Validation
            {
                Expected = "true",
                // ReSharper disable once RedundantTernaryExpression
                Actual = (matchedExpectations.Length == 0) ? "true" : "false"
            };
        }

        # endregion


    }
}
