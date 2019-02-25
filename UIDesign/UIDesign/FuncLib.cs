using VM.Platform.TestAutomationFramework.Core;
using Microsoft.TeamFoundation.TestManagement.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.XPath;

namespace UIDesign
{
    public class FuncLib
    {
        public string dbConnectionString;
        public string testresultsPath;
        Dictionary<int, string> keywords = new Dictionary<int, string>();
        Dictionary<int, string> repositiory = new Dictionary<int, string>();
        int testcaseId;
        SqlDataAdapter da = new SqlDataAdapter();
        Dictionary<int, string> pages = new Dictionary<int, string>();
        public FuncLib()
        {
            var configuration = XDocument.Load(@"Config\Config.xml");
            dbConnectionString = GetDbConnection(configuration);
            testresultsPath = GetTestResultsPath(configuration);
        }
        private string GetDbConnection(XDocument xDoc)
        {
            return xDoc.XPathSelectElement("/TestAutomationFramework/DBExecution/DBConnectionString").Value;
        }
        private string GetTestResultsPath(XDocument xDoc)
        {
            return xDoc.XPathSelectElement("/TestAutomationFramework/DBExecution/TestResultsPath").Value;
        }
        public bool IsNullOrEmpty(string input)
        {
            return string.IsNullOrEmpty(input);
        }
        public bool VerifyUserLogin(string userId)
        {
            return string.Equals(System.Environment.UserName, userId, StringComparison.OrdinalIgnoreCase);
        }
        public bool IsEqual(string string1,string string2)
        {
            return string.Equals(string1, string2, StringComparison.OrdinalIgnoreCase);
        }
        public DataTable binddataTable(string query)
        {
            DataTable dtable = new DataTable();
            try
            {
                //string[] dataspt = query.Split(new[] { "FROM" }, StringSplitOptions.None);
                //string sTable = dataspt[1].Split(' ')[1].ToUpper();
                da = new SqlDataAdapter(query, dbConnectionString);
                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(da);
                dtable.Locale = System.Globalization.CultureInfo.InvariantCulture;
                da.Fill(dtable);
                switch (HomePage.SName)
                {
                    case "MasterOr":
                        if (dtable.Columns.Contains("PageID")) dtable.Columns.Remove("PageID");
                        if (dtable.Columns.Contains("ProjectID")) dtable.Columns.Remove("ProjectID");
                        break;
                    case "ActionFlow":
                       // ReplacePageID(dtable);
                        break;
                    case "TestData":
                        if (dtable.Columns.Contains("PageID")) dtable.Columns.Remove("PageID");
                        if (dtable.Columns.Contains("MasterORID")) dtable.Columns.Remove("MasterORID");
                        //ReplacePageID(dtable);
                        //ReplaceIndicator(dtable);
                        //ReplaceMasterORID(dtable);
                        break;
                    case "PageName":
                        if (dtable.Columns.Contains("PageID")) dtable.Columns.Remove("PageID");
                        //ReplaceUIDs(dtable);
                        break;
                    case "TestResult":
                       // ReplaceUserID(dtable);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }            
            return dtable;
        }
        public void ReplacePageID(DataTable dt)
        {
            Dictionary<string, string> dictpagetitles = new Dictionary<string, string>();
            dictpagetitles = GetPageTitles();
            dt.Columns.Add("PageName");
            dt.Columns["PageName"].SetOrdinal(2);
            foreach (DataRow dr in dt.Rows)
                dr["PageName"] = dictpagetitles[dr["PageID"].ToString()];
            dt.Columns.Remove("PageID");
        }
        public void ReplaceIndicator(DataTable dt)
        {
            Dictionary<string, string> dictkeywords = new Dictionary<string, string>();
            dictkeywords = GetKeywords();
            dt.Columns.Add("Keyword");
            dt.Columns["Keyword"].SetOrdinal(6);
            dt.Columns["MasterORID"].SetOrdinal(5);
            //dt.Columns["TestCaseId"].SetOrdinal(1);
            foreach (DataRow dr in dt.Rows)
                dr["Keyword"] = dictkeywords[dr["Indicator"].ToString()];
            dt.Columns.Remove("Indicator");
        }
        public void ReplaceMasterORID(DataTable dt)
        {
            Dictionary<string, string> dictorlabels = new Dictionary<string, string>();
            dictorlabels = GetORLables();
            dt.Columns.Add("Label");
            dt.Columns["Label"].SetOrdinal(5);
            foreach (DataRow dr in dt.Rows)
                dr["Label"] = dictorlabels[dr["MasterORID"].ToString()];
            dt.Columns.Remove("MasterORID");
        }
        public void ReplaceUserID(DataTable dt)
        {
            Dictionary<string, string> dictusers = new Dictionary<string, string>();
            dictusers = GetUserInfo();
            foreach (DataRow dr in dt.Rows)
                //dr["RunBy"] = GetUsers[dr["RunBy"].ToString().ToUpper()];
                dr["RunBy"] = dictusers[dr["RunBy"].ToString().ToUpper()];
        }
        public void ReplaceUIDs(DataTable dt)
        {
            Dictionary<string, string> dictusers = new Dictionary<string, string>();
            dictusers = GetUserInfo();
            foreach (DataRow dr in dt.Rows)
            {             
                dr["CreatedBy"] = string.IsNullOrEmpty(dr["CreatedBy"].ToString())?"": dictusers[dr["CreatedBy"].ToString().ToUpper()];
                dr["UpdatedBy"] = string.IsNullOrEmpty(dr["ModifiedBy"].ToString()) ? "" : dictusers[dr["UpdatedBy"].ToString().ToUpper()];
            }                
        }
        public void RunQuery(string query)
        {
            try
            {
                using (SqlConnection objConnection = new SqlConnection(dbConnectionString))
                {
                    objConnection.Open();
                    using (SqlCommand objCommand = new SqlCommand(query, objConnection))
                    {
                        objCommand.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }            
        }
        public int ExecuteStoredProcedure(string xmlDoc, string spName)
        {
            int result = 0;
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(spName))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Connection = con;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.AddWithValue("@inputXML", xmlDoc.ToString());
                    con.Open();
                    result = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            return result;
        }
        public int ExecuteQuery(string query)
        {
            using (SqlConnection objConnection = new SqlConnection(dbConnectionString))
            {
                objConnection.Open();
                using (SqlCommand objCommand = new SqlCommand(query, objConnection))
                {
                    return (objCommand.ExecuteNonQuery());
                }
            }
        }
        public Dictionary<string, string> GetAdminUsers()
        {
            Dictionary<string, string> dictAdminUsers = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT UserID,IsAdmin FROM USERS WHERE PROJECTID=" + SignIn.projectId + " ORDER BY USERID", con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        dictAdminUsers.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                }
            }
            return dictAdminUsers;
        }
        public Dictionary<string,string> GetLabels()
        {
            Dictionary<string, string> dictAllLabels = new Dictionary<string, string>();
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT distinct PageID,Label FROM MasterOr WHERE PROJECTID=" + SignIn.projectId + " ORDER BY PageId", con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        dictAllLabels.Add(reader.GetValue(0).ToString() + "_" + reader.GetValue(1).ToString(), reader.GetValue(1).ToString());
                }
            }
            return dictAllLabels;
        }
        public object ExecuteScalar(string strQuery)
        {
            using (SqlConnection objConnection = new SqlConnection(dbConnectionString))
            {
                objConnection.Open();
                using (SqlCommand objCommand = new SqlCommand(strQuery, objConnection))
                {
                    return (objCommand.ExecuteScalar());
                }
            }
        }
        public Dictionary<string, string> GetUserInfo()
        {
            Dictionary<string, string> dictUsers = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT USERID,USERNAME FROM USERS WHERE PROJECTID=" + SignIn.projectId + " ORDER BY USERID ", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictUsers.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictUsers;
        }
        public Dictionary<string, string> GetFeatures()
        {
            Dictionary<string, string> dictFeatures = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT FeatureID,NAME FROM FEATURES WHERE PROJECTID=" + SignIn.projectId + " ORDER BY FeatureID ", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictFeatures.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictFeatures;
        }
        public Dictionary<string, string> GetFavorites()
        {
            Dictionary<string, string> dictFavorites = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT FAVOURITEID,FAVOURITENAME FROM FAVOURITES WHERE PROJECTID=" + SignIn.projectId + " And USERID = '" + GetUserInfo()[SignIn.userId.ToUpper()].ToString() + "'", con))
                    //using (SqlCommand command = new SqlCommand("SELECT DISTINCT FAVOURITEID,FAVOURITENAME FROM FAVOURITES WHERE PROJECTID=" + SignIn.projectId, con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictFavorites.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictFavorites;
        }
        public Dictionary<string,string> GetProjInfo()
        {
            Dictionary<string, string> dictProjects = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT PROJECTID,PROJECTNAME FROM PROJECTINFO", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictProjects.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }                
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR : "+e.Message);
            }
            return dictProjects;
        }
        public Dictionary<string, string> GetProjMaps(string projectName)
        {
            Dictionary<string, string> dictProjMapping = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT U.USERID,P.PROJECTID FROM USERS U JOIN PROJECTINFO P ON U.PROJECTID=P.PROJECTID AND P.PROJECTNAME='" + projectName + "'", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictProjMapping.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictProjMapping;
        }
        public Dictionary<string, string> GetPageTitles()
        {
            Dictionary<string, string> dictpageTitles = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT PageID,PageName FROM PAGENAMES WHERE PROJECTID=" + SignIn.projectId + " ORDER BY PAGEID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictpageTitles.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictpageTitles;
        }
        public Dictionary<string, string> GetPageNames(string TestcateId)
        {
            Dictionary<string, string> dictpageTitles = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    string sqlquery = "select distinct PageName, pg.pageId  from PageNames pg inner join TestData td on pg.PageID = td.PageID where td.TestCaseId =" + TestcateId;
                    con.Open();
                    using (SqlCommand command = new SqlCommand(sqlquery, con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictpageTitles.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictpageTitles;
        }
        private void getpages()
        {
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT PageID,PageName FROM PAGENAMES WHERE PROJECTID=" + SignIn.projectId + " ORDER BY PAGENAME", con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        pages.Add(Int32.Parse(reader.GetValue(0).ToString()), reader.GetValue(1).ToString().ToLower());
                }
            }
        }
        public string ToCamelCase(string input)
        {
            string[] words = input.ToLower().Split(' ');
            StringBuilder sb = new StringBuilder();
            foreach (string s in words)
            {
                string firstLetter = s.Substring(0, 1);
                string rest = s.Substring(1, s.Length-1);
                sb.Append(firstLetter.ToUpper() + rest);
                sb.Append(" ");
            }
            return sb.ToString().Substring(0, sb.ToString().Length-1);
        }
        public Dictionary<string, string> GetKeywords()
        {
            Dictionary<string, string> dictkeyWords = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT ActionKeyword_ID,ActionName FROM ActionKeyword ORDER BY ActionKeyword_ID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictkeyWords.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictkeyWords;            
        }
        public Dictionary<string, string> GetTags(string query)
        {
            Dictionary<string, string> dicttags = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dicttags.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dicttags;
        }
        public Dictionary<string, string> GetORLables()
        {
            Dictionary<string, string> dictORLabels = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT MasterORID,Label FROM MasterOR WHERE PROJECTID=" + SignIn.projectId + " ORDER BY MasterORID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictORLabels.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictORLabels;
        }
        public Dictionary<string, string> GetFunctionalities()
        {
            Dictionary<string, string> dictFunctionalities = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT TestCaseID,Functionality FROM TestCaseInfo WHERE PROJECTID=" + SignIn.projectId + "AND ISDELETED IS NULL ORDER BY TestCaseID ", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictFunctionalities.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictFunctionalities;
        }
        public Dictionary<string, string> GetRepoVersion()
        {
            Dictionary<string, string> dictRepositiories = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT distinct [Version] FROM MasterOR WHERE PROJECTID=" + SignIn.projectId + " ORDER BY [Version] ", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictRepositiories.Add(reader.GetValue(0).ToString(), reader.GetValue(0).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictRepositiories;
        }
        public Dictionary<string, string> GetReleases()
        {
            Dictionary<string, string> dictReleases = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT TestCaseID,Release FROM TestCaseInfo WHERE PROJECTID=" + SignIn.projectId + "AND ISDELETED IS NULL ORDER BY TestCaseID ", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictReleases.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictReleases;
        }
        public Dictionary<string, string> GetTestCaseTitles()
        {
            Dictionary<string, string> dictTitles = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT TestCaseID,TestCaseTitle FROM TestCaseInfo WHERE PROJECTID=" + SignIn.projectId + " ORDER BY TestCaseID ", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictTitles.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictTitles;
        }
        public Dictionary<string, string> GetORLables(int pageId)
        {
            Dictionary<string, string> dictPageLevelLabels = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT MasterORID,Label FROM MasterOR WHERE PROJECTID=" + SignIn.projectId +" AND PageID=" + pageId + " ORDER BY MasterORID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictPageLevelLabels.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dictPageLevelLabels;
        }
        public Dictionary<string, string> GetTestCaseIDs()
        {
            Dictionary<string, string> dicttcIds = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT TESTCASEID,FUNCTIONALITY FROM TESTCASEINFO WHERE PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL ORDER BY TESTCASEID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dicttcIds.Add(reader.GetValue(0).ToString(), reader.GetValue(1).ToString());
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dicttcIds;
        }
        public Dictionary<string, string> GetTestCaseIDsAndTitles()
        {
            Dictionary<string, string> dicttcIds = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT TESTCASEID,TESTCASETITLE,FUNCTIONALITY FROM TESTCASEINFO WHERE PROJECTID=" + SignIn.projectId + " AND ISDELETED IS NULL ORDER BY TESTCASEID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dicttcIds.Add(reader.GetValue(0).ToString()+" : "+reader.GetValue(1).ToString(), reader.GetValue(2).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dicttcIds;
        }
        public Dictionary<string, string> GetTestDataIDs()
        {
            Dictionary<string, string> dicttdIds = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT TESTCASEID FROM TESTDATA WHERE PROJECTID = " + SignIn.projectId + " ORDER BY TESTCASEID", con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dicttdIds.Add(reader.GetValue(0).ToString(), reader.GetValue(0).ToString());
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }
            return dicttdIds;
        }
        public Dictionary<string, string> GetTestCategories()
        {
            Dictionary<string, string> dictTestCategories = new Dictionary<string, string>();
            try
            {
                using (SqlConnection con = new SqlConnection(dbConnectionString))
                {
                    con.Open();
                    using (SqlCommand command = new SqlCommand("SELECT DISTINCT TestCategory FROM TestCaseInfo WHERE PROJECTID=" + SignIn.projectId, con))
                    {
                        SqlDataReader reader = command.ExecuteReader();
                        while (reader.Read())
                            dictTestCategories.Add(reader.GetValue(0).ToString(), reader.GetValue(0).ToString().ToUpper());
                    }
                }
            }
            catch(Exception e)
            {
                MessageBox.Show("ERROR : " + e.Message);
            }            
            return dictTestCategories;
        }
        public void InsertMasterOR(Dictionary<string,Dictionary<string,Dictionary<string,string>>> pagerepo)
        {
            repositiory.Clear();
            getrepositiory();
            //====================================INSERT INTO MASTEROR===================================
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                {
                    foreach (var page in pagerepo)
                    {
                        foreach (var item in page.Value)
                        {
                            if (!repositiory.ContainsValue((pages.FirstOrDefault(x => x.Value == page.Key.ToString().ToLower()).Key) + "_" + (item.Value["label"].ToString().ToLower())))
                            {
                                //string insertcommand = "INSERT INTO MasterOR(PAGEID,LABEL,TAGINSTANCE,CONTROLDEFINITION,CLASSNAME,VALUEATTRIBUTE,CONTROLTYPE,TAGNAME,PARENTCONTROL,CONTROLID,CLASS,TYPE,LABELFOR,XPATH,FRIENDLYNAME,INNERTEXT,PROJECTID) VALUES(" +pages.FirstOrDefault(x => x.Value == page.Key.ToString().ToLower()).Key + ",'" +item.Value["label"].ToString().Replace("'", "''") + "','" +item.Value["taginstance"] == null ? null : item.Value["taginstance"].ToString().Replace("'", "''") + "','" +item.Value["controldefinition"]==null?null:item.Value["controldefinition"].ToString().Replace("'", "''") + "','" +item.Value["classname"] == null ? null : item.Value["classname"].ToString().Replace("'", "''") + "','" +item.Value["valueattribute"] == null ? null : item.Value["valueattribute"].ToString().Replace("'", "''") + "','" +item.Value["controltype"] == null ? null : item.Value["controltype"].ToString().Replace("'", "''") + "','" +item.Value["tagname"] == null ? null : item.Value["tagname"].ToString().Replace("'", "''") + "','" +item.Value["parentcontrol"] == null ? null : item.Value["parentcontrol"].ToString().Replace("'", "''") + "','" +item.Value["controlid"] == null ? null : item.Value["controlid"].ToString().Replace("'", "''") + "','" +item.Value["class"] == null ? null : item.Value["class"].ToString().Replace("'", "''") + "','" +item.Value["type"] == null ? null : item.Value["type"].ToString().Replace("'", "''") + "','" +                                item.Value["labelfor"] == null ? null : item.Value["labelfor"].ToString().Replace("'", "''") + "','" +item.Value["xpath"] == null ? null : item.Value["xpath"].ToString().Replace("'", "''") + "','" +item.Value["friendlyname"] == null ? null : item.Value["friendlyname"].ToString().Replace("'", "''") + "','" +item.Value["innertext"] == null ? null : item.Value["innertext"].ToString().Replace("'", "''") + "',1)";
                                string insertcommand = "INSERT INTO MasterOR(PAGEID,LABEL,TAGINSTANCE,CONTROLDEFINITION,CLASSNAME,VALUEATTRIBUTE,CONTROLTYPE,TAGNAME,PARENTCONTROL,CONTROLID,CLASS,TYPE,LABELFOR,XPATH,FRIENDLYNAME,INNERTEXT,PROJECTID,VERSION) VALUES(@PageID,@Label,@TagInstance,@ControlDefinition,@ClassName,@ValueAttribute,@ControlType,@TagName,@ParentControl,@ControlID,@Class,@Type,@LabelFor,@Xpath,@FriendlyName,@InnerText,@ProjectID,@Version)";
                                using (SqlCommand command = new SqlCommand(insertcommand, con))
                                {
                                    command.Parameters.AddWithValue("@PageID", pages.First(x => x.Value == page.Key.ToString().ToLower()).Key);
                                    command.Parameters.AddWithValue("@Label", item.Value["label"].ToString());
                                    command.Parameters.AddWithValue("@TagInstance", (item.Value["taginstance"] == null) ? Convert.DBNull : item.Value["taginstance"].ToString());
                                    command.Parameters.AddWithValue("@ControlDefinition", (item.Value["controldefinition"] == null) ? Convert.DBNull : item.Value["controldefinition"].ToString());
                                    command.Parameters.AddWithValue("@ClassName", (item.Value["classname"] == null) ? Convert.DBNull : item.Value["classname"].ToString());
                                    command.Parameters.AddWithValue("@ValueAttribute", (item.Value["valueattribute"] == null) ? Convert.DBNull : item.Value["valueattribute"].ToString());
                                    command.Parameters.AddWithValue("@ControlType", (item.Value["controltype"] == null) ? Convert.DBNull : item.Value["controltype"].ToString());
                                    command.Parameters.AddWithValue("@TagName", (item.Value["tagname"] == null) ? Convert.DBNull : item.Value["tagname"].ToString());
                                    command.Parameters.AddWithValue("@ParentControl", (item.Value["parentcontrol"] == null) ? Convert.DBNull : item.Value["parentcontrol"].ToString());
                                    command.Parameters.AddWithValue("@ControlID", (item.Value["controlid"] == null) ? Convert.DBNull : item.Value["controlid"].ToString());
                                    command.Parameters.AddWithValue("@Class", (item.Value["class"] == null) ? Convert.DBNull : item.Value["class"].ToString());
                                    command.Parameters.AddWithValue("@Type", (item.Value["type"] == null) ? Convert.DBNull : item.Value["type"].ToString());
                                    command.Parameters.AddWithValue("@LabelFor", (item.Value["labelfor"] == null) ? Convert.DBNull : item.Value["labelfor"].ToString());
                                    command.Parameters.AddWithValue("@Xpath", (item.Value["xpath"] == null) ? Convert.DBNull : item.Value["xpath"].ToString());
                                    command.Parameters.AddWithValue("@FriendlyName", (item.Value["friendlyname"] == null) ? Convert.DBNull : item.Value["friendlyname"].ToString());
                                    command.Parameters.AddWithValue("@InnerText", (item.Value["innertext"] == null) ? Convert.DBNull : item.Value["innertext"].ToString());
                                    command.Parameters.AddWithValue("@ProjectID", SignIn.projectId);
                                    command.Parameters.AddWithValue("@Version", "Master");
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                }
            }
            repositiory.Clear();
            getrepositiory();
        }
        public void InsertPageNames(string[] pagenames)
        {
            pages.Clear();
            getpages();
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                foreach (var item in pagenames)
                {
                    if (!pages.ContainsValue(item.ToString().ToLower()))
                    {
                        string executecommand = "INSERT INTO PAGENAMES(PAGENAME,PROJECTID,TAGS) VALUES('" + item + "'," + SignIn.projectId + ",'')";
                        using (SqlCommand command = new SqlCommand(executecommand, con))
                        {
                            command.ExecuteNonQuery();
                            pages.Clear();
                            getpages();
                        }                    
                    }
                }
            }
        }
        public void ImportData(ITestCase tc)
        {

        }
        public List<TestCaseFlowInteraction> GetTestCaseDirective()
        {
            var tcdirective = new List<TestCaseFlowInteraction>();
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("select PageID,FlowIdentifier from ActionFlow where TestCaseId=" + testcaseId + " and ActionFlow='Page' and ProjectID=" + SignIn.projectId + " order by SeqNumber asc", con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        tcdirective.Add(new TestCaseFlowInteraction { PageId = Int32.Parse(reader.GetValue(0).ToString()), FlowIdentifier = Int32.Parse(reader.GetValue(1).ToString()) });
                }
            }
         
            return tcdirective;
        }

        public DataTable GetSampleTestData(string TestCaseId, string pageId)
        {
            string strsql = @"SELECT DISTINCT 'Del' as Del,[Execute], PageName, FlowIdentifier, DataIdentifier,ActionName as Indicator,Mr.Label,ActionORData, SeqNumber from TestData td
                             INNER JOIN PageNames pg on td.PageID=pg.PageID
                             INNER JOIN MasterOr Mr on td.MasterORID= mr.MasterORID 
                             INNER JOIN ActionKeyword ak on td.Indicator = ak.ActionKeyword_ID
                             WHERE td.TestCaseId =" + TestCaseId + " and td.PageID= " + pageId + " order by DataIdentifier , SeqNumber";

            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(strsql, con))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        private void getrepositiory()
        {
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT MasterORID, PageID,Label FROM MasterOR WHERE PROJECTID=" + SignIn.projectId + " ORDER BY Label", con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Dictionary<int, string> temp = new Dictionary<int, string>();
                        repositiory.Add(Int32.Parse(reader.GetValue(0).ToString()), (reader.GetValue(1).ToString() + "_" + reader.GetValue(2).ToString()).ToLower());
                    }
                }
            }
        }
        public void InsertTestData(List<TestCaseFlowInteraction> td, Dictionary<string, IEnumerable<TestDataDirective>> directiveMap,int tfstestcaseId)
        {
            keywords.Clear();
            getkeywords();
            repositiory.Clear();
            getrepositiory();
            int seqno = 0;
            try
            {
                foreach (var item in td)
                {
                    if (directiveMap.ContainsKey(pages[item.PageId].ToString()))
                    {
                        var res = directiveMap[pages[item.PageId]].ToArray();                
                        foreach (var i in res)
                        {
                            if ((i.FlowIdentifier == item.FlowIdentifier) && (!((i.Indicator.ToLower() == "startcondition") || (i.Indicator.ToLower() == "stopcondition"))))
                            {
                                int tcaseid = testcaseId;
                                int pageId = item.PageId;
                                int flowId = i.FlowIdentifier;
                                int dataId = i.DataIdentifier;
                                int indid=0;
                                try
                                {
                                    indid = keywords.First(x => x.Value.ToLower() == i.Indicator.ToString().ToLower()).Key;
                                }
                                catch(Exception ex)
                                {
                                    //MessageBox.Show("Invalid Keyword OR Keyword NOT exists. \nTestCase ID : " + tfstestcaseId + "\nFlowIdentifier : " + i.FlowIdentifier + "\nDataIdentifier : " + i.DataIdentifier + "\nIndicator : " + i.Indicator + "\nPage Name : " + pages[item.PageId] + "\n\nERROR : " + ex.Message);
                                    throw new Exception("Invalid Keyword OR Keyword NOT exists. \n\nTESTCASE MIGRATION FAILED FOR TEST CASEID : " + tfstestcaseId + "\nFlowIdentifier : " + i.FlowIdentifier + "\nDataIdentifier : " + i.DataIdentifier + "\nIndicator : " + i.Indicator + "\nPage Name : " + pages[item.PageId] + "\n\nERROR : " + ex.Message);
                                }
                                int projId = SignIn.projectId;
                                string exeflag = (i.ShouldExecute == true) ? "Yes" : ((i.ShouldExecute == false) ? "No" : "Yes");
                                foreach (var interaction in i.Interactions)
                                {
                                    if (interaction.Value.ToLower() != "nofill")
                                    {
                                        int masterorid = 0;
                                        try
                                        {
                                            masterorid = repositiory.First(x => x.Value.ToLower().Trim() == (pageId.ToString() + "_" + interaction.LogicalFieldName.ToString().Trim()).ToLower()).Key;
                                        }
                                        catch(Exception ex)
                                        {
                                            //MessageBox.Show("Invalid Label OR Label NOT exists. \nTestCase ID : " + tfstestcaseId + "\nFlowIdentifier : " + i.FlowIdentifier + "\nDataIdentifier : " + i.DataIdentifier + "\nIndicator : " + i.Indicator + "\nPage Name : " + pages[item.PageId] + "\nLabel : "+interaction.LogicalFieldName.ToString()+ "\n\nERROR : " + ex.Message);
                                            throw new Exception("Invalid Label OR Label NOT exists. \n\n TESTCASE MIGRATION FAILED FOR TEST CASEID : " + tfstestcaseId + "\nFlowIdentifier : " + i.FlowIdentifier + "\nDataIdentifier : " + i.DataIdentifier + "\nIndicator : " + i.Indicator + "\nPage Name : " + pages[item.PageId] + "\nLabel : " + interaction.LogicalFieldName.ToString() + "\n\nERROR : " + ex.Message);
                                        }
                                        string testdata = interaction.Value;
                                        seqno = seqno + 1;
                                        using (SqlConnection con = new SqlConnection(dbConnectionString))
                                        {
                                            con.Open();
                                            {
                                                string insertcommand = "INSERT INTO TESTDATA(TESTCASEID,PAGEID,FLOWIDENTIFIER,DATAIDENTIFIER,MASTERORID,INDICATOR,ACTIONORDATA,SEQNUMBER,[EXECUTE],PROJECTID) VALUES(@TestCaseID,@PageID,@FlowIdentifier,@DataIdentifier,@MasterORID,@Indicator,@ActionORData,@SeqNumber,@Execute,@ProjectID)";
                                                using (SqlCommand command = new SqlCommand(insertcommand, con))
                                                {
                                                    command.Parameters.AddWithValue("@TestCaseID", tcaseid);
                                                    command.Parameters.AddWithValue("@PageID", pageId);
                                                    command.Parameters.AddWithValue("@FlowIdentifier", flowId);
                                                    command.Parameters.AddWithValue("@DataIdentifier", dataId);
                                                    command.Parameters.AddWithValue("@MasterORID", masterorid);
                                                    command.Parameters.AddWithValue("@Indicator", indid);
                                                    command.Parameters.AddWithValue("@ActionORData", testdata);
                                                    command.Parameters.AddWithValue("@SeqNumber", seqno);
                                                    command.Parameters.AddWithValue("@Execute", exeflag);
                                                    command.Parameters.AddWithValue("@ProjectID", projId);
                                                    command.ExecuteNonQuery();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }           
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void getkeywords()
        {
            using (SqlConnection con = new SqlConnection(dbConnectionString))
            {
                con.Open();
                using (SqlCommand command = new SqlCommand("SELECT ActionKeyword_ID,ActionName FROM ActionKeyword ORDER BY ActionKeyword_ID", con))
                {
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                        keywords.Add(Int32.Parse(reader.GetValue(0).ToString()), reader.GetValue(1).ToString());
                }
            }
        }
        public void ImportActionflow(ITestCase tc)
        {
            string querytestcaseInfo = string.Empty;
            querytestcaseInfo = "INSERT INTO TestCaseInfo(TestCaseTitle,TestCaseSummary,DesignedBy, AssignedTo, State,TestCategory,Functionality,ProjectID) OUTPUT INSERTED.TESTCASEID Values('"
                + tc.Title.ToString().Replace("'", "''") + "','','"
                + tc.OwnerName.ToString() + "','"
                + tc.OwnerName.ToString() + "','"                
                + tc.State.ToString() + "','Regression','"
                + tc.Id.ToString().Replace("'", "''") + "',"
                + SignIn.projectId + ")";
            testcaseId = Int32.Parse(ExecuteScalar(querytestcaseInfo).ToString());

            var testSteps = (from a in tc.Actions
                             where a is ITestStep
                             select ((ITestStep)a).Title.ToPlainText()).ToList();

            string action = null;
            int pageid = 0;
            int tcid = 0;
            int fid = 0;
            int seqnum = 0;
            int projid = 0;

            foreach (var step in testSteps)
            {
                string temp = step.ToString().ToLower().Split('{')[0].Trim();
                switch (temp)
                {
                    case "testdata":
                    case "abort":
                        break;
                    case "page":
                        action = step.ToString().Split('{')[0].Trim();
                        pageid = pages.FirstOrDefault(x => x.Value == step.ToString().Split('{')[1].Split('-')[0].Trim().ToLower()).Key;
                        fid = Int32.Parse(step.ToString().Contains("[DI]") ? step.ToString().Split('{')[1].Split('-')[1].Split('[')[0].Trim() : step.ToString().Split('{')[1].Split('-')[1].Split('}')[0].Trim());
                        tcid = testcaseId;
                        seqnum = seqnum + 1;
                        projid = SignIn.projectId;
                        break;
                    case "loadtestdataitem":
                        action = step.ToString().Split('{')[0].Trim();
                        pageid = pages.FirstOrDefault(x => x.Value == step.ToString().Split('{')[1].Split('}')[0].Trim().ToLower()).Key;
                        //fid = item[0].Value.ToString().Contains("[DI]") ? item[0].Value.ToString().Split('{')[1].Split('-')[1].Split('[')[0].Trim() : item[0].Value.ToString().Split('{')[1].Split('-')[1].Split('}')[0].Trim();
                        tcid = testcaseId;
                        seqnum = seqnum + 1;
                        projid = SignIn.projectId;
                        break;
                    case "closeallbrowsers":
                    case "invokeapplication":
                    case "browserclose":
                    case "startiteration":
                    case "stopiteration":
                    case "closealert":
                        action = step.ToString().Trim();
                        pageid = pages.FirstOrDefault(x => x.Value.ToString() == string.Empty).Key;
                        fid = 0;
                        tcid = testcaseId;
                        seqnum = seqnum + 1;
                        projid = SignIn.projectId;
                        break;
                    default:
                        break;
                }
                if (!((temp == "testdata") || (temp == "abort")))
                {
                    using (SqlConnection con = new SqlConnection(dbConnectionString))
                    {
                        con.Open();
                        {
                            string insertcommand = "INSERT INTO ActionFlow(PAGEID,TESTCASEID,ACTIONFLOW,FLOWIDENTIFIER,SEQNUMBER,PROJECTID,CreatedBy,CreatedDate) VALUES(@PageID,@TestCaseID,@ActionFlow,@FlowIdentifier,@SeqNumber,@ProjectID,@CreatedBy,@CreatedDate)";
                            string createdby = SignIn.userId.ToUpper();
                            using (SqlCommand command = new SqlCommand(insertcommand, con))
                            {
                                command.Parameters.AddWithValue("@PageID", pageid);
                                command.Parameters.AddWithValue("@TestCaseID", tcid);
                                command.Parameters.AddWithValue("@ActionFlow", action);
                                command.Parameters.AddWithValue("@FlowIdentifier", fid == 0 ? Convert.DBNull : fid);
                                command.Parameters.AddWithValue("@SeqNumber", seqnum);
                                command.Parameters.AddWithValue("@ProjectID", projid);
                                command.Parameters.AddWithValue("@CreatedBy", createdby);
                                command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                                command.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
        }
    }
}
