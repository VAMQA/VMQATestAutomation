namespace VM.Platform.TestAutomationFramework.Core
{
    using System;

    [Flags]
    public enum ClickWaitOption
    {
        DontWait,
        WaitBefore,
        WaitAfter,
        WaitBeforeAndAfter
    }
}