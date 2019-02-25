namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^ValidateRowCount\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateRowCountCommand : InteractionCommand
    {
        public ValidateRowCountCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        {
        }

        #region Execution Method
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];

                var validation = new Validation
                {
                    Actual = this.uiAdapter.GetTableRowCount(controlDefinition),
                    Expected = this.logicalFieldValue
                };

                var testFinding = new TestFinding
                {
                    FlowIdentifier = context.FlowIdentifier,
                    DataIdentifier = context.Iteration,
                    Action = string.Format("Validate item count in {0}", this.logicalFieldName),
                    ActualResult = validation.Actual,
                    ExpectedResult = validation.Expected,
                    Value = validation.Succeeds
                        ? "Expectations matched."
                        : string.Format("Unexpected value: {0}", validation.Actual),
                    TestResult = validation.Succeeds
                        ? TestResult.Pass
                        : TestResult.Fail
                };
                context.TestFindings.Add(testFinding);

                if (!validation.Succeeds)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new ExpectationFailedException(string.Format("{0} was expected to be {1}, but was {2}",
                        this.logicalFieldName, validation.Expected, validation.Actual));
                }
            }
        }
        #endregion
    }
}
