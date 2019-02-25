namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Decorators;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using StructureMap.Configuration.DSL;

    public class CommandRegistryDecoration : Registry
    {
        public CommandRegistryDecoration()
        {
            this.For<ITestCommand>().
                DecorateAllWith((ctx, d) => new CommandLogger(d, ctx.GetInstance<Logger>())).

                DecorateAllWith((ctx, d) => new CommandErrorHandler(d, 
                    ctx.GetInstance<IUiAdapter>(), 
                    ctx.GetInstance<Logger>(), 
                    ctx.GetInstance<TestCaseConfiguration>())).

                DecorateAllWith((ctx, d) => new CommandTimer(d, 
                    ctx.GetInstance<Logger>())).

                DecorateAllWith((ctx, d) => new IgnorePagesDecorator(d, ctx.GetInstance<TestCaseConfiguration>()));
        }
    }
}
