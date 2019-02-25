
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    [CanParse(@"^\s*CloseAllBrowsers\s*$")]
    public class CloseAllBrowserCommand : BaseCommand
    {

        private readonly ExecutionParameters executionParameters;
        private readonly IUiAdapter uiAdapter;

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }
        public CloseAllBrowserCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.uiAdapter = uiAdapter;
        }

        #region Only signature of Command Execution Methods
        public override void Execute(TestRunContext context)
        {
            this.uiAdapter.CloseCurrentBrowser();            
            this.uiAdapter.Quit();
        }
        #endregion

        #region Only signature of the Validation method
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {

        }
        #endregion

    }
}
