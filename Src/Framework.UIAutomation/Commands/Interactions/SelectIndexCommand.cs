
namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    [CanParse(@"^SelectIndex\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class SelectIndexCommand : InteractionCommand
    {
        private ControlDefinition ctrlDefinition;
        public SelectIndexCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters, uiAdapter)
        {
        }

        public override void Execute(TestRunContext context)
        {
            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
            {
                try
                {
                    this.ctrlDefinition = context.ControlMap[context.CurrentPage][this.logicalFieldName];
                    SetDropDownByIndex();
                }
                catch (Exception ex)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new WorkflowFailedException(
                        string.Format("Could not set test data for field '{0}'.", this.logicalFieldName),
                        ex);
                }
            }
           
        }

        private void SetDropDownByIndex()
        {
            var dropDownElementCommand = new DropDownElementCommand(this.uiAdapter, this.ctrlDefinition, true,
                this.logicalFieldValue);
            dropDownElementCommand.Execute();
        }

    }
}
