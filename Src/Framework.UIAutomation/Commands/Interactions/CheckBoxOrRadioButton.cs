namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class CheckBoxOrRadioButton : IElementCommand
    {
        private readonly ControlDefinition ctrlDefinition;
        private readonly bool logicalFieldValue;
        private readonly IUiAdapter uiAdapter;

        public CheckBoxOrRadioButton(IUiAdapter uiAdapter, ControlDefinition ctrlDefinition, string logicalFieldValue)
        {
            this.ctrlDefinition = ctrlDefinition;
            this.logicalFieldValue = logicalFieldValue.Equals("true", StringComparison.OrdinalIgnoreCase);
            this.uiAdapter = uiAdapter;
        }

        # region Element Command Execution Methods

        public void Execute()
        {
            try
            {
                if (String.Equals("true", this.logicalFieldValue.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var priorState = this.uiAdapter.IsSelectedButtons(this.ctrlDefinition);
                    if (String.Equals("false", priorState.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        this.uiAdapter.ClickElement(this.ctrlDefinition);
                        var isSelected = this.uiAdapter.IsSelectedButtons(this.ctrlDefinition);

                        if (isSelected.ToString() == "false")
                        {
                            this.uiAdapter.ClickElement(this.ctrlDefinition);
                        }
                    }
                }
                else if (String.Equals("false", this.logicalFieldValue.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    var priorState1 = this.uiAdapter.IsSelectedButtons(this.ctrlDefinition);
                    if (String.Equals("true", priorState1.ToString(), StringComparison.OrdinalIgnoreCase))
                    {
                        this.uiAdapter.ClickElement(this.ctrlDefinition);
                    }
                }


            }
            catch (Exception ex)
            {
                this.uiAdapter.TakeScreenshot();
               
               throw new WorkflowFailedException(ex.Message);
            }
           
           
        }

        # endregion
    }
}
