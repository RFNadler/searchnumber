using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace SearchCombinations.repository
{
    public class DBASE
    {
        public string TableName { get; set; }
        public enum TypeExecute
        {
            SQL = 1,
            UPDATE = 2,
            DELETE = 3,
            INSERT = 4
        }

        private Dictionary<string, object> parameters;

        private Dictionary<string, object> Parameters
        {
            get
            {
                if (parameters == null)
                {
                    parameters = new Dictionary<string, object>();
                }
                return parameters;
            }
        }

        private DbConnection DConnection;
        private DbProviderFactory DPFactory;
        private DbTransaction DTransection;
        private DbCommand DCommand;

        private string strSelect = "";
        private string strUpdate = "";
        private string strDelete = "";
        private string strInsert = "";
        private string strWhere = "";

        public void AddParameter(string parameterName, object parameterValue)
        {
            Parameters.Add(parameterName, parameterValue);
        }

        public void EraseParameter()
        {
            if (Parameters.Count > 0)
            {
                DCommand.Parameters.Clear();
                Parameters.Clear();
            }

        }

        public TypeExecute TypeCommand { get; set; }
        public string SQL { get; set; }
        public string ConnectionString { get; set; }
        public string ErrorDescription { get; set; }

        private string getParameterString()
        {
            return "?";
        }

        private Boolean createConnection()
        {
            try
            {
                string ConnectionString = ConfigurationManager.ConnectionStrings["NTCONECTION"].ToString();

                DPFactory = DbProviderFactories.GetFactory("MySql.Data.MySqlClient");
                DConnection = DPFactory.CreateConnection();
                DConnection.ConnectionString = ConnectionString;
                DConnection.Open();

                return true;
            }
            catch (Exception ex)
            {
                string strErro;
                string strTemp = ex.Message.ToString();
                strErro = strTemp.Replace("\r", " ");
                strErro = strTemp.Replace("\n", " ");
                ErrorDescription = strErro;
            }
            return false;
        }

        public void closeConnection(Boolean pCloseCommand)
        {
            try
            {
                if (pCloseCommand)
                    DCommand.Dispose();

                if (DConnection.State == ConnectionState.Open)
                {
                    DConnection.Close();
                    DConnection.Dispose();
                }
            }
            catch (Exception ex)
            {
                string strErro;
                string strTemp = ex.Message.ToString();
                strErro = strTemp.Replace("\r", " ");
                strErro = strTemp.Replace("\n", " ");
                ErrorDescription = strErro;
            }
        }

        private void mountSQLCommand()
        {
            int intCount = 0;
            string strTempUpdate = "";
            string strTempInsert = "";
            string strTempInsertParam = "";
            string strTempSelect = "";

            foreach (string key in Parameters.Keys)
            {
                if (intCount == 0)
                {
                    strTempUpdate = key.ToString() + " = @" + key.ToString();
                    strTempInsert = key.ToString();
                    strTempInsertParam = "@" + key.ToString();
                    strTempSelect = key.ToString();
                    strDelete = key.ToString() + " = @" + key.ToString();
                }
                else
                {
                    strTempUpdate += ", " + key.ToString() + " = @" + key.ToString();
                    strTempInsert += ", " + key.ToString();
                    strTempInsertParam += ", @" + key.ToString();
                    strTempSelect += ", " + key.ToString();

                }
                intCount++;

            }

            strSelect = " select " + strTempSelect + " from " + TableName;
            strUpdate = " update " + TableName + " set " + strTempUpdate;
            strInsert = " insert into " + TableName + "(" + strTempInsert + ") values (" + strTempInsertParam + ")";

            switch (TypeCommand)
            {
                case TypeExecute.SQL:
                    SQL = strSelect;
                    break;

                case TypeExecute.UPDATE:
                    SQL = strUpdate;
                    break;

                case TypeExecute.DELETE:
                    SQL = strDelete;
                    break;

                case TypeExecute.INSERT:
                    SQL = strInsert;
                    break;
            }

        }

        private Boolean mountCommand()
        {
            try
            {

                DCommand = DPFactory.CreateCommand();
                DCommand.Connection = DConnection;
                DCommand.CommandType = System.Data.CommandType.Text;
                DCommand.CommandText = SQL;

                return true;

            }
            catch (Exception ex)
            {
                string strErro;
                string strTemp = ex.Message.ToString();
                strErro = strTemp.Replace("\r", " ");
                strErro = strTemp.Replace("\n", " ");
                ErrorDescription = strErro;
                DCommand.Dispose();

            }
            return false;
        }

        public Boolean ExecutaComandoSQL()
        {
            Boolean blnOpen = false;
            int intCount = 0;
            DbParameter[] DParameter;

            try
            {
                blnOpen = createConnection();
                if (blnOpen)
                    mountSQLCommand();

                if (mountCommand())
                {
                    if (Parameters.Count > 0)
                    {
                        DParameter = new DbParameter[Parameters.Count];
                        foreach (string key in Parameters.Keys)
                        {
                            DParameter[intCount] = DPFactory.CreateParameter();
                            DParameter[intCount].ParameterName = "@" + key.ToString();
                            DParameter[intCount].Value = (Parameters[key] == null) ? DBNull.Value : Parameters[key];
                            DParameter[intCount].Direction = System.Data.ParameterDirection.Input;

                            DCommand.Parameters.Add(DParameter[intCount]);

                            intCount++;
                        }
                        
                    }
                    
                    DCommand.ExecuteNonQuery();

                    return true;
                }
            }
            catch (Exception ex)
            {
                string strErro;
                string strTemp = ex.Message.ToString();
                strErro = strTemp.Replace("\r", " ");
                strErro = strTemp.Replace("\n", " ");
                ErrorDescription = strErro;
            }
            if (blnOpen)
                closeConnection(true);

            return false;
        }

        public DbDataReader DataReader()
        {
            DbDataReader dr = null;
            DbParameter[] DParameter;
            Boolean blnOpen = false;
            string strTemp = "";
            int intCount = 0;
            ErrorDescription = "";

            try
            {
                if (DCommand != null)
                    DCommand.Dispose();

                blnOpen = createConnection();
                if (blnOpen)

                DCommand = DPFactory.CreateCommand();
                DCommand.Connection = DConnection;
                DCommand.CommandType = System.Data.CommandType.Text;
                DCommand.CommandText = SQL;

                if (Parameters.Count > 0)
                {
                    DParameter = new DbParameter[Parameters.Count];
                    foreach (string key in Parameters.Keys)
                    {
                        DParameter[intCount] = DPFactory.CreateParameter();
                        DParameter[intCount].ParameterName = "@" + key.ToString();
                        DParameter[intCount].Value = (Parameters[key] == null) ? DBNull.Value : Parameters[key];
                        DParameter[intCount].Direction = System.Data.ParameterDirection.Input;

                        DCommand.Parameters.Add(DParameter[intCount]);

                        intCount++;
                    }

                }

                dr = DCommand.ExecuteReader(CommandBehavior.CloseConnection);

                return dr;
                            
            }
            catch (Exception ex)
            {
                string strErro;
                strTemp = ex.Message.ToString();
                strErro = strTemp.Replace("\r", " ");
                strErro = strTemp.Replace("\n", " ");
                ErrorDescription = strErro;
            }
            
            return dr;
        }
    }
}