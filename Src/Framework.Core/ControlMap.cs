namespace VM.Platform.TestAutomationFramework.Core
{
    using System.Collections.Generic;

    public class ControlMap : Dictionary<string, Dictionary<string, ControlDefinition>>
    {
        public ControlMap(Dictionary<string, Dictionary<string, ControlDefinition>> dictionary)
        {
            foreach (var page in dictionary)
            {
                this.Add(page.Key, page.Value);
            }
        }

        public string Source { get; set; }
    }
}