namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using System;
    using System.Diagnostics;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using VM.Platform.TestAutomationFramework.Extensions;

    public class CommandTimer : CommandDecorator
    {
        private readonly Logger logger;
        public TimeSpan ElapsedTime { get; set; }

        public CommandTimer(ITestCommand command, Logger logger) : base(command)
        {
            this.logger = logger;
        }

        public override void Execute(TestRunContext context)
        {
            var stopwatch = new Stopwatch();
            try
            {
                stopwatch.Start();

                base.Execute(context);
            }
            finally
            {
                stopwatch.Stop();

                this.logger.Debug("Execution Time: {0} ms".Indented(), stopwatch.ElapsedMilliseconds);
                this.ElapsedTime = stopwatch.Elapsed;
            }
        }
    }
}
