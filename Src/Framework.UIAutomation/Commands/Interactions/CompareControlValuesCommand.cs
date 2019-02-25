namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;

    [CanParse(@"^CompareControlValues\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class CompareControlValuesCommand : InteractionCommand
    {
        public CompareControlValuesCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        {
        }

        #region Execution Method
        public override void Execute(TestRunContext context)
        {
            var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
            var otherControlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldValue];

            var validation = this.CompareControlsNumerically(controlDefinition, otherControlDefinition);

            var testFinding = new TestFinding
            {
                FlowIdentifier = context.FlowIdentifier,
                DataIdentifier = context.Iteration,
                Action =
                    string.Format("Compare the numeric values of {0} and {1}.", 
                        this.logicalFieldName,
                        this.logicalFieldValue),
                ActualResult = validation.Actual,
                ExpectedResult = validation.Expected,
                TestResult = validation.Succeeds
                    ? TestResult.Pass
                    : TestResult.Fail,
                Value = string.Format("{0} is {1}greater than {2}.",
                    this.logicalFieldName,
                    validation.Succeeds ? string.Empty : "not ",
                    this.logicalFieldValue)
            };
            context.TestFindings.Add(testFinding);

            if (!validation.Succeeds)
            {
                this.uiAdapter.TakeScreenshot();
                throw new ExpectationFailedException(
                    string.Format("{0} ({2}) was expected to be greater than {1} ({3}), but wasn't",
                        this.logicalFieldName, this.logicalFieldValue, validation.Actual, validation.Expected));
            }
        }

        private Validation CompareControlsNumerically(ControlDefinition controlDefinition, ControlDefinition otherControlDefinition)
        {
            var validation = new Validation
            {
                Actual = this.uiAdapter.GetElementValue(controlDefinition).TryGetDoubleValue(),
                Expected = this.uiAdapter.GetElementValue(otherControlDefinition).TryGetDoubleValue(),
                SuccessMeasure = v => ((double?) v.Actual) > ((double?) v.Expected)
            };
            return validation;
        }

        #endregion
    }
}
