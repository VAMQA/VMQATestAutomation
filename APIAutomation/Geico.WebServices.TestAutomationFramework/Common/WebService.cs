using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Data;
using System.Net;

namespace VM.WebServices.TestAutomationFramework.Common
{
    public class WebService : FWLogger
    {
        HttpClient httpClient { get; set; }
        public WebService(DataTable dtenvironments, string strFilter)
        {
            string UserName = dtenvironments.Select(strFilter)[0].ItemArray[2].ToString();
            string Password= dtenvironments.Select(strFilter)[0].ItemArray[3].ToString();
            string appid = dtenvironments.Select(strFilter)[0].ItemArray[4].ToString();
            string token = dtenvironments.Select(strFilter)[0].ItemArray[5].ToString();

            httpClient = new HttpClient();
            if (UserName != "NA" && Password!= "NA")
            {
                System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
                byte[] byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", UserName, Password));
                string encodedCredentials = Convert.ToBase64String(byteArray);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
                //httpClient.DefaultRequestHeaders.Add("x-token", "Basic " + encodedCredentials);

                httpClient.DefaultRequestHeaders.Add("x-appid", appid.ToString());
                httpClient.DefaultRequestHeaders.Add("x-token", token.ToString());
            }
        }

        //public WebService(string UserName = "", String Password = "")
        //{
        //    System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        //    httpClient = new HttpClient();
        //    ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
        //    if (UserName == "")
        //        UserName = ConfigurationManager.AppSettings["SDCUserName"];
        //    if (Password == "")
        //        Password = ConfigurationManager.AppSettings["SDCPassword"];
        //    byte[] byteArray = Encoding.ASCII.GetBytes(string.Format("{0}:{1}", UserName, Password));
        //    string encodedCredentials = Convert.ToBase64String(byteArray);
        //    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", encodedCredentials);
        //    //httpClient.DefaultRequestHeaders.Add("x-token", "Basic " + encodedCredentials);

        //    httpClient.DefaultRequestHeaders.Add("x-appid", "CDD8BF1B-40B7-4745-8AF9-3EF7EF0938C0");
        //   httpClient.DefaultRequestHeaders.Add("x-token", "Basic Z2VpY29cc3J2LUJJTC1BVVRPVEVTVC1OUDpjclVxZWtlUDQhcHVnK21h");
        //}

        public string MakeServiceCall(HttpMethod httpMethod, string targetURI, string inputXML, out bool Success, string requestMessageContentType = Constants.sDefaultContentType)
        {

            string response = null;
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.RequestUri = new Uri(targetURI);
                requestMessage.Method = httpMethod;
                string[] currStepActions = requestMessageContentType.Split(':');

                if (requestMessage.Method.ToString() == "POST")
                {
                    if (currStepActions.Length > 1)
                    {
                        requestMessage.Content = new StringContent(Convert.ToString(inputXML), Encoding.UTF8, currStepActions[1].ToString());
                    }
                    else
                    {
                        requestMessage.Content = new StringContent(Convert.ToString(inputXML), Encoding.UTF8, requestMessageContentType);
                    }

                    //requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    httpClient.Timeout = new TimeSpan(0, 0, 500);

                    httpResponse = httpClient.PostAsync(requestMessage.RequestUri, requestMessage.Content).Result;

                    response = httpResponse.Content.ReadAsStringAsync().Result;
                }
                else if (requestMessage.Method.ToString() == "GET")
                {
                    requestMessage.Headers.Add("Accept", "application/json");
                    if (currStepActions.Length > 1)
                    {
                        if (!currStepActions[0].ToString().StartsWith("Content-Type"))
                        {
                            string sContentHeaderValue = string.Empty;
                            sContentHeaderValue = currStepActions[1].ToString();
                            if (currStepActions[1].ToString().StartsWith("~"))
                            {
                                sContentHeaderValue = Library.Keywords[currStepActions[1].ToString()];
                            }

                            requestMessage.Headers.Add(currStepActions[0].ToString(), sContentHeaderValue);
                        }
                    }
                    else
                    {
                        // requestMessage.Content = new StringContent(Convert.ToString(inputXML), Encoding.UTF8, requestMessageContentType);
                    }
                    httpClient.Timeout = new TimeSpan(0, 0, 1500);
                    httpResponse = httpClient.SendAsync(requestMessage).Result;
                    response = httpResponse.Content.ReadAsStringAsync().Result;
                }

                if (httpResponse.IsSuccessStatusCode)
                    Success = true;
                else
                    Success = false;
                return response;

            }
            finally
            {
                httpClient.Dispose();
            }
        }

        public string MakeServiceCall(HttpMethod httpMethod, string targetURI, string inputXML, DataTable dtCustHeader, string TestCaseID, out bool Success, string requestMessageContentType = Constants.sDefaultContentType)
        {

            string response = null;
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            try
            {
                HttpRequestMessage requestMessage = new HttpRequestMessage();
                requestMessage.Version = System.Net.HttpVersion.Version10;
                requestMessage.RequestUri = new Uri(targetURI);
                requestMessage.Method = httpMethod;
                if (requestMessage.Method.ToString() == "POST")
                {
                    string[] currStepActions = requestMessageContentType.Split(':');
                    if (currStepActions.Length > 1)
                    {
                        string strcontentType = currStepActions[1].ToString();
                        if (strcontentType == "custom")
                        {
                            string Filter = string.Format("([{0}]='{1}')", Constants.TESTCASE, TestCaseID);
                            DataRow[] FilteredTable = dtCustHeader.Select(Filter);                            

                            foreach (DataRow CurrentRow in FilteredTable)
                            {
                                 if ((CurrentRow[1].ToString().ToLower() =="content-type") &&  (CurrentRow[1].ToString() != "" && CurrentRow[2].ToString() != ""))
                                 {
                                     requestMessage.Content = new StringContent(Convert.ToString(inputXML), Encoding.UTF8, CurrentRow[2].ToString());
                                 }
                                 else if (CurrentRow[1].ToString() != "" && CurrentRow[2].ToString() != "")
                                 {
                                     requestMessage.Headers.Add(CurrentRow[1].ToString(), (CurrentRow[2].ToString().StartsWith("~") ? Library.Keywords[CurrentRow[2].ToString()] : CurrentRow[2].ToString()));
                                 }                                                               
                            }
                        }
                        else
                        {
                            requestMessage.Content = new StringContent(Convert.ToString(inputXML), Encoding.UTF8, currStepActions[1].ToString());
                        }
                    }
                    else
                    {
                        requestMessage.Content = new StringContent(Convert.ToString(inputXML), Encoding.UTF8, requestMessageContentType);
                    }

                    //requestMessage.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

                    httpClient.Timeout = new TimeSpan(0, 0, 500);

                    httpResponse = httpClient.PostAsync(requestMessage.RequestUri, requestMessage.Content).Result;
                    System.Threading.Thread.Sleep(3000);

                     response = httpResponse.Content.ReadAsStringAsync().Result;
                }
                else if (requestMessage.Method.ToString() == "GET")
                {
                    requestMessage.Headers.Add("Accept", "application/json");
                    httpResponse = httpClient.SendAsync(requestMessage).Result;
                    response = httpResponse.Content.ReadAsStringAsync().Result;
                }
                if (httpResponse.IsSuccessStatusCode)
                    Success = true;
                else
                    Success = false;
                return response;

            }
            finally
            {
                httpClient.Dispose();
            }
        }
    }
}
