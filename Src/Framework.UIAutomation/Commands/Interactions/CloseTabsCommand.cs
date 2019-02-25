namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using System;
    using System.Text.RegularExpressions;


    [CanParse(@"^CloseAdditonalTabs\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class CloseTabsCommand : InteractionCommand
    {
        private readonly IUiAdapter uiAdapter;
        private readonly ExecutionParameters executionParameters;
        //private readonly TestCaseConfiguration testCaseConfiguration;
        //private ControlDefinition controlDefinition;

        public CloseTabsCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        {
            this.executionParameters = executionParameters;
            this.uiAdapter = uiAdapter;
        }
        #region Command Execution Methods
        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                try
                {
                    if (this.logicalFieldValue.Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        this.uiAdapter.CloseAndReturn();
                    }
                }
                catch (Exception ex)
                {
                    ReportFailureToHtml ReportError = new ReportFailureToHtml();
                    this.uiAdapter.TakeScreenshot();
                    throw new WorkflowFailedException(ex.Message, ex);
                }
            }
        }


        #endregion

        #region Only signature of the Validation method

        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {

        }

        #endregion


    }
}