namespace VM.Platform.TestAutomationFramework.UIAutomation.Scripts.PostScripts
{
    using System;
    using CredentialManagement;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow;

    [RunAfter(CommandType = typeof(InvokeApplicationCommand))]
    public class AutomateThreatManagementGateway : IPostCommandScript
    {
        private readonly TestCaseConfiguration testCaseConfiguration;
        private readonly IUiAdapter uiAdapter;

        public AutomateThreatManagementGateway(TestCaseConfiguration testCaseConfiguration, IUiAdapter uiAdapter)
        {
            this.testCaseConfiguration = testCaseConfiguration;
            this.uiAdapter = uiAdapter;
        }

        public void Execute()
        {
            try
            {
                if (!this.uiAdapter.CurrentPageTitle.Equals("Microsoft Forefront TMG", StringComparison.OrdinalIgnoreCase)) return;
                if (this.testCaseConfiguration.TfsCredentialAddress == null) return;

                var credential = new Credential
                {
                    Target = this.testCaseConfiguration.TfsCredentialAddress
                };

                if (!credential.Exists()) return;

                credential.Load();

                this.uiAdapter.SetControlValue(new ControlDefinition
                {
                    Label = "TMG_UserName",
                    ControlId = "username"
                }, credential.Username);

                this.uiAdapter.SetControlValue(new ControlDefinition
                {
                    Label = "TMG_Password",
                    ControlId = "password"
                }, credential.Password);

                this.uiAdapter.ClickElement(new ControlDefinition
                {
                    Label = "TMG_SubmitCreds",
                    ControlId = "SubmitCreds",
                });
            }
            catch (Exception)
            {
                // If the TMG cannot be automated, no harm, no foul. Do nothing. 
            }
        }
    }
}
