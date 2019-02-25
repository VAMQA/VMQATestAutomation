using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VM.Platform.TestAutomationFramework.UIAutomation.Properties;

namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using System.Text.RegularExpressions;
    
    [CanParse(@"^ActionAlert\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*}$")]
    public class ActionAlertCommand : InteractionCommand
    {
        public ActionAlertCommand(ExecutionParameters parameters, IUiAdapter uiAdapter, TestCaseConfiguration testCaseConfiguration)
            : base(parameters, uiAdapter)
        {
            
        }
        public override void Execute(TestRunContext context)
        {
            try
            {
                var acceptOrDismiss = this.logicalFieldValue.Equals("Accept", StringComparison.OrdinalIgnoreCase);
                
                this.uiAdapter.CloseAlert(acceptOrDismiss);
                this.uiAdapter.SwitchToDefaultContent();
            }
            catch (Exception ex)
            {
                this.uiAdapter.TakeScreenshot();
                //Report the Error to HTML REPORT
                ReportFailureToHtml ReportError = new ReportFailureToHtml();
                context = ReportError.Run(context, string.Format("Error for {0}", this.logicalFieldName), string.Format("Waited, but the {0} control never appeared.", this.logicalFieldName), TestResult.Fail);


                throw new WorkflowFailedException(ex.Message, ex);
            }            
        }
    }
}
