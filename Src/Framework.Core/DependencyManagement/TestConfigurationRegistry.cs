namespace VM.Platform.TestAutomationFramework.Core.DependencyManagement
{
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using StructureMap.Configuration.DSL;

    internal class TestConfigurationRegistry : Registry
    {
        public TestConfigurationRegistry(string targetBrowserName, string projectName, string testPlan)
        {            
            var targetBrowser = GetTargetBrowser(targetBrowserName);            
            For<TestCaseConfiguration>().Use(TestCaseConfiguration.Init(targetBrowser, projectName, testPlan)).Singleton();
        }

        private static SupportedBrowser GetTargetBrowser(string targetBrowserName)
        {
            SupportedBrowser browser;
            switch (targetBrowserName.ToLower())
            {
                case "ie":
                    browser = SupportedBrowser.Ie;
                    break;
                case "chromeheadless":
                    browser = SupportedBrowser.ChromeHeadless;
                    break;
                case "chrome":
                    browser = SupportedBrowser.Chrome;
                    break;
                case "firefox":
                    browser = SupportedBrowser.Firefox;
                    break;
                case "phantomjs":
                    browser = SupportedBrowser.PhantomJS;
                    break;
                case "android":
                    browser = SupportedBrowser.Android;
                    break;
                case "androidmobilelabs":
                    browser = SupportedBrowser.AndroidMobilelabs;
                    break;
                case "iosmobilelabs":
                    browser = SupportedBrowser.IOSMobilelabs;
                    break;
                case "remote":
                    browser = SupportedBrowser.Remote;
                    break;
                case "remoteie":                
                    browser = SupportedBrowser.RemoteIe;
                    break;
                case "remotechrome":
                    browser = SupportedBrowser.RemoteChrome;
                    break;
                case "remotefirefox":
                    browser = SupportedBrowser.RemoteFirefox;
                    break;
                case "remotephantomjs":
                    browser = SupportedBrowser.RemotePhantomJS;
                    break;
                default:
                    throw new FrameworkFatalException("Unsupported browser: " + targetBrowserName);
            }
            return browser;
        }
    }

    public enum SupportedBrowser
    {
        Ie,
        ChromeHeadless,
        Chrome,
        Firefox,
        PhantomJS,
        Android,
        AndroidMobilelabs,
        IOSMobilelabs,
        Remote,
        RemoteIe,
        RemoteChrome,
        RemoteFirefox,
        RemotePhantomJS
    }
}