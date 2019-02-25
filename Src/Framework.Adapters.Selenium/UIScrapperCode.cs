using OpenQA.Selenium;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using HtmlAgilityPack;
using VM.Platform.TestAutomationFramework.Core.Contracts;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Internal;
using System.Xml;

namespace VM.Platform.TestAutomationFramework.Adapters.Selenium
{
    public class UIScrapperCode
    {
        public static IWebDriver driver = null;
        public IWebElement currentElement = null;
        public By globBy = null;
        //private readonly IUiAdapter uiAdapter;

        //public UIScrapperCode(IUiAdapter uiAdapter)
        //    : base()
        //{
        //    this.uiAdapter = uiAdapter;

        //}
        public void navigateToUrl(string browser, string url)
        {
            try
            {
                if (browser.ToLower().Equals("ie"))
                {
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

                    driver = new InternetExplorerDriver(internetExplorerOptions);
                }
                else if (browser.ToLower().Equals("chrome"))
                {
                    var chromeoptions = new ChromeOptions();
                    chromeoptions.AddArguments("disable-infobars");
                    chromeoptions.AddArguments("disable-impl-side-painting");
                    ChromeDriverService chromeservice = ChromeDriverService.CreateDefaultService();
                    chromeservice.HideCommandPromptWindow = true;
                    driver = new ChromeDriver(chromeservice, chromeoptions, TimeSpan.FromSeconds(120));
                }

                Thread.Sleep(3000);
                // driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                driver.Manage().Window.Maximize();
                driver.Navigate().GoToUrl(url);
            }
            catch (Exception e)
            {
                MessageBox.Show("Unexpected error while lauching browser...", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        public Dictionary<int, string> getElements(string desiredXpath)
        {
            Dictionary<int, string> oVar = new Dictionary<int, string>();
            oVar.Clear();
            int counter = 0;
            string pageSource = driver.PageSource.ToString();

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            HtmlNodeCollection nodes;
            IList<IWebElement> frames = null;

            //elements inside page
            doc.LoadHtml(pageSource);
            nodes = doc.DocumentNode.SelectNodes(desiredXpath);
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    var id = node.Id;
                    counter++;
                    oVar.Add(counter, getNodeProperties(node) + "");

                }

            }
            string framename;
            if (driver.FindElements(By.XPath("//iframe | //frame")).Count > 0)
            {
                //Elements inside frames
                frames = driver.FindElements(By.XPath("//iframe | //frame"));


                foreach (IWebElement frame in frames)
                {
                    framename = frame.GetAttribute("id").ToString();
                    driver.SwitchTo().Frame(frame.GetAttribute("id").ToString());
                    string frameSource = driver.PageSource.ToString();
                    doc.LoadHtml(frameSource);
                    nodes = doc.DocumentNode.SelectNodes(desiredXpath);
                    if (nodes != null)
                    {
                        foreach (var node in nodes)
                        {
                            var id = node.Id;
                            counter++;
                            oVar.Add(counter, getNodeProperties(node) + framename);

                        }
                    }

                    if (driver.FindElements(By.XPath("//iframe | //frame")).Count > 0)
                    {

                        frames = driver.FindElements(By.XPath("//iframe | //frame"));

                        string framename1;
                        foreach (IWebElement frame1 in frames)
                        {
                            framename1 = frame1.GetAttribute("id").ToString();
                            driver.SwitchTo().Frame(frame1.GetAttribute("id").ToString());
                            framename1 = framename + ";" + framename1;
                            string frameSource1 = driver.PageSource.ToString();
                            doc.LoadHtml(frameSource1);
                            nodes = doc.DocumentNode.SelectNodes(desiredXpath);
                            if (nodes != null)
                            {
                                foreach (var node in nodes)
                                {
                                    var id = node.Id;
                                    counter++;
                                    oVar.Add(counter, getNodeProperties(node) + framename1);

                                }

                            }
                            driver.SwitchTo().DefaultContent();
                            driver.SwitchTo().Frame(framename);

                        }
                    }
                    driver.SwitchTo().DefaultContent();
                }
            }
            driver.SwitchTo().DefaultContent();
            return oVar;


        }

        public string getNodeProperties(HtmlNode node)
        {
            string nodeProperties = "";
            string relXPath = "";
            try
            {
                nodeProperties += node.Name + "~";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.Id + "~";
                if (!node.Id.Equals(""))
                    relXPath += "//" + node.Name + "[@id = '" + node.Id + "']";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.Attributes["name"].Value + "~";
                if (!node.Attributes["name"].Value.Equals("") && relXPath.Equals(""))
                    relXPath = "//" + node.Name + "[@name = '" + node.Attributes["name"].Value + "']";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.Attributes["label"].Value + "~";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.XPath + "~";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.Attributes["href"].Value + "~";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.Attributes["class"].Value + "~";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                nodeProperties += node.InnerText.Trim() + "~";
                if (!node.InnerText.Trim().Equals("") && relXPath.Equals(""))
                    relXPath = "//" + node.Name + "[. = '" + node.InnerText.Trim() + "']";
            }
            catch
            {
                nodeProperties += "~";
            }
            try
            {
                //generate relative xpath

                nodeProperties += relXPath + "~";
            }
            catch
            {
                nodeProperties += "~";
            }

            return nodeProperties;
        }

        public bool highlightElement(string frame_name, string selectBy, string property)
        {
            try
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                driver.SwitchTo().DefaultContent();
                if (frame_name != "")
                {
                    string[] frame_name1 = frame_name.Split(';');
                    for (int i = 0; i <= frame_name1.Count() - 1; i++)
                    {
                        driver.SwitchTo().Frame(frame_name1[i]);
                    }
                }
                switch (selectBy.ToLower())
                {
                    case "id":
                        globBy = By.Id(property);
                        //currentElement = driver.FindElement(By.Id(property));
                        break;
                    case "absolute xpath":
                        globBy = By.XPath(property);
                        // currentElement = driver.FindElement(By.XPath(property));
                        break;
                    case "name":
                        globBy = By.Name(property);
                        //currentElement = driver.FindElement(By.Name(property));
                        break;
                    case "custom_xpath":
                        globBy = By.XPath(property);
                        ////currentElement = driver.FindElement(By.XPath(property));
                        break;
                    case "href":
                        globBy = By.XPath("//*[@href = '" + property + "']");
                        //currentElement = driver.FindElement(By.XPath("//*[@href = '"+ property + "']"));
                        break;
                    case "classname":
                        globBy = By.XPath("//*[@class = '" + property + "']");
                        //currentElement = driver.FindElement(By.XPath("//*[@class = '" + property + "']"));
                        break;
                    case "text":

                        globBy = By.XPath("//*[. = '" + property + "']");
                        //currentElement = driver.FindElement(By.XPath("//*[. = '" + property + "']"));
                        break;
                    case "relative xpath":
                        globBy = By.XPath(property);
                        //currentElement = driver.FindElement(By.XPath(property));
                        break;
                }
                if (WaitForElement(globBy))
                {
                    currentElement = driver.FindElement(globBy);

                    var javaScriptDriver = (IJavaScriptExecutor)driver;
                    string highlightJavascript = @"arguments[0].style.cssText = ""border-width: 4px; border-style: solid; border-color: red"";";
                    string clearJavaScript = @"arguments[0].style.cssText = ""border-width: 0px; border-style: solid; border-color: green""; ";
                    javaScriptDriver.ExecuteScript("window.focus();");
                    javaScriptDriver.ExecuteScript("arguments[0].scrollIntoView(true);", currentElement);
                    javaScriptDriver.ExecuteScript(highlightJavascript, new object[] { currentElement });
                    Thread.Sleep(4000);
                    javaScriptDriver.ExecuteScript(clearJavaScript, new object[] { currentElement });
                    driver.SwitchTo().DefaultContent();
                    return true;
                }
                else
                {
                    driver.SwitchTo().DefaultContent();
                    //  MessageBox.Show("Could not highlight specified element.", "Highlight Element", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    return false;
                }
            }

            catch (Exception e)
            {
                driver.SwitchTo().DefaultContent();
                //MessageBox.Show("Could not highlight specified element.", "Highlight Element", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                return false;
            }
        }
        public bool verifyElement(string frame_name, string selectBy, string property)
        {
            try
            {
                driver.SwitchTo().Window(driver.WindowHandles.Last());
                driver.SwitchTo().DefaultContent();
                if (frame_name != "")
                {
                    string[] frame_name1 = frame_name.Split(';');
                    for (int i = 0; i <= frame_name1.Count() - 1; i++)
                    {
                        driver.SwitchTo().Frame(frame_name1[i]);
                    }
                }

                switch (selectBy.ToLower())
                {
                    case "id":
                        globBy = By.Id(property);
                        //currentElement = driver.FindElement(By.Id(property));
                        break;
                    case "xpath":
                        globBy = By.XPath(property);
                        //currentElement = driver.FindElement(By.XPath(property));
                        break;
                }
                if (WaitForElement(globBy))
                {
                    currentElement = driver.FindElement(globBy);
                    driver.SwitchTo().DefaultContent();
                    return true;
                }
                else
                {
                    driver.SwitchTo().DefaultContent();
                    return false;

                }
            }

            catch (Exception e)
            {
                driver.SwitchTo().DefaultContent();
                return false;

            }
        }
        public bool WaitForElement(By by)
        {
            try
            {
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(2));
                wait.IgnoreExceptionTypes(typeof(WebDriverTimeoutException));
                wait.Until(
                ExpectedConditions.ElementExists(by));
                return true;
            }
            catch (WebDriverTimeoutException e)
            {
                return false;
            }
            catch (NullReferenceException e)
            {
                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }


    }
}
