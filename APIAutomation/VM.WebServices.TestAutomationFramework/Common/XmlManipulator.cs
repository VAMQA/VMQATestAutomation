using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.Data;

namespace VM.WebServices.TestAutomationFramework
{
    class XmlManipulator
    {
        public static XmlNode ReadSingleNodeByXPath(string xmlFile, string strXPath, XmlNamespaceManager nmSpaceMgr)
        {
            XmlDocument xmlDoc = null;
            XmlNode singleNode = null;
            try
            {
                xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlFile);
                singleNode = ReadSingleNodeByXPath(xmlDoc, strXPath, nmSpaceMgr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xmlDoc = null;
            }
            return singleNode;
        }
        public static XmlNode ReadSingleNodeByXPath(XmlDocument xmlDoc, string strXPath, XmlNamespaceManager nmSpaceMgr)
        {
            XmlNode singleNode = null;
            XmlElement root = xmlDoc.DocumentElement;
            if (nmSpaceMgr != null)
            {
                strXPath = SetNameSpace(strXPath, nmSpaceMgr);
                singleNode = root.SelectSingleNode(strXPath, nmSpaceMgr);
            }
            else
            {
                singleNode = root.SelectSingleNode(strXPath);
            }
            return singleNode;
        }
        public static XmlNodeList GetNodesByXpath(XmlDocument xmlDoc, string strXPath, XmlNamespaceManager xmlNameSpaceMgr)
        {
            XmlNodeList nodeList = null;
            XmlElement root = xmlDoc.DocumentElement;
            string strXMLQuery = xmlNameSpaceMgr != null ? SetNameSpace(strXPath, xmlNameSpaceMgr) : strXPath;
            nodeList = root.SelectNodes(strXMLQuery, xmlNameSpaceMgr);
            return nodeList;
        }
        public static string SetNameSpace(string strXpath, XmlNamespaceManager xmlNameSpaceMgr)
        {
            string transFormedXpath = string.Empty;

            string[] strTokens = strXpath.Split('/');
            foreach (string strNode in strTokens)
            {
                if (strNode != string.Empty)
                {
                    string sTransFormedNode = SetDefaultNameSpace(strNode, xmlNameSpaceMgr);
                    if (sTransFormedNode != string.Empty)
                    {
                        transFormedXpath = transFormedXpath + "/" + SetDefaultNameSpace(strNode, xmlNameSpaceMgr);
                    }
                }
            }
            return transFormedXpath;
        }
        public static string SetDefaultNameSpace(string strNode, XmlNamespaceManager xmlNameSpaceMgr)
        {
            string transFormedNode = string.Empty;
            if (!strNode.Contains(":"))
            {
                transFormedNode = xmlNameSpaceMgr.LookupNamespace("Default") != null ? "Default:" + strNode : strNode;
            }
            else
            {
                transFormedNode = strNode;
            }
            return transFormedNode;
        }
        public static XmlNamespaceManager GetNameSpaceManager(Hashtable hashNmSpaceMgr, XmlDocument xmlDoc)
        {
            XmlNamespaceManager xmlNameSpaceMgr = new XmlNamespaceManager(xmlDoc.NameTable);
            foreach (string str in hashNmSpaceMgr.Keys)
                xmlNameSpaceMgr.AddNamespace(str, hashNmSpaceMgr[str].ToString());
            return xmlNameSpaceMgr;
        }
    }
}
