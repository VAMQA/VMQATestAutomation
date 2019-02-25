namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.IO;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Security;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.DependencyManagement;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using VM.Platform.TestAutomationFramework.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using StructureMap.Building;

    public class TestAutomation
    {
        private readonly Logger logger;
        private readonly ITestCaseLoader testCaseLoader;
        private readonly ITestCaseRunner testCaseRunner;
        private readonly ITestRunPublisher testRunPublisher;
        private readonly IUiAdapter uiAdapter;

        public TestAutomation(
            ITestCaseLoader testCaseLoader,
            Logger logger,
            ITestRunPublisher testRunPublisher,
            ITestCaseRunner testCaseRunner,
            IUiAdapter uiAdapter)
        {
            this.testCaseLoader = testCaseLoader;
            this.logger = logger;
            this.testRunPublisher = testRunPublisher;
            this.testCaseRunner = testCaseRunner;
            this.uiAdapter = uiAdapter;
        }

        public TestAutomationInitialization InitializationParameters { get; set; }

        public static TestAutomation Init(string repositoryLocation,
                                          string projectName,
                                          string testPlan,
                                          string targetBrowser = "IE",
                                          string controlMapSource = null,
                                          string mapName = null,
                                          string userName = null,
                                          SecureString password = null)
        {
            var container = StructureMapBootstrapper.GetContainer(
                repositoryLocation,
                projectName,
                userName,
                password,
                targetBrowser,
                controlMapSource,
                mapName,
                testPlan);
            try
            {
                var testAutomation = container.GetInstance<TestAutomation>();
                testAutomation.InitializationParameters = new TestAutomationInitialization
                {
                    CoreVersion = Assembly.GetExecutingAssembly().GetName().Version,
                    UICommandsVersion = container.Model.InstancesOf<ITestCommand>().First().ReturnedType.Assembly.GetName().Version,
                    RepoUri = repositoryLocation,
                    Project = projectName,
                    TestPlan = testPlan,
                    TargetBrowser = targetBrowser,
                    ControlMapSource = controlMapSource,
                };
                var logger = testAutomation.logger;                

                if (controlMapSource.IsNotNullOrEmptyOrWhiteSpace())
                {
                    string mapSource = string.Format("Initializing control map with map # {0}", controlMapSource);
                    if (mapName.IsNotNullOrEmptyOrWhiteSpace())
                    {
                        mapSource += string.Format(" [{0}]", mapName);
                    }
                    logger.Info(mapSource.Indented());
                }
                if (userName.IsNotNullOrEmptyOrWhiteSpace())
                {
                    logger.Info("Running as user: {0}", userName);
                }

                return testAutomation;
            }
            catch (Exception ex)
            {
                if (ex is StructureMapBuildException)
                {
                    throw ex.InnerException;
                }

                throw;
            }
        }

        public void Execute(string testSuite,string targetEnvironment = null)
        {

            //LogInitializationParameters();
            //this.logger.Info("Executing the {0} test suite", testSuite);

            //IEnumerable<int> testCaseIds;
            //var filesToAttach = new List<string>
            //{
            //    GetExecutionLogPath()
            //};

            //try
            //{
            //    testCaseIds = this.testCaseLoader.GetTestCaseIds(testSuite).ToArray();
            //}
            //catch (ArgumentOutOfRangeException ex)
            //{
            //    throw new FrameworkFatalException("Invalid or unknown test suite.", ex);
            //}

            //this.logger.Info(" The suite includes the following test cases: {0}",
            //    testCaseIds
            //        .Select(tc => tc.ToString(CultureInfo.InvariantCulture))
            //        .Aggregate((a, i) => a + ", " + i));

            //this.testRunPublisher.CreateTestRun(testSuite, testCaseIds.ToArray());
            //var suiteRunResult = TestResult.Pending;
            //try
            //{
            //    foreach (int testCaseId in testCaseIds)
            //    {
            //        var testCaseResult = TestResult.Pending;
            //        try
            //        {
            //            this.testRunPublisher.StartTestCase(testCaseId);
            //            this.testCaseRunner.ExecuteSingleTestCase(testCaseId,targetEnvironment);
            //            testCaseResult = TestResult.Pass;
            //            suiteRunResult = suiteRunResult == TestResult.Pending
            //                ? testCaseResult
            //                : suiteRunResult;
            //        }
            //        catch (FrameworkFatalException ex)
            //        {
            //            this.logger.Error(
            //                "An error has caused test case #{0} to abort. Proceeding to the next test case.\n\r{1}",
            //                testCaseId, ex);
            //            testCaseResult = TestResult.Fail;
            //            suiteRunResult = TestResult.Fail;
            //        }
            //        finally
            //        {
            //            this.testRunPublisher.SaveTestCaseRunResult(testCaseId, testCaseResult);
            //            filesToAttach.Add(GetTestRunReportPath(testCaseId));
            //        }
            //    }
            //}
            //catch (Exception)
            //{
            //    Assert.Inconclusive("Unexpected error while running test suite. Cannot proceed with test run.");
            //}
            //finally
            //{
            //    this.testRunPublisher.SaveTestRun(filesToAttach);
            //    if (suiteRunResult == TestResult.Fail)
            //    {
            //        Assert.Fail("Test suite execution has failed due to the failure of one or more test cases in it." +
            //                    "\n See log and report for more details.");
            //    }
            //}

        }


        private void LogInitializationParameters()
        {
            logger.Info("Initializing Test Automation");
            //logger.Info("VM Test Automation Framework UI version {0}".Indented(),
            //    this.InitializationParameters.UICommandsVersion);
            //logger.Info("test plan: {0}/{1}/{2}".Indented(), 
            //    this.InitializationParameters.RepoUri,
            //    this.InitializationParameters.Project, 
            //    this.InitializationParameters.TestPlan);
            //logger.Info("Control Map Source: {0}", this.InitializationParameters.ControlMapSource);
            logger.Info("target browser: {0}".Indented(), 
                this.InitializationParameters.TargetBrowser);
        }

        public TestRunContext Execute(int testCaseId, int Retry, string targetEnvironment = null)
        {        
            //this.logger.ExecutionLogFileName = string.Format("Execution_TC{0}.log", testCaseId);
            TestRunContext testruncontext = new TestRunContext();
            this.LogInitializationParameters();
            this.testRunPublisher.CreateTestRun(testCaseId);

            var testCaseRunResult = TestResult.Pending;
            try
            {
                try
                {
                    this.testRunPublisher.StartTestCase(testCaseId);
                    testruncontext = this.testCaseRunner.ExecuteSingleTestCase(testCaseId, targetEnvironment, InitializationParameters.ControlMapSource, Retry);
                    testCaseRunResult = testruncontext.TestResult == TestResult.Fail ? TestResult.Fail : TestResult.Pass;
                    //testCaseRunResult = TestResult.Pass;
                }
                catch (FrameworkFatalException ex)
                {                    
                    this.logger.Error(
                        "An error has caused test case #{0} to abort. Proceeding to the next test case.\n\r{1}",
                        testCaseId, ex);
                    testCaseRunResult = TestResult.Fail;
                }
                catch(Exception ex)
                {
                    this.logger.Error(
                        "An error has caused test case #{0} to abort. Proceeding to the next test case.\n\r{1}",
                        testCaseId, ex);
                    testCaseRunResult = TestResult.Fail;
                }
                finally
                {
                    int testrunId=this.testRunPublisher.SaveTestCaseRunResult(testCaseId, testCaseRunResult);
                    testruncontext.TestRunID = testrunId;
                }
            }
            finally
            {
                //var filesToAttach = new[]
                //{
                //    GetExecutionLogPath(testCaseId),
                //    GetTestRunReportPath(testCaseId)
                //};
                File.Copy(this.logger.ExecutionLogFileName, string.Format("Execution_TC{0}.log", testCaseId));
                File.Delete(this.logger.ExecutionLogFileName);
                string[] filesToAttach;
                if (testCaseRunResult == TestResult.Fail)
                    filesToAttach = new[]
                    {
                        GetExecutionLogPath(testCaseId),
                        GetTestRunReportPath(testCaseId),
                        GetFailurePageScreenshotPath()
                    };
                else
                    filesToAttach = new[]
                    {
                        GetExecutionLogPath(testCaseId),
                        GetTestRunReportPath(testCaseId)
                    };

                try
                {
                    this.testRunPublisher.SaveTestRun(filesToAttach, testruncontext.TestRunID, this.InitializationParameters.TestPlan.Split('_')[1]);
                }
                catch (Exception ex)
                {
                    this.logger.Error("An error has caused while saving test case results for TestCaseID={0}.Error={1}", testCaseId, ex);
                }
                
                //if (testCaseRunResult == TestResult.Fail)
                //{
                //    Assert.Fail("Test execution has failed due to the failure of the executed test case." +
                //                "\n See log and report for more details.");
                //}
            }
            return testruncontext;
        }

        public void TestRunCleanup()
        {
            this.uiAdapter.Quit();
        }

        private string GetTestRunReportPath(int testCaseId)
        {
            return string.Format("TestRunReport_TC{0}.html", testCaseId);
        }

        private string GetExecutionLogPath(int testCaseId)
        {
            return string.Format("Execution_TC{0}.log", testCaseId);
        }

        private string GetExecutionLogPath()
        {
            return "Execution.log";
        }

        private string GetFailurePageScreenshotPath()
        {
            return "ErrorPageScreenshot.jpg";
        }
    }
}