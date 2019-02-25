
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^ValidateVisibleStatus\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateVisibleStatusCommand: InteractionCommand
    {
        public ValidateVisibleStatusCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        { }

        # region execute method
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                var validation = ValidateVisibilityStatus(controlDefinition);

                var testFinding = new TestFinding
                {
                    FlowIdentifier = context.FlowIdentifier,
                    DataIdentifier = context.Iteration,
                    Action = string.Format("Validate that the {0} is {1}displayed on the page.",
                        this.logicalFieldName,
                        Convert.ToBoolean(this.logicalFieldValue) ? string.Empty : "not "),
                    ActualResult = validation.Actual,
                    ExpectedResult = validation.Expected,
                    Value = validation.Succeeds
                        ? "Expectation matched."
                        : string.Format("Unexpected value: {0}", validation.Actual),
                    TestResult = validation.Succeeds
                        ? TestResult.Pass
                        : TestResult.Fail
                };
                context.TestFindings.Add(testFinding);

                if (!validation.Succeeds)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new ExpectationFailedException(string.Format("{0} was {1}expected to be displayed.",
                        this.logicalFieldName, Convert.ToBoolean(this.logicalFieldValue) ? string.Empty : "not "));
                }
            }
        }

        private Validation ValidateVisibilityStatus(ControlDefinition controlDefinition)
        {
            return new Validation
            {
                Actual = uiAdapter.IsElementDisplay(controlDefinition),
                Expected = Convert.ToBoolean(this.logicalFieldValue)
            };
        }
        #endregion
    }
}
