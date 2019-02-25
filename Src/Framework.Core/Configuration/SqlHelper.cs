using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Xml;

namespace VM.Platform.TestAutomationFramework.Core.Configuration
{
    public sealed class SqlHelper
    {
        public string connectionString { get; set; }

        #region "private utility methods &amp; constructors"
        private static void AttachParameters(SqlCommand command, SqlParameter[] commandParameters)
        {
            try
            {
                if (command == null) throw new ArgumentNullException("command");

                if (commandParameters != null)
                {
                    foreach (SqlParameter p in commandParameters)
                    {
                        if (p != null)
                        {
                            if ((p.Direction == ParameterDirection.InputOutput || p.Direction == ParameterDirection.Input) && p.Value == null)
                            {
                                p.Value = DBNull.Value;
                            }

                            if (p.Value == null)
                            {
                                if (p.SqlDbType == SqlDbType.Int)
                                {
                                    p.Value = 0;
                                }
                                else
                                {
                                    p.Value = " ";
                                }
                            }

                            command.Parameters.Add(p);
                        }
                    }
                }
            }
            catch
            {
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, DataRow dataRow)
        {
            try
            {
                if (commandParameters == null || dataRow == null)
                {
                    return;
                }

                int i = 0;

                foreach (SqlParameter commandParameter in commandParameters)
                {
                    if ((commandParameter.ParameterName == null || commandParameter.ParameterName.Length <= 1))
                    {
                        throw new Exception(string.Format("Please provide a valid parameter name on the parameter #{0}, the ParameterName property has the following value: ' {1}' .", i, commandParameter.ParameterName));
                    }

                    if (dataRow.Table.Columns.IndexOf(commandParameter.ParameterName.Substring(1)) != -1)
                    {
                        commandParameter.Value = dataRow[commandParameter.ParameterName.Substring(1)];
                    }

                    i = i + 1;
                }
            }
            catch
            {
            }
        }

        private static void AssignParameterValues(SqlParameter[] commandParameters, object[] parameterValues)
        {
            int i = 0;

            int j = 0;

            try
            {
                if ((commandParameters == null) && (parameterValues == null))
                {
                    return;
                }

                if (commandParameters.Length != parameterValues.Length)
                {
                    throw new ArgumentException("Parameter count does not match Parameter Value count.");
                }

                j = commandParameters.Length - 1;

                for (i = 0; i <= j; i++)
                {
                    if (parameterValues[i] is IDbDataParameter)
                    {
                        IDbDataParameter paramInstance = (IDbDataParameter)parameterValues[i];

                        if ((paramInstance.Value == null))
                        {
                            commandParameters[i].Value = DBNull.Value;
                        }
                        else
                        {
                            commandParameters[i].Value = paramInstance.Value;
                        }
                    }
                    else if ((parameterValues[i] == null))
                    {
                        commandParameters[i].Value = DBNull.Value;
                    }
                    else
                    {
                        commandParameters[i].Value = parameterValues[i];
                    }
                }
            }
            catch
            {
            }
        }

        //private CommandType commandType, private string commandText, SqlParameter[] commandParameters, ref private bool mustCloseConnection)

        public static object HttpContext { get; private set; }

        private static void PrepareCommand(SqlCommand command, SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, ref bool mustCloseConnection)
        {
            try
            {
                if ((command == null)) throw new ArgumentNullException("command");

                if ((commandText == null || commandText.Length == 0)) throw new ArgumentNullException("commandText");

                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();

                    mustCloseConnection = true;
                }
                else
                {
                    mustCloseConnection = false;
                }

                command.Connection = connection;

                command.CommandText = commandText;

                if ((transaction != null))
                {
                    if (transaction.Connection == null) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                    command.Transaction = transaction;
                }

                command.CommandType = commandType;

                if ((commandParameters != null))
                {
                    AttachParameters(command, commandParameters);
                }

                return;
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        #endregion "private utility methods &amp; constructors"

        #region "ExecuteNonQuery"        

        public static int ExecuteNonQuery(string connectionString, CommandType commandType,IDictionary res)
        {
            int testrunId = 0;
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            SqlConnection connection = default(SqlConnection);

            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);
                connection.Open();
                using (SqlCommand command = new SqlCommand(
                    "INSERT INTO TestResults OUTPUT INSERTED.TESTRUNID VALUES(@TestCaseID,@Result,@RunBy,@Environment,@Browser,@DateOfExe,@ExeTime,@CondExecuted,@CondPassed,@CondFailed,@ProjectID)", connection))
                {
                    command.Parameters.Add(new SqlParameter("TestCaseID", res["TestCaseID"]));
                    command.Parameters.Add(new SqlParameter("Result", res["Result"]));
                    command.Parameters.Add(new SqlParameter("RunBy", res["RunBy"]));
                    command.Parameters.Add(new SqlParameter("Environment", res["Environment"]));
                    command.Parameters.Add(new SqlParameter("Browser", res["Browser"]));
                    command.Parameters.Add(new SqlParameter("DateOfExe", res["DateOfExe"]));
                    command.Parameters.Add(new SqlParameter("ExeTime", res["ExeTime"]));
                    command.Parameters.Add(new SqlParameter("CondExecuted", res["No.Of Cond Executed"]));
                    command.Parameters.Add(new SqlParameter("CondPassed", res["No.Of Cond Passed"]));
                    command.Parameters.Add(new SqlParameter("CondFailed", res["No.Of Cond Failed"]));
                    command.Parameters.Add(new SqlParameter("ProjectID", res["ProjectID"]));
                    //command.ExecuteNonQuery();
                    testrunId = Int32.Parse((command.ExecuteScalar()).ToString());
                }
                res.Clear();
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));                
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
            return testrunId;
        }

        //public static int ExecuteNonQuery(string connectionString, CommandType commandType, IDictionary res)
        //{
        //    int runID=0;
        //    if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

        //    SqlConnection connection = default(SqlConnection);

        //    connection = null;

        //    try
        //    {
        //        connection = new SqlConnection(connectionString);
        //        connection.Open();
        //        using (SqlCommand command = new SqlCommand(
        //            "INSERT INTO TestResults OUTPUT INSERTED.TESTRUNID VALUES(@TestCaseID,@Result,@RunBy,@Environment,@Browser,@DateOfExe,@ExeTime,@CondExecuted,@CondPassed,@CondFailed,@ProjectID)", connection))
        //        {
        //            command.Parameters.Add(new SqlParameter("TestCaseID", res["TestCaseID"]));
        //            command.Parameters.Add(new SqlParameter("Result", res["Result"]));
        //            command.Parameters.Add(new SqlParameter("RunBy", res["RunBy"]));
        //            command.Parameters.Add(new SqlParameter("Environment", res["Environment"]));
        //            command.Parameters.Add(new SqlParameter("Browser", res["Browser"]));
        //            command.Parameters.Add(new SqlParameter("DateOfExe", res["DateOfExe"]));
        //            command.Parameters.Add(new SqlParameter("ExeTime", res["ExeTime"]));
        //            command.Parameters.Add(new SqlParameter("CondExecuted", res["No.Of Cond Executed"]));
        //            command.Parameters.Add(new SqlParameter("CondPassed", res["No.Of Cond Passed"]));
        //            command.Parameters.Add(new SqlParameter("CondFailed", res["No.Of Cond Failed"]));
        //            command.Parameters.Add(new SqlParameter("ProjectID", res["ProjectID"]));
        //            command.ExecuteNonQuery();
        //            runID = Int32.Parse((command.ExecuteScalar()).ToString());
        //        }
        //        res.Clear();                
        //    }
        //    catch (Exception err)
        //    {
        //        //HttpContext.Current.Response.Write(Convert.ToString(err));                
        //    }
        //    finally
        //    {
        //        if ((connection != null)) connection.Dispose();                
        //    }
        //    return runID;
        //}

        public static int ExecuteNonQuery(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            SqlConnection connection = default(SqlConnection);

            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return ExecuteNonQuery(connection, commandType, commandText, commandParameters);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        public static int ExecuteNonQuery(string connectionString, string spName, params object[] parameterValues)
        {
            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return 0;
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText)
        {
            try
            {
                return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return ExecuteNonQuery(connection, commandType, commandText, (SqlParameter[])null);
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                SqlCommand cmd = new SqlCommand();

                int retval = 0;

                bool mustCloseConnection = false;

                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

                int i = cmd.Parameters.Count;

                retval = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();

                if ((mustCloseConnection)) connection.Close();

                return retval;
            }
            catch
            {
                return 0;
            }
        }

        public static int ExecuteNonQuery(SqlConnection connection, string spName, params object[] parameterValues)
        {
            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return 0;
            }
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            try
            {
                return ExecuteNonQuery(transaction, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return 0;
            }
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                SqlCommand cmd = new SqlCommand();

                int retval = 0;

                bool mustCloseConnection = false;

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

                retval = cmd.ExecuteNonQuery();

                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return 0;
            }
        }

        public static int ExecuteNonQuery(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                // If we receive parameter values, we need to figure out where they go

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                return 0;
            }
        }

        #endregion "ExecuteNonQuery"

        #region "ExecuteDataset"

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText)
        {
            DataSet functionReturnValue = default(DataSet);

            // Pass through the call providing null for the set of SqlParameters

            functionReturnValue = null;

            try
            {
                return ExecuteDataset(connectionString, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static DataSet ExecuteDataset(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            SqlConnection connection = default(SqlConnection);

            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                return ExecuteDataset(connection, commandType, commandText, commandParameters);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }

            return functionReturnValue;
        }

        //public static DataTable ExecuteAccesDB(string connectionString, string tableName,string condition)
        //{
        //    DataTable results = new DataTable();
        //    string commandText = string.Format("select * from {0} where {1}", tableName, condition);
        //    using (OleDbConnection conn = new OleDbConnection(connectionString))
        //    {
        //        OleDbCommand cmd = new OleDbCommand(commandText, conn);
        //        conn.Open();
        //        OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
        //        adapter.Fill(results);
        //    }
        //    return results;
        //}
        public static DataSet ExecuteDataset(string connectionString, string spName, params object[] parameterValues)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                return ExecuteDataset(connection, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static DataSet ExecuteDataset(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            SqlCommand cmd = new SqlCommand();

            DataSet ds = new DataSet();

            SqlDataAdapter dataAdatpter = null;

            bool mustCloseConnection = false;

            PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

            try
            {
                dataAdatpter = new SqlDataAdapter(cmd);

                dataAdatpter.Fill(ds);

                cmd.Parameters.Clear();
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if (((dataAdatpter != null))) dataAdatpter.Dispose();
            }

            if ((mustCloseConnection)) connection.Close();

            return ds;
        }

        public static DataSet ExecuteDataset(SqlConnection connection, string spName, params object[] parameterValues)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteDataset(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                return ExecuteDataset(transaction, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            SqlCommand cmd = new SqlCommand();

            DataSet ds = new DataSet();

            SqlDataAdapter dataAdatpter = null;

            bool mustCloseConnection = false;

            PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            try
            {
                dataAdatpter = new SqlDataAdapter(cmd);

                dataAdatpter.Fill(ds);

                cmd.Parameters.Clear();
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if (((dataAdatpter != null))) dataAdatpter.Dispose();
            }

            return ds;
        }

        public static DataSet ExecuteDataset(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        #endregion "ExecuteDataset"

        #region "ExecuteReader"

        private enum SqlConnectionOwnership
        {
            // Connection is owned and managed by SqlHelper

            Internal,

            // Connection is owned and managed by the caller

            External
        }

        private static SqlDataReader ExecuteReader(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, SqlParameter[] commandParameters, SqlConnectionOwnership connectionOwnership)
        {
            if ((connection == null)) throw new ArgumentNullException("connection");

            bool mustCloseConnection = false;

            SqlCommand cmd = new SqlCommand();

            try
            {
                SqlDataReader dataReader = default(SqlDataReader);

                PrepareCommand(cmd, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

                if (connectionOwnership == SqlConnectionOwnership.External)
                {
                    dataReader = cmd.ExecuteReader();
                }
                else
                {
                    dataReader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                }

                bool canClear = true;

                foreach (SqlParameter commandParameter in cmd.Parameters)
                {
                    if (commandParameter.Direction != ParameterDirection.Input)
                    {
                        canClear = false;
                    }
                }

                if ((canClear)) cmd.Parameters.Clear();

                return dataReader;
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                if ((mustCloseConnection)) connection.Close();

                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                return ExecuteReader(connectionString, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(ex));
            }

            return functionReturnValue;
        }

        public static SqlDataReader ExecuteReader(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.Internal);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));

                if ((connection != null)) connection.Dispose();

                throw;
            }
        }

        public static SqlDataReader ExecuteReader(string connectionString, string spName, params object[] parameterValues)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                return ExecuteReader(connection, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                return ExecuteReader(connection, (SqlTransaction)null, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(ex));
            }

            return functionReturnValue;
        }

        public static SqlDataReader ExecuteReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                // If we receive parameter values, we need to figure out where they go

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteReader(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                return ExecuteReader(transaction, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            if ((transaction == null)) throw new ArgumentNullException("transaction");

            if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

            return ExecuteReader(transaction.Connection, transaction, commandType, commandText, commandParameters, SqlConnectionOwnership.External);
        }

        public static SqlDataReader ExecuteReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteReader(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        #endregion "ExecuteReader"

        #region "ExecuteScalar"

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                return ExecuteScalar(connectionString, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(string connectionString, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            object functionReturnValue = null;

            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            functionReturnValue = null;

            SqlConnection connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                return ExecuteScalar(connection, commandType, commandText, commandParameters);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(string connectionString, string spName, params object[] parameterValues)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                return ExecuteScalar(connection, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(SqlConnection connection, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                SqlCommand cmd = new SqlCommand();

                object retval = null;

                bool mustCloseConnection = false;

                PrepareCommand(cmd, connection, (SqlTransaction)null, commandType, commandText, commandParameters, ref mustCloseConnection);

                retval = cmd.ExecuteScalar();

                cmd.Parameters.Clear();

                if ((mustCloseConnection)) connection.Close();

                return retval;
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(SqlConnection connection, string spName, params object[] parameterValues)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteScalar(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                return ExecuteScalar(transaction, commandType, commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(SqlTransaction transaction, CommandType commandType, string commandText, params SqlParameter[] commandParameters)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                SqlCommand cmd = new SqlCommand();

                object retval = null;

                bool mustCloseConnection = false;

                PrepareCommand(cmd, transaction.Connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

                retval = cmd.ExecuteScalar();

                cmd.Parameters.Clear();

                return retval;
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static object ExecuteScalar(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    return ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        #endregion "ExecuteScalar"

        #region "ExecuteXmlReader"

        public static XmlReader ExecuteXmlReader(SqlConnection connection, CommandType commandType, string commandText)
        {
            XmlReader functionReturnValue = default(XmlReader);

            functionReturnValue = null;

            try
            {
                return ExecuteXmlReader(connection, commandType.ToString(), commandText, (SqlParameter[])null);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static XmlReader ExecuteXmlReader(SqlConnection connection, string spName, params object[] parameterValues)
        {
            XmlReader functionReturnValue = default(XmlReader);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteXmlReader(connection, Convert.ToString(CommandType.StoredProcedure), spName, commandParameters);
                }
                else
                {
                    return ExecuteXmlReader(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, CommandType commandType, string commandText)
        {
            XmlReader functionReturnValue = default(XmlReader);

            functionReturnValue = null;

            try
            {
                return ExecuteXmlReader(transaction, commandType, commandText);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        public static XmlReader ExecuteXmlReader(SqlTransaction transaction, string spName, params object[] parameterValues)
        {
            XmlReader functionReturnValue = default(XmlReader);

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlParameter[] commandParameters = null;

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    return ExecuteXmlReader(transaction, Convert.ToString(CommandType.StoredProcedure), spName, commandParameters);
                }
                else
                {
                    return ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        // ExecuteXmlReader

        #endregion "ExecuteXmlReader"

        #region "FillDataset"

        public static void FillDataset(string connectionString, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            SqlConnection connection = default(SqlConnection);

            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                FillDataset(connection, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        public static void FillDataset(string connectionString, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            SqlConnection connection = default(SqlConnection);

            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                connection.Open();

                FillDataset(connection, spName, dataSet, tableNames, parameterValues);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }
        }

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            try
            {
                FillDataset(connection, commandType, commandText, dataSet, tableNames, null);
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        public static void FillDataset(SqlConnection connection, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            try
            {
                FillDataset(connection, null, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        public static void FillDataset(SqlConnection connection, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((dataSet == null)) throw new ArgumentNullException("dataSet");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
                }
                else
                {
                    FillDataset(connection, CommandType.StoredProcedure, spName, dataSet, tableNames);
                }
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames)
        {
            try
            {
                FillDataset(transaction, commandType, commandText, dataSet, tableNames, null);
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        public static void FillDataset(SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                FillDataset(transaction.Connection, transaction, commandType, commandText, dataSet, tableNames, commandParameters);
            }
            catch
            {
                //HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        public static void FillDataset(SqlTransaction transaction, string spName, DataSet dataSet, string[] tableNames, params object[] parameterValues)
        {
            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((dataSet == null)) throw new ArgumentNullException("dataSet");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                if ((parameterValues != null) && parameterValues.Length > 0)
                {
                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    AssignParameterValues(commandParameters, parameterValues);

                    FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames, commandParameters);
                }
                else
                {
                    FillDataset(transaction, CommandType.StoredProcedure, spName, dataSet, tableNames);
                }
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
        }

        private static void FillDataset(SqlConnection connection, SqlTransaction transaction, CommandType commandType, string commandText, DataSet dataSet, string[] tableNames, params SqlParameter[] commandParameters)
        {
            if ((connection == null)) throw new ArgumentNullException("connection");

            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            SqlCommand command = new SqlCommand();

            bool mustCloseConnection = false;

            PrepareCommand(command, connection, transaction, commandType, commandText, commandParameters, ref mustCloseConnection);

            SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

            try
            {
                if ((tableNames != null) && tableNames.Length > 0)
                {
                    string tableName = "Table";

                    int index = 0;

                    for (index = 0; index <= tableNames.Length - 1; index++)
                    {
                        if ((tableNames[index] == null || tableNames[index].Length == 0)) throw new ArgumentException("The tableNames parameter must contain a list of tables, a value was provided as null or empty string.", "tableNames");

                        dataAdapter.TableMappings.Add(tableName, tableNames[index]);

                        tableName = tableName + (index + 1).ToString();
                    }
                }

                // Fill the DataSet using default values for DataTable names, etc

                dataAdapter.Fill(dataSet);

                command.Parameters.Clear();
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if (((dataAdapter != null))) dataAdapter.Dispose();
            }

            if ((mustCloseConnection)) connection.Close();
        }

        #endregion "FillDataset"

        #region "UpdateDataset"

        public static void UpdateDataset(SqlCommand insertCommand, SqlCommand deleteCommand, SqlCommand updateCommand, DataSet dataSet, string tableName)
        {
            if ((insertCommand == null)) throw new ArgumentNullException("insertCommand");

            if ((deleteCommand == null)) throw new ArgumentNullException("deleteCommand");

            if ((updateCommand == null)) throw new ArgumentNullException("updateCommand");

            if ((dataSet == null)) throw new ArgumentNullException("dataSet");

            if ((tableName == null || tableName.Length == 0)) throw new ArgumentNullException("tableName");

            SqlDataAdapter dataAdapter = new SqlDataAdapter();

            try
            {
                dataAdapter.UpdateCommand = updateCommand;

                dataAdapter.InsertCommand = insertCommand;

                dataAdapter.DeleteCommand = deleteCommand;

                dataAdapter.Update(dataSet, tableName);

                dataSet.AcceptChanges();
            }
            catch
            {
                ////HttpContext.Current.Response.Write(Convert.ToString(err));
            }
            finally
            {
                if (((dataAdapter != null))) dataAdapter.Dispose();
            }
        }

        #endregion "UpdateDataset"

        #region "CreateCommand"

        public static SqlCommand CreateCommand(SqlConnection connection, string spName, params string[] sourceColumns)
        {
            SqlCommand functionReturnValue = default(SqlCommand);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                SqlCommand cmd = new SqlCommand(spName, connection);

                cmd.CommandType = CommandType.StoredProcedure;

                if ((sourceColumns != null) && sourceColumns.Length > 0)
                {
                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    int index = 0;

                    for (index = 0; index <= sourceColumns.Length - 1; index++)
                    {
                        commandParameters[index].SourceColumn = sourceColumns[index];
                    }

                    AttachParameters(cmd, commandParameters);
                }

                functionReturnValue = cmd;
            }
            catch
            {
                // //HttpContext.Current.Response.Write(Convert.ToString(err));
            }

            return functionReturnValue;
        }

        #endregion "CreateCommand"

        #region "ExecuteNonQueryTypedParams"

        public static int ExecuteNonQueryTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            int functionReturnValue = 0;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteNonQuery(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified SqlConnection

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.

        // Parameters:

        // -connection:a valid SqlConnection object

        // -spName: the name of the stored procedure

        // -dataRow:The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // an int representing the number of rows affected by the command

        public static int ExecuteNonQueryTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            int functionReturnValue = 0;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteNonQuery(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns no resultset) against the specified

        // SqlTransaction using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.

        // Parameters:

        // -transaction:a valid SqlTransaction object

        // -spName:the name of the stored procedure

        // -dataRow:The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // an int representing the number of rows affected by the command

        public static int ExecuteNonQueryTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            int functionReturnValue = 0;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        #endregion "ExecuteNonQueryTypedParams"

        #region "ExecuteDatasetTypedParams"

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in

        // the connection string using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.

        // Parameters:

        // -connectionString: A valid connection string for a SqlConnection

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // a dataset containing the resultset generated by the command

        public static DataSet ExecuteDatasetTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection

        // using the dataRow column values as the store procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.

        // Parameters:

        // -connection: A valid SqlConnection object

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // a dataset containing the resultset generated by the command

        public static DataSet ExecuteDatasetTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteDataset(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on row values.

        // Parameters:

        // -transaction: A valid SqlTransaction object

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // a dataset containing the resultset generated by the command

        public static DataSet ExecuteDatasetTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            DataSet functionReturnValue = default(DataSet);

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteDataset(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        #endregion "ExecuteDatasetTypedParams"

        #region "ExecuteReaderTypedParams"

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the database specified in

        // the connection string using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -connectionString: A valid connection string for a SqlConnection

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // a SqlDataReader containing the resultset generated by the command

        public static SqlDataReader ExecuteReaderTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteReader(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -connection: A valid SqlConnection object

        // -spName: The name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // a SqlDataReader containing the resultset generated by the command

        public static SqlDataReader ExecuteReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteReader(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -transaction: A valid SqlTransaction object

        // -spName" The name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // a SqlDataReader containing the resultset generated by the command

        public static SqlDataReader ExecuteReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            SqlDataReader functionReturnValue = default(SqlDataReader);

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteReader(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        #endregion "ExecuteReaderTypedParams"

        #region "ExecuteScalarTypedParams"

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the database specified in

        // the connection string using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -connectionString: A valid connection string for a SqlConnection

        // -spName: The name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // An object containing the value in the 1x1 resultset generated by the command

        public static object ExecuteScalarTypedParams(string connectionString, string spName, DataRow dataRow)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connectionString, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteScalar(connectionString, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlConnection

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -connection: A valid SqlConnection object

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // an object containing the value in the 1x1 resultset generated by the command

        public static object ExecuteScalarTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteScalar(connection, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a 1x1 resultset) against the specified SqlTransaction

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -transaction: A valid SqlTransaction object

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // an object containing the value in the 1x1 resultset generated by the command

        public static object ExecuteScalarTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            object functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName, commandParameters);
                }
                else
                {
                    functionReturnValue = SqlHelper.ExecuteScalar(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        #endregion "ExecuteScalarTypedParams"

        #region "ExecuteXmlReaderTypedParams"

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlConnection

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -connection: A valid SqlConnection object

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // an XmlReader containing the resultset generated by the command

        public static XmlReader ExecuteXmlReaderTypedParams(SqlConnection connection, string spName, DataRow dataRow)
        {
            XmlReader functionReturnValue = default(XmlReader);

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // If the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = ExecuteXmlReader(connection, Convert.ToString(CommandType.StoredProcedure), spName, commandParameters);
                }
                else
                {
                    functionReturnValue = ExecuteXmlReader(connection, Convert.ToString(CommandType.StoredProcedure), spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // Execute a stored procedure via a SqlCommand (that returns a resultset) against the specified SqlTransaction

        // using the dataRow column values as the stored procedure' s parameters values.

        // This method will query the database to discover the parameters for the

        // stored procedure (the first time each stored procedure is called), and assign the values based on parameter order.

        // Parameters:

        // -transaction: A valid SqlTransaction object

        // -spName: the name of the stored procedure

        // -dataRow: The dataRow used to hold the stored procedure' s parameter values.

        // Returns:

        // an XmlReader containing the resultset generated by the command

        public static XmlReader ExecuteXmlReaderTypedParams(SqlTransaction transaction, string spName, DataRow dataRow)
        {
            XmlReader functionReturnValue = default(XmlReader);

            functionReturnValue = null;

            try
            {
                if ((transaction == null)) throw new ArgumentNullException("transaction");

                if ((transaction != null) && (transaction.Connection == null)) throw new ArgumentException("The transaction was rollbacked or commited, please provide an open transaction.", "transaction");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                // if the row has values, the store procedure parameters must be initialized

                if (((dataRow != null) && dataRow.ItemArray.Length > 0))
                {
                    // Pull the parameters for this stored procedure from the parameter cache (or discover them &amp; populate the cache)

                    SqlParameter[] commandParameters = ODBCHelperParameterCache.GetSpParameterSet(transaction.Connection, spName);

                    // Set the parameters values

                    AssignParameterValues(commandParameters, dataRow);

                    functionReturnValue = ExecuteXmlReader(transaction, Convert.ToString(CommandType.StoredProcedure), spName, commandParameters);
                }
                else
                {
                    functionReturnValue = ExecuteXmlReader(transaction, CommandType.StoredProcedure, spName);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        //public static bool ExecuteNonQuery(SqlCommand _cmd, bool p, string p_3)

        //{
        //    throw new NotImplementedException();

        //}
    }

        #endregion "ExecuteXmlReaderTypedParams"

    // SqlHelper

    public sealed class ODBCHelperParameterCache
    {
        #region "private methods, variables, and constructors"

        // Since this class provides only static methods, make the default constructor private to prevent

        // instances from being created with "new odbcHelperParameterCache()".

        private ODBCHelperParameterCache()
        {
        }

        // New

        private static Hashtable paramCache = Hashtable.Synchronized(new Hashtable());

        // resolve at run time the appropriate set of SqlParameters for a stored procedure

        // Parameters:

        // - connectionString - a valid connection string for a SqlConnection

        // - spName - the name of the stored procedure

        // - includeReturnValueParameter - whether or not to include their return value parameter>

        // Returns: SqlParameter()

        private static SqlParameter[] DiscoverSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter, params object[] parameterValues)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                SqlCommand cmd = new SqlCommand(spName, connection);

                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] discoveredParameters = null;

                connection.Open();

                SqlCommandBuilder.DeriveParameters(cmd);

                connection.Close();

                if (!includeReturnValueParameter)
                {
                    cmd.Parameters.RemoveAt(0);
                }

                discoveredParameters = new SqlParameter[cmd.Parameters.Count];

                cmd.Parameters.CopyTo(discoveredParameters, 0);

                // Init the parameters with a DBNull value

                //SqlParameter discoveredParameter = default(SqlParameter);

                foreach (SqlParameter discoveredParameter in discoveredParameters)
                {
                    discoveredParameter.Value = DBNull.Value;
                }

                return discoveredParameters;
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // DiscoverSpParameterSet

        // Deep copy of cached SqlParameter array

        private static SqlParameter[] CloneParameters(SqlParameter[] originalParameters)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                int i = 0;

                int j = originalParameters.Length - 1;

                SqlParameter[] clonedParameters = new SqlParameter[j + 1];

                for (i = 0; i <= j; i++)
                {
                    clonedParameters[i] = (SqlParameter)((ICloneable)originalParameters[i]).Clone();
                }

                return clonedParameters;
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // CloneParameters

        #endregion "private methods, variables, and constructors"

        #region "caching functions"

        // add parameter array to the cache

        // Parameters

        // -connectionString - a valid connection string for a SqlConnection

        // -commandText - the stored procedure name or T-SQL command

        // -commandParameters - an array of SqlParamters to be cached

        public static void CacheParameterSet(string connectionString, string commandText, params SqlParameter[] commandParameters)
        {
            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((commandText == null || commandText.Length == 0)) throw new ArgumentNullException("commandText");

                string hashKey = connectionString + ":" + commandText;

                paramCache[hashKey] = commandParameters;
            }
            catch
            {
            }
        }

        // CacheParameterSet

        // retrieve a parameter array from the cache

        // Parameters:

        // -connectionString - a valid connection string for a SqlConnection

        // -commandText - the stored procedure name or T-SQL command

        // Returns: An array of SqlParamters

        public static SqlParameter[] GetCachedParameterSet(string connectionString, string commandText)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

                if ((commandText == null || commandText.Length == 0)) throw new ArgumentNullException("commandText");

                string hashKey = connectionString + ":" + commandText;

                SqlParameter[] cachedParameters = (SqlParameter[])paramCache[hashKey];

                if (cachedParameters == null)
                {
                    return null;
                }
                else
                {
                    return CloneParameters(cachedParameters);
                }
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // GetCachedParameterSet

        #endregion "caching functions"

        #region "Parameter Discovery Functions"

        // Retrieves the set of SqlParameters appropriate for the stored procedure.

        // This method will query the database for this information, and then store it in a cache for future requests.

        // Parameters:

        // -connectionString - a valid connection string for a SqlConnection

        // -spName - the name of the stored procedure

        // Returns: An array of SqlParameters

        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                return GetSpParameterSet(connectionString, spName, false);
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.

        // This method will query the database for this information, and then store it in a cache for future requests.

        // Parameters:

        // -connectionString - a valid connection string for a SqlConnection

        // -spName - the name of the stored procedure

        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results

        // Returns: An array of SqlParameters

        public static SqlParameter[] GetSpParameterSet(string connectionString, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] functionReturnValue = null;

            if ((connectionString == null || connectionString.Length == 0)) throw new ArgumentNullException("connectionString");

            SqlConnection connection = default(SqlConnection);

            connection = null;

            try
            {
                connection = new SqlConnection(connectionString);

                functionReturnValue = GetSpParameterSetInternal(connection, spName, includeReturnValueParameter);
            }
            finally
            {
                if ((connection != null)) connection.Dispose();
            }

            return functionReturnValue;
        }

        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.

        // This method will query the database for this information, and then store it in a cache for future requests.

        // Parameters:

        // -connection - a valid SqlConnection object

        // -spName - the name of the stored procedure

        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results

        // Returns: An array of SqlParameters

        public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                functionReturnValue = GetSpParameterSet(connection, spName, false);
            }
            catch
            {
            }

            return functionReturnValue;
        }

        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.

        // This method will query the database for this information, and then store it in a cache for future requests.

        // Parameters:

        // -connection - a valid SqlConnection object

        // -spName - the name of the stored procedure

        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results

        // Returns: An array of SqlParameters

        public static SqlParameter[] GetSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            if ((connection == null)) throw new ArgumentNullException("connection");

            SqlConnection clonedConnection = default(SqlConnection);

            clonedConnection = null;

            try
            {
                clonedConnection = (SqlConnection)(((ICloneable)connection).Clone());

                functionReturnValue = GetSpParameterSetInternal(clonedConnection, spName, includeReturnValueParameter);
            }
            finally
            {
                if ((clonedConnection != null)) clonedConnection.Dispose();
            }

            return functionReturnValue;
        }

        // GetSpParameterSet

        // Retrieves the set of SqlParameters appropriate for the stored procedure.

        // This method will query the database for this information, and then store it in a cache for future requests.

        // Parameters:

        // -connection - a valid SqlConnection object

        // -spName - the name of the stored procedure

        // -includeReturnValueParameter - a bool value indicating whether the return value parameter should be included in the results

        // Returns: An array of SqlParameters

        private static SqlParameter[] GetSpParameterSetInternal(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            SqlParameter[] functionReturnValue = null;

            functionReturnValue = null;

            try
            {
                if ((connection == null)) throw new ArgumentNullException("connection");

                SqlParameter[] cachedParameters = null;

                string hashKey = null;

                if ((spName == null || spName.Length == 0)) throw new ArgumentNullException("spName");

                hashKey = connection.ConnectionString + ":" + spName + (includeReturnValueParameter == true ? ":include ReturnValue Parameter" : "").ToString();

                cachedParameters = (SqlParameter[])paramCache[hashKey];

                if ((cachedParameters == null))
                {
                    SqlParameter[] spParameters = DiscoverSpParameterSet(connection, spName, includeReturnValueParameter);

                    paramCache[hashKey] = spParameters;

                    cachedParameters = spParameters;
                }

                return CloneParameters(cachedParameters);
            }
            catch
            {
            }

            return functionReturnValue;
        }
    }

    // GetSpParameterSet

        #endregion "Parameter Discovery Functions"
}