﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Interface;
using LinqToDB.Mapping;
using System.Reflection;

namespace NK.Data
{
    /// <summary>
    /// MSSQL
    /// </summary>
    public partial class MSSql : DbConnectionHelper, IDisposable, iDataBase
    {

        #region 构造

        private void init()
        {
            chksql = " Select Convert(varchar(10),Getdate(),111)";
            if (Conn == null)
                Conn = new SqlConnection(this.Connection);
            initialization();
        }

        public MSSql(DBInfo info):base(info)
        {
            dy = DBType.SQLite;
            ClassName = this.GetType().ToString();
        }
        public MSSql(string connection = "", int Timeouts = 60) : base(connection, Timeouts)
        {
            dy = DBType.SQLite;
            ClassName = this.GetType().ToString();
        }

        public MSSql(DbConnection Connection) : base(Connection)
        {
            dy = DBType.SQLite;
            ClassName = this.GetType().ToString();
        }

        ~MSSql()
        {
            Dispose(false);
        }

        #endregion

        #region 方法

        public DbConnection GetConnection()
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                SqlConnection conn = new SqlConnection(this.Connection);
                return conn;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return null;
            }
        }

        public bool CheckConnection(out string ErrMsg)
        {
            ErrMsg = "";
            SqlConnection conn = new SqlConnection(this.Connection);
            try
            {
                conn.Open();
                System.Threading.Thread.Sleep(100);
                conn.Open();
                return true;
            }
            catch (Exception ex)
            {
                ErrMsg = ex.Message;
                return false;
            }
            finally
            {
                conn.Dispose();
                conn = null;
            }
        }

        public int ExecuteNonQuery(string sql)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                init();
                if (log != null) log(ClassName, MethodName, Log_Type.Test, sql);
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                int res = cmd.ExecuteNonQuery();
                End();
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return -1;
            }
        }
         
        public bool IsExist(string sql)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (log != null) log(ClassName, MethodName, Log_Type.Test, sql);
            try
            {
                init();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                SqlDataReader da = cmd.ExecuteReader();
                bool res = da.Read();
                da.Close();
                End();
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        public Dictionary<string, object> Find(string sql)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Dictionary<string, object> res = new Dictionary<string, object>();
            if (log != null) log(ClassName, MethodName, Log_Type.Test, sql);
            try
            {
                init();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                SqlDataReader da = cmd.ExecuteReader();
                if (da.Read())
                {
                    if (da.FieldCount > 0)
                    {
                        for (int i = 0; i < da.FieldCount; i++)
                        {
                            if (da.IsDBNull(i))
                                res.Add(da.GetName(i), null);
                            else
                                res.Add(da.GetName(i), da[i]);
                        }
                    }
                }
                da.Close();
                End();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }

        public object ExecuteScalar(string sql)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (log != null) log(ClassName, MethodName, Log_Type.Test, sql);
            try
            {
                init();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandText = sql;
                cmd.CommandTimeout = this.Timeout * 1000;
                object res = null;
                try
                {
                    res = cmd.ExecuteScalar();
                    if (res is DBNull)
                        res = null;
                }
                catch
                { }
                End();
                return res;
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return null;
            }
        }

        public bool TableIsExist(string TableName)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (string.IsNullOrEmpty(TableName))
                return false;
            try
            {
                init();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandText = "select count(1) from " + TableName;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.ExecuteScalar();
                End();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public DataTable getDataTableByRam(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            string Sql = "";
            if (!string.IsNullOrEmpty(Where))
            {
                if (!Where.ToLower().Trim().Contains("where"))
                    Where = " WHERE " + Where;
            }
            if (string.IsNullOrEmpty(DisplayField))
                DisplayField = "*";
            if (!string.IsNullOrEmpty(OrderBy))
            {
                if (!OrderBy.ToLower().Trim().Contains("order"))
                    OrderBy = " ORDER BY " + OrderBy;
            }
            if (!string.IsNullOrEmpty(GroupBy))
            {
                if (!GroupBy.ToLower().Trim().Contains("group"))
                    GroupBy = " GROUP BY " + GroupBy;
            }
            Sql = "SELECT " + DisplayField + " FROM  " + TableName + " " + Where + " " + GroupBy + " " + OrderBy;
            if (log != null) log(ClassName, MethodName, Log_Type.Test, Sql);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            RecodeCount = 0;
            PageCount = 0;
            try
            {
                init();
                Page(TableName, Where, GroupBy, PageSize, out RecodeCount, out PageCount);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = (SqlConnection)Conn;
                da.SelectCommand.CommandTimeout = this.Timeout * 1000;
                da.SelectCommand.CommandText = Sql;
                da.Fill(ds, (PageIndex - 1) * PageSize, PageSize, TableName);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                else
                    dt = null;
                End();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return dt;
        }

        public DataTable getDataTableByDB(int PageIndex, int PageSize, string DisplayField, string TableName, string Where, string OrderField, string OrderBy, string GroupBy, out int RecodeCount, out int PageCount)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            string Sql = "";
            if (!string.IsNullOrEmpty(Where))
            {
                if (!Where.ToLower().Trim().Contains("where"))
                    Where = " WHERE " + Where;
            }
            if (string.IsNullOrEmpty(DisplayField))
                DisplayField = "*";
            if (!string.IsNullOrEmpty(OrderBy))
            {
                if (!OrderBy.ToLower().Trim().Contains("order"))
                    OrderBy = " ORDER BY " + OrderBy;
            }
            if (!string.IsNullOrEmpty(GroupBy))
            {
                if (!GroupBy.ToLower().Trim().Contains("group"))
                    GroupBy = " GROUP BY " + GroupBy;
            }
            Sql = "SELECT " + DisplayField + " FROM  " + TableName + " " + Where + " " + GroupBy + " " + OrderBy + " limit " + PageSize.ToString() + " OFFSET " + (PageSize * PageIndex).ToString();
            if (log != null) log(ClassName, MethodName, Log_Type.Test, Sql);
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            RecodeCount = 0;
            PageCount = 0;
            try
            {
                init();
                Page(TableName, Where, GroupBy, PageSize, out RecodeCount, out PageCount);
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = new SqlCommand();
                da.SelectCommand.Connection = (SqlConnection)Conn;
                da.SelectCommand.CommandTimeout = this.Timeout * 1000;
                da.SelectCommand.CommandText = Sql;
                da.Fill(ds, 0, PageSize, TableName);
                if (ds.Tables.Count > 0)
                    dt = ds.Tables[0];
                else
                    dt = null;
                End();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return dt;
        }

        public override DataTable getDataTable(string sql, int PageIndex, int PageSize, out int RecCount, out int PageCount, string TableName = "")
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (log != null) log(ClassName, MethodName, Log_Type.Test, sql);
            PageCount = 0;
            RecCount = 0;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                init();
                SqlCommand CMD = new SqlCommand();
                CMD.Connection = (SqlConnection)Conn;
                CMD.CommandTimeout = this.Timeout * 1000;
                CMD.CommandText = "SELECT COUNT(1) AS NUM FROM (" + sql + ") Tab";
                RecCount = Convert.ToInt32(CMD.ExecuteScalar());
                CMD.CommandText = sql;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = CMD;
                if (string.IsNullOrEmpty(TableName)) TableName = "Query";
                da.Fill(ds, (PageIndex - 1) * PageSize, PageSize, TableName);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = null;
                }
                if (PageSize == 0)
                    PageCount = RecCount;
                else if (RecCount % PageSize == 0)
                    PageCount = RecCount / PageSize;
                else
                    PageCount = (RecCount / PageSize) + 1;
                End();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return dt;
        }

        public override DataTable getDataTable(string sql, int PageSize, out int RecCount, out int PageCount, string TableName = "")
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            if (log != null) log(ClassName, MethodName, Log_Type.Test, sql);
            PageCount = 0;
            RecCount = 0;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            try
            {
                init();
                SqlCommand CMD = new SqlCommand();
                CMD.Connection = (SqlConnection)Conn;
                CMD.CommandTimeout = this.Timeout * 1000;
                CMD.CommandText = "SELECT COUNT(1) AS NUM FROM (" + sql + ") Tab";
                RecCount = Convert.ToInt32(CMD.ExecuteScalar());
                CMD.CommandText = sql;
                SqlDataAdapter da = new SqlDataAdapter();
                da.SelectCommand = CMD;
                if (string.IsNullOrEmpty(TableName))
                    da.Fill(ds);
                else
                    da.Fill(ds, TableName);
                if (ds.Tables.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = null;
                }
                if (PageSize == 0)
                    PageCount = RecCount;
                else if (RecCount % PageSize == 0)
                    PageCount = RecCount / PageSize;
                else
                    PageCount = (RecCount / PageSize) + 1;
                End();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return dt;
        }
        public bool CheckField(string TableName, string Field, out System.Type FieldType, out bool CanBeNull, out bool IsPrimaryKey)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            bool res = false;
            FieldType = typeof(object);
            CanBeNull = false;
            IsPrimaryKey = false;
            if (string.IsNullOrEmpty(TableName))
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, "TableName Is  Null Or Empty");
                if (this.HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException("TableName Is  Null Or Empty"));
                else
                    throw new NullReferenceException("TableName Is  Null Or Empty");
                return res;
            }
            else if (string.IsNullOrEmpty(Field))
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, "Field Is  Null Or Empty");
                if (this.HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException("Field Is  Null Or Empty"));
                else
                    throw new NullReferenceException("Field Is  Null Or Empty");
                return res;
            }
            try
            {
                init();
                DataTable dt = null;
                SqlDataAdapter da = new SqlDataAdapter();
                DataSet ds = new DataSet();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = "select * from " + TableName + " where 1<>1";
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                da.SelectCommand = cmd;
                da.Fill(ds);
                dt = ds.Tables[0];
                var PriKeys = dt.PrimaryKey;
                DataColumn[] dcs = new DataColumn[dt.Columns.Count];
                dt.Columns.CopyTo(dcs, 0);
                var dc = dcs.FirstOrDefault(c => c.ColumnName.ToUpper() == Field.ToUpper());
                if (dc != null)
                {
                    CanBeNull = dc.AllowDBNull;
                    if (PriKeys.FirstOrDefault(c => c.ColumnName.ToUpper().Trim() == Field.ToUpper().Trim()) != null)
                        IsPrimaryKey = true;
                    else
                        IsPrimaryKey = false;
                    FieldType = dc.DataType;
                    res = true;
                }
                End();

            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }

        public List<string> Tables
        {
            get
            {
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                List<string> res = new List<string>();
                try
                {
                    init();
                    DataTable dt = null;
                    SqlConnection conn = (SqlConnection)Conn;
                    dt = conn.GetSchema("Tables");
                    int m = dt.Columns.IndexOf("TABLE_NAME");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        res.Add(dr.ItemArray.GetValue(m).ToString());
                    }
                    End();
                }
                catch (Exception ex)
                {
                    if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                    if (this.HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                }
                return res;
            }
        }

        public List<string> Views
        {
            get
            {
                MethodName = "";
                try
                {
                    MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                    MethodName = method.Name;
                }
                catch { }
                List<string> res = new List<string>();
                try
                {
                    init();
                    DataTable dt = null;
                    SqlConnection conn = (SqlConnection)Conn;
                    dt = conn.GetSchema("Views");
                    int m = dt.Columns.IndexOf("TABLE_NAME");
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        DataRow dr = dt.Rows[i];
                        res.Add(dr.ItemArray.GetValue(m).ToString());
                    }
                    End();
                }
                catch (Exception ex)
                {
                    if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                    if (this.HasError != null)
                        HasError(ClassName, MethodName, ex);
                    else
                        throw ex;
                }
                return res;
            }
        }

        /// <summary>
        /// 表结构
        /// </summary>
        /// <param name="TableName">表</param>
        /// <returns>字段</returns>
        public Dictionary<ColumnAttribute, Type> Columns(string TableName)
        {
            MethodName = "";
            try
            {
                MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Dictionary<ColumnAttribute, Type> res = new Dictionary<ColumnAttribute, Type>();
            if (string.IsNullOrEmpty(TableName))
            {
                if (this.HasError != null)
                    HasError(ClassName, MethodName, new NullReferenceException("TableName Is  Null Or Empty"));
                else
                    throw new NullReferenceException("TableName Is  Null Or Empty");
                return res;
            }
            try
            {
                init();
                DataTable dt = null;
                DataSet ds = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter();
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = (SqlConnection)Conn;
                cmd.CommandTimeout = this.Timeout * 1000;
                cmd.CommandText = "select * from " + TableName + " where 1<>1";
                da.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                da.SelectCommand = cmd;
                da.Fill(ds);
                dt = ds.Tables[0];
                var PriKeys = dt.PrimaryKey;
                DataColumn[] dcs = new DataColumn[dt.Columns.Count];
                dt.Columns.CopyTo(dcs, 0);
                foreach (var dc in dcs)
                {
                    if (dc != null)
                    {
                        ColumnAttribute col = new ColumnAttribute();
                        col.Name = dc.ColumnName;
                        col.CanBeNull = dc.AllowDBNull;
                        col.IsIdentity = dc.AutoIncrement;
                        if (PriKeys.FirstOrDefault(c => c.ColumnName.ToUpper().Trim() == dc.ColumnName.ToUpper().Trim()) != null)
                            col.IsPrimaryKey = true;
                        else
                            col.IsPrimaryKey = false;
                        col.DataType = dc.DataType.ToDataType();
                        res.Add(col, dc.DataType);
                    }
                }
                End();
            }
            catch (Exception ex)
            {
                if (log != null) log(ClassName, MethodName, Log_Type.Error, ex.Message);
                if (this.HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }

        #endregion

    }
}
