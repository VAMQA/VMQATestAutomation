namespace VM.Platform.TestAutomationFramework.Adapters.Local
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using VM.Platform.TestAutomationFramework.Core;
    using VM.Platform.TestAutomationFramework.Core.Contracts;
    using VM.Platform.TestAutomationFramework.Core.Exceptions;
    using LinqToExcel;
    using Core.Configuration;
    using System.Data;
    public class LocalExcelMapReader : IDataFileReader
    {
        private readonly TestCaseConfiguration testCaseConfiguration;
        //private string connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=UmbrellaTestDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //private string connString = @"Data Source = GP143067\SQLEXPRESS;Initial Catalog = UmbrellaTestDb; Integrated Security = True; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //private string connString = @"Data Source = FRVPCMIND87\MSSQLEXPRESS;Initial Catalog = UmbrellaTestDb; Integrated Security = True; Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private string connString;
        public LocalExcelMapReader(TestCaseConfiguration testCaseConfiguration)
        {
            this.testCaseConfiguration = testCaseConfiguration;
            connString = testCaseConfiguration.DataBaseConnectionString;
        }

        public Dictionary<string, Dictionary<string, T>> GetMapOfMaps<T>(string filter) where T : class, new()
        {
            string testCaseID = filter.Split(':')[0];
            string testDataType = filter.Split(':')[1];
            var controlMap = new Dictionary<string, Dictionary<string, T>>();
            string sqlQuery = string.Empty;

            if (testDataType.Equals("DataFlow"))
            {
                var collection = GetMapsTestFlow<T>(testCaseID);
                controlMap.Add(testCaseID, collection);
            }
            else if (testDataType.Equals("MasterOR"))
            {
                controlMap = GetMapsMasterOR<T>(testCaseID);
            }
            return controlMap;
        }



        private Dictionary<string, T> GetMapsTestFlow<T>(string TestCaseID)
            where T : class, new()
        {
            string sqlQuery = string.Empty;
            //--sqlQuery = "SELECT CASE WHEN LEN(p.ActionFlow) > 0 THEN p.ActionFlow ELSE '' END +CASE WHEN PG.pageName is null  THEN ' ' WHEN LEN(PG.pageName) > 0 THEN '{'+PG.pageName+'-'+CONVERT(varchar, p.FlowIdentifier)+'[DI]}' END AS Action	 from [dbo].[ActionFlow] P left outer join PageNames PG on P.PageID=PG.PageID where TestCaseId=" + TestCaseID;
            //sqlQuery = "SELECT CASE WHEN LEN(p.ActionFlow) > 0 THEN p.ActionFlow ELSE '' END +CASE WHEN PG.pageName is null  THEN ' ' WHEN LEN(PG.pageName) > 0 THEN '{'+PG.pageName+'-'+CONVERT(varchar, p.FlowIdentifier)+'[DI]}' END AS Action	 from [dbo].[ActionFlow] P left outer join PageNames PG on P.PageID=PG.PageID where TestCaseId=" + TestCaseID;
            sqlQuery = "SELECT CASE WHEN LEN(p.ActionFlow) > 0 THEN p.ActionFlow ELSE '' END +CASE WHEN PG.pageName is null  THEN ' ' WHEN LEN(PG.pageName) > 0 THEN '{'+PG.pageName+'-'+CONVERT(varchar, p.FlowIdentifier)+'[DI]}' END AS Action	 from [dbo].[ActionFlow] P left outer join PageNames PG on P.PageID=PG.PageID and P.ProjectID=PG.ProjectID where TestCaseId=" + TestCaseID+" and P.ProjectID="+this.testCaseConfiguration.TestPlan+" order by SeqNumber asc";
            System.Data.DataSet result = SqlHelper.ExecuteDataset(connString, System.Data.CommandType.Text, sqlQuery);
            string[] columnNames = (from dc in result.Tables[0].Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();


            var collection = new Dictionary<string, T>();
            var stepIndex = 0;
            foreach (DataRow row in result.Tables[0].Rows)
            {
                var obj = new T();
                var type = typeof(T);
                var props = type.GetProperties();

                foreach (var prop in props)
                {
                    var columnName = columnNames.SingleOrDefault(cn => cn.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                    prop.SetValue(obj, GetCellValue(columnName, row));
                }
                var key = stepIndex+"-"+row[0].ToString();
                stepIndex++;
                try
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        collection.Add(key, obj);
                    }
                }
                catch (ArgumentException ex)
                {
                    var message = string.Format("Duplicate row found in control map ({0}:{1})",
                        TestCaseID, key);
                    throw new FrameworkFatalException(message, ex);
                }
            }
            return collection;
        }

        private Dictionary<string, Dictionary<string, T>> GetMapsMasterOR<T>(string TestCaseID)
            where T : class, new()
        {
            string sqlQuery = string.Empty;
            var controlMap = new Dictionary<string, Dictionary<string, T>>();
            //sqlQuery = "select PageName,Label,	ControlType,	ControlID,	Class,	ParentControl,	TagName,	FriendlyName,	ValueAttribute,	TagInstance,	[Type],	ClassName,	InnerText	,ControlDefinition	,LabelFor	,Xpath from masterOr left outer join PageNames on MasterOR.PageID=PageNames.PageID ";
            sqlQuery = "select PageName,Label,	ControlType,	ControlID,	Class,	ParentControl,	TagName,	FriendlyName,	ValueAttribute,	TagInstance,	[Type],	ClassName,	InnerText	,ControlDefinition	,LabelFor	,Xpath,[Version] ,[ImagePath] ";
            sqlQuery += "from masterOr join PageNames on MasterOR.PageID=PageNames.PageID and MasterOR.ProjectID=Pagenames.ProjectID and MasterOR.ProjectID=" + this.testCaseConfiguration.TestPlan + " and  [Version]!='archive' order by MasterORID desc";

            System.Data.DataSet result = SqlHelper.ExecuteDataset(connString, System.Data.CommandType.Text, sqlQuery);
            string[] columnNames = (from dc in result.Tables[0].Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();

            var resultPageWise = result.Tables[0].AsEnumerable()
                                            .GroupBy(row => row.Field<String>("PageName"))
                                            .Select(g => g.CopyToDataTable())
                                            .ToList();
            //foreach(DataTable dt in resultPageWise)
            //{
            //    dt.TableName = dt.Rows[0]["PageName"].ToString();
            //}
            //resultPageWise.ForEach(dt => dt.TableName = dt.Rows[0]["PageName"].ToString());
            foreach (DataTable dt in resultPageWise)
            {
                dt.TableName = dt.Rows[0]["PageName"].ToString();
                var collection = new Dictionary<string, T>();
                foreach (DataRow row in dt.Rows)
                {

                    var obj = new T();
                    var type = typeof(T);
                    var props = type.GetProperties();

                    foreach (var prop in props)
                    {
                        var columnName = columnNames.SingleOrDefault(cn => cn.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                        prop.SetValue(obj, GetCellValue(columnName, row));
                    }
                    var key = row[1].ToString();
                    var version = row[16].ToString();
                    try
                    {
                        if ((!string.IsNullOrWhiteSpace(key)) && (!collection.ContainsKey(key)))
                        {
                            collection.Add(key, obj);
                        }
                        else if(string.Compare(version,testCaseConfiguration.ControlMapSource,StringComparison.OrdinalIgnoreCase)==0)
                        {                            
                            collection.Remove(key);
                            collection.Add(key, obj);
                        }


                    }
                    catch (ArgumentException ex)
                    {
                        var message = string.Format("Duplicate row found in control map ({0}:{1})",
                            TestCaseID, key);
                        throw new FrameworkFatalException(message, ex);
                    }

                }
                controlMap.Add(dt.TableName, collection);
            }

            return controlMap;
        }
        private static string GetCellValue(string columnName, DataRow row)
        {
            if (columnName == null) return null;
            else return row[columnName].ToString();
        }

        #region TestData
        public Dictionary<string, IEnumerable<TestDataDirective>> GetTestDataDirectives(string TestCaseID)
        {
            //excel.FileName = excelFileName;
            //var worksheetNames = this.excel.GetWorksheetNames();
            //string TestCaseID = "629097";

            //foreach (var worksheetName in worksheetNames)
            //{
            return GetDirectives(TestCaseID);
            //directiveMap.Add(TestCaseID, directives);
            //}


        }

        private Dictionary<string, IEnumerable<TestDataDirective>> GetDirectives(string testCaseID)
        {
            //Modified by Sathiya 
            var directiveMap = new Dictionary<string, IEnumerable<TestDataDirective>>();
            string sqlQuery = string.Empty;
            var collection = new Dictionary<string, TestDataDirective>();
            //if (tableName.Contains("TestData"))
            //sqlQuery = "Select [Execute],P.PageName,T.FlowIdentifier,T.DataIdentifier, A.ActionName [Indicator] , M.Label [FieldName], T.ActionORData [Data Or Action] from TestData T left outer join PageNames P on P.PageID = T.PageID left outer join ActionKeyword A on a.ActionKeyword_ID = T.Indicator left outer join MasterOR M on M.MasterORID = T.MasterORID where T.TestCaseId = " + testCaseID + "  order by SeqNumber";
            sqlQuery = "Select [Execute],P.PageName,T.FlowIdentifier,T.DataIdentifier, A.ActionName [Indicator] , M.Label [FieldName], T.ActionORData [Data Or Action] from TestData T left outer join PageNames P on P.PageID = T.PageID and P.ProjectID=T.ProjectID left outer join ActionKeyword A on a.ActionKeyword_ID = T.Indicator left outer join MasterOR M on M.MasterORID = T.MasterORID and M.ProjectID=T.ProjectID where T.TestCaseId = " + testCaseID + " and T.ProjectID="+this.testCaseConfiguration.TestPlan+" order by SeqNumber";
            
            System.Data.DataSet result = SqlHelper.ExecuteDataset(connString, System.Data.CommandType.Text, sqlQuery);
            string[] columnNames = (from dc in result.Tables[0].Columns.Cast<DataColumn>()
                                    select dc.ColumnName).ToArray();

            var resultPageWise = result.Tables[0].AsEnumerable()
                                            .GroupBy(row => row.Field<String>("PageName"))
                                            .Select(g => g.CopyToDataTable())
                                            .ToList();
            columnNames = columnNames.Where(val => !val.Equals("PageName")).ToArray();
            foreach (DataTable dt in resultPageWise)
            {
                var directives = new List<TestDataDirective>();
                dt.TableName = dt.Rows[0]["PageName"].ToString();
                foreach (DataRow row in dt.Rows)
                {
                    var directive = new TestDataDirective();
                    foreach (var key in columnNames.Where(IsImportantColumn))
                    {
                        if (string.IsNullOrWhiteSpace(row[key].ToString())) continue;
                        switch (key.Replace(" ", string.Empty).ToLower())
                        {
                            case "execute":
                                directive.ShouldExecute = IsPositive(row[key].ToString());
                                break;
                            case "tc#":
                            case "tc #":
                                directive.TestCaseNumber = row[key].ToString();
                                break;
                            case "flowidentifier":
                                directive.FlowIdentifier = int.Parse(row[key].ToString());
                                break;
                            case "dataidentifier":
                                directive.DataIdentifier = int.Parse(row[key].ToString());
                                break;
                            case "indicator":
                                directive.Indicator = row[key].ToString();
                                break;
                            default:
                                directive.Interactions.Add(new TestDataInteraction { LogicalFieldName = key, Value = row[key].ToString() });
                                break;
                        }
                    }
                    directives.Add(directive);
                }
                directiveMap.Add(dt.TableName, directives);
            }


            return directiveMap;
        }

        private bool IsPositive(string value)
        {
            return string.IsNullOrWhiteSpace(value)
                || new[] { "yes", "true" }.Contains(value, StringComparer.OrdinalIgnoreCase);
        }

        private bool IsImportantColumn(string columnName)
        {
            var ignorableColumnPatterns = new[]
            {
                @"^Comments?\s*\d*$",
                @"^Scenario\s*Id$",
                @"^Scenario\s*Description$",
                @"^States?$",
                @"^Rule\s*Description$",
                @"^Validations?$",
            };

            return this.testCaseConfiguration.IgnorableColumnPatterns.All(
                    p => !Regex.IsMatch(columnName, p, RegexOptions.IgnoreCase));
        }
        #endregion
    }
    public class LocalExcelMapReaders : IDataFileReader
    {
        private readonly TestCaseConfiguration testCaseConfiguration;
        private readonly IExcelQueryFactory excel;

        public LocalExcelMapReaders(TestCaseConfiguration testCaseConfiguration)
        {
            this.testCaseConfiguration = testCaseConfiguration;
            this.excel = new ExcelQueryFactory();
        }

        public Dictionary<string, Dictionary<string, T>> GetMapOfMaps<T>(string excelFileName)
            where T : class, new()
        {
            excel.FileName = excelFileName;
            var worksheetNames = this.excel.GetWorksheetNames();
            var controlMap = new Dictionary<string, Dictionary<string, T>>();
            foreach (var worksheetName in worksheetNames)
            {
                var controlDefinitions = GetMaps<T>(worksheetName);
                controlMap.Add(worksheetName, controlDefinitions);
            }


            return controlMap;
        }

        public Dictionary<string, IEnumerable<TestDataDirective>> GetTestDataDirectives(string excelFileName)
        {
            excel.FileName = excelFileName;
            var worksheetNames = this.excel.GetWorksheetNames();
            var directiveMap = new Dictionary<string, IEnumerable<TestDataDirective>>();
            foreach (var worksheetName in worksheetNames)
            {
                var directives = GetDirectives(worksheetName);
                directiveMap.Add(worksheetName, directives);
            }

            return directiveMap;
        }

        private IEnumerable<TestDataDirective> GetDirectives(string worksheetName)
        {
            var rows = excel.Worksheet(worksheetName);
            var directives = new List<TestDataDirective>();

            foreach (var row in rows)
            {
                var directive = new TestDataDirective();
                foreach (var key in row.ColumnNames.Where(IsImportantColumn))
                {
                    if (string.IsNullOrWhiteSpace(row[key])) continue;
                    switch (key.Replace(" ", string.Empty).ToLower())
                    {
                        case "execute":
                            directive.ShouldExecute = IsPositive(row[key]);
                            break;
                        case "tc#":
                        case "tc #":
                            directive.TestCaseNumber = row[key];
                            break;
                        case "flowidentifier":
                            directive.FlowIdentifier = int.Parse(row[key]);
                            break;
                        case "dataidentifier":
                            directive.DataIdentifier = int.Parse(row[key]);
                            break;
                        case "indicator":
                            directive.Indicator = row[key];
                            break;
                        default:
                            directive.Interactions.Add(new TestDataInteraction { LogicalFieldName = key, Value = row[key] });
                            break;
                    }
                }
                directives.Add(directive);
            }

            return directives;
        }

        private bool IsPositive(Cell cell)
        {
            return string.IsNullOrWhiteSpace(cell.Value.ToString())
                || new[] { "yes", "true" }.Contains(cell.Value.ToString(), StringComparer.OrdinalIgnoreCase);
        }

        private bool IsImportantColumn(string columnName)
        {
            var ignorableColumnPatterns = new[]
            {
                @"^Comments?\s*\d*$",
                @"^Scenario\s*Id$",
                @"^Scenario\s*Description$",
                @"^States?$",
                @"^Rule\s*Description$",
                @"^Validations?$",
            };

            return this.testCaseConfiguration.IgnorableColumnPatterns.All(
                    p => !Regex.IsMatch(columnName, p, RegexOptions.IgnoreCase));
        }

        private Dictionary<string, T> GetMaps<T>(string worksheetName)
            where T : class, new()
        {
            var collection = new Dictionary<string, T>();

            var columns = excel.GetColumnNames(worksheetName).ToArray();
            var rows = excel.Worksheet(worksheetName);

            foreach (var row in rows)
            {
                var obj = new T();
                var type = typeof(T);
                var props = type.GetProperties();

                foreach (var prop in props)
                {
                    var columnName = columns.SingleOrDefault(cn => cn.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
                    prop.SetValue(obj, GetCellValue(columnName, row));
                }
                var key = row[0].Value.ToString();
                try
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        collection.Add(key, obj);
                    }
                }
                catch (ArgumentException ex)
                {
                    var message = string.Format("Duplicate row found in control map ({0}:{1})",
                        worksheetName, key);
                    throw new FrameworkFatalException(message, ex);
                }
            }
            return collection;
        }

        private static string GetCellValue(string columnName, Row row)
        {
            if (columnName == null) return null;
            else return row[columnName].Value.ToString();
        }
    }
}


//namespace VM.Platform.TestAutomationFramework.Adapters.Local
//{
//    using System;
//    using System.Collections.Generic;
//    using System.Linq;
//    using System.Text.RegularExpressions;
//    using VM.Platform.TestAutomationFramework.Core;
//    using VM.Platform.TestAutomationFramework.Core.Contracts;
//    using VM.Platform.TestAutomationFramework.Core.Exceptions;
//    using LinqToExcel;

//    public class LocalExcelMapReader : IDataFileReader
//    {
//        private readonly TestCaseConfiguration testCaseConfiguration;
//        private readonly IExcelQueryFactory excel;

//        public LocalExcelMapReader(TestCaseConfiguration testCaseConfiguration)
//        {
//            this.testCaseConfiguration = testCaseConfiguration;
//            this.excel = new ExcelQueryFactory();
//        }

//        public Dictionary<string, Dictionary<string, T>> GetMapOfMaps<T>(string excelFileName)
//            where T : class, new()
//        {
//            excel.FileName = excelFileName;
//            var worksheetNames = this.excel.GetWorksheetNames();
//            var controlMap = new Dictionary<string, Dictionary<string, T>>();
//            foreach (var worksheetName in worksheetNames)
//            {
//                var controlDefinitions = GetMaps<T>(worksheetName);
//                controlMap.Add(worksheetName, controlDefinitions);
//            }


//            return controlMap;
//        }

//        public Dictionary<string, IEnumerable<TestDataDirective>> GetTestDataDirectives(string excelFileName)
//        {
//            excel.FileName = excelFileName;
//            var worksheetNames = this.excel.GetWorksheetNames();
//            var directiveMap = new Dictionary<string, IEnumerable<TestDataDirective>>();
//            foreach (var worksheetName in worksheetNames)
//            {
//                var directives = GetDirectives(worksheetName);
//                directiveMap.Add(worksheetName, directives);
//            }

//            return directiveMap;
//        }

//        private IEnumerable<TestDataDirective> GetDirectives(string worksheetName)
//        {
//            var rows = excel.Worksheet(worksheetName);
//            var directives = new List<TestDataDirective>();

//            foreach (var row in rows)
//            {
//                var directive = new TestDataDirective();
//                foreach (var key in row.ColumnNames.Where(IsImportantColumn))
//                {
//                    if (string.IsNullOrWhiteSpace(row[key])) continue;
//                    switch (key.Replace(" ", string.Empty).ToLower())
//                    {
//                        case "execute":
//                            directive.ShouldExecute = IsPositive(row[key]);
//                            break;
//                        case "tc#":
//                        case "tc #":
//                            directive.TestCaseNumber = row[key];
//                            break;
//                        case "flowidentifier":
//                            directive.FlowIdentifier = int.Parse(row[key]);
//                            break;
//                        case "dataidentifier":
//                            directive.DataIdentifier = int.Parse(row[key]);
//                            break;
//                        case "indicator":
//                            directive.Indicator = row[key];
//                            break;
//                        default:
//                            directive.Interactions.Add(new TestDataInteraction { LogicalFieldName = key, Value = row[key] });
//                            break;
//                    }
//                }
//                directives.Add(directive);
//            }

//            return directives;
//        }

//        private bool IsPositive(Cell cell)
//        {
//            return string.IsNullOrWhiteSpace(cell.Value.ToString()) 
//                || new[] {"yes", "true"}.Contains(cell.Value.ToString(), StringComparer.OrdinalIgnoreCase);
//        }

//        private bool IsImportantColumn(string columnName)
//        {
//            var ignorableColumnPatterns = new[]
//            {
//                @"^Comments?\s*\d*$",
//                @"^Scenario\s*Id$",
//                @"^Scenario\s*Description$",
//                @"^States?$",
//                @"^Rule\s*Description$",
//                @"^Validations?$",
//            };

//            return this.testCaseConfiguration.IgnorableColumnPatterns.All(
//                    p => !Regex.IsMatch(columnName, p, RegexOptions.IgnoreCase));
//        }

//        private Dictionary<string, T> GetMaps<T>(string worksheetName)
//            where T : class, new()
//        {
//            var collection = new Dictionary<string, T>();

//            var columns = excel.GetColumnNames(worksheetName).ToArray();
//            var rows = excel.Worksheet(worksheetName);

//            foreach (var row in rows)
//            {
//                var obj = new T();
//                var type = typeof(T);
//                var props = type.GetProperties();

//                foreach (var prop in props)
//                {
//                    var columnName = columns.SingleOrDefault(cn => cn.Equals(prop.Name, StringComparison.OrdinalIgnoreCase));
//                    prop.SetValue(obj, GetCellValue(columnName, row));
//                }
//                var key = row[0].Value.ToString();
//                try
//                {
//                    if (!string.IsNullOrWhiteSpace(key))
//                    {
//                        collection.Add(key, obj);
//                    }
//                }
//                catch (ArgumentException ex)
//                {
//                    var message = string.Format("Duplicate row found in control map ({0}:{1})",
//                        worksheetName, key);
//                    throw new FrameworkFatalException(message, ex);
//                }
//            }
//            return collection;
//        }

//        private static string GetCellValue(string columnName, Row row)
//        {
//            if (columnName == null) return null;
//            else return row[columnName].Value.ToString();
//        }
//    }
//}

//===============================================================================================================================

