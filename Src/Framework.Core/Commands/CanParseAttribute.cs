namespace VM.Platform.TestAutomationFramework.Core.Commands
{
    using System;

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class CanParseAttribute : Attribute
    {
        public string CommandPattern { get; private set; }

        public CanParseAttribute(string commandPattern)
        {
            this.CommandPattern = commandPattern;
        }
    }
}