namespace VM.Platform.TestAutomationFramework.Core.Exceptions
{
    using System;

    public abstract class FrameworkExceptionBase : Exception
    {
        protected FrameworkExceptionBase()
            : base ()
        { }

        protected FrameworkExceptionBase(string message)
            : base(message)
        { }

        protected FrameworkExceptionBase(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}