namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using StructureMap;
    using StructureMap.Configuration.DSL;

    public class TestAutomationWorkflowRegistry : Registry
    {
        public TestAutomationWorkflowRegistry(string projectName)
        {
            For<ITestCaseRunner>().Use<SingleTestCaseRunner>();
            if (projectName != null)
            {
                For<ITestCaseLoader>().Use<TfsTestCaseLoader>();
                For<IControlMapLoader>().Use<TfsControlMapLoader>();
                For<ITestDataLoader>().Use<TfsTestDataLoader>();
            }
            else
            {
                For<ITestCaseLoader>().Use<LocalTestCaseLoader>();
                For<IControlMapLoader>().Use<LocalControlMapLoader>();
                For<ITestDataLoader>().Use<LocalTestDataLoader>();                
            }
            
            For<ITestCommandRunnerFactory>().Use<TestCommandRunnerFactory>();
            For<IDateTimeProvider>().Use<DateTimeProvider>();
            For<IPersistedValuesHandler>().Use<LocallyPersistedValuesHandler>();
        }
    }
}
