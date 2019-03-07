namespace VM.Platform.TestAutomationFramework.Core
{
    using System;

    public class ControlDefinition
    {
        public string Label { get; set; }
        public string ControlType { get; set; }
        public string ControlId { get; set; }
        public string Class { get; set; }
        public string ParentControl { get; set; }
        public string TagName { get; set; }
        public string ValueAttribute { get; set; }
        public string TagInstance { get; set; }
        public string Type { get; set; }
        public string ClassName { get; set; }
        public string InnerText { get; set; }
        public string LabelFor { get; set; }
        public string Xpath { get; set; }
        public string Version { get; set; }
        public string ImagePath { get; set; }

        public ControlDefinition OrUnderlyingControl()
        {
            return this.TagName.Equals("Label", StringComparison.OrdinalIgnoreCase)
                ? new ControlDefinition {ControlId = this.LabelFor, TagName = "*"}
                : this;
        }
    }
}