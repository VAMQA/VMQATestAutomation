
using VM.Platform.TestAutomationFramework.Core;

namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public class DefaultInteractionCommand : BaseElementCommand
    {
        private readonly ControlDefinition ctrlDefinition;
        private readonly string logicalFieldValue;
        private readonly IUiAdapter uiAdapter;

        public DefaultInteractionCommand(ControlDefinition controlDefination, string value, IUiAdapter uiAdapter)
        {
            this.ctrlDefinition = controlDefination;
            this.logicalFieldValue = value;
            this.uiAdapter = uiAdapter;
        }

        # region ElementCommand Execution Methods

        public override void Execute()
        {
            uiAdapter.SetControlValue(this.ctrlDefinition, this.logicalFieldValue);

        }

        # endregion

    }
}
