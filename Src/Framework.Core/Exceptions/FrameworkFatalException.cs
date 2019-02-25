namespace VM.Platform.TestAutomationFramework.Core.Exceptions
{
    using System;

    public class FrameworkFatalException : FrameworkExceptionBase
    {
        public FrameworkFatalException()
            : base()
        {
        }

        public FrameworkFatalException(string message)
            : base(message)
        {
        }

        public FrameworkFatalException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}