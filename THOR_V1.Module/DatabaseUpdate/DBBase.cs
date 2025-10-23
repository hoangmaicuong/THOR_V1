using System;
using System.Data;
using System.Data.SqlClient;
using THOR_V1.Module.BusinessObjects;

namespace THOR_V1.Module.DatabaseUpdate
{
    public class DBBase
    {
        private SqlConnection m_cn = new SqlConnection();

        public DBBase() { }

        public void OpenCN()
        {
            try
            {

                if (m_cn != null && m_cn.State == ConnectionState.Open)
                    return;

                ReadConnectionString readName = new ReadConnectionString();
                String strConnectionString = readName.ReadCnnStrName();

                if (string.IsNullOrEmpty(strConnectionString))
                    return;
                m_cn = new SqlConnection(strConnectionString);
                m_cn.Open();
            }
            catch (Exception ex)
            {
                //Log.Write("OpenCN", ex.Source, ex.Message, ex.StackTrace);
            }
        }

        public void OpenCN(string strConnectionString)
        {
            try
            {
                if (m_cn != null && m_cn.State == ConnectionState.Open)
                    return;

                if (string.IsNullOrEmpty(strConnectionString))
                    return;
                m_cn = new SqlConnection(strConnectionString);
                m_cn.Open();
            }
            catch (Exception ex)
            {
                //Log.Write("OpenCN", ex.Source, ex.Message, ex.StackTrace); 
            }
        }

        public void OpenCN(string dataSrc, string dbName, string userName, string password)
        {
            try
            {
                if (m_cn != null && m_cn.State == ConnectionState.Open)
                    return;

                m_cn = new SqlConnection();
                m_cn = new SqlConnection(string.Format("server={0};uid={1};pwd={2};database={3}", dataSrc, userName, password, dbName));
                m_cn.Open();
            }
            catch { }
        }

        public void Close()
        {
            try
            {
                if (m_cn != null && m_cn.State == ConnectionState.Open)
                {
                    m_cn.Close();
                    m_cn.Dispose();
                }
            }
            catch (Exception ex)
            {
                //Log.Write("Close", ex.Source, ex.Message, ex.StackTrace); 
            }

        }

        private SqlCommand CreateCommand(string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sqlString)) return new SqlCommand();
            if (m_cn.State != ConnectionState.Open)
                this.OpenCN();
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = cmdType;
            if (parameters != null) sqlCmd.Parameters.AddRange(parameters);
            sqlCmd.Connection = m_cn;
            sqlCmd.CommandText = sqlString;
            return sqlCmd;
        }

        private SqlCommand CreateCommand(string strConnection, string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            if (string.IsNullOrEmpty(sqlString)) return new SqlCommand();
            if (m_cn.State != ConnectionState.Open)
                this.OpenCN(strConnection);
            SqlCommand sqlCmd = new SqlCommand();
            sqlCmd.CommandType = cmdType;
            if (parameters != null) sqlCmd.Parameters.AddRange(parameters);
            sqlCmd.Connection = m_cn;
            sqlCmd.CommandText = sqlString;
            return sqlCmd;
        }

        public int ExecuteNonQuery(string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            try
            {
                SqlCommand sqlCmd = CreateCommand(sqlString, cmdType, parameters);
                return sqlCmd.ExecuteNonQuery();
                if (m_cn.State == ConnectionState.Open)
                    this.Close();
            }
            catch (Exception ex)
            {
                //Log.Write("ExcuteNonQuery", ex.Source, ex.Message, ex.StackTrace);
                return 0;
            }
        }

        public int ExecuteNonQuery(string strConnect, string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            try
            {
                SqlCommand sqlCmd = CreateCommand(strConnect, sqlString, cmdType, parameters);
                return sqlCmd.ExecuteNonQuery();
                if (m_cn.State == ConnectionState.Open)
                    this.Close();
            }
            catch (Exception ex)
            {
                //Log.Write("ExcuteNonQuery", ex.Source, ex.Message, ex.StackTrace);
                return 0;
            }
        }


        public object ExecuteScalar(string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            try
            {
                SqlCommand sqlCmd = CreateCommand(sqlString, cmdType, parameters);
                return sqlCmd.ExecuteScalar();
                if (m_cn.State == ConnectionState.Open)
                    this.Close();

            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteScalar", ex.Source, ex.Message, ex.StackTrace);
                return -1;
            }
        }

        public object ExecuteScalar(string strConnection, string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            try
            {
                SqlCommand sqlCmd = CreateCommand(strConnection, sqlString, cmdType, parameters);
                return sqlCmd.ExecuteScalar();
                if (m_cn.State == ConnectionState.Open)
                    this.Close();
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteScalar", ex.Source, ex.Message, ex.StackTrace);
                return 0;
            }
        }


        public SqlDataReader ExecuteReader(string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            try
            {
                SqlCommand sqlCmd = CreateCommand(sqlString, cmdType, parameters);
                return sqlCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteReader", ex.Source, ex.Message, ex.StackTrace);
                return null;
            }
        }

        public SqlDataReader ExecuteReader(string strConnection, string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            try
            {
                SqlCommand sqlCmd = CreateCommand(strConnection, sqlString, cmdType, parameters);
                return sqlCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteReader", ex.Source, ex.Message, ex.StackTrace);
                return null;
            }
        }

        public DataSet ExecuteQuery(string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand sqlCmd = CreateCommand(sqlString, cmdType, parameters);
                SqlDataAdapter adpter = new SqlDataAdapter();
                adpter.SelectCommand = sqlCmd;
                adpter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteQuery", ex.Source, ex.Message, ex.StackTrace);
                return null;
            }
        }

        public DataSet ExecuteQuery(string strConnection, string sqlString, CommandType cmdType, SqlParameter[] parameters)
        {
            DataSet ds = new DataSet();
            try
            {
                SqlCommand sqlCmd = CreateCommand(strConnection, sqlString, cmdType, parameters);
                SqlDataAdapter adpter = new SqlDataAdapter();
                adpter.SelectCommand = sqlCmd;
                adpter.Fill(ds);
                return ds;
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteQuery", ex.Source, ex.Message, ex.StackTrace);
                return null;
            }
        }

        public SqlParameter[] BSSqlParameter(string[] sqlParams, SqlDbType[] dbTypes, object[] vals)
        {
            if (sqlParams == null || dbTypes == null) return null;
            int nCount = (sqlParams.Length > dbTypes.Length ? dbTypes.Length : sqlParams.Length);
            SqlParameter[] parameters = new SqlParameter[nCount];
            for (int i = 0; i < nCount; i++)
            {
                parameters[i] = new SqlParameter(sqlParams[i], dbTypes[i]);
                parameters[i].Value = vals[i];
            }
            return parameters;
        }

        public SqlDataReader ExecuteReader(string sqlString, CommandType cmdType, SqlParameter[] parameters, SqlTransaction trans)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlString)) return null;
                if (m_cn == null || m_cn.State != ConnectionState.Open)
                    this.OpenCN();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = cmdType;
                if (parameters != null) sqlCmd.Parameters.AddRange(parameters);

                sqlCmd.Connection = m_cn;
                sqlCmd.CommandText = sqlString;
                if (trans != null)
                    sqlCmd.Transaction = trans;
                return sqlCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteReader", ex.Source, ex.Message, ex.StackTrace);
                return null;
            }
        }

        SqlTransaction mTransaction;

        public SqlTransaction MTransaction
        {
            get { return mTransaction; }
            set { mTransaction = value; }
        }

        public SqlTransaction BeginTransaction()
        {
            try
            {
                //if (string.IsNullOrEmpty(sqlString)) return null;
                if (m_cn == null || m_cn.State != ConnectionState.Open)
                    this.OpenCN();
                return m_cn.BeginTransaction();
            }
            catch { return null; }
            return null;
        }

        public void CommitTransaction(SqlTransaction trans)
        {
            try
            {
                if (trans == null) return;
                trans.Commit();
            }
            catch { }
        }

        public void RollbackTransaction(SqlTransaction trans)
        {
            try
            {
                if (trans == null) return;
                trans.Rollback();
            }
            catch { }
        }

        public int ExecuteNonQuery(string sqlString, CommandType cmdType, SqlParameter[] parameters, SqlTransaction trans)
        {
            try
            {
                if (string.IsNullOrEmpty(sqlString) || parameters == null) return 0;
                if (m_cn == null || m_cn.State != ConnectionState.Open)
                    this.OpenCN();
                SqlCommand sqlCmd = new SqlCommand();
                sqlCmd.CommandType = cmdType;
                sqlCmd.Parameters.AddRange(parameters);

                sqlCmd.Connection = m_cn;
                sqlCmd.CommandText = sqlString;
                if (trans != null)
                    sqlCmd.Transaction = trans;
                return sqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //Log.Write("ExecuteNonQuery", ex.Source, ex.Message, ex.StackTrace);
                return 0;
            }
        }

    }
}
