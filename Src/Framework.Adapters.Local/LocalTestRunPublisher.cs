namespace VM.Platform.TestAutomationFramework.Adapters.Local
{
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using System;

    public class LocalTestRunPublisher : ITestRunPublisher
    {
        private readonly ILocalReaderAdapter LocalReaderAdapter;
        private int TestCaseID { get; set; }        
        public TestRun TestRun { get; private set; }

        public LocalTestRunPublisher(ILocalReaderAdapter localReaderAdapter)
        {
            this.LocalReaderAdapter = localReaderAdapter;
            
        }

        public void CreateTestRun(string testSuite, int[] testCaseIds)
        {
            throw new NotImplementedException();
        }

        public void CreateTestRun(int testCaseId)
        {
            TestCaseID = testCaseId;
        }

        public void StartTestCase(int testCaseId)
        {
            TestCaseID = testCaseId;
        }

        public int SaveTestCaseRunResult(int testCaseId, TestResult testCaseResult)
        {
            TestCaseID = testCaseId;
            int runId=this.LocalReaderAdapter.SaveTestCaseRunResult(testCaseId, testCaseResult);
            return runId;
        }

        public void SaveTestRun(IEnumerable<string> filesToAttach, int testRunId,string project)
        {
            this.LocalReaderAdapter.SaveTestResults(TestCaseID, filesToAttach, testRunId,project);
        }
        public void SaveTestDetailsTab(TestExecutionDeatils testRunContext, TestCaseConfiguration testCaseConfiguration)
        {
            this.LocalReaderAdapter.SaveTestDetailsTab(testRunContext, testCaseConfiguration);
        }
    }
}
