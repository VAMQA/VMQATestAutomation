using System;

namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
    }
}
