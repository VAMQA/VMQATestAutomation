namespace VM.Platform.TestAutomationFramework.Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using VM.Platform.TestAutomationFramework.Core.DependencyManagement;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using VM.Platform.TestAutomationFramework.Core.Properties;

    public class TestCaseConfiguration
    {

        public string RequestedEnvironment { get; set; }
        public string ControlMapSource { get; set; }
        public IEnumerable<TestEnvironment> Environments { get; set; }
        public IEnumerable<string> PagesToIgnore { get; set; }
        public IEnumerable<string> IgnoreErrorsOnPages { get; set; }
        public Dictionary<string, string> SubstituteValues { get; set; }
        public Dictionary<string, string> RemoteExecutionDetails { get; set; }
        public SupportedBrowser TargetBrowser { get; set; }        
        public TimeSpan WaitTime { get; set; }
        public TimeSpan PageLoadWaitTime { get; set; }
        public string RetentionId { get; set; }
        public string ChromeProfileToUse { get; set; }
        public string TeamProjectName { get; set; }
        public string TestPlan { get; set; }
        public string ProjectName { get; set; }
        public IEnumerable<string> IgnorableColumnPatterns { get; set; }
        public string TfsCredentialAddress { get; set; }
        public string DataBaseConnectionString { get; set; }
        public string ConnectionString { get; set; }
        public string PersistedValuesFileName { get; set; }
        public ClickWaitOption ClickWaitOption { get; set; }
        public string TrackExecutionDetails{ get; set; }
        public string UpdateExecutionDetails { get; set; }
        public string EnableCloseBrowser { get; set; }
        public int ProjectID { get; set; }
        
        public TestCaseConfiguration()
        {
            Environments = new List<TestEnvironment>();
        }

        public static TestCaseConfiguration Init(SupportedBrowser targetBrowser,string projectName, string testPlan)
        {
            var configurationFile = XDocument.Load(@"Config\Config.xml");

            var configuration = new TestCaseConfiguration
            {
                TeamProjectName = projectName,
                TargetBrowser = targetBrowser,                
                TestPlan = testPlan.Split('_')[0],
                ProjectName = testPlan.Split('_')[1],
                WaitTime = GetImplicitWaitTimeFromConfigurationFile(configurationFile),
                PageLoadWaitTime = GetPageLoadWaitTimeFromConfigurationFile(configurationFile),
                IgnorableColumnPatterns = GetIgnorableColumnPatternsFromFile(configurationFile),
                PagesToIgnore = GetIgonrePages(configurationFile),
                IgnoreErrorsOnPages = GetIgnoreErrorsOnPages(configurationFile),
                ChromeProfileToUse = GetChromeProfile(configurationFile),
                SubstituteValues = GetSubstituteValue(configurationFile),
                RemoteExecutionDetails=GetRemoteExecutionDetails(configurationFile),
                Environments = GetTestEnvironmentsFromConfigurationFile(configurationFile),
                TfsCredentialAddress = GetTfsCredentialAddress(configurationFile),
                DataBaseConnectionString = GetDBConnectionString(configurationFile),
                ConnectionString = GetConnectionString(configurationFile),
                PersistedValuesFileName = GetPersistedValuesFileName(configurationFile),
                ClickWaitOption = GetClickWaitOption(configurationFile),
                TrackExecutionDetails = GetTrackExecutionDetails(configurationFile),
                UpdateExecutionDetails = GetUpdateExecutionDetails(configurationFile),
                EnableCloseBrowser = GetEnableCloseBrowser(configurationFile),
            };

            return configuration;
        }

        private static ClickWaitOption GetClickWaitOption(XDocument configurationFile)
        {
            var option = configurationFile.XPathSelectElement("/TestAutomationFramework/ClickWaitOption");
            var optionString = option == null ? ClickWaitOption.DontWait.ToString() : option.Value;

            return (ClickWaitOption) Enum.Parse(typeof (ClickWaitOption), optionString, ignoreCase: true);
        }       

        private static string GetPersistedValuesFileName(XDocument configurationFile)
        {
            var fileName = configurationFile.XPathSelectElement("/TestAutomationFramework/PersistedValuesFileName");
            return fileName == null
                ? "PersistValues.bin"
                : fileName.Value;
        }

        private static string GetTfsCredentialAddress(XDocument configurationFile)
        {
            var address = configurationFile.XPathSelectElement("/TestAutomationFramework/TfsCredentialAddress");
            return address == null
                ? null
                : address.Value;
        }

        private static string GetDBConnectionString(XDocument configurationFile)
        {
            var connstr = configurationFile.XPathSelectElement("/TestAutomationFramework/DBExecution/DBConnectionString");
            return connstr == null
                ? null
                : connstr.Value;
        }

        private static string GetConnectionString(XDocument configurationFile)
        {
            var connstr = configurationFile.XPathSelectElement("/TestAutomationFramework/DBTestAutomation/ConnectionString");
            return connstr == null
                ? null
                : connstr.Value;
        }

        private static IEnumerable<string> GetIgnoreErrorsOnPages(XDocument configurationFile)
        {
            return configurationFile
                .XPathSelectElements("/TestAutomationFramework/IgnoreErrorsOnPages/page")
                .Select(x => x.Value);
        }

        private static string GetChromeProfile(XDocument configurationFile)
        {
            var dataDirectory = configurationFile.XPathSelectElement("/TestAutomationFramework/Browsers/Chrome/profile");

            return dataDirectory == null
                ? null
                : dataDirectory.Value;
        }

        private static IEnumerable<string> GetIgnorableColumnPatternsFromFile(XDocument configurationFile)
        {
            return configurationFile
                .XPathSelectElements("/TestAutomationFramework/IgnorableColumnNamePatterns/pattern")
                .Select(x => x.Value);
        }

        private static TimeSpan GetImplicitWaitTimeFromConfigurationFile(XDocument configurationFile)
        {
            var implicitWaitTime = configurationFile.XPathSelectElement("TestAutomationFramework/ImplicitWaitTime");
            var millisecondsToWait = implicitWaitTime == null
                ? Resources.DefaultImplicitWaitTime
                : implicitWaitTime.Value;

            return TimeSpan.FromMilliseconds(double.Parse(millisecondsToWait));
        }

        private static TimeSpan GetPageLoadWaitTimeFromConfigurationFile(XDocument configurationFile)
        {
            var implicitWaitTime = configurationFile.XPathSelectElement("TestAutomationFramework/PageLoadWaitTime");
            var millisecondsToWait = implicitWaitTime == null
                ? Resources.DefaultImplicitWaitTime
                : implicitWaitTime.Value;

            return TimeSpan.FromMilliseconds(double.Parse(millisecondsToWait));
        }

        private static string GetTrackExecutionDetails(XDocument configurationFile)
        {
            var trackExecution = configurationFile.XPathSelectElement("/TestAutomationFramework/TrackExecutionDetails");

            return trackExecution == null
                ? null
                : trackExecution.Value;
        }

        private static string GetUpdateExecutionDetails(XDocument configurationFile)
        {
            var updateExecution = configurationFile.XPathSelectElement("/TestAutomationFramework/UpdateExecutionDetails");

            return updateExecution == null
                ? null
                : updateExecution.Value;
        }

        private static string GetEnableCloseBrowser(XDocument configurationFile)
        {
            var enableCloseBrowser = configurationFile.XPathSelectElement("/TestAutomationFramework/EnableCloseBrowser");

            return enableCloseBrowser == null
                ? null
                : enableCloseBrowser.Value;
        }


        private static IEnumerable<TestEnvironment> GetTestEnvironmentsFromConfigurationFile(XDocument configurationFile)
        {
            return configurationFile
                .XPathSelectElements("/TestAutomationFramework/Environments/*")
                .Select(x => new TestEnvironment { Name = x.Name.ToString(), StartPoint = x.Value });
        }

        private static IEnumerable<string> GetIgonrePages(XDocument configurationFile)
        {
            return configurationFile
                .XPathSelectElements("/TestAutomationFramework/IgnorePages/page")
                .Select(x => x.Value);
        }

        private static Dictionary<string, string> GetSubstituteValue(XDocument configurationFile)
        {
            return configurationFile
                .XPathSelectElements("/TestAutomationFramework/SubstituteValues/*")
                .ToDictionary(
                    e => e.Name.LocalName,
                    e => e.Value
                );

        }

        private static Dictionary<string, string> GetRemoteExecutionDetails(XDocument configurationFile)
        {
            return configurationFile
                .XPathSelectElements("/TestAutomationFramework/RemoteExecutionDetails/*")
                .ToDictionary(
                    e => e.Name.LocalName,
                    e => e.Value
                );

        }

        public string GetUrl(string targetEnvironment)
        {
            try
            {
                var testEnvironment = this.Environments.Single(e => e.Name == targetEnvironment).StartPoint;
                return testEnvironment;
            }
            catch (InvalidOperationException ex)
            {
                throw new FrameworkFatalException("Environment Url missing!", ex);
            }
        }

        public bool ShouldIgnorePage(string page)
        {
            return this.IgnoreErrorsOnPages.Contains(page, StringComparer.OrdinalIgnoreCase);
        }
    }
}