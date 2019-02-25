namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System.Threading;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using System;

    public class TextBoxElementCommand : IElementCommand
    {
        private readonly ControlDefinition ctrlDefinition;
        private string logicalFieldValue;
        private readonly IUiAdapter uiAdapter;

        public TextBoxElementCommand(IUiAdapter uiAdapter, ControlDefinition ctrlDefinition, string value)
        {

            this.ctrlDefinition = ctrlDefinition;
            this.logicalFieldValue = value;
            this.uiAdapter = uiAdapter;

        }

        # region Element Command Execution Methods

        public void Execute()
        {
            try
            {
                EvaluateFunction();
                
                uiAdapter.SetControlValue(this.ctrlDefinition, logicalFieldValue);
            }
            catch (Exception ex)
            {
                this.uiAdapter.TakeScreenshot();
                //Report the Error to HTML REPORT
               
                throw new WorkflowFailedException(ex.Message);
            }

        }

        # endregion

        # region textbox keyword function

        private void EvaluateFunction()
        {
            if (logicalFieldValue.IndexOf(',') > 0)
            {
                switch (this.logicalFieldValue.Split(',')[0])
                {
                    case "DOB":
                        var currentDateAndTime = DateTime.Now;
                        this.logicalFieldValue =
                            currentDateAndTime.AddYears(-Convert.ToInt32(logicalFieldValue.Split(',')[1]))
                                .ToString("MMddyyyy");
                        break;
                }
            }
        }

        # endregion



    }
}
