using LinqToDB;
using System;
using System.ComponentModel;
using NK.ENum;
using System.Data;
using System.Data.OleDb;
using System.Data.Odbc;
using Oracle.ManagedDataAccess.Client;
using MySql.Data.MySqlClient;
using NpgsqlTypes;

namespace LinqToDB.Mapping
{
    /// <summary>
    ///  ColumnAttribute扩展类型
    /// </summary>
    public static partial class ColumnAttributeEX
    {

        /// <summary>
        /// 支持的类型
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool SupportedType(this Type t)
        {
            if (t.IsEnum || t.IsValueType || t == typeof(char) || t == typeof(string) || t == typeof(bool) || t == typeof(DateTime) || t == typeof(byte[]))
                return true;
            return false;
        }

        /// <summary>
        /// 数据库类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="DBMode"></param>
        /// <returns></returns>
        public static string DBType(this DataType type, DBType DBMode)
        {
            switch (DBMode)
            {
                #region Access
                case NK.ENum.DBType.Access:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                            return "OleObject";
                        case LinqToDB.DataType.Blob:
                            return "MEMO";
                        case LinqToDB.DataType.Boolean:
                            return "BIT";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "datetime";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                            return "Float";
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "Real";
                        case LinqToDB.DataType.Int16:
                            return "Smallint";
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                            return "Integer";
                        case LinqToDB.DataType.Int64:
                            return "Integer";
                        case LinqToDB.DataType.UInt16:
                            return "Smallint";
                        case LinqToDB.DataType.UInt32:
                            return "Integer";
                        case LinqToDB.DataType.UInt64:
                            return "Integer";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(255)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(255)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(5000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(5000)";
                    }
                    return "";
                #endregion

                #region MYSQL
                case NK.ENum.DBType.MYSQL:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                            return "LONGBLOB";
                        case LinqToDB.DataType.Blob:
                            return "LONGTEXT";
                        case LinqToDB.DataType.Boolean:
                            return "BIT(1)";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "datetime";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                            return "double";
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "float";
                        case LinqToDB.DataType.Int16:
                            return "Smallint";
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                            return "INT";
                        case LinqToDB.DataType.Int64:
                            return "BIGINT";
                        case LinqToDB.DataType.UInt16:
                            return "Smallint";
                        case LinqToDB.DataType.UInt32:
                            return "INT";
                        case LinqToDB.DataType.UInt64:
                            return "BIGINT";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(200)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(200)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(5000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(5000)";
                    }
                    return "";
                #endregion

                #region MSSQL
                case NK.ENum.DBType.MSSQL:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                            return "image";
                        case LinqToDB.DataType.Blob:
                            return "text";
                        case LinqToDB.DataType.Boolean:
                            return "BIT";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "datetime";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                            return "real";
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "float";
                        case LinqToDB.DataType.Int16:
                            return "Smallint";
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                            return "INT";
                        case LinqToDB.DataType.Int64:
                            return "INT";
                        case LinqToDB.DataType.UInt16:
                            return "Smallint";
                        case LinqToDB.DataType.UInt32:
                            return "INT";
                        case LinqToDB.DataType.UInt64:
                            return "INT";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(200)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(200)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(5000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(5000)";
                    }
                    return "";
                #endregion

                #region Oracle
                case NK.ENum.DBType.Oracle:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                            return "BLOB";
                        case LinqToDB.DataType.Blob:
                            return "CLOB";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "date";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "float";
                        case LinqToDB.DataType.Boolean:
                        case LinqToDB.DataType.Int16:
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                        case LinqToDB.DataType.Int64:
                        case LinqToDB.DataType.UInt16:
                        case LinqToDB.DataType.UInt32:
                        case LinqToDB.DataType.UInt64:
                            return "INTEGER";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(200)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(200)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(4000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(4000)";
                    }
                    return "";
                #endregion

                #region SQLite
                case NK.ENum.DBType.SQLite:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                            return "BLOB";
                        case LinqToDB.DataType.Blob:
                            return "CLOB";
                        case LinqToDB.DataType.Boolean:
                            return "BOOLEAN";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "TEXT";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                            return "DOUBLE";
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "FLOAT";
                        case LinqToDB.DataType.Int16:
                            return "SMALLINT";
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                            return "INT";
                        case LinqToDB.DataType.Int64:
                            return "INT";
                        case LinqToDB.DataType.UInt16:
                            return "SMALLINT";
                        case LinqToDB.DataType.UInt32:
                            return "INT";
                        case LinqToDB.DataType.UInt64:
                            return "INT";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(200)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(200)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(3000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(3000)";
                    }
                    return "";
                #endregion

                #region PostgreSQL
                case NK.ENum.DBType.PostgreSQL:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                            return "bytea";
                        case LinqToDB.DataType.Blob:
                            return "text";
                        case LinqToDB.DataType.Boolean:
                            return "boolean";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "timestamp";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                            return "double";
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "real";
                        case LinqToDB.DataType.Int16:
                            return "smallint";
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                            return "integer";
                        case LinqToDB.DataType.Int64:
                            return "bigint";
                        case LinqToDB.DataType.UInt16:
                            return "smallint";
                        case LinqToDB.DataType.UInt32:
                            return "integer";
                        case LinqToDB.DataType.UInt64:
                            return "bigint";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(200)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(200)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(5000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(5000)";
                    }
                    return "";
                #endregion

                #region OTHER
                case NK.ENum.DBType.OleDB:
                case NK.ENum.DBType.ODBC:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
                        case LinqToDB.DataType.Blob:
                            return "BINARY";
                        case LinqToDB.DataType.Boolean:
                            return "BIT";
                        case LinqToDB.DataType.Byte:
                        case LinqToDB.DataType.Char:
                            return "char";
                        case LinqToDB.DataType.NChar:
                            return "nchar";
                        case LinqToDB.DataType.Date:
                        case LinqToDB.DataType.DateTime:
                        case LinqToDB.DataType.DateTime2:
                        case LinqToDB.DataType.DateTimeOffset:
                        case LinqToDB.DataType.Time:
                        case LinqToDB.DataType.Timestamp:
                            return "Datetime";
                        case LinqToDB.DataType.Single:
                        case LinqToDB.DataType.Double:
                            return "Real";
                        case LinqToDB.DataType.Money:
                        case LinqToDB.DataType.SmallMoney:
                            return "FLOAT";
                        case LinqToDB.DataType.Int16:
                            return "INT";
                        case LinqToDB.DataType.Int32:
                        case LinqToDB.DataType.Decimal:
                            return "INT";
                        case LinqToDB.DataType.Int64:
                            return "INT";
                        case LinqToDB.DataType.UInt16:
                            return "INT";
                        case LinqToDB.DataType.UInt32:
                            return "INT";
                        case LinqToDB.DataType.UInt64:
                            return "INT";
                        case LinqToDB.DataType.VarChar:
                            return "varchar(200)";
                        case LinqToDB.DataType.NVarChar:
                            return "nvarchar(200)";
                        case LinqToDB.DataType.NText:
                            return "nvarchar(5000)";
                        case LinqToDB.DataType.Text:
                            return "varchar(5000)";
                    }
                    return "";
                #endregion

                default:
                    return "";
            }
        }

        /// <summary>
        /// ColumnAttribute类中的DataType转TYPE
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Type ToSystemType(this DataType obj)
        {
            switch (obj)
            {
                case LinqToDB.DataType.Binary:
                case LinqToDB.DataType.VarBinary:
                    return typeof(byte[]);
                case LinqToDB.DataType.Blob:
                    return typeof(string);
                case LinqToDB.DataType.Boolean:
                    return typeof(bool);
                case LinqToDB.DataType.Byte:
                case LinqToDB.DataType.Char:
                    return typeof(char);
                case LinqToDB.DataType.NChar:
                    return typeof(char);
                case LinqToDB.DataType.Date:
                case LinqToDB.DataType.DateTime:
                case LinqToDB.DataType.DateTime2:
                case LinqToDB.DataType.DateTimeOffset:
                case LinqToDB.DataType.Time:
                case LinqToDB.DataType.Timestamp:
                    return typeof(DateTime);
                case LinqToDB.DataType.Single:
                case LinqToDB.DataType.Double:
                    return typeof(double);
                case LinqToDB.DataType.Money:
                case LinqToDB.DataType.SmallMoney:
                    return typeof(float);
                case LinqToDB.DataType.Int16:
                    return typeof(short);
                case LinqToDB.DataType.Int32:
                case LinqToDB.DataType.Decimal:
                    return typeof(int);
                case LinqToDB.DataType.Int64:
                    return typeof(long);
                case LinqToDB.DataType.UInt16:
                    return typeof(ushort);
                case LinqToDB.DataType.UInt32:
                    return typeof(uint);
                case LinqToDB.DataType.UInt64:
                    return typeof(ulong);
                case LinqToDB.DataType.VarChar:
                case LinqToDB.DataType.Text:
                    return typeof(string);
                case LinqToDB.DataType.NVarChar:
                case LinqToDB.DataType.NText:
                    return typeof(string);
                default:
                    return null;
            }
        }

        /// <summary>
        ///TYPE转 ColumnAttribute类中的DataType
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DataType ToDataType(this Type obj)
        {
            if (obj == null)
                return DataType.Undefined;
            else
            {
                if (obj.IsGenericType && obj.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
                {
                    NullableConverter nullableConverter = new NullableConverter(obj);
                    obj = nullableConverter.UnderlyingType;
                }
                if (obj.Name.ToUpper().Contains("NULL"))
                    obj = obj.GetGenericArguments().Length > 0 ? obj.GetGenericArguments()[0] : obj;
                if (obj.IsEnum)
                    return DataType.Int32;
                else if (obj == typeof(string))
                    return DataType.VarChar;
                else if (obj == typeof(bool))
                    return DataType.Boolean;
                else if (obj == typeof(int) || obj == typeof(decimal))
                    return DataType.Int32;
                else if (obj == typeof(short))
                    return DataType.Int16;
                else if (obj == typeof(long))
                    return DataType.Int64;
                else if (obj == typeof(uint))
                    return DataType.UInt32;
                else if (obj == typeof(ushort))
                    return DataType.UInt16;
                else if (obj == typeof(ulong))
                    return DataType.UInt64;
                else if (obj == typeof(byte) || obj == typeof(char))
                    return DataType.Char;
                else if (obj == typeof(float))
                    return DataType.Money;
                else if (obj == typeof(double))
                    return DataType.Double;
                else if (obj == typeof(byte[]))
                    return DataType.Binary;
                else if (obj == typeof(DateTime))
                    return DataType.DateTime;
                else
                    return DataType.Undefined;

            }

        }

        /// <summary>
        /// 数据库日期类型
        /// </summary>
        /// <param name="DT"></param>
        /// <param name="DBMode"></param>
        /// <returns></returns>
        public static string DBDate(this DateTime DT, DBType DBMode)
        {
            switch (DBMode)
            {
                case NK.ENum.DBType.MSSQL:
                    return " cast('" + DT.ToString("yyyy-MM-dd HH:mm:ss") + "' as datetime)";
                case NK.ENum.DBType.Oracle:
                    return "to_date('" + DT.ToString("yyyy-MM-dd HH:mm:ss") + "','yyyy-mm-dd hh24:mi:ss')";
                default:
                    return "'" + DT.ToString("yyyy-MM-dd HH:mm:ss") + "'";
            }
        }

        /// <summary>
        /// 数据库BLOB类型
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="val"></param>
        /// <param name="DBMode"></param>
        /// <param name="PName"></param>
        /// <returns></returns>
        public static string DBClob(string Key, object val, DBType DBMode, out string PName)
        {
            PName = "";
            switch (DBMode)
            {
                case NK.ENum.DBType.Oracle:
                    PName = "P_" + Key;
                    return ":" + PName;
                default:
                    return "'" + val.ToString() + "'";
            }
        }

        /// <summary>
        /// 数据类型转换
        /// </summary>
        /// <param name="type"></param>
        /// <param name="DBMode"></param>
        /// <returns></returns>
        public static int DBDataType(this DataType type,DBType DBMode)
        {
            SqlDbType MSTYPE = SqlDbType.NVarChar;
            OleDbType OLTYPE = OleDbType.VarChar;
            OdbcType ODTYPE = OdbcType.VarChar;
            OracleDbType OTYPE = OracleDbType.Varchar2;
            MySqlDbType MYTYPE = MySqlDbType.VarChar;
            NpgsqlDbType PTYPE = NpgsqlDbType.Varchar;
            DbType SLTYPE = DbType.AnsiString;
            switch (type)
            {
                case LinqToDB.DataType.Binary:
                    MSTYPE = SqlDbType.Binary;
                    OLTYPE = OleDbType.Binary;
                    ODTYPE = OdbcType.Binary;
                    OTYPE = OracleDbType.Blob;
                    MYTYPE = MySqlDbType.Binary;
                    PTYPE = NpgsqlDbType.Array;
                    SLTYPE = DbType.Binary;
                    break;
                case LinqToDB.DataType.VarBinary:
                    MSTYPE = SqlDbType.VarBinary;
                    OLTYPE = OleDbType.VarBinary;
                    ODTYPE = OdbcType.VarBinary;
                    OTYPE = OracleDbType.Blob;
                    MYTYPE = MySqlDbType.VarBinary;
                    PTYPE = NpgsqlDbType.Array;
                    SLTYPE = DbType.Binary;
                    break;
                case LinqToDB.DataType.Blob:
                    MSTYPE = SqlDbType.Text;
                    OLTYPE = OleDbType.VarBinary;
                    ODTYPE = OdbcType.VarBinary;
                    OTYPE = OracleDbType.Clob;
                    MYTYPE = MySqlDbType.Blob;
                    PTYPE = NpgsqlDbType.Array;
                    SLTYPE = DbType.Binary;
                    break;
                case LinqToDB.DataType.Boolean:
                    MSTYPE = SqlDbType.Bit;
                    OLTYPE = OleDbType.Boolean;
                    ODTYPE = OdbcType.Bit;
                    OTYPE = OracleDbType.Byte;
                    MYTYPE = MySqlDbType.Bit;
                    PTYPE = NpgsqlDbType.Bit;
                    SLTYPE = DbType.Boolean;
                    break;
                case LinqToDB.DataType.Byte:
                case LinqToDB.DataType.Char:
                    MSTYPE = SqlDbType.Char;
                    OLTYPE = OleDbType.Char;
                    ODTYPE = OdbcType.Char;
                    OTYPE = OracleDbType.Char;
                    MYTYPE = MySqlDbType.String;
                    PTYPE = NpgsqlDbType.Char;
                    SLTYPE = DbType.AnsiStringFixedLength;
                    break;
                case LinqToDB.DataType.NChar:
                    MSTYPE = SqlDbType.NChar;
                    OLTYPE = OleDbType.Char;
                    ODTYPE = OdbcType.NChar;
                    OTYPE = OracleDbType.NChar;
                    MYTYPE = MySqlDbType.String;
                    PTYPE = NpgsqlDbType.Char;
                    SLTYPE = DbType.AnsiString;
                    break;
                case LinqToDB.DataType.Date:
                case LinqToDB.DataType.DateTime:
                case LinqToDB.DataType.DateTime2:
                case LinqToDB.DataType.DateTimeOffset:
                case LinqToDB.DataType.Time:
                case LinqToDB.DataType.Timestamp:
                    MSTYPE = SqlDbType.DateTime;
                    OLTYPE = OleDbType.DBDate;
                    ODTYPE = OdbcType.DateTime;
                    OTYPE = OracleDbType.Date;
                    MYTYPE = MySqlDbType.Date;
                    PTYPE = NpgsqlDbType.Date;
                    SLTYPE = DbType.DateTime;
                    break;
                case LinqToDB.DataType.Single:
                case LinqToDB.DataType.Double:
                    MSTYPE = SqlDbType.Real;
                    OLTYPE = OleDbType.Double;
                    ODTYPE = OdbcType.Double;
                    OTYPE = OracleDbType.Double;
                    MYTYPE = MySqlDbType.Double;
                    PTYPE = NpgsqlDbType.Double;
                    SLTYPE = DbType.Double;
                    break;
                case LinqToDB.DataType.Money:
                case LinqToDB.DataType.SmallMoney:
                    MSTYPE = SqlDbType.Money;
                    OLTYPE = OleDbType.Decimal;
                    ODTYPE = OdbcType.Decimal;
                    OTYPE = OracleDbType.Decimal;
                    MYTYPE = MySqlDbType.Decimal;
                    PTYPE = NpgsqlDbType.Money;
                    SLTYPE = DbType.Decimal;
                    break;
                case LinqToDB.DataType.Int64:
                case LinqToDB.DataType.Int32:
                case LinqToDB.DataType.Int16:
                case LinqToDB.DataType.UInt16:
                case LinqToDB.DataType.UInt32:
                case LinqToDB.DataType.UInt64:
                    MSTYPE = SqlDbType.Int;
                    OLTYPE = OleDbType.Integer;
                    ODTYPE = OdbcType.Int;
                    OTYPE = OracleDbType.Int64;
                    MYTYPE = MySqlDbType.Int64;
                    PTYPE = NpgsqlDbType.Integer;
                    SLTYPE = DbType.Int64;
                    break; 
                case LinqToDB.DataType.Decimal:
                    MSTYPE = SqlDbType.Decimal;
                    OLTYPE = OleDbType.Decimal;
                    ODTYPE = OdbcType.Decimal;
                    OTYPE = OracleDbType.Decimal;
                    MYTYPE = MySqlDbType.Decimal;
                    PTYPE = NpgsqlDbType.Money;
                    SLTYPE = DbType.Decimal;
                    break;  
                case LinqToDB.DataType.VarChar:
                case LinqToDB.DataType.Text:
                    MSTYPE = SqlDbType.VarChar;
                    OLTYPE = OleDbType.VarChar;
                    ODTYPE = OdbcType.VarChar;
                    OTYPE = OracleDbType.Varchar2;
                    MYTYPE = MySqlDbType.VarString;
                    PTYPE = NpgsqlDbType.Varchar;
                    SLTYPE = DbType.String;
                    break;
                case LinqToDB.DataType.NVarChar:
                case LinqToDB.DataType.NText:
                    MSTYPE = SqlDbType.NVarChar;
                    OLTYPE = OleDbType.VarChar;
                    ODTYPE = OdbcType.NVarChar;
                    OTYPE = OracleDbType.NVarchar2;
                    MYTYPE = MySqlDbType.String;
                    PTYPE = NpgsqlDbType.Varchar;
                    SLTYPE = DbType.StringFixedLength;
                    break;
                default:
                    break;
            }
            switch(DBMode)
            {
                case NK.ENum.DBType.Access:
                case NK.ENum.DBType.OleDB:
                    return (int)OLTYPE;
                case NK.ENum.DBType.MSSQL:
                    return (int)MSTYPE;
                case NK.ENum.DBType.MYSQL:
                    return (int)MYTYPE;
                case NK.ENum.DBType.Oracle:
                    return (int)OTYPE;
                case NK.ENum.DBType.ODBC:
                    return (int)ODTYPE;
                case NK.ENum.DBType.PostgreSQL:
                    return (int)PTYPE;
                case NK.ENum.DBType.SQLite:
                    return (int)SLTYPE;
                default:
                    return -1;
            }
        }
    }
}
