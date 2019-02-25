
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
    
    [CanParse(@"^ValidateDBValue\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateDBValueCommand : InteractionCommand
    {
        private readonly ScriptRunner scriptRunner;
        public ValidateDBValueCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter, ScriptRunner scriptRunner)
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

                    validation = this.ValidateExpectedValue(controlDefinition, context);

                    var testFinding = new TestFinding
                    {
                        FlowIdentifier = context.FlowIdentifier,
                        DataIdentifier = context.Iteration,
                        Action = string.Format("Validate DB value for" +
                                               " {0} as {1}", this.logicalFieldName, this.logicalFieldValue),
                        ActualResult = validation.Actual,
                        ExpectedResult = validation.Expected,
                        Value =
                            //"Was: " + this.logicalFieldValue.Split('|')[1] + "\n" +
                            GetMatchedExpectationsMessage(validation),
                        TestResult = validation.Succeeds
                            ? TestResult.Pass
                            : TestResult.Fail
                    };
                    context.TestFindings.Add(testFinding);

                }
                catch (Exception ex)
                {
                    throw new WorkflowFailedException("failed to run the validate control value command", ex);
                }

                if (!validation.Succeeds)
                {
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
                : string.Format("Expectated Results NOT matched with Actual Results"));
        }

        private Validation ValidateExpectedValue(ControlDefinition controlDefinition, TestRunContext context)
        {
            var expectedValues = (this.logicalFieldValue.Split('|')[0]).Split(',');
            var expectedList = expectedValues.Select(val =>
                        val.StartsWith("{") && val.EndsWith("}")
                            ? context.CapturedValues[val.ToUpper()]
                            : val
                    ).ToArray();
            
            var actualValueKey = (this.logicalFieldValue.Split('|')[1]);
            var actualValue=(actualValueKey.StartsWith("{") && actualValueKey.EndsWith("}"))?context.CapturedValues[actualValueKey.ToUpper()]:actualValueKey;
            var actualList = actualValue.Split(',');

            bool result = expectedList.SequenceEqual(actualList);            

            return new Validation
            {
                Expected = expectedList.Aggregate((a, i) => a + ", " + i),
                Actual = actualList.Aggregate((a, i) => a + ", " + i)
            };

            //var matchedExpectations = result
            //    ? actualList.Intersect(expectedList)
            //    : expectedList.Except(actualList);

            //return new Validation
            //{
            //    Expected = expectedList.Aggregate((a, i) => a + ", " + i),
            //    Actual = matchedExpectations.Aggregate((a, i) => a + ", " + i)
            //};
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

        private string SetLogicalFieldValue(TestRunContext context, string value)
        {   
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
            return this.logicalFieldValue;
        }

        #endregion
    }
}
