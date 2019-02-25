namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;

    public interface ITestRunPublisher
    {
        void CreateTestRun(string testSuite, int[] testCaseIds);
        void CreateTestRun(int testCaseId);
        int SaveTestCaseRunResult(int testCaseId, TestResult testCaseResult);
        void SaveTestRun(IEnumerable<string> filesToAttach, int id,string projecName);
        void StartTestCase(int testCaseId);
        void SaveTestDetailsTab(TestExecutionDeatils testRunText, TestCaseConfiguration testCaseConfiguration);
    }
}