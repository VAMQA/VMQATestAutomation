namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;

    public interface ILocalReaderAdapter
    {
        void SaveTestResults(int TestCaseID, IEnumerable<string> filesToAttach);
        void SaveTestResults(int TestCaseID, IEnumerable<string> filesToAttach, int runId,string project);
        int SaveTestCaseRunResult(int testCaseId, TestResult testCaseResult);
        void SaveTestDetailsTab(TestExecutionDeatils testRunText, TestCaseConfiguration testCaseConfiguration);
    }
}