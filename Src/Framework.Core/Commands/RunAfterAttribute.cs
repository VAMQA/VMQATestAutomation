namespace VM.Platform.TestAutomationFramework.Core.Commands
{
    using System;

    public class RunAfterAttribute : Attribute
    {
        public Type CommandType { get; set; }
    }
}