namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.IO;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;

    public class LocalControlMapLoader : IControlMapLoader
    {
        private readonly ILocalReaderAdapter LocalWorkItemStoreAdapter;
        private readonly IDataFileReader mapReader;

        public LocalControlMapLoader(ILocalReaderAdapter LocalWorkItemStoreAdapter, IDataFileReader mapReader)
        {
            this.LocalWorkItemStoreAdapter = LocalWorkItemStoreAdapter;
            this.mapReader = mapReader;
        }

        public ControlMap GetControlMapFromTestCase(int testCaseId, string mapName = null)
        {   // Local Exe from Excel
            //var controlMap = this.mapReader.GetMapOfMaps<ControlDefinition>(this.LocalWorkItemStoreAdapter.GetTestORPath());
            //DB
            var controlMap = this.mapReader.GetMapOfMaps<ControlDefinition>(String.Format("{0}:{1}", testCaseId.ToString(), "MasterOR"));
            return new ControlMap(controlMap);

        }

        public ControlMap GetControlMapFromSharedStep(string sharedStepId, string mapName = null)
        {
            return null;
        }

        internal ControlMap GetControlMapFromSharedStep(SharedTestStep sharedStep, string mapName)
        {
            return null;
        }

        private ControlMap GetControlMapFromAttachment(TestAttachment attachment)
        {
            return null;
        }
    }
}
