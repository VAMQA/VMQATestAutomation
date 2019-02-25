namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using VM.Platform.TestAutomationFramework.Core.Contracts;

    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now { get { return DateTime.Now; } }
    }
}