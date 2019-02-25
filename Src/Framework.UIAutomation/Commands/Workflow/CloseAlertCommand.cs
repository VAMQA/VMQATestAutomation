namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using System;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.UIAutomation.Properties;

    [CanParse(@"^\s*CloseAlert\s*\{\s*(?<Action>.*)\s*\}\s*$")]
    public class CloseAlertCommand : BaseCommand
    {
        private readonly IUiAdapter uiAdapter;
        private readonly ExecutionParameters executionParameters;
        private readonly string action;

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        public CloseAlertCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.uiAdapter = uiAdapter;
            this.action = this.executionParameters["Action"];
        }
        #region Command Execution Methods
        public override void Execute(TestRunContext context)
        {
            var acceptOrDismiss = this.action.Equals("Accept", StringComparison.OrdinalIgnoreCase);
            
            this.uiAdapter.CloseAlert(acceptOrDismiss);
        }

        #endregion

        #region Only signature of the Validation method

        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {
            var isValid = HasValidAction(parameters);

            if (!isValid)
            {
                throw new ArgumentException(Resources.InvalidAlertClosingAction, "parameters");
            }
        }

        private bool HasValidAction(ExecutionParameters parameters)
        {
            var validActions = new[] {"Accept", "Dismiss"};
            return parameters.ContainsKey("Action") &&
                   validActions.Any(va => parameters["Action"].Equals(va, StringComparison.OrdinalIgnoreCase));
        }

        #endregion


    }
}