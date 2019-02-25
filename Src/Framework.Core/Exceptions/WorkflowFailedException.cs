namespace VM.Platform.TestAutomationFramework.Core.Exceptions
{
    using System;

    public class WorkflowFailedException : FrameworkExceptionBase
    {
        public WorkflowFailedException()
            : base()
        { }

        public WorkflowFailedException(string message)
            : base(message)
        { }

        public WorkflowFailedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}