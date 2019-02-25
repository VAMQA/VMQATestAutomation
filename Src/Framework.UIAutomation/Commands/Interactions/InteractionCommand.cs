namespace VM.Platform.TestAutomationFramework.UIAutomation.Commands.Interactions
{
    using System;
    using System.Collections.Generic;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Commands;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Extensions;
    using VM.Platform.TestAutomationFramework.UIAutomation.Properties;

    public abstract class InteractionCommand : BaseCommand
    {
        private readonly ExecutionParameters executionParameters;
        protected readonly IUiAdapter uiAdapter;
        protected readonly string logicalFieldName;
        protected string logicalFieldValue;

        public InteractionCommand(ExecutionParameters executionParameters, IUiAdapter uiAdapter)
            : base(executionParameters)
        {
            this.executionParameters = executionParameters;

            this.uiAdapter = uiAdapter;
            this.logicalFieldName = this.executionParameters["LogicalFieldName"];
            this.logicalFieldValue = this.executionParameters["LogicalFieldValue"];
        }

        public override ExecutionParameters ExecutionParameters
        {
            get { return this.executionParameters; }
        }

        #region Validation methods
        protected override void ValidateExecutionParameters(ExecutionParameters parameters)
        {
            var isValid = this.HasValidLogicalFieldName(parameters) &&
                          this.HasValidLogicalFieldValue(parameters);

            if (!isValid)
            {
                throw new ArgumentException(Resources.ValidateExecutionParameters_Invalid_Parameter, this.GetType().Name);
            }
        }

        private bool HasValidLogicalFieldName(IReadOnlyDictionary<string, string> parameters)
        {

            return parameters.ContainsKey("LogicalFieldName") &&
                   parameters["LogicalFieldName"].IsNotNullOrEmpty();
        }

        private bool HasValidLogicalFieldValue(IReadOnlyDictionary<string, string> parameters)
        {
            return parameters.ContainsKey("LogicalFieldValue") &&
                   parameters["LogicalFieldValue"].IsNotNullOrEmpty();
        } 
        #endregion
    }
}