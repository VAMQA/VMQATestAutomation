namespace VM.Platform.TestAutomationFramework.Adapters.Selenium
{
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    public class SeleniumAdapterRegistry : Registry
    {
        public SeleniumAdapterRegistry()
        {
            For<IUiAdapter>().Use<SeleniumAdapter>().SetLifecycleTo<SingletonLifecycle>();
        }
    }
}