namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using System;
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.UIAutomation.Properties;

    [CanParse(@"^Page\s*\{\s*(?<CurrentPage>.+?)\s*(?:-\s*(?<FlowIdentifier>.+?))?\s*(?:\[(?<DiIndicator>.+?)\])?\s*\}\s*$")]
    public class PageCommand : BaseCommand
    {
        private int? flowIdentifier;
        private bool hasDiIndicator;

        private readonly ExecutionParameters executionParameters;
        private string currentPage;

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        public PageCommand(ExecutionParameters executionParameters)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.ParseExecutionParameters();
        }

        private void ParseExecutionParameters()
        {
            this.currentPage = this.ExecutionParameters["CurrentPage"];
            this.flowIdentifier = this.executionParameters.ContainsKey("FlowIdentifier")
                ? int.Parse(this.executionParameters["FlowIdentifier"])
                : (int?) null;
            this.hasDiIndicator = this.executionParameters.ContainsKey("DiIndicator") 
                && this.ExecutionParameters["DiIndicator"].IsNotNullOrEmptyOrWhiteSpace();
        }

        #region Validation methods

        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {
            var isValid = HasValidPageName(parameters) &&
                          (HasNoIteration(parameters) || HasValidIteration(parameters)) &&
                          (HasNoDiIndicator(parameters) || HasValidDiIndicator(parameters));

            if (!isValid)
            {
                throw new ArgumentException(
                    Resources.PageCommand_ValidateExecutionParameters_Invalid_execution_parameters_for_the_Page_command,
                    "parameters");
            }
        }

        private bool HasValidDiIndicator(IReadOnlyDictionary<string, string> parameters)
        {
            return parameters["DiIndicator"].Equals("DI", StringComparison.OrdinalIgnoreCase);
        }

        private bool HasNoDiIndicator(IReadOnlyDictionary<string, string> parameters)
        {
            return !parameters.ContainsKey("DiIndicator") || 
                string.IsNullOrWhiteSpace(parameters["DiIndicator"]);
        }


        private bool HasValidIteration(IReadOnlyDictionary<string, string> parameters)
        {
            return parameters["FlowIdentifier"].IsNumeric(x => x > 0);
        }

        private bool HasNoIteration(IReadOnlyDictionary<string, string> parameters)
        {
            return !parameters.ContainsKey("FlowIdentifier");
        }


        private bool HasValidPageName(IReadOnlyDictionary<string, string> parameters)
        {
            return parameters.ContainsKey("CurrentPage") &&
                   parameters["CurrentPage"].IsAlphaNumeric(x => x.Length > 0);
        }

        #endregion

        #region Command Execution methods

        public override void Execute(TestRunContext context)
        {
            context.CurrentPage = this.currentPage;

            context.FlowIdentifier = this.flowIdentifier;
            context.DiIndicator = this.hasDiIndicator;

            //context.Iteration = this.flowIdentifier.GetValueOrDefault();
            //context.FlowIdentifier = this.flowIdentifier.GetValueOrDefault();
            //context.ReqIdentifier = GetReqIdentifierValue(context);
        }

        private int? GetReqIdentifierValue(TestRunContext context)
        {
            int? reqIdentifier;
            if (this.hasDiIndicator)
            {
                // TODO: ExecutionIsStartingFromTheMiddle is not yet supported
                reqIdentifier = context.ExecutionIsStartingFromTheMiddle
                    ? this.flowIdentifier.GetValueOrDefault()
                    : context.DiLoopVariable;

                if (reqIdentifier <= 0)
                {
                    throw new FrameworkFatalException("Requirement identifier must be greater than 0");
                }
            }
            else
            {
                reqIdentifier = null;
            }

            return reqIdentifier;
        }

        #endregion
    }
}
