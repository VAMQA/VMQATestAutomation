namespace VM.Platform.TestAutomationFramework.Core.Contracts
{
    using System.Collections.Generic;
    using System.IO;

    public interface IPersistedValuesHandler
    {
        Dictionary<string, string> LoadValues();
        void SaveValues(Dictionary<string, string> values);
    }
}