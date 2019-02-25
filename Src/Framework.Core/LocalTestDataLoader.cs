namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class LocalTestDataLoader : ITestDataLoader
    {
        private readonly ILocalReaderAdapter LocalWorkItemStoreAdapter;
        private readonly IDataFileReader dataFileReader;


        public LocalTestDataLoader(ILocalReaderAdapter LocalWorkItemStoreAdapter, IDataFileReader dataFileReader)
        {
            this.LocalWorkItemStoreAdapter = LocalWorkItemStoreAdapter;
            this.dataFileReader = dataFileReader;

        }

        public Dictionary<string, IEnumerable<TestDataDirective>> GetTestData(int testCaseId)
        {
            //Local Exe from Excel
            //DirectoryInfo dirTestCase = new DirectoryInfo(this.LocalWorkItemStoreAdapter.GetTestDataPath());
            //var testCaseFile = dirTestCase.GetFiles(testCaseId + "*" + ".xlsx");
            //return GetTestDataDirectivesFromAttachment(dirTestCase + testCaseFile[0].ToString());
            
            //DB
            return GetTestDataDirectivesFromAttachment(testCaseId.ToString());
        }

        private Dictionary<string, IEnumerable<TestDataDirective>> GetTestDataDirectivesFromAttachment(string testCasePath)
        {
            try
            {
                return this.dataFileReader.GetTestDataDirectives(testCasePath);
            }
            catch (Exception ex)
            {
                throw new FrameworkFatalException("Could not read invalid test data file", ex);
            }
        }
    }
}
