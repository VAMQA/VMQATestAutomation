namespace VM.Platform.TestAutomationFramework.Core.Exceptions
{
    using System;

    public class ExpectationFailedException : FrameworkExceptionBase
    {
        public ExpectationFailedException()
            : base()
        { }

        public ExpectationFailedException(string message)
            : base(message)
        { }

        public ExpectationFailedException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}