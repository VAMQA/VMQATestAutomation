using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using System.Text.RegularExpressions;

    [CanParse(@"^SwitchToBrowser\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class SwitchToBrowserCommand : InteractionCommand
    {
        public SwitchToBrowserCommand(ExecutionParameters parameters, IUiAdapter uiAdapter, TestCaseConfiguration testCaseConfiguration)
            : base(parameters, uiAdapter)
        {

        }
        public override void Execute(TestRunContext context)
        {
            try
            {
                this.SwitchToBrowserWindow(this.logicalFieldValue);
            }
            catch (Exception ex)
            {
                this.uiAdapter.TakeScreenshot();
                throw new WorkflowFailedException(ex.Message, ex);
            }
        }
        private void SwitchToBrowserWindow(string logicFieldValue)
        {
            this.uiAdapter.SwitchBrowser(logicFieldValue);
        }

    }
}
