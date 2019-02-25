namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    [CanParse(@"^WriteLog\s*{\s*(?<LogicalFieldName>.*?)\s*\|\|\s*(?<LogicalFieldValue>(.|\n|\r)*?)\s*}$")]
    public class CaptureValueToHtmlCommand : InteractionCommand
    {
        public CaptureValueToHtmlCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter) :
            base(executionParameters, uiAdapter)
        {

        }

        public override void Execute(TestRunContext context)
        {
            var validation = new Validation()
            {
                Expected = true,
                Actual = true,
            }; 
           
            try
            {
                var testFinding = new TestFinding{
                    FlowIdentifier = context.FlowIdentifier,
                    DataIdentifier = context.Iteration,
                    Action = string.Format("Log Comments"),
                    Value =
                        logicalFieldValue,
                    TestResult = TestResult.Pass
                };
                context.TestFindings.Add(testFinding);


            }
            catch (Exception ex)
            {
                this.uiAdapter.TakeScreenshot();
                //Report the Error to HTML REPORT
                ReportFailureToHtml ReportError = new ReportFailureToHtml();
                context = ReportError.Run(context, string.Format("Error for {0}", this.logicalFieldName), string.Format("Waited, but the {0} control never appeared.", this.logicalFieldName), TestResult.Fail);


                throw new WorkflowFailedException("failed to run the CaptureValue To Html command", ex);
            }

            if (!validation.Succeeds)
            {
                this.uiAdapter.TakeScreenshot();
                throw new ExpectationFailedException("was expected to capture test data value into html report.");
            }



        }
    }
}
