namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class TfsTestCaseLoader : ITestCaseLoader
    {
        private readonly ITfsReaderAdapter tfsWorkItemStoreAdapter;

        public TfsTestCaseLoader(ITfsReaderAdapter tfsWorkItemStoreAdapterAdapter)
        {
            this.tfsWorkItemStoreAdapter = tfsWorkItemStoreAdapterAdapter;
        }

        public TestCase GetTestCase(int testCaseId)
        {
            if (testCaseId <= 0)
            {
                throw new ArgumentOutOfRangeException("testCaseId", testCaseId, "positive integer expected");
            }
            
            this.ConnectToTfs();
            
            var testCase = this.tfsWorkItemStoreAdapter.GetTestCaseById(testCaseId);

            if (testCase == null)
            {
                throw new ArgumentOutOfRangeException("testCaseId", testCaseId, "not a test case");
            }
            testCase.TestSteps = this.tfsWorkItemStoreAdapter.GetTestSteps(testCaseId);

            return testCase;
        }

        public IEnumerable<int> GetTestCaseIds(string testSuitePath)
        {
            if (string.IsNullOrWhiteSpace(testSuitePath))
            {
                throw new ArgumentOutOfRangeException("testSuitePath", testSuitePath, "valid suite name expected");
            }

            this.ConnectToTfs();

            var testCases = this.tfsWorkItemStoreAdapter.GetTestCasesByTestSuite(testSuitePath);

            return testCases.Select(tc => tc.Id);
        }

        private void ConnectToTfs()
        {
            try
            {
                this.tfsWorkItemStoreAdapter.Connect();
            }
            catch (AuthenticationException ex)
            {
                throw new FrameworkFatalException("user not authorized to connect to work item store", ex);
            }
            catch (Exception ex)
            {
                throw new FrameworkFatalException("Cannot connect to server", ex);
            }
        }
    }
}