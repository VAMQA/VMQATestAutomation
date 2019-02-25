namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using System;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using VM.Platform.TestAutomationFramework.Extensions;
    using System.Collections.Generic;

    public class CommandErrorHandler : CommandDecorator
    {
        private readonly Logger logger;
        private readonly TestCaseConfiguration testCaseConfiguration;
        private readonly IUiAdapter uiAdapter;
        List<int?> Iteration = new List<int?>();

        public CommandErrorHandler(ITestCommand command, IUiAdapter uiAdapter, Logger logger, TestCaseConfiguration testCaseConfiguration)
            : base(command)
        {
            this.logger = logger;
            this.testCaseConfiguration = testCaseConfiguration;
            this.uiAdapter = uiAdapter;
        }

        public override void Execute(TestRunContext context)
        {
            try
            {
                base.Execute(context);
            }
            catch (ExpectationFailedException ex)
            {
                if (this.testCaseConfiguration.ShouldIgnorePage(context.CurrentPage))
                {
                    this.logger.Warn("TC # : {0}, Ignoring error on the {0} page, as per configuration. Attempting to proceed with test.", context.TestCaseId,
                        context.CurrentPage);
                }
                else
                {
                    this.logger.Error("TC # : {0}, " + ex.Message, context.TestCaseId);
                    this.logger.Debug("TC # : {0}, " + ex.ToString(), context.TestCaseId);

                    throw;
                }
            }
            catch (WorkflowFailedException ex)
            {


                if (this.testCaseConfiguration.ShouldIgnorePage(context.CurrentPage))
                {
                    this.logger.Warn("Ignoring error on the {0} page, as per configuration. Attempting to proceed with test.",
                        context.CurrentPage);
                }
                else
                {
                    if (context.IsCleanUpWebDriverEndOfIteration)
                    {
                        this.uiAdapter.CloseCurrentBrowser();
                    }
                    this.logger.Error("TC # : {0}, " + ex.Message, context.TestCaseId);
                    this.logger.Debug("TC # : {0}, " + ex.ToString(), context.TestCaseId);

                    throw;
                }
            }
            catch (FrameworkFatalException ex)
            {
                context.TestResult = TestResult.Fail;

                this.logger.Error("TC # : {0}, " + ex.Message, context.TestCaseId);
                this.logger.Debug("TC # : {0}, " + ex.ToString(), context.TestCaseId);
                this.logger.Error("TC # : {0}, The test run will be aborted.", context.TestCaseId);
                this.uiAdapter.Quit();
                throw;
            }
            catch (Exception ex)
            {
                context.TestResult = TestResult.Fail;
                if (!(Iteration.Contains(context.Iteration)) && context.Retry == 1)
                {
                    this.logger.Error("Due to an unexpected error, the test run will be aborted. Please contact your administrator".Indented());
                    this.logger.Debug(ex.ToString().Indented());
                    ReportFailureToHtml ReportError = new ReportFailureToHtml();
                    context = ReportError.Run(context, string.Format("Iteration failed", this.Iteration.ToString()), string.Format("Failed"), TestResult.Fail);
                    this.uiAdapter.CloseCurrentBrowser();
                    this.uiAdapter.Quit();
                    Iteration.Add(context.Iteration);
                    context.NextCommand = context.StartIterationCommand;
                    //this.ExecutedCommands.Add(currentCommand);
                }
                else
                {
                    this.logger.Error("TC # : {0}, Due to an unexpected error, the test run will be aborted. Please contact your administrator", context.TestCaseId);
                    this.logger.Error("TC # : {0}, " + ex.ToString(), context.TestCaseId);
                    this.logger.Debug("TC # : {0}, " + ex.ToString(), context.TestCaseId);
                    this.uiAdapter.Quit();
                    throw;
                }
            }
        }
    }
}
   
