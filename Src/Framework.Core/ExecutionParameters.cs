namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Collections.Generic;

    public class ExecutionParameters : Dictionary<string, string>
    {
        public string TargetBrowser { get; set; }
    }
}
