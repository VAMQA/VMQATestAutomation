
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{

    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public class HyperLinkElementCommand : IElementCommand
    {

        private readonly ControlDefinition ctrlDefinition;
        private readonly IUiAdapter uiAdapter;

        public HyperLinkElementCommand(IUiAdapter uiAdapter, ControlDefinition ctrlDefinition)
        {
            this.ctrlDefinition = ctrlDefinition;

            this.uiAdapter = uiAdapter;
        }
        # region Element Command Execution Methods

        public void Execute()
        {
           this.uiAdapter.ClickElement(this.ctrlDefinition);
        }
        #endregion
    }
}


