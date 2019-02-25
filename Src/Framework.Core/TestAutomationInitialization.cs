namespace VM.Platform.TestAutomationFramework.Core
{
    using System;

    public struct TestAutomationInitialization
    {
        public Version CoreVersion { get; set; }
        public string RepoUri { get; set; }
        public string Project { get; set; }
        public string TestPlan { get; set; }
        public string TargetBrowser { get; set; }
        public string ControlMapSource { get; set; }
        public Version UICommandsVersion { get; set; }
    }
}