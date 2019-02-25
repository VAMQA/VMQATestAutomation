namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;

    [CanParse(@"^ValidateExistStatus\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ValidateExistStatusCommand: InteractionCommand
    {
        public ValidateExistStatusCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        { }

        # region execute method
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                var controlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                var validation = ValidateExistStatus(controlDefinition);

                var testFinding = new TestFinding
                {
                    FlowIdentifier = context.FlowIdentifier,
                    DataIdentifier = context.Iteration,
                    Action = string.Format("Validate that the {0} does {1}exist on the page.",
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
                    throw new ExpectationFailedException(string.Format("{0} was {1}expected to exist.",
                        this.logicalFieldName, Convert.ToBoolean(this.logicalFieldValue) ? string.Empty : "not "));
                }
            }
        }

        private Validation ValidateExistStatus(ControlDefinition controlDefinition)
        {
            return new Validation
            {
                Actual = uiAdapter.IsElementPresent(controlDefinition),
                Expected = Convert.ToBoolean(this.logicalFieldValue)
            };
        }

        # endregion
    }
}
