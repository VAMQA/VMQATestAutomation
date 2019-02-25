namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    [CanParse(@"^\s*BrowserClose\s*$")]
    public class CloseBrowserCommand : BaseCommand
    {
        private readonly IUiAdapter uiAdapter;
        private readonly ExecutionParameters executionParameters;

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        public CloseBrowserCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.uiAdapter = uiAdapter;
        }
        #region Command Execution Methods
        public override void Execute(TestRunContext context)
        {
                this.uiAdapter.CloseCurrentBrowser();
        }
        #endregion

        #region Only signature of the Validation method

        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {

        }

        #endregion


    }
}
