namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using StructureMap;

    class TestCommandRunnerFactory : ITestCommandRunnerFactory
    {
        public ITestCommandRunner Create(IEnumerable<ITestCommand> commandsToRun, Logger logger)
        {
            return new TestCommandRunner(commandsToRun, logger);
        }
    }
}