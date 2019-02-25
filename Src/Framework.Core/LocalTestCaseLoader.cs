namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Authentication;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class LocalTestCaseLoader : ITestCaseLoader
    {
        private readonly ILocalReaderAdapter LocalWorkItemStoreAdapter;
        private readonly IDataFileReader mapReader;

        public LocalTestCaseLoader(ILocalReaderAdapter LocalWorkItemStoreAdapter, IDataFileReader mapReader)
        {
            this.LocalWorkItemStoreAdapter = LocalWorkItemStoreAdapter;
            this.mapReader = mapReader;

        }

        public TestCase GetTestCase(int testCaseId)
        {

            var testCase = new TestCase(testCaseId) { Title = testCaseId.ToString() };

            testCase.TestSteps = GetTestSteps(testCaseId);

            return testCase;
        }

        public IEnumerable<TestStep> GetTestSteps(int testCaseId)
        {
            try
            {   //Local Exe from Excel
                //var TestCaseMaps = this.mapReader.GetMapOfMaps<TestStep>(this.LocalWorkItemStoreAdapter.GetTestCasePath());
                //DB Execution
                var TestCaseMaps = this.mapReader.GetMapOfMaps<TestStep>(String.Format("{0}:{1}", testCaseId.ToString(), "DataFlow"));
                var TestCaseMap = TestCaseMaps[testCaseId.ToString()];
                return TestCaseMap.Values.AsEnumerable<TestStep>();
            }
            catch (Exception ex)
            {
                throw new FrameworkFatalException("Error Loading TestCase File", ex);
            }

        }

        public IEnumerable<int> GetTestCaseIds(string testSuitePath)
        {
            return null;
        }


    }
}