namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class TfsTestDataLoader : ITestDataLoader
    {
        private readonly ITfsReaderAdapter tfsWorkItemStoreAdapter;
        private readonly IDataFileReader dataFileReader;
        private readonly IFileWriter fileWriter;

        public TfsTestDataLoader(ITfsReaderAdapter tfsWorkItemStoreAdapter, IDataFileReader dataFileReader, IFileWriter fileWriter)
        {
            this.tfsWorkItemStoreAdapter = tfsWorkItemStoreAdapter;
            this.dataFileReader = dataFileReader;
            this.fileWriter = fileWriter;
        }

        public Dictionary<string, IEnumerable<TestDataDirective>> GetTestData(int testCaseId)
        {
            TestStep testStep;

            try
            {
                testStep = this.tfsWorkItemStoreAdapter.GetTestDataStep(testCaseId);
            }
            catch (InvalidOperationException ex)
            {
                throw new FrameworkFatalException(string.Format("Test case {0} has no test data step", testCaseId), ex);
            }

            if (testStep == null)
            {
                throw new FrameworkFatalException(string.Format("Test case {0} has no test data step", testCaseId));
            }

            try
            {
                var attachment = testStep.Attachments.First();
                var directives = this.GetTestDataDirectivesFromAttachment(attachment);
                return directives;
            }
            catch (InvalidOperationException ex)
            {
                throw new FrameworkFatalException("test data step must have exactly one attachment", ex);
            }
        }

        private Dictionary<string, IEnumerable<TestDataDirective>> GetTestDataDirectivesFromAttachment(TestAttachment attachment)
        {
            try
            {
                var buffer = Convert.FromBase64String(attachment.Content);
                var attachmentFileName = this.fileWriter.CreateFile(buffer, Path.GetExtension(attachment.Name));

                return this.dataFileReader.GetTestDataDirectives(attachmentFileName);
            }
            catch (Exception ex)
            {
                throw new FrameworkFatalException("Could not read invalid test data file", ex);
            }
        }
    }
}
