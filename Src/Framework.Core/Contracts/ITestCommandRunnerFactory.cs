namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core.Logging;

    public interface ITestCommandRunnerFactory
    {
        ITestCommandRunner Create(IEnumerable<ITestCommand> commandsToRun, Logger logger);
    }
}