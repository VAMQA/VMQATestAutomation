namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.UIAutomation.Scripts.PostScripts;
    using System.Text.RegularExpressions;

    [CanParse(@"^\s*InvokeApplication(?:\s*\{\s*(?<Target>.*)\s*\})?\s*$")]
    public class InvokeApplicationCommand : BaseCommand
    {
        private readonly IUiAdapter uiAdapter;
        private readonly TestCaseConfiguration configuration;
        private readonly ExecutionParameters executionParameters;

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        public InvokeApplicationCommand(IUiAdapter uiAdapter, TestCaseConfiguration configuration, ExecutionParameters executionParameters)
            : base(executionParameters)
        {
            this.uiAdapter = uiAdapter;
            this.configuration = configuration;
            this.executionParameters = executionParameters;


        }

        #region Validation Methods
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {
            var isValid = HasNoTarget(parameters) || HasValidTarget(parameters);
            if (!isValid)
            {
                throw new ArgumentException("Invalid execution parameters for Invoke Application command", "parameters");
            }
        }

        private bool HasNoTarget(ExecutionParameters parameters)
        {
            return !parameters.ContainsKey("Target") || string.IsNullOrWhiteSpace(parameters["Target"]);
        }

        private bool HasValidTarget(IReadOnlyDictionary<string, string> parameters)
        {
            return parameters["Target"].IsAlphaNumeric(x => x.Length > 0);
        }

        #endregion

        #region Command Execution Methods
        public override void Execute(TestRunContext context)
        {

            if (context.Iteration.HasValue)
            {
                context.IsCleanUpWebDriverEndOfIteration = true;
            }
            var environmentName = this.SelectEnvironmentName(context);
            if (string.IsNullOrWhiteSpace(environmentName))
            {
                this.uiAdapter.TakeScreenshot();
                throw new FrameworkFatalException("environment not specified or environment undefined");
            }

            TestEnvironment environment;
            if (this.TryGetEnvironment(environmentName, out environment))
            {
                try
                {
                    //context.Url = new Uri(environment.StartPoint);
                    //Added code to handle dynamic URL                
                    Regex regexObj = new Regex(@"\{.*?\}");
                    Match match = regexObj.Match(environment.StartPoint);
                    context.Url = match.Success ? new Uri(regexObj.Replace(environment.StartPoint, context.CapturedValues[match.Value.ToUpper()]))
                                                : new Uri(environment.StartPoint);   
                    this.uiAdapter.GoToUrl(context.Url);
                }
                catch (UriFormatException ex)
                {
                    this.uiAdapter.TakeScreenshot();
                    throw new FrameworkFatalException("environment URL is invalid", ex);
                }
            }
            else
            {                
                throw new FrameworkFatalException("environment undefined in configuration file"); 
            }

            new AutomateThreatManagementGateway(this.configuration, this.uiAdapter).Execute();

        }


        private string SelectEnvironmentName(TestRunContext context)
        {
            string parameterizedEnvironment;
          
            this.executionParameters.TryGetValue("Target", out parameterizedEnvironment);
            var environmentName = string.IsNullOrWhiteSpace(parameterizedEnvironment)
                ?  context.RequestedEnvironment
                : (parameterizedEnvironment.Trim().Equals("CNBNLF_RECALL", StringComparison.OrdinalIgnoreCase)
                    ?  context.RequestedEnvironment + "RECALL"
                    : parameterizedEnvironment);

            return environmentName;
        }

        private bool TryGetEnvironment(string environmentName, out TestEnvironment requestedEnvironment)
        {
            requestedEnvironment = this.configuration.Environments.SingleOrDefault(e => e.Name == environmentName);
            return requestedEnvironment != null;
        }
        #endregion
    }
}