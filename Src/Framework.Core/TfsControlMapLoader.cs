namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class TfsControlMapLoader : IControlMapLoader
    {
        private readonly ITfsReaderAdapter tfsWorkItemStoreAdapter;
        private readonly IFileWriter fileWriter;
        private readonly IDataFileReader mapReader;

        public TfsControlMapLoader(ITfsReaderAdapter tfsWorkItemStoreAdapter, IFileWriter fileWriter, IDataFileReader mapReader)
        {
            this.tfsWorkItemStoreAdapter = tfsWorkItemStoreAdapter;
            this.fileWriter = fileWriter;
            this.mapReader = mapReader;
        }

        public ControlMap GetControlMapFromTestCase(int testCaseId, string mapName = null)
        {
            SharedTestStep sharedStep;
            try
            {
                sharedStep = this.tfsWorkItemStoreAdapter.GetSharedStepForTestCase(Convert.ToInt32(testCaseId));
            }
            catch (InvalidOperationException ex)
            {
                throw new FrameworkFatalException(string.Format("Test case {0} has no shared step", testCaseId), ex);
            }

            return this.GetControlMapFromSharedStep(sharedStep, mapName);
        }

        public ControlMap GetControlMapFromSharedStep(string sharedStepId, string mapName = null)
        {
            SharedTestStep sharedStep;
            try
            {
                this.tfsWorkItemStoreAdapter.Connect();
                sharedStep = this.tfsWorkItemStoreAdapter.GetSharedStepById(Convert.ToInt32(sharedStepId));
            }
            catch (InvalidOperationException ex)
            {
                throw new FrameworkFatalException(string.Format("Unknown shared step: {0}", sharedStepId), ex);
            }

            return this.GetControlMapFromSharedStep(sharedStep, mapName);
        }

        internal ControlMap GetControlMapFromSharedStep(SharedTestStep sharedStep, string mapName)
        {
            TestAttachment attachment;
            try
            {
                attachment = (from a in sharedStep.Attachments
                              where mapName == null || a.Name == mapName
                              select a).First();
            }
            catch (InvalidOperationException ex)
            {
                throw new FrameworkFatalException("Shared step has no attachments", ex);
            }

            var controlMap = this.GetControlMapFromAttachment(attachment);
            controlMap.Source = sharedStep.Id;
            return controlMap;
        }

        private ControlMap GetControlMapFromAttachment(TestAttachment attachment)
        {
            try
            {
                var buffer = Convert.FromBase64String(attachment.Content);
                var attachmentFileName = this.fileWriter.CreateFile(buffer, Path.GetExtension(attachment.Name));
                var controlMap = this.mapReader.GetMapOfMaps<ControlDefinition>(attachmentFileName);

                return new ControlMap(controlMap);
            }
            catch (Exception ex)
            {
                throw new FrameworkFatalException("Could not read invalid control map file", ex);
            }
        }
    }
}
