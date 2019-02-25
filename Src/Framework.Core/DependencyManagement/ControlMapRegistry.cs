namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Decorators;
    using StructureMap.Configuration.DSL;

    public class ControlMapRegistry : Registry
    {
        public ControlMapRegistry(string precachedControlMapSource, string mapName)
        {
            this.For<IControlMapLoader>().
                DecorateAllWith((ctx, d) => new ControlMapCacher(d, precachedControlMapSource, mapName));
        }
    }
}