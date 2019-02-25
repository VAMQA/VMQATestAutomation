namespace VM.Platform.TestAutomationFramework.Core.Decorators
{
    using System;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public class IgnorePagesDecorator : CommandDecorator
    {
        private readonly TestCaseConfiguration testCaseConfiguration;

        public IgnorePagesDecorator(ITestCommand command, TestCaseConfiguration testCaseConfiguration)
            : base(command)
        {
            this.testCaseConfiguration = testCaseConfiguration;
        }

        public override void Execute(TestRunContext context)
        {
          
            if (this.ShouldIgnoreCurrentPage())
            {
              return;
            }
            base.Execute(context);

           
        }

        private string CurrentPage
        {
            get
            {
                return this.Command.ExecutionParameters.ContainsKey("CurrentPage")
                    ? this.Command.ExecutionParameters["CurrentPage"]
                    : string.Empty;
            }
        }

        private bool ShouldIgnoreCurrentPage()
        {
            return this.CurrentPageIsValid() && this.IsInPagesToIgnore();
        }

        private bool CurrentPageIsValid()
        {
            return !string.IsNullOrWhiteSpace(this.CurrentPage);
        }

        private bool IsInPagesToIgnore()
        {
            return this.testCaseConfiguration.PagesToIgnore.Contains(this.CurrentPage, StringComparer.OrdinalIgnoreCase);
        }
    }
}
