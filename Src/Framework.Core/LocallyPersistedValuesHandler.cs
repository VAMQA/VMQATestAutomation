namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Logging;

    public class LocallyPersistedValuesHandler : IPersistedValuesHandler
    {
        private readonly TestCaseConfiguration testCaseConfiguration;
        private readonly Logger logger;

        public LocallyPersistedValuesHandler(TestCaseConfiguration testCaseConfiguration, Logger logger)
        {
            this.testCaseConfiguration = testCaseConfiguration;
            this.logger = logger;
        }

        public Dictionary<string, string> LoadValues()
        {
            var capturedValues = new Dictionary<string, string>();
            var persistedValuesFileName = this.testCaseConfiguration.PersistedValuesFileName;
            if (!File.Exists(persistedValuesFileName))
            {
                this.logger.Warn(
                    "Could not find persisted values file {0}. A new file will be created at the end of the test.",
                    persistedValuesFileName);
            }
            else
            {
                try
                {
                    using (var file = File.OpenRead(persistedValuesFileName))
                    using (var reader = new BinaryReader(file))
                    {
                        logger.Info("Loading captured values from persistence.");
                        var count = reader.ReadInt32();
                        for (var i = 0; i < count; i++)
                        {
                            var key = reader.ReadString();
                            var value = reader.ReadString();
                            capturedValues[key] = value;
                        }

                        reader.Close();
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("Could not read persisted values from file. File may be locked by user or corrupt.", ex);
                }
            }
            return capturedValues;
        }

        public void SaveValues(Dictionary<string, string> values)
        {
            var persistedValuesFileName = this.testCaseConfiguration.PersistedValuesFileName;
            if (!File.Exists(persistedValuesFileName))
            {
                this.logger.Info("Creating new value persistence file {0}", persistedValuesFileName);
                File.Create(persistedValuesFileName)
                    .Close();
            }
            using (var file = File.OpenWrite(persistedValuesFileName))
                using (var writer = new BinaryWriter(file))
                {
                    this.logger.Info("Saving captured values to file.");

                    writer.Write(values.Count);
                    foreach (var pair in values)
                    {
                        writer.Write(pair.Key);
                        writer.Write(pair.Value);
                    }

                    writer.Close();
                }
        }
    }
}
