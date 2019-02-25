namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Workflow
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Logging;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.UIAutomation.Properties;

    [CanParse(@"^\s*StartIteration\s*{\s*(?<StartNumber>\d*?)(?:\s*-\s*(?<EndNumber>\d+))\s*}\s*$")]
    [CanParse(@"^\s*StartIteration\s*{\s*(?<Iterations>\d+(?<Iterations>,\d+)*?)\s*\}\s*$")]
    public class StartIterationCommand : BaseCommand
    {

        private readonly ExecutionParameters executionParameters;
        private int startNumber;
        private int endNumber;
        private int[] iterations;
        private readonly Logger logger;
        private int i = 0;
        List<int?> Iteration = new List<int?>();

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        public StartIterationCommand(ExecutionParameters executionParameters, Logger logger)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;
            this.logger = logger;
            this.ParseExecutionParameters();

        }

        private void ParseExecutionParameters()
        {
            if(this.ExecutionParameters.ContainsKey("EndNumber"))
            {
                this.startNumber = Convert.ToInt32(this.ExecutionParameters["StartNumber"]);
                this.endNumber = Convert.ToInt32(this.ExecutionParameters["EndNumber"]);
            }
            else
            {
                this.iterations = Array.ConvertAll(this.ExecutionParameters["Iterations"].Split(','), int.Parse);
            }
            
        }

        #region Validation Method
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {
            var isValid = parameters.ContainsKey("EndNumber")?((HasValidStartNumber(parameters) &&
                           HasValidEndNumber(parameters) &&
                           StartNumberIsLessThanEndNumber(parameters))) : HasValidNumbers(parameters);

            if (!isValid)
            {
                throw new ArgumentException(Resources.ValidateExecutionParameters_Invalid_Parameter, this.GetType().Name);
            }
        }

        private bool HasValidNumbers(IDictionary<string, string> parameters)
        {
            var ids = Array.ConvertAll(parameters["Iterations"].Split(','), int.Parse);

            return (parameters.ContainsKey("Iterations")) && ids.All(x => x > 0);
        }

        private bool HasValidStartNumber(IDictionary<string, string> parameters)
        {
            return (parameters.ContainsKey("StartNumber")) &&
                parameters["StartNumber"].IsNumeric(x => x > 0);
        }

        private bool HasValidEndNumber(IDictionary<string, string> parameters)
        {
            return (parameters.ContainsKey("EndNumber")) &&
                parameters["EndNumber"].IsNumeric(x => x > 0);
        }

        private bool StartNumberIsLessThanEndNumber(IDictionary<string, string> parameters)
        {

            return (Convert.ToInt32(parameters["EndNumber"]) > Convert.ToInt32(parameters["StartNumber"]) ||
                    parameters["StartNumber"] == parameters["EndNumber"]);
        }

        #endregion


        # region Command Execution methods
        public override void Execute(TestRunContext context)
        {
            if ((!context.Iteration.HasValue)&&(ExecutionParameters.ContainsKey("EndNumber")))
            {
                context.DiStart = this.startNumber;
                context.DiStop = this.endNumber;
                context.Iteration = this.startNumber;
            }
            else if (this.ExecutionParameters.ContainsKey("Iterations") && context.Retry == 1)
            {
                int count = context.TestFindings.Count;
                context.IsRandomExecution = true;
                context.DiStart = this.iterations[0];
                context.DiStop = this.iterations[this.iterations.Length - 1];
                if (i <= this.iterations.Length - 1)
                {
                    context.Iteration = this.iterations[i];
                }

                if (context.TestFindings.Count == 0 && context.TestResult.ToString() == "Pending")
                {
                    context.Iteration = this.iterations[i];
                    
                }
                else if ((context.TestFindings[count - 1].TestResult.ToString() == "Fail" || context.TestFindings.Count == 0) && (!(Iteration.Contains(context.Iteration))))
                {
                    context.Iteration = this.iterations[i];
                    Iteration.Add(context.Iteration);
                }
                else if (context.TestFindings[count - 1].TestResult.ToString() == "Pass" && i < this.iterations.Length - 1)
                {
                    i = i + 1;
                    context.Iteration = this.iterations[i];
                    
                }

                else
                {
                    i = i + 1;
                    context.Iteration = this.iterations[i];
                    
                }

            }

            else if (this.ExecutionParameters.ContainsKey("Iterations") && context.Retry == 0)
            {

                context.IsRandomExecution = true;
                context.DiStart = this.iterations[0];
                context.DiStop = this.iterations[this.iterations.Length - 1];
                context.Iteration = this.iterations[i];
                i = i + 1;

            }
            this.logger.Info("TC # : {0}, Starting iteration #{1}", context.TestCaseId, context.Iteration.Value);
            context.StartIterationCommand = context.CurrentCommand;

        }

        #endregion




    }
}
