namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Dynamic;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Core.Logging;

    public class TestCommandRunner : ITestCommandRunner
    {
        private readonly IEnumerable<ITestCommand> commandsToRun;
        private readonly Logger logger;
        private int? getIternation;
        List<int?> Iteration = new List<int?>();

        public IList<ITestCommand> ExecutedCommands { get; private set; }

        public TestCommandRunner(IEnumerable<ITestCommand> commandsToRun, Logger logger)
        {
            this.commandsToRun = commandsToRun;
            this.logger = logger;
            this.ExecutedCommands = new List<ITestCommand>();
        }

        public void Execute(TestRunContext testRunContext)
        {
            testRunContext.TestResult = TestResult.Pending;

            var commandList = new LinkedList<ITestCommand>(this.commandsToRun);
            testRunContext.CurrentCommand = commandList.First;

            while (testRunContext.CurrentCommand != null)
            {
                testRunContext.NextCommand = testRunContext.CurrentCommand.Next;
                getIternation = testRunContext.Iteration;

                var currentCommand = testRunContext.CurrentCommand.Value;

                try
                {
                    currentCommand.Execute(testRunContext);
                    GetPassedConditions(testRunContext);

                }
                catch (ExpectationFailedException)
                {

                    testRunContext.TestResult = TestResult.Fail;
                    this.logger.Error("TC # : {0}, Expectation failed, the test will proceed despite execution failure", testRunContext.TestCaseId);
                    GetFailedCondition(testRunContext);

                }
                catch (WorkflowFailedException)
                {
                    GetFailedCondition(testRunContext);
                    
                    if (!(Iteration.Contains(testRunContext.Iteration)) && testRunContext.Retry == 1)
                    {
                        Iteration.Add(testRunContext.Iteration);
                        testRunContext.NextCommand = testRunContext.StartIterationCommand;
                    }
                    else if (testRunContext.HasAnotherIterationToRun)
                    {
                        testRunContext.TestResult = TestResult.Fail;
                        this.logger.Error("TC # : {0}, Aborting iteration. Will resume execution with next iteration.", testRunContext.TestCaseId);
                        if (!testRunContext.IsRandomExecution) testRunContext.MoveToNextIteration();
                        testRunContext.NextCommand = testRunContext.StartIterationCommand;
                    }
                    else
                    {
                        testRunContext.TestResult = TestResult.Fail;
                        this.logger.Error("TC # : {0}, No more iterations to run. Test execution will abort.", testRunContext.TestCaseId);
                        throw;
                    }
                }
                catch (Exception)
                {
                    GetFailedCondition(testRunContext);
                    if (!(Iteration.Contains(testRunContext.Iteration)) && testRunContext.Retry == 1)
                    {
                        Iteration.Add(testRunContext.Iteration);
                        testRunContext.NextCommand = testRunContext.StartIterationCommand;
                     }
                    else if (testRunContext.HasAnotherIterationToRun)
                    {
                        this.logger.Error("Aborting iteration. Will resume execution with next iteration.");
                        testRunContext.TestResult = TestResult.Fail;
                        if (!(testRunContext.sb.ToString().Contains(testRunContext.Iteration.ToString())))
                        {
                            testRunContext.sb.Append(" " + testRunContext.Iteration.ToString());
                        }
                        testRunContext.MoveToNextIteration();
                        testRunContext.NextCommand = testRunContext.StartIterationCommand;
                    }
                    else
                    {
                        this.logger.Error("No more iterations to run. Test execution will abort.");
                        testRunContext.TestResult = TestResult.Fail;
                        if (!(testRunContext.sb.ToString().Contains(testRunContext.Iteration.ToString())))
                        {
                            testRunContext.sb.Append(" " + testRunContext.Iteration.ToString());
                        }
                        throw;
                    }
                }
                finally
                {
                    this.ExecutedCommands.Add(currentCommand);
                }
                testRunContext.CurrentCommand = testRunContext.NextCommand;
            }
            if (testRunContext.TestResult == TestResult.Pending)
            {
                testRunContext.TestResult = TestResult.Pass;
                testRunContext.DetailsTab.TestCaseResult = testRunContext.TestResult.ToString();
                GetPassedConditions(testRunContext);
            }
        }

        private void GetPassedConditions(TestRunContext testRunContext)
        {
            if (testRunContext.Iteration.HasValue
                && !testRunContext.DetailsTab.TotalconditionsPassed.Contains(testRunContext.Iteration.Value)
                && !testRunContext.DetailsTab.TotalconditionsFailed.Contains(testRunContext.Iteration.Value) 
                && !string.IsNullOrEmpty(testRunContext.CurrentPage))
            {
                testRunContext.DetailsTab.TotalconditionsPassed.Add(testRunContext.Iteration.Value);
            }
        }

        private void GetFailedCondition(TestRunContext testRunContext)
        {
            if (testRunContext.Iteration.HasValue &&
                !testRunContext.DetailsTab.TotalconditionsFailed.Contains(testRunContext.Iteration.Value))
            {

                testRunContext.DetailsTab.TotalconditionsPassed.RemoveAll(x => x == testRunContext.Iteration.Value);
                testRunContext.DetailsTab.TotalconditionsFailed.Add(testRunContext.Iteration.Value);
            }
        }
    }

}