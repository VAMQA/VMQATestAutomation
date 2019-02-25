namespace VM.Platform.TestAutomationFramework.Core.Logging
{
    using StructureMap.Configuration.DSL;
    using StructureMap.Pipeline;

    public class LoggerRegistry : Registry
    {
        public LoggerRegistry()
        {
            this.For<Logger>().Use<Log4NetLogger>().SetLifecycleTo<SingletonLifecycle>();
        }

    }
}