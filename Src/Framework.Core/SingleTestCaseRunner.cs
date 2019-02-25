namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using VM.Platform.TestAutomationFramework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public class SingleTestCaseRunner : ITestCaseRunner
    {
        private readonly ITestCaseLoader testCaseLoader;
        private readonly IControlMapLoader controlMapLoader;
        private readonly ITestDataLoader testDataLoader;
        private readonly TestStepParser testStepParser;
        private readonly ITestCommandRunnerFactory commandRunnerFactory;
        private readonly Logger logger;
        private readonly HtmlReportGenerator htmlReportGenerator;
        private readonly TestCaseConfiguration testCaseConfiguration;
        private readonly IPersistedValuesHandler persistedValuesHandler;
        private readonly ITestRunPublisher testRunPublisher;

        public SingleTestCaseRunner(ITestCaseLoader testCaseLoader, IControlMapLoader controlMapLoader, ITestDataLoader testDataLoader, TestStepParser testStepParser, ITestCommandRunnerFactory commandRunnerFactory, Logger logger, HtmlReportGenerator htmlReportGenerator, TestCaseConfiguration testCaseConfiguration, IPersistedValuesHandler persistedValuesHandler, ITestRunPublisher testRunPublisher)
        {
            this.testCaseLoader = testCaseLoader;
            this.controlMapLoader = controlMapLoader;
            this.testDataLoader = testDataLoader;
            this.testStepParser = testStepParser;
            this.commandRunnerFactory = commandRunnerFactory;
            this.logger = logger;
            this.htmlReportGenerator = htmlReportGenerator;
            this.testCaseConfiguration = testCaseConfiguration;
            this.persistedValuesHandler = persistedValuesHandler;
            this.testRunPublisher = testRunPublisher;
        }
        //public virtual void ExecuteSingleTestCase(int testCaseId,int projID, string targetEnvironment)
        //{
        //    var testRunContext = new TestRunContext();
        //    try
        //    {
        //        this.logger.Info("Downloading test case from DB");
        //        testRunContext.RequestedEnvironment = targetEnvironment;
        //        testRunContext.ProjectID = projID;
        //        testCaseConfiguration.RequestedEnvironment = targetEnvironment;
        //        testRunContext.TestCase = this.testCaseLoader.GetTestCase(testCaseId);
        //        Parallel.Invoke(
        //            () =>
        //            {
        //                this.logger.Info("Getting control map from DB");
        //                testRunContext.ControlMap =
        //                    this.controlMapLoader.GetControlMapFromTestCase(testRunContext.TestCase.Id);
        //                this.logger.Info("Control map retrieval completed");
        //            },
        //            () =>
        //            {
        //                this.logger.Info("Getting test data from DB");
        //                testRunContext.TestData = this.testDataLoader.GetTestData(testRunContext.TestCase.Id);
        //                this.logger.Info("Test data retrieval completed");
        //            }
        //            );

        //        this.logger.Info("Parsing commands");
        //        var commandsToRun =
        //            this.testStepParser.GetCommands(testRunContext.TestCase.TestSteps.Select(ts => ts.Action));

        //        testRunContext.CapturedValues = this.persistedValuesHandler.LoadValues();

        //        var commandRunner = this.commandRunnerFactory.Create(commandsToRun, this.logger);

        //        this.logger.Info("Starting to execute test case #{0} in the {1} environment ({2})",
        //            testCaseId,
        //            targetEnvironment,
        //            this.testCaseConfiguration.GetUrl(targetEnvironment));
        //        testRunContext.DetailsTab.ExecutionTimeStamp = System.DateTime.Now.ToString();
        //        commandRunner.Execute(testRunContext);

        //        this.testRunPublisher.SaveTestDetailsTab(testRunContext.DetailsTab, testCaseConfiguration);
        //    }
        //    catch (FrameworkFatalException ex)
        //    {
        //        // Something was wrong with the test case
        //        testRunContext.TestResult = TestResult.Fail;
        //        testRunContext.DetailsTab.TestCaseResult = testRunContext.TestResult.ToString();
        //        this.testRunPublisher.SaveTestDetailsTab(testRunContext.DetailsTab, testCaseConfiguration);
        //        this.logger.Error("Framework failed to run the test due to a test configuration error.\n{0}",
        //            ex.Message.Indented());
        //    }
        //    catch (Exception ex)
        //    {

        //        // Something unexpected went wrong with the framework during the execution
        //        testRunContext.TestResult = TestResult.Fail;
        //        testRunContext.DetailsTab.TestCaseResult = testRunContext.TestResult.ToString();
        //        this.testRunPublisher.SaveTestDetailsTab(testRunContext.DetailsTab, testCaseConfiguration);
        //        this.logger.Error("Framework failed to run the test due to a test configuration error.\n{0}",
        //            ex.ToString().Indented());
        //    }

        //    var reportWriter = new StreamWriter(string.Format("TestRunReport_TC{0}.html", testCaseId));
        //    this.htmlReportGenerator.Run(testRunContext, reportWriter);

        //    this.persistedValuesHandler.SaveValues(testRunContext.CapturedValues);

        //    switch (testRunContext.TestResult)
        //    {

        //        case TestResult.Pass:
        //            this.logger.Info("Test case #{0} has completed successfully.", testCaseId);
        //            break;
        //        case TestResult.Fail:
        //            var failureMessage = string.Format("Test case #{0} has failed", testCaseId);
        //            this.logger.Error(failureMessage);
        //            Assert.Fail(failureMessage);
        //            break;
        //        default:
        //            var unexpectedFailure = string.Format("Could not execute test case #{0}.", testCaseId);
        //            this.logger.Error(unexpectedFailure);
        //            Assert.Inconclusive(unexpectedFailure);
        //            break;
        //    }
        //}

        public virtual TestRunContext ExecuteSingleTestCase(int testCaseId, string targetEnvironment, string repositiory, int Retry)
        {
            var testRunContext = new TestRunContext();
            try
            {
                this.logger.Info("TC # : {0}, Downloading test case from DB", testCaseId);
                DateTime calc = DateTime.Now;
                testRunContext.RequestedEnvironment = targetEnvironment;
                testRunContext.Retry = Retry;
                testCaseConfiguration.RequestedEnvironment = targetEnvironment;
                testCaseConfiguration.ControlMapSource = repositiory;
                testRunContext.TestCase = this.testCaseLoader.GetTestCase(testCaseId);
                Parallel.Invoke(
                    () =>
                    {
                        this.logger.Info("TC # : {0}, Getting control map :{1} from DB", testCaseId,testCaseConfiguration.ControlMapSource);
                        testRunContext.ControlMap =
                            this.controlMapLoader.GetControlMapFromTestCase(testRunContext.TestCase.Id);
                        this.logger.Info("TC # : {0}, Control map retrieval completed", testCaseId);
                    },
                    () =>
                    {
                        this.logger.Info("TC # : {0}, Getting test data from DB", testCaseId);
                        testRunContext.TestData = this.testDataLoader.GetTestData(testRunContext.TestCase.Id);
                        this.logger.Info("TC # : {0}, Test data retrieval completed", testCaseId);
                    }
                    );

                this.logger.Info("TC # : {0}, Parsing commands", testCaseId);
                var commandsToRun =
                    this.testStepParser.GetCommands(testRunContext.TestCase.TestSteps.Select(ts => ts.Action));

                testRunContext.CapturedValues = this.persistedValuesHandler.LoadValues();

                var commandRunner = this.commandRunnerFactory.Create(commandsToRun, this.logger);

                this.logger.Info("TC # : {0}, Started executing test case in the {1} environment ({2})", 
                    testCaseId, 
                    targetEnvironment,
                    this.testCaseConfiguration.GetUrl(targetEnvironment));
                testRunContext.DetailsTab.ExecutionTimeStamp = System.DateTime.Now.ToString();
                commandRunner.Execute(testRunContext);

                this.testRunPublisher.SaveTestDetailsTab(testRunContext.DetailsTab, testCaseConfiguration);
            }
            catch (FrameworkFatalException ex)
            {               
                // Something was wrong with the test case
                testRunContext.TestResult = TestResult.Fail;
                testRunContext.DetailsTab.TestCaseResult = testRunContext.TestResult.ToString();
                this.testRunPublisher.SaveTestDetailsTab(testRunContext.DetailsTab, testCaseConfiguration);
                this.logger.Error("TC # : {0}, Framework failed to run the test due to a test configuration error.\n{1}", testCaseId,
                    ex.Message.Indented());
            }
            catch (Exception ex)
            {
               
                // Something unexpected went wrong with the framework during the execution
                testRunContext.TestResult = TestResult.Fail;
                testRunContext.DetailsTab.TestCaseResult = testRunContext.TestResult.ToString();
                this.testRunPublisher.SaveTestDetailsTab(testRunContext.DetailsTab, testCaseConfiguration);
                this.logger.Error("TC # : {0}, Framework failed to run the test due to a test configuration error.\n{1}", testCaseId,
                    ex.ToString().Indented());
            }

            var run = new TestRun();
            var reportWriter = new StreamWriter(string.Format("TestRunReport_TC{0}.html", testCaseId));
            this.htmlReportGenerator.Run(testRunContext, reportWriter,run);
            
            this.persistedValuesHandler.SaveValues(testRunContext.CapturedValues);

            switch (testRunContext.TestResult)
            {
                     
                case TestResult.Pass:
                    this.logger.Info("TC # : {0}, has completed successfully.", testCaseId);
                    break;
                case TestResult.Fail:
                    var failureMessage = string.Format("TC # : {0},  has failed", testCaseId);
                    this.logger.Error(failureMessage);
                    //Assert.Fail(failureMessage);
                    break;
                default:
                    var unexpectedFailure = string.Format("TC # : {0}, Could not execute test case.", testCaseId);
                    this.logger.Error(unexpectedFailure);
                    Assert.Inconclusive(unexpectedFailure);
                    break;
            }
            return testRunContext;
        }
    }
}