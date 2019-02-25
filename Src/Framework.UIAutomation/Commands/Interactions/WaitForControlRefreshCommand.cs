
//namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
//{
//    using System;
//    using VM.Platform.TestAutomationFramework.Core;
//    using VM.Platform.TestAutomationFramework.Core.Commands;
//    using VM.Platform.TestAutomationFramework.Core.Contracts;
//    using VM.Platform.TestAutomationFramework.Core.Exceptions;
//    [CanParse(@"^WaitForControlRefresh\s*\{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>.*?)\s*\}$")]
//    public class WaitForControlRefreshCommand : InteractionCommand
//    {

//       public WaitForControlRefreshCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
//            : base(executionParameters, uiAdapter)
//        {
//        }

//        public override void Execute(TestRunContext context)
//        {
//            using (new FrameHandler(this.uiAdapter, context, this.logicalFieldName))
//            {
//                try
//                {
//                    this.uiAdapter.WaitForElementToDisplay();
//                }
//                catch (Exception ex)
//                {
//                    throw new WorkflowFailedException(
//                        string.Format("Waited, but the {0} control never appeared.", this.logicalFieldName), ex);
//                } 
//            }
//        }
//    }
//}
