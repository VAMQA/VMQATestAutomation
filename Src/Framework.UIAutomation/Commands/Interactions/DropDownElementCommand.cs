
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Extensions;

    public class DropDownElementCommand : BaseElementCommand
    {
        private readonly ControlDefinition controlDefinition;
        private readonly string logicalFieldValue;
        private readonly IUiAdapter uiAdapter;
        private readonly bool setIndex;

        public DropDownElementCommand(IUiAdapter driver, ControlDefinition controlDefinition, bool isSetIndex,  string value)
        {
            this.controlDefinition = controlDefinition;
            this.logicalFieldValue = value;
            this.uiAdapter = driver;
            this.setIndex = isSetIndex;
        }
        # region Element Command Execution Methods

        public override void Execute()
        {
            // We do not have a special implementation for the SPECIAL keyword, so we drop it
            var value = Regex.Replace(this.logicalFieldValue, @"special\s*=\s*", string.Empty, RegexOptions.IgnoreCase);
            try
            {
                this.uiAdapter.WaitForElementToBeClickable(this.controlDefinition);
            }
            catch
            {
                this.uiAdapter.WaitForElement(this.controlDefinition);
            }

            if (this.setIndex)
            {
                uiAdapter.SetDropDownByIndex(this.controlDefinition, Convert.ToInt32(value));
            }
            else
            {
                uiAdapter.SetDropDownValue(this.controlDefinition, value);   
            }
            
           

        }
        # endregion
    }
}
