namespace VM.Platform.TestAutomationFramework.Adapters.Local
{

    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class LocalAdapterRegistry : Registry
    {
        public LocalAdapterRegistry()
        {
            For<ILocalReaderAdapter>().Use<LocalWorkItemStoreAdapter>().Singleton();
            For<ITestRunPublisher>().Use<LocalTestRunPublisher>();
            For<IDataFileReader>().Use<LocalExcelMapReader>();
            //For<IDataFileReader>().Use<DataBaseMapReader>();
                        
        }
    }
}
