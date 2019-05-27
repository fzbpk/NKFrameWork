using LinqToDB;
using System;
using System.ComponentModel;
using NK.ENum;
namespace   LinqToDB.Mapping
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
        public static string DBType(this DataType type,DBType DBMode)
        {
            switch (DBMode)
            {
                #region Access
                case NK.ENum.DBType.Access:
                    switch (type)
                    {
                        case LinqToDB.DataType.Binary:
                        case LinqToDB.DataType.VarBinary:
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
                        case LinqToDB.DataType.Blob:
                            return "BLOB";
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
                        case LinqToDB.DataType.Blob:
                            return "Varbinary(5000)";
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
                        case LinqToDB.DataType.Blob:
                            return "BLOB"; 
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
                        case LinqToDB.DataType.Blob:
                            return "BLOB";
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
                        case LinqToDB.DataType.Blob:
                            return "bytea";
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
                   return  "";
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
                case LinqToDB.DataType.Blob:
                    return typeof(byte[]);
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
                    obj = obj.GetGenericArguments().Length>0? obj.GetGenericArguments()[0]: obj;
                if (obj.IsEnum)
                    return DataType.Int32;
                else if (obj == typeof(string))
                    return DataType.VarChar;
                else if (obj == typeof(bool))
                    return DataType.Boolean; 
                else if (obj == typeof(int) ||   obj ==typeof(decimal))
                    return DataType.Int32;
                else if (obj == typeof(short))
                    return DataType.Int16;
                else if (obj == typeof(long))
                    return DataType.Int64;
                else if (obj == typeof(uint) )
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
    }
}
