
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
    using Microsoft.CSharp;

    [CanParse(@"^ValidateSpecialControlValue\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateSpecialControlValue : InteractionCommand
    {
        string notMatchedValue ;
        public ValidateSpecialControlValue(ExecutionParameters executionParameters, IUiAdapter uiAdapter
            )
            : base(executionParameters, uiAdapter)
        {

        }

        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                notMatchedValue = string.Empty;
                Validation validation ;
                try
                {
                    var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                    if (this.logicalFieldValue.Contains("~"))
                    {
                     validation=  GetTableValue(controlDefinition, context);
                    }
                    else if (this.logicalFieldValue.Contains(";;"))
                    {
                        validation = GetDropDownValue(controlDefinition, context);

                    }
                    else
                    {
                        validation = GetValue(controlDefinition, context);
                    }
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
                        string.Format("{0} was expected to contain {1}, but value is not matched.",
                            this.logicalFieldName, (notMatchedValue != string.Empty)? notMatchedValue:validation.Expected));
                }
            }

        }

        private Validation GetDropDownValue(ControlDefinition controlDefinition, TestRunContext context)
        {
            var expectedValues =
                this.logicalFieldValue
                    .Split(new[] {";;"}, StringSplitOptions.RemoveEmptyEntries);
            var validation = DropDownContains(expectedValues, context, controlDefinition);

            var testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action = string.Format("Validate Speical control value for" +
                                       " {0} as {1}", this.logicalFieldName, this.logicalFieldValue),
                ActualResult = validation.Actual,
                ExpectedResult = validation.Expected,
                Value =
                    "Was: " + this.uiAdapter.GetTableValue(controlDefinition) + "\n" +
                    GetMatchedExpectationsMessage(validation),
                TestResult = validation.Succeeds
                    ? TestResult.Pass
                    : TestResult.Fail
            };
            context.TestFindings.Add(testFinding);
            return validation;
        }

        private Validation GetValue(ControlDefinition controlDefinition, TestRunContext context)
        {
            var expectedValues =
                this.logicalFieldValue
                    .Split(new[] {";;"}, StringSplitOptions.RemoveEmptyEntries);

            var validation = GetValueContains(expectedValues, context, controlDefinition);
            var testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action = string.Format("Validate Speical control value for" +
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
            return validation;
        }

        private Validation GetTableValue(ControlDefinition controlDefinition, TestRunContext context)
        {
           

            var expectedValues =
                this.logicalFieldValue
                    .Split(new[] {'~'}, StringSplitOptions.RemoveEmptyEntries);
            if (expectedValues.Length <= 0) return null;
            var validation = TableContains(expectedValues, context, controlDefinition);
            var testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action = string.Format("Validate Special control value for" +
                                       " {0} as {1}", this.logicalFieldName, this.logicalFieldValue),
                ActualResult = validation.Actual,
                ExpectedResult = validation.Expected,
                Value =
                    "Was: " + this.uiAdapter.GetTableValue(controlDefinition) + "\n" +
                    GetMatchedExpectationsMessage(validation),
                TestResult = validation.Succeeds
                    ? TestResult.Pass
                    : TestResult.Fail

            };

            {
                context.TestFindings.Add(testFinding);
                

            }
            return validation;



        }

        private static string GetMatchedExpectationsMessage(Validation validation)
        {
            return (validation.Succeeds
                ? "Expectations matched."
                : string.Format("Matched expectations: {0}",
                    ((string) validation.Actual).IsNullOrEmpty()
                        ? "None"
                        : validation.Actual));
        }


        private Validation TableContains(IEnumerable<string> expectedValue, TestRunContext context,
            ControlDefinition controlDefinition)
        {
            var actual = "true";
            var regex = new Regex(@"\{(?<variableName>.+)\}");
            var matchVariable = regex.Match(expectedValue.ToArray().ToString());
            var expectedValues =
                this.logicalFieldValue
                    .Split(new[] {'~'}, StringSplitOptions.RemoveEmptyEntries)
                    .Select(val =>
                        matchVariable.Success
                            ? context.CapturedValues["{" + val.ToUpper() + "}"]
                            : val
                    );
            var actualValue = this.uiAdapter.GetElementValue(controlDefinition);
            foreach (var itemValue in expectedValues.Select(x =>x))
            {
                if (actualValue.IndexOf(itemValue, StringComparison.Ordinal) == -1)
                {
                    notMatchedValue = itemValue;
                    actual = "false";
                    break;
                }
            }
            return new Validation
            {
                Expected = true.ToString(),
                Actual = actual

            };
        }

        private Validation DropDownContains(IEnumerable<string> expectedValue, TestRunContext context,
            ControlDefinition controlDefinition)
        {

            var actualValue = this.uiAdapter.GetDropDownOptions(controlDefinition);
            var enumerable = expectedValue as string[] ?? expectedValue.ToArray();
            var matchedExpectations = enumerable.Where(actualValue.Contains).ToArray();
            return new Validation
            {
                Expected = enumerable.Aggregate((a, i) => a + ", " + i),
                Actual =
                    matchedExpectations.Any() ? matchedExpectations.Aggregate((a, i) => a + ", " + i) : string.Empty
            };


        }

        private Validation GetValueContains(IEnumerable<string> expectedValue, TestRunContext context,
            ControlDefinition controlDefinition)
        {
            var actualValue = this.uiAdapter.GetElementValue(controlDefinition);
            var enumerable = expectedValue as string[] ?? expectedValue.ToArray();
            var matchedExpectations = enumerable.Where(actualValue.Contains).ToArray();
            return new Validation
            {
                Expected = enumerable.Aggregate((a, i) => a + ", " + i),
                Actual =
                    matchedExpectations.Any() ? matchedExpectations.Aggregate((a, i) => a + ", " + i) : string.Empty

            };
        }




    }
}
