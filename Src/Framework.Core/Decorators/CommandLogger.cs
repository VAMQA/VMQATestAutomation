namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class CommandLogger : CommandDecorator
    {
        private readonly Logger logger;

        public CommandLogger(ITestCommand command, Logger logger) : base(command)
        {
            this.logger = logger;
        }

        //public override void Execute(TestRunContext context)
        //{
        //    var executedCommand = this.GetCommandExecutionString();
        //    this.logger.Info(string.Format("TC # : {0}, Beginning ", context.TestCaseId) + executedCommand);

        //    base.Execute(context);

        //    this.logger.Info(string.Format("TC # : {0}, Command completed.", context.TestCaseId));
        //}

        public override void Execute(TestRunContext context)
        {
            // var executedCommand = this.GetCommandExecutionString();
            string cascadeMsg = "";

            var executedCommand = this.GetCommandExecutionString();

            //find which command is Test Data and get sequence number from sql query

            if (executedCommand.Split('(')[1].ToString().Trim().StartsWith("Logical"))
            {

                cascadeMsg = "[Sequence No - " + context.getSequenceNumber[context.iSeqNumberIndicator] + "]";
                if (context.previousPage.IsNotNullOrEmpty() && !context.previousPage.Equals(context.CurrentPage) && context.debugReturnValue.IsNotNullOrEmpty() && context.debugReturnValue.Equals("next"))
                {
                    //context.debugFromSeqNumber = context.getSequenceNumber[0];
                    context.debugFromSeqNumber += "," + context.getSequenceNumber[0];
                }
                //if (context.debugMode == true && context.debugFromSeqNumber.Equals(context.getSequenceNumber[context.iSeqNumberIndicator]))
                if (context.debugMode == true && context.debugFromSeqNumber.Split(',').Any(c => c == context.getSequenceNumber[context.iSeqNumberIndicator]))
                {
                    context.previousPage = context.CurrentPage;
                    DebugMessageBox messagebox = new DebugMessageBox();
                    context.debugReturnValue = messagebox.returnVal;
                    if (messagebox.returnVal.Equals("next") && context.iSeqNumberIndicator < context.getSequenceNumber.Count - 1)
                    {
                        if (!context.debugFromSeqNumber.Split(',').Any(c => c == context.getSequenceNumber[context.iSeqNumberIndicator + 1]))
                            //context.debugFromSeqNumber = context.getSequenceNumber[context.iSeqNumberIndicator + 1];
                            context.debugFromSeqNumber += "," + context.getSequenceNumber[context.iSeqNumberIndicator + 1];
                    }
                    else if (messagebox.returnVal.Equals("abort"))
                    {
                        throw new WorkflowFailedException("Debug Mode Aborted");
                    }

                }

                context.iSeqNumberIndicator += 1;

            }

        RetryLabel:
            try
            {
                context.debugRetryCount = 0;
                this.logger.Info(cascadeMsg + " Beginning " + executedCommand);
                base.Execute(context);
            }
            catch (WorkflowFailedException e)
            {

                if (context.debugMode == true && context.debugRetryCount < 1)
                {
                    DebugExecutionFailure debugFail = new DebugExecutionFailure(context.getSequenceNumber[context.iSeqNumberIndicator - 1], e.Message.ToString());
                    if (debugFail.resumeMode.Equals("resume"))
                    {
                        goto RetryLabel;
                    }
                    else
                    {
                        context.debugRetryCount += 1;
                        throw;
                    }
                }
                else
                    throw;


            }
            catch (ExpectationFailedException e)
            {

                if (context.debugMode == true && context.debugRetryCount < 1)
                {
                    DebugExecutionFailure debugFail = new DebugExecutionFailure(context.getSequenceNumber[context.iSeqNumberIndicator - 1], e.Message.ToString());
                    if (debugFail.resumeMode.Equals("resume"))
                    {
                        goto RetryLabel;
                    }
                    else
                    {
                        context.debugRetryCount += 1;
                        throw;
                    }
                }
                else
                    throw;
            }

            this.logger.Info(cascadeMsg + " Command completed.".Indented());
        }

        private string GetCommandExecutionString()
        {
            var commandName = this.Command.GetType().Name;
            var flattenedExecutionParameters = this.HasExecutionParameters()
                ? this.ExecutionParameters.Select(x => x.Key + " : " + x.Value)
                      .Aggregate((all, next) => all + ", " + next)
                : string.Empty;

            return string.Format("{0} ( {1} )", commandName, flattenedExecutionParameters);
        }

        private bool HasExecutionParameters()
        {
            return this.Command.ExecutionParameters != null &&
                   this.Command.ExecutionParameters.Count > 0;
        }
    }
}