namespace VM.Platform.TestAutomationFramework.Adapters.Selenium
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.DependencyManagement;
    using VM.Platform.TestAutomationFramework.Extensions;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Firefox;
    using OpenQA.Selenium.IE;
    using OpenQA.Selenium.Interactions;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium.Support.UI;
    using OpenQA.Selenium.Remote;
    using System.Data.SqlClient;
    using System.Text;
    using System.IO;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Windows.Forms;
    using Core.Exceptions;
    using System.Xml.Linq;
    using System.Xml.XPath;
    using OpenQA.Selenium.Appium.Android;
    using OpenQA.Selenium.Appium.iOS;
    //using Sikuli4Net.sikuli_REST;
    //using Sikuli4Net.sikuli_UTIL;

    public class SeleniumAdapter : IUiAdapter
    {
        private IWebDriver driver;
        private readonly TestCaseConfiguration testCaseConfiguration;
        //Sikuli4Net.sikuli_REST.Screen scr;

        public SeleniumAdapter(TestCaseConfiguration testCaseConfiguration)
        {
            this.testCaseConfiguration = testCaseConfiguration;
            this.CreateNewBrowserWindow();
            //APILauncher launch = new APILauncher(true);
            //scr = new Sikuli4Net.sikuli_REST.Screen();
        }

        private void CreateNewBrowserWindow()
        {
            string browser = string.Empty;
            switch (this.testCaseConfiguration.TargetBrowser)
            {
                case SupportedBrowser.Ie:
                    var internetExplorerOptions = new InternetExplorerOptions
                    {
                        RequireWindowFocus = true,
                        EnablePersistentHover = true,
                        EnableNativeEvents = true,
                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                        IgnoreZoomLevel = true
                    };
                    InternetExplorerDriverService Ieservice = InternetExplorerDriverService.CreateDefaultService();
                    Ieservice.HideCommandPromptWindow = true;
                    //this.driver = new InternetExplorerDriver(Ieservice, internetExplorerOptions,TimeSpan.FromSeconds(120));
                    this.driver = new InternetExplorerDriver(internetExplorerOptions);
                    break;
                case SupportedBrowser.ChromeHeadless:
                    //accessing Chromeoptions
                    ChromeOptions option = new ChromeOptions();
                    //taking headless chrome
                    option.AddArgument("--headless");
                    option.AddArgument("disable-gpu");
                    this.driver = new ChromeDriver(option);
                    break;
                case SupportedBrowser.Chrome:
                    //this.driver = new ChromeDriver();
                    var chromeoptions = new ChromeOptions();                    
                    chromeoptions.AddArguments("disable-infobars");
                    chromeoptions.AddArguments("disable-impl-side-painting");
                    chromeoptions.AddArguments("--no-sandbox");
                    chromeoptions.AddArguments("--disable-dev-shm-usage");
                    ChromeDriverService chromeservice = ChromeDriverService.CreateDefaultService();
                    chromeservice.HideCommandPromptWindow = true;
                    this.driver = new ChromeDriver(chromeservice, chromeoptions,TimeSpan.FromSeconds(120)); 
                    break;
                case SupportedBrowser.Firefox:
                    this.driver = new FirefoxDriver();
                    break;
                case SupportedBrowser.PhantomJS:
                    this.driver = new PhantomJSDriver();
                    break;
                case SupportedBrowser.Android:
                    DesiredCapabilities capabilities = new DesiredCapabilities();
                    {
                        var configurationFile = XDocument.Load(@"Config\Config.xml");

                        capabilities.SetCapability("browserName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidExecutionDetails/BrowserName").Value));
                        capabilities.SetCapability("platformName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidExecutionDetails/PlatformName").Value));
                        capabilities.SetCapability("deviceName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidExecutionDetails/DeviceName").Value));
                        capabilities.SetCapability("newCommandTimeout", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/RemoteExecutionDetails/ServerURLWaitTime").Value));
                        this.driver = new RemoteWebDriver(new Uri(Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidExecutionDetails/ServerURL").Value)), capabilities);
                    }
                    break;
                case SupportedBrowser.AndroidMobilelabs:
                    {
                        var configurationFile = XDocument.Load(@"Config\Config.xml");
                        Uri deviceConnectHost = new Uri("http://mobiletestingtool/Appium");
                        DesiredCapabilities capabilities1 = new DesiredCapabilities();
                        capabilities1.SetCapability("deviceConnectUsername", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidMobileLabsExecutionDetails/userName").Value));//same as iOs
                        capabilities1.SetCapability("deviceConnectApiKey", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidMobileLabsExecutionDetails/apiKey").Value));//same as iOS
                        capabilities1.SetCapability("udid", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidMobileLabsExecutionDetails/deviceID").Value)); //Serial number of the device. Grab it from ML
                        capabilities1.SetCapability("browserName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidMobileLabsExecutionDetails/browserName").Value));
                        capabilities1.SetCapability("platformName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/AndroidMobileLabsExecutionDetails/platformName").Value));
                        capabilities1.SetCapability("automationName", "Appium");

                        this.driver = new AndroidDriver<IWebElement>(deviceConnectHost, capabilities1, TimeSpan.FromMinutes(10));
                        Thread.Sleep(20000);
                        this.driver.Navigate().GoToUrl("http://www.google.com");
                    }
                    break;
                case SupportedBrowser.IOSMobilelabs:
                    {
                        try
                        {
                            var configurationFile = XDocument.Load(@"Config\Config.xml");
                            Uri deviceConnectHost = new Uri("http://mobiletestingtool/Appium");
                            DesiredCapabilities capabilities2 = new DesiredCapabilities();
                            capabilities2.SetCapability("deviceConnectUsername", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/userName").Value)); //ML Username
                            capabilities2.SetCapability("deviceConnectApiKey", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/apiKey").Value)); //API Key from Mobile Labs
                            capabilities2.SetCapability("udid", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/vendorID").Value)); //  vendor Id of the device. Grab it from Mobile labs
                            capabilities2.SetCapability("browserName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/browserName").Value));
                            capabilities2.SetCapability("platformName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/platformName").Value));
                            capabilities2.SetCapability("platformVersion", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/platformVersion").Value));
                            capabilities2.SetCapability("automationName", "XCUITest");
                            capabilities2.SetCapability("deviceName", Convert.ToString(configurationFile.XPathSelectElement("/TestAutomationFramework/IOSMobileLabsExecutionDetails/deviceName").Value));

                            this.driver = new IOSDriver<IWebElement>(deviceConnectHost, capabilities2, TimeSpan.FromMinutes(45));
                            Thread.Sleep(20000);
                            this.driver.Navigate().GoToUrl("http://www.google.com");
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                    break;
                case SupportedBrowser.Remote:
                case SupportedBrowser.RemoteIe:
                case SupportedBrowser.RemoteChrome:
                case SupportedBrowser.RemoteFirefox:
                case SupportedBrowser.RemotePhantomJS:
                    {
                        switch (this.testCaseConfiguration.TargetBrowser.ToString().Replace("Remote",""))
                        {
                            case "Ie":
                                browser = "internet explorer";
                                break;
                            case "Chrome":
                                browser = "chrome";
                                break;
                            case "Firefox":
                                browser = "firefox";
                                break;
                            case "PhantomJS":
                                browser = "phantomjs";
                                break;
                            case "":
                                browser = this.testCaseConfiguration.RemoteExecutionDetails["DefaultBrowser"];
                                break;
                            default:
                                break;
                        }
                        DesiredCapabilities cap = new DesiredCapabilities();
                        cap.SetCapability(CapabilityType.BrowserName, browser);
                        cap.SetCapability(CapabilityType.Platform, this.testCaseConfiguration.RemoteExecutionDetails["PlatformName"]);
                        this.driver = new RemoteWebDriver(new Uri(this.testCaseConfiguration.RemoteExecutionDetails["ServerURL"]), cap, TimeSpan.FromSeconds(Convert.ToDouble(this.testCaseConfiguration.RemoteExecutionDetails["ServerURLWaitTime"])));                    
                    }                    
                    break;                
                default:
                    throw new ArgumentOutOfRangeException();
            }
            this.driver.Manage().Timeouts().ImplicitWait=this.testCaseConfiguration.WaitTime;

            if (!(this.testCaseConfiguration.TargetBrowser == SupportedBrowser.Remote) && !(this.testCaseConfiguration.TargetBrowser == SupportedBrowser.AndroidMobilelabs) && !(this.testCaseConfiguration.TargetBrowser == SupportedBrowser.IOSMobilelabs))
            {
                this.driver.Manage().Window.Maximize();
            }
        }

        public IWebDriver Driver
        {
            get { return this.driver; }
        }

        public string CurrentPageTitle
        {
            get { return this.driver.Title; }
        }

        public void GoToUrl(Uri url)
        {
            
            if (DriverWindowCount() > 0)
            {
                try
                {
                    this.driver.Navigate().GoToUrl(url.AbsoluteUri);
                }
                catch (Exception ex)
                {
                    if (ex is WebDriverException || ex is WebDriverTimeoutException)
                        this.driver.Navigate().GoToUrl(url.AbsoluteUri);
                }
            }
            else
            {
                this.CreateNewBrowserWindow();
                this.driver.Navigate().GoToUrl(url.AbsoluteUri);

            }
        }

        private int DriverWindowCount()
        {
            try
            {
                return this.driver.WindowHandles.Count;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public void CloseCurrentBrowser()
        {
            if (DriverWindowCount() > 0)
            {
                if (Convert.ToBoolean(this.testCaseConfiguration.EnableCloseBrowser))
                this.driver.Close(); 
            }
            if (DriverWindowCount() == 0)
            {
                Quit();
            }
        }

        public void TakeScreenshot()
        {
            try
            {
                if (File.Exists("ErrorPageScreenshot.jpg"))
                {
                    File.Delete("ErrorPageScreenshot.jpg");
                }
                Screenshot ss = ((ITakesScreenshot)driver).GetScreenshot();
                ss.SaveAsFile("ErrorPageScreenshot.jpg", OpenQA.Selenium.ScreenshotImageFormat.Jpeg);
            }
            catch (Exception ex)
            {
                //throw new Exception("Screenshot NOT Possible Exception");
                Rectangle bounds = System.Windows.Forms.Screen.GetBounds(Point.Empty);
                using (Bitmap bitmap = new Bitmap(bounds.Width, bounds.Height))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(Point.Empty, Point.Empty, bounds.Size);
                    }
                    bitmap.Save("ErrorPageScreenshot.jpg", ImageFormat.Jpeg);
                }
            }

        }

        public void CloseAndReturn()
        {
            string BaseWindow = driver.CurrentWindowHandle;
            int tabs = driver.WindowHandles.Count();
            for (int tab = tabs; tab > 1; tab--)
            {
                this.driver.SwitchTo().Window(driver.WindowHandles.Last());
                if (driver.WindowHandles.Last() != BaseWindow)
                {
                    driver.Close();
                }
            }
            this.driver.SwitchTo().Window(BaseWindow);
        }        

        public void Quit()
        {
            if(Convert.ToBoolean(this.testCaseConfiguration.EnableCloseBrowser))
            this.driver.Quit();
        }

        public void SetControlValue(ControlDefinition ctrlDefination, string value)
        {
            IWebElement element = FindElement(ctrlDefination);

            /* IBU IBU  new  requirement
             * //set Current date
            value = SetCurrentDate(value);
            // Clear the text from control
            value = CleanTextArea(element, value);*/
            // We want to overwrite the existing value, rather than appending to it   
         


            //if(value.StartsWith("~"))
            //{
            //    element.Click();
            //    ((IJavaScriptExecutor)this.driver).ExecuteScript("arguments[0].setAttribute('value','" + value.Split('~')[1] + "')", element);
            //}
            //else
            //{
            //    if ((element.Text != string.Empty) || (element.GetAttribute("value") != string.Empty))
            //    {
            //        element.Clear();
            //    }

            //    // Some element types require clicking before entering data, for some unknown reason
            //    if (element.GetAttribute("type").Equals("tel", StringComparison.OrdinalIgnoreCase) ||
            //        HasPlaceHolder(element))
            //    {
            //        element.Click();
            //    }

            //    element.SendKeys(value);
            //}

            try
            {
                // We want to overwrite the existing value, rather than appending to it            
                if (((element.Text != string.Empty) || (element.GetAttribute("value") != string.Empty))
                    && (!value.StartsWith("~")))
                {
                    element.Clear();
                }

                // Some element types require clicking before entering data, for some unknown reason
                if (element.GetAttribute("type").Equals("tel", StringComparison.OrdinalIgnoreCase) || HasPlaceHolder(element))
                {
                    element.Click();
                }
                
                //Claims team setting value requires Java Script Executor
                if (value.StartsWith("~"))
                {
                    value = value.Split('~')[1];
                    ((IJavaScriptExecutor)this.driver).ExecuteScript("arguments[0].setAttribute('value','" + value + "')", element);
                }
                else
                {
                    element.SendKeys(value);
                }
           
                var sentValue = element.GetAttribute("value");

                if (sentValue != value && !HasPlaceHolder(element))
                {
                    WaitAndTryEnteringDataAgain(element, value);
                }
            }
            catch (StaleElementReferenceException)
            {
                element = FindElement(ctrlDefination);
                var sentValue = element.GetAttribute("value");
                if (sentValue != value && !HasPlaceHolder(element))
                {
                    WaitAndTryEnteringDataAgain(element, value);
                }
            }  
        }       

        private bool HasPlaceHolder(IWebElement element)
        {
            return element.GetAttribute("placeholder").IsNotNullOrEmpty();
        }

        private static void WaitAndTryEnteringDataAgain(IWebElement element, string value)
        {
            
                // Sometimes, the keys are not entered properly (in full), therefore we wait, and try again
                Thread.Sleep(1000);
                element.Clear();
                element.SendKeys(value);
        }

        public void SetControlValue(string key, string value)
        {
            this.driver.FindElement(By.Id(key)).SendKeys(value);
        }

        public void ClickElement(ControlDefinition ctrlDefinition)
        {
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(false);", FindElement(ctrlDefinition));
                FindElement(ctrlDefinition).Click();
                 
            }
            catch (Exception)
            {                
                ClickUsingJavascript(ctrlDefinition);
            }            
        }

        public void DoubleClickElement(ControlDefinition ctrlDefinition)
        {
            try
            {
                Actions action = new Actions(driver).DoubleClick(FindElement(ctrlDefinition));                
                action.Build().Perform();                
            }
            catch (Exception)
            {
                ClickUsingJavascript(ctrlDefinition);
            }
        }

        public void HoverElement(ControlDefinition ctrlDefinition,string value)
        {
            try
            {
                IWebElement submenu = driver.FindElement(By.XPath(@"//*[contains(text(),'" + value + "')]"));
                ((IJavaScriptExecutor)this.driver).ExecuteScript("arguments[0].click();", submenu);
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Not found element on UI Page");
            }
        }        

        public void SendKeyboadKeys(ControlDefinition ctrlDefinition, string value)
        {
            try
            {
                Actions action = new Actions(this.driver);
                string keypairvalue = value.Split('_')[1].ToLower();

               if(string.Equals(value.Split('_')[0],"alt",StringComparison.OrdinalIgnoreCase))
                {
                    action.MoveToElement(FindElement(ctrlDefinition)).Click().KeyDown(FindElement(ctrlDefinition), OpenQA.Selenium.Keys.Alt).SendKeys(keypairvalue).KeyUp(FindElement(ctrlDefinition), OpenQA.Selenium.Keys.Alt).Build().Perform();
                }                    
                else if(string.Equals(value.Split('_')[0],"control",StringComparison.OrdinalIgnoreCase))
                {
                    action.MoveToElement(FindElement(ctrlDefinition)).Click().KeyDown(FindElement(ctrlDefinition), OpenQA.Selenium.Keys.Control).SendKeys(keypairvalue).KeyUp(FindElement(ctrlDefinition), OpenQA.Selenium.Keys.Control).Build().Perform();
                }                    
                else if(string.Equals(value.Split('_')[0],"shift",StringComparison.OrdinalIgnoreCase))
                {
                    //action.KeyDown(Keys.Shift).SendKeys(keypairvalue).Perform();
                    action.MoveToElement(FindElement(ctrlDefinition)).Click().KeyDown(FindElement(ctrlDefinition), OpenQA.Selenium.Keys.Shift).SendKeys(keypairvalue).KeyUp(FindElement(ctrlDefinition), OpenQA.Selenium.Keys.Shift).Build().Perform();
                }                  
            }
            catch (Exception)
            {
                throw new NoSuchElementException("Not found element on UI Page");
            }
        }

        public void SwitchToIFrame(ControlDefinition parent, ControlDefinition child)
        {
            //this.driver.SwitchTo().Frame(FindElement(parent)).SwitchTo().ParentFrame();
            this.driver.SwitchTo().Frame(FindElement(parent)).SwitchTo().Frame(FindElement(child));
        }

        public string GetValue(string controlId, string property)
        {
            return this.driver.FindElement(By.Id(controlId)).GetAttribute(property);
        }

        public void SetDropDownValue(ControlDefinition controlDefinition, string value)
        {
            var element = FindElement(controlDefinition);
            //v3.5 changes
            if (element.GetAttribute("type").Equals("tel", StringComparison.OrdinalIgnoreCase) || HasPlaceHolder(element))
            {
                element.Click();
            }
            if (controlDefinition.ParentControl.Contains("LoginFrame"))
            {
                ((IJavaScriptExecutor)this.driver).ExecuteScript("arguments[0].scrollIntoView(true);", element);
                element.Click();
            }            

            var selectElement = new SelectElement(element);

            if (value.StartsWith("~"))
            {
                value = value.Split('~')[1];
                WaitForOptionToBeSelectable(controlDefinition, value);
                var allOptions=selectElement.Options;
                value = allOptions.First(x => x.Text.Contains(value)).Text.ToString();
            }
            else
            {
                WaitForOptionToBeSelectable(controlDefinition, value);
            }
            
            try
            {
                selectElement.SelectByText(value);
            }
            catch (StaleElementReferenceException)
            {
                element = FindElement(controlDefinition);
                selectElement = new SelectElement(element);
                selectElement.SelectByText(value);
            }
        }

        public void SetDropDownByIndex(ControlDefinition controlDefinition, int indexValue)
        {
            var element = FindElement(controlDefinition);            
            var selectElement = new SelectElement(element);
            WaitForIndexToBeSelectable(controlDefinition);
            try
            {
                selectElement.SelectByIndex(indexValue);
            }
            catch (StaleElementReferenceException)
            {
                element = FindElement(controlDefinition);
                selectElement = new SelectElement(element);
                selectElement.SelectByIndex(indexValue);                
            }
        }

        public string GetElementValue(ControlDefinition ctrlDefinition)
        {
            return ctrlDefinition.TagName.Equals("Select", StringComparison.OrdinalIgnoreCase)
                ? this.GetElementSelectedValue(ctrlDefinition)
                : this.GetElementValueOrText(ctrlDefinition);
        }

        public string GetValueFromDB(string query)
        {
            return this.RunDBQuery(query);
        }

        private string RunDBQuery(string dbQuery)
        {   
            List<string> colValues = new List<string>();
            using (SqlConnection con = new SqlConnection(this.testCaseConfiguration.ConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand(dbQuery, con))
                {   
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                            colValues.Add(Convert.ToString(reader.GetValue(i)));                            
                        break;
                    }   
                }
            }
            return String.Join(", ", colValues.ToArray());
        }

        private string GetElementSelectedValue(ControlDefinition ctrlDefinition)
        {
            var selector = new SelectElement(this.FindElement(ctrlDefinition));
            return selector.SelectedOption.Text;
        }

        private string GetElementValueOrText(ControlDefinition ctrlDefinition)
        {
            IWebElement element = this.FindElement(ctrlDefinition);
            return element.GetAttribute("value")
                   ?? element.Text;
        }

        public void WaitForLoad(ControlDefinition ctrlDefinition)
        {
            IJavaScriptExecutor js = (IJavaScriptExecutor)driver;            
            var millisecondsToWait = this.testCaseConfiguration.PageLoadWaitTime;
            var waittime = new WebDriverWait(this.driver, millisecondsToWait);
            int timeoutSec = Convert.ToInt32(waittime.Timeout.TotalSeconds);
            WebDriverWait wait = new WebDriverWait(driver, new TimeSpan(0, 0, timeoutSec));
            wait.Until(wd => js.ExecuteScript("return document.readyState").ToString() == "complete");
        }

        public string GetTableValue(ControlDefinition ctrlControlDefinition)
        {
            var element = this.FindElement(ctrlControlDefinition);
            return element.GetAttribute("value");

        }

        public void DriverWaitTime(long timeInSeconds)
        {
            this.driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromSeconds(timeInSeconds);
        }

        public void BackNavigation()
        {
            this.driver.Navigate().Back();
        }

        public bool IsElementPresent(ControlDefinition controlDefinition)
        {
            // TODO: Check whether to remove this wait.

            this.driver.Manage().Timeouts().ImplicitWait=TimeSpan.FromMilliseconds(0);
            bool result = this.driver.FindElements(ByControlDefinition(controlDefinition)).Count > 0;
            this.driver.Manage().Timeouts().ImplicitWait=this.testCaseConfiguration.WaitTime;

            return result;
        }

        public bool FindElementEnableStatus(ControlDefinition controlDefinition)
        {
            return FindElement(controlDefinition).Enabled;
        }

        public IEnumerable<string> GetTableColumn(ControlDefinition controlDefinition, int columnNumber)
        {
            IWebElement table = FindElement(controlDefinition);
            return table
                .FindElements(By.XPath(string.Format("tbody/tr[*]/td[{0}]", columnNumber)))
                .Select(e => e.Text);
        }

        public int GetTableRowCount(ControlDefinition controlDefinition)
        {
            return FindElement(controlDefinition)
                .FindElements(By.XPath("tbody/tr"))
                .Count;
        }

        public void CloseAlert(bool shouldAccept)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                if (shouldAccept)
                {
                    alert.Accept();
                }
                else
                {
                    alert.Dismiss();
                }                
            }
            catch (NoAlertPresentException)
            {
                // Swallow any missing alert exception - if there is none, do nothing.
            }
        }

        public int GetNumberOfOptions(ControlDefinition controlDefinition)
        {
            return new SelectElement(this.FindElement(controlDefinition)).Options.Count;
        }

        public void WaitExplicitly(TimeSpan duration)
        {
            Thread.Sleep(duration);
        }

        public bool IsSelected(ControlDefinition controlDefinition)
        {
            var element = this.FindElement(controlDefinition.OrUnderlyingControl());            

            return element.Selected;
        }
        public bool IsSelectedButtons(ControlDefinition controlDefinition)
        {
            var element = this.FindElement(controlDefinition);
            return element.Selected;
        }

        public bool IsElementDisplay(ControlDefinition controlDefinition)
        {
            return FindElement(controlDefinition).Displayed;
        }

        public bool IsSelectableInput(ControlDefinition controlDefinition)
        {
            return this.IsRadio(controlDefinition) 
                || this.IsCheckbox(controlDefinition);
        }

        private bool IsCheckbox(ControlDefinition controlDefinition)
        {
            return controlDefinition.TagName.Equals("label", StringComparison.OrdinalIgnoreCase)
                   &&
                   this.driver.FindElement(this.ByControlDefinition(controlDefinition))
                       .GetAttribute("class")
                       .StartsWith("checkbox", StringComparison.OrdinalIgnoreCase);
        }

        private bool IsRadio(ControlDefinition controlDefinition)
        {
            return controlDefinition.TagName.Equals("label", StringComparison.OrdinalIgnoreCase)
                   &&
                   this.driver.FindElement(this.ByControlDefinition(controlDefinition))
                       .GetAttribute("class")
                       .StartsWith("radio", StringComparison.OrdinalIgnoreCase);
        }

        public void WaitForElementToDisplay()
        {
            Thread.Sleep(5000);
        }

        public void WaitForElement(ControlDefinition controlDefinition)
        {
            var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
            wait.Until(
                ExpectedConditions.ElementExists(ByControlDefinition(controlDefinition)));
        }

        public void WaitForElementToBeClickable(ControlDefinition ctrlDefinition)
        {
            try
            {
                var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
                wait.Until(
                    ExpectedConditions.ElementToBeClickable(ByControlDefinition(ctrlDefinition)));
            }
            catch (Exception)
            {
                if (!string.IsNullOrEmpty(ctrlDefinition.ImagePath))
                {

                }
                else {
                    throw;
                }                
            }
            
        }

        public void WaitForAjaxCallComplete(ControlDefinition ctrlDefinition)
        {
            var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
            wait.Until(x => (bool)(x as IJavaScriptExecutor).ExecuteScript("return jQuery.active == 0"));
            Thread.Sleep(1000);            
        }
   
        public void WaitForOptionToBeSelectable(ControlDefinition controlDefinition, string option)
        {
            var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
            var selector = this.FindElement(controlDefinition);
            
            wait.Until(ExpectedConditions.TextToBePresentInElement(selector, option));
        }

        public void WaitForIndexToBeSelectable(ControlDefinition controlDefinition)
        {
            var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
            var selector = this.FindElement(controlDefinition);
            wait.Until(ExpectedConditions.ElementToBeClickable(selector));
        }

        public void WaitForElementToBeSelectable(ControlDefinition ctrlDefinition)
        {
            var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
            wait.Until(
                ExpectedConditions.ElementToBeSelected(ByControlDefinition(ctrlDefinition)));
        }

        public void ClickUsingJavascript(ControlDefinition controlDefinition)
        {
            // TODO: Create a strategy pattern fix based on browser type            
            try
            {
                ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true);", FindElement(controlDefinition));
                FindElement(controlDefinition).Click();
            }
            catch
            {
                IWebElement element = FindElement(controlDefinition);
                var jsExecutor = (IJavaScriptExecutor)this.driver;
                jsExecutor.ExecuteScript("arguments[0].click();", element);
            }            
        }

        public void ClickElementAt(ControlDefinition controlDefinition, ClickPoint clickPoint)
        {
            int x, y;
            IWebElement element = FindElement(controlDefinition);
            switch (clickPoint)
            {
                case ClickPoint.Center:
                    x = element.Size.Width / 2;
                    y = element.Size.Height / 2;
                    break;
                case ClickPoint.Top:
                    x = element.Size.Width / 2;
                    y = element.Size.Height / 10;
                    break;
                case ClickPoint.Left:
                    x = element.Size.Width / 10;
                    y = element.Size.Height / 2;
                    break;
                case ClickPoint.Right:
                    x = element.Size.Width * 90 / 100;
                    y = element.Size.Height / 2;
                    break;
                case ClickPoint.Bottom:
                    x = element.Size.Width / 2;
                    y = element.Size.Height * 90 / 100;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("clickPoint");
            }

            var actions = new Actions(this.driver);
            actions
                .MoveToElement(element, x, y)
                .Click()
                .Perform();
        }

        public string GetRetentionKey(ControlDefinition controlDefinition)
        {
            //Todo : Temporary resolution to capture retention key
            this.driver.FindElement(By.XPath(@"//span[@class='icon-plus']")).Click();
            WaitForElementToVisible(By.XPath(@"//span[@data-bind='text: retentionKey']"));
            string reteintionKey = this.driver.FindElement(By.XPath(@"//span[@data-bind='text: retentionKey']")).Text;
            this.driver.FindElement(By.XPath(@"//span[@class='icon-plus']")).Click();
            return reteintionKey;
        }

        public string GetDebugInfo(ControlDefinition controlDefinition)
        {
            //Todo : Temporary resolution to capture retention key
            this.driver.FindElement(By.XPath(@"//span[@class='icon-plus']")).Click();
            WaitForElementToVisible(By.XPath(@"//span[@data-bind='text: retentionKey']"));
            string reteintionKey = this.driver.FindElement(By.ClassName("debug-info")).Text;
            this.driver.FindElement(By.XPath(@"//span[@class='icon-plus']")).Click();
            return reteintionKey;
        }

        public string GetAttributeValue(ControlDefinition controlDefinition, string attributeName)
        {
            IWebElement element = this.driver.FindElement(ByControlDefinition(controlDefinition));
            return element.GetAttribute(attributeName);
        }

        public IEnumerable<string> GetDropDownOptions(ControlDefinition controlDefinition)
        {
            IWebElement element = this.driver.FindElement(ByControlDefinition(controlDefinition));
            var select = new SelectElement(element);
            return select.Options.Select(o => o.Text);
        }

        public void SwitchToIFrame(ControlDefinition controlDefinition)
        {
            this.driver.SwitchTo().Frame(FindElement(controlDefinition));
        }

        public void SwitchToDefaultContent()
        {
            driver.SwitchTo().DefaultContent();
        }

        public void SwitchToLastWindow()
        {            
            driver.SwitchTo().Window(driver.WindowHandles.Last());
        }

        public void SwitchBrowser(string value)
        {            
            if(!value.IsNumeric())
            {
                var browserTabs = driver.WindowHandles;
                foreach (var tab in browserTabs)
                {
                    try
                    {
                        driver.SwitchTo().Window(tab);
                        if (driver.Title.Equals(value, StringComparison.OrdinalIgnoreCase))
                            return;
                    }
                    catch
                    {
                        /// sometimes window title throwing exception becuase of dependnt pages
                        browserTabs = driver.WindowHandles;
                        foreach (var window in browserTabs)
                        {
                            driver.SwitchTo().Window(window);
                            if (driver.Title.Equals(value, StringComparison.OrdinalIgnoreCase))
                                return;
                        }
                    }
                }
            }
            else
            {                
                driver.SwitchTo().Window(driver.WindowHandles[Convert.ToInt32(value)-1]);
            }            
        }
        
        public void SwitchToFirstWindow()
        {
            driver.SwitchTo().Window(driver.WindowHandles.First());
        }

        public void SwitchToAlert()
        {
            driver.SwitchTo().Alert();
        }

        public string GetTableCellValue(ControlDefinition controlDefinition, int row, int column)
        {
            var cell = GetTableCellElement(controlDefinition, row, column);
            return cell.Text;
        }

        private IWebElement GetTableCellElement(ControlDefinition controlDefinition, int row, int column)
        {
            var table = this.driver.FindElement(ByControlDefinition(controlDefinition));
            var tableBody = table.FindElement(By.TagName("tbody"));
            var cell = tableBody.FindElement(By.XPath(string.Format(".//tr[{0}]/td[{1}]", row, column)));
            return cell;
        }

        private IWebElement GetTableCellElement(ControlDefinition controlDefinition, int row, int column, string objectName)
        {
            var table = this.driver.FindElement(ByControlDefinition(controlDefinition));
            var tableBody = table.FindElement(By.TagName("tbody"));
            ////tr[1]/td[4]/*[2]
            //.//tr[{0}]/td[{1}]/*[.={2}]
            var cell = tableBody.FindElement(By.XPath(string.Format(".//tr[{0}]/td[{1}]/*[.='{2}']", row, column, objectName)));
            return cell;   
        }
        public void ClickTableCell(ControlDefinition controlDefinition, int row, int column, string objectName)
        {
            if (objectName == "Delete" || objectName == "Edit")
            {
                var cell = GetTableCellElement(controlDefinition, row, column, objectName);
                cell.Click();
            }
            else
            {
                var cell = GetTableCellElement(controlDefinition, row, column);
                cell.Click();
            }
        }


        public void ClickThis(string controlId)
        {
            throw new NotImplementedException();
        }

        private void WaitForElementToVisible(By elem)
        {
            var wait = new WebDriverWait(this.driver, this.testCaseConfiguration.WaitTime);
            wait.Until(
                ExpectedConditions.ElementIsVisible(elem));
        }

        private By ByControlDefinition(ControlDefinition controlDefinition)
        {
            By by;
            if (controlDefinition.TagName.IsNullOrEmpty())
            {
                controlDefinition.TagName = "*";
            }

            if (!string.IsNullOrWhiteSpace(controlDefinition.ControlId))
            {
                var searchTerm = controlDefinition.ControlId;

                if (searchTerm.EndsWith("*"))
                {
                    by = By.XPath(string.Format("//{0}[contains(@id, {1})]",
                        controlDefinition.TagName,
                        controlDefinition.ControlId.TrimEnd('*').ConcatWithSingleQuotes()));
                }
                else
                {
                    by = By.Id(searchTerm);
                }
            }
            else if (!string.IsNullOrWhiteSpace(controlDefinition.LabelFor))
            {
                var searchTerm = controlDefinition.LabelFor;

                if (searchTerm.EndsWith("*"))
                {
                    by = By.XPath(string.Format("//{0}[contains(@for, {1})]",
                        controlDefinition.TagName,
                        controlDefinition.LabelFor.TrimEnd('*').ConcatWithSingleQuotes()));
                }
                else
                {
                    by = By.XPath(
                        string.Format("//{1}[@for={0}]",
                            controlDefinition.LabelFor.ConcatWithSingleQuotes(), controlDefinition.TagName));
                }
            }
            else if (!string.IsNullOrWhiteSpace(controlDefinition.Xpath))
            {
                by = By.XPath(controlDefinition.Xpath);
            }
            else if (!string.IsNullOrWhiteSpace(controlDefinition.ImagePath))
            {
                by = null;
            }
            else if (!string.IsNullOrWhiteSpace(controlDefinition.InnerText))
            {
                string searchFormat = controlDefinition.InnerText.EndsWith("*")
                    ? "//{0}[contains(text(), {1})]"
                    : "//{0}[text()={1}]";

                string innerText = controlDefinition.InnerText.TrimEnd('*');
                by = By.XPath(
                    string.Format(searchFormat, controlDefinition.TagName, innerText.ConcatWithSingleQuotes()));
            }

            else
            {
                throw new NoSuchElementException("Not found element on UI Page");
            }

            return by;
        }

        private IWebElement FindElement(ControlDefinition controlDefinition)
        {
            return this.driver.FindElement(ByControlDefinition(controlDefinition));
        }
    }
}