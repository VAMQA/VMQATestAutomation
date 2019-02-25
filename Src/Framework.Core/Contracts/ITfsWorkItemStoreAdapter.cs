namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;
  

    public interface ITfsReaderAdapter
    {
        void Connect();
        TestCase GetTestCaseById(int workItemId);
        IEnumerable<TestStep> GetTestSteps(int testCaseId);
        SharedTestStep GetSharedStepForTestCase(int testCaseId);
        SharedTestStep GetSharedStepById(int sharedStepId);
        TestStep GetTestDataStep(int testCaseId);
        IEnumerable<TestCase> GetTestCasesByTestSuite(string testSuitePath);
        TestRun CreateNewTestRun(string testSuite, int[] testCaseIds);
        TestRun CreateNewTestRun(int testCaseId);
        TestRun StartTestCaseRun(int testCaseId);
        TestRun SaveTestCaseRunResult(int testCaseId, TestResult testCaseResult);
        TestRun SaveTestRun(IEnumerable<string> filesToAttach);
        void SaveTestDetailsTab(TestExecutionDeatils testRunText, TestCaseConfiguration testCaseConfiguration);
       
    }
}