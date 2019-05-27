using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using LinqToDB.Mapping;
using System.Reflection;
using NK.ENum;
using NK.Attribut;
using LinqToDB;

namespace NK.Data
{
    /// <summary>
    /// TSQL扩展
    /// </summary>
    public static partial class TSQLHelper
    { 

        /// <summary>
        /// 字段转T-SQL
        /// </summary>
        /// <param name="Key">字段</param>
        /// <param name="Mode">运算类型</param>
        /// <param name="Value">字段值</param>
        /// <returns>T-SQL</returns>
        public static string KeyToSql(this string Key, ExpressionType Mode, object Value)
        {
            if (string.IsNullOrEmpty(Key))
                return "";
            string res = "";
            Type t = Value.GetType();
            if (Value == null)
            {
                if (Mode == ExpressionType.Equal)
                    res = " " + Key + " IS NULL ";
                else if (Mode == ExpressionType.NotEqual)
                    res = " " + Key + " IS NOT NULL ";
            }
            else
            {
                if (t == typeof(string))
                {
                    if (Mode == ExpressionType.Constant)
                        res = " '%" + Value.ToString() + "%' ";
                    else
                        res = " '" + Value.ToString() + "' ";
                }
                else if (t == typeof(bool))
                {
                    if ((bool)Value)
                        res = "1";
                    else
                        res = "0";

                }
                else if (t.IsEnum)
                {
                    string ss = Value.ToString().ToUpper().Trim();
                    FieldInfo[] fields = Value.GetType().GetFields();
                    if (fields != null)
                    {
                        if (fields.Length > 0)
                        {
                            foreach (var field in fields)
                            {
                                string EnumName = field.Name.ToUpper().Trim();
                                if (ss == EnumName)
                                {
                                    res = field.GetRawConstantValue().ToString();
                                    break;

                                }
                            }
                        }
                    }
                }
                else if (t.IsValueType)
                    res = " " + Value.ToString() + " ";
                switch (Mode)
                {
                    case ExpressionType.Equal:
                        res = " " + Key + " = " + res;
                        break;
                    case ExpressionType.NotEqual:
                        res = " " + Key + " <> " + res;
                        break;
                    case ExpressionType.GreaterThan:
                        res = " " + Key + " > " + res;
                        break;
                    case ExpressionType.GreaterThanOrEqual:
                        res = " " + Key + " >= " + res;
                        break;
                    case ExpressionType.LessThan:
                        res = " " + Key + " < " + res;
                        break;
                    case ExpressionType.LessThanOrEqual:
                        res = " " + Key + " <= " + res;
                        break;
                    case ExpressionType.Constant:
                        res = " " + Key + " Like " + res;
                        break;
                    case ExpressionType.Add:
                        res = " " + Key + " + " + res;
                        break;
                    case ExpressionType.Subtract:
                        res = " " + Key + " - " + res;
                        break;
                    case ExpressionType.Multiply:
                        res = " " + Key + " * " + res;
                        break;
                    case ExpressionType.Divide:
                        res = " " + Key + " / " + res;
                        break;
                    case ExpressionType.Modulo:
                        res = " " + Key + " % " + res;
                        break;
                    case ExpressionType.And:
                        res = " " + Key + " & " + res;
                        break;
                    case ExpressionType.Or:
                        res = " " + Key + " | " + res;
                        break;
                }
            }
            return res;
        }

        /// <summary>
        /// T-SQL合并
        /// </summary>
        /// <param name="sql1">语句1</param>
        /// <param name="Mode">运算类型</param>
        /// <param name="sql2">语句2</param>
        /// <returns></returns>
        public static string JoinToSql(this string sql1, ExpressionType Mode, string sql2)
        {
            sql1 = sql1.ToUpper();
            sql2 = sql2.ToUpper();
            if (string.IsNullOrEmpty(sql1))
                sql1 = "";
            if (string.IsNullOrEmpty(sql2))
                sql2 = "";
            switch (Mode)
            {
                case ExpressionType.AndAlso:
                    return " (" + sql1 + ") AND (" + sql2 + ") ";
                case ExpressionType.OrElse:
                    return " (" + sql1 + ") OR  (" + sql2 + ") ";
                default:
                    return "";
            }

        }

        /// <summary>
        /// T-SQL取反
        /// </summary>
        /// <param name="sql">语句</param>
        /// <returns></returns>
        public static string NotToSql(this string sql)
        {
            sql = sql.ToUpper();
            if (string.IsNullOrEmpty(sql))
                return "";
            return " NOT (" + sql + ")";
        }

        /// <summary>
        /// 条件语句合成
        /// </summary>
        /// <param name="Column">字段，字段值</param>
        /// <param name="CVMode">字段与其值的运算</param>
        /// <param name="CCMode">字段间逻辑运算</param>
        /// <returns>不带WHERE关键字的条件语句</returns>
        public static string WhereToSql(this Dictionary<string, object> Column, ExpressionType CVMode, ExpressionType CCMode)
        {
            string sql = "";
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    if (string.IsNullOrEmpty(sql))
                        sql = tmp.Key.KeyToSql(CVMode, tmp.Value);
                    else
                        sql = sql.JoinToSql(CCMode, tmp.Key.KeyToSql(CVMode, tmp.Value));
                }
            }
            return sql;
        }

        /// <summary>
        /// 合成ORDER语句
        /// </summary>
        /// <param name="Column">字段，字段顺序倒序</param>
        /// <returns>不带ORDERBY关键字的ORDERBY语句</returns>
        public static string OrderbyToSql(this Dictionary<string, bool> Column)
        {
            string sql = "";
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    if (string.IsNullOrEmpty(sql))
                        sql = tmp.Key + " " + (tmp.Value ? "Desc" : "");
                    else
                        sql += "," + tmp.Key + " " + (tmp.Value ? "Desc" : "");
                }
            }
            return sql;
        }

        /// <summary>
        /// 查询语句合成T-SQL
        /// </summary>
        /// <param name="TableName">表名</param>
        ///  <param name="Column">字段，字段顺序倒序</param>
        /// <returns>T-SQL</returns>
        public static string SelectToSql(this string TableName, List<string> Column)
        {
            if (string.IsNullOrEmpty(TableName)) return "";
            string sql = "";
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    if (string.IsNullOrEmpty(sql))
                        sql = tmp;
                    else
                        sql += "," + tmp;
                }
            }
            if (string.IsNullOrEmpty(sql))
                sql = "*";
            return "SELECT " + sql + " FROM " + TableName;
        }

        /// <summary>
        /// 合成添加记录T-SQL
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Mode">数据库类型</param>
        /// <param name="Column">字段，字段值</param> 
        /// <returns>T-SQL</returns>
        public static string InsertToSQL(this string TableName, DBType Mode, Dictionary<string, object> Column )
        { 
            if (string.IsNullOrEmpty(TableName)) return "";
            string sql = "INSERT INTO " + TableName;
            string Col, Val;
            Col = "";
            Val = "";
            int n = 0;
            if (Column != null)
            { 
                foreach (var tmp in Column)
                {
                    Col += "," + tmp.Key;
                    if (tmp.Value == null)
                        Val += ",NULL";
                    else
                    {
                        Type t = tmp.Value.GetType();
                        if (t == typeof(string))
                            Val += ",'" + tmp.Value.ToString() + "'";
                        else if (t == typeof(DateTime))
                            Val +=","+ ColumnAttributeEX. DBDate (Convert.ToDateTime(tmp.Value), Mode); 
                        else if (t == typeof(bool))
                        {
                            if ((bool)tmp.Value)
                                Val += ",1";
                            else
                                Val += ",0";
                        }
                        else if (t.IsEnum)
                        {
                            string ss = tmp.Value.ToString().ToUpper().Trim();
                            FieldInfo[] fields = tmp.Value.GetType().GetFields();
                            if (fields != null)
                            {
                                if (fields.Length > 0)
                                {
                                    foreach (var field in fields)
                                    {
                                        string EnumName = field.Name.ToUpper().Trim();
                                        if (ss == EnumName)
                                        {
                                            Val += "," + field.GetRawConstantValue().ToString();
                                            break; 
                                        }
                                    }
                                }
                            }
                        }
                        else if (t.IsValueType)
                            Val += "," + tmp.Value.ToString();
                    }
                }
            }
            if (string.IsNullOrEmpty(Col) || string.IsNullOrEmpty(Val))
                return "";
            if (Val.StartsWith(",")) Col = Col.Substring(1);
            if (Val.StartsWith(",")) Val = Val.Substring(1);
            sql = sql + "(" + Col + ")VALUES(" + Val + ")";
            return sql;
        }

        /// <summary>
        /// 合成添加记录T-SQL
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Mode"></param>
        /// <param name="Column"></param>
        /// <param name="Parameter"></param>
        /// <returns></returns>
        public static string InsertToSQLEX(this string TableName, DBType Mode, Dictionary<KeyValuePair<string, DataType>, object> Column, out Dictionary<KeyValuePair<string, DataType>, object> Parameter)
        {
            Parameter = new Dictionary<KeyValuePair<string, DataType>, object>();
            if (string.IsNullOrEmpty(TableName)) return "";
            string sql = "INSERT INTO " + TableName;
            string Col, Val;
            Col = "";
            Val = ""; 
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    Col += "," + tmp.Key.Key;
                    if (tmp.Value == null)
                        Val += ",NULL";
                    else
                    {
                        Type t = tmp.Value.GetType();
                        if (tmp.Key.Value == DataType.Blob)
                        {
                            string prm = "";
                            Val += "," + ColumnAttributeEX.DBClob(tmp.Key.Key, tmp.Value.ToString(), Mode, out prm);
                            if (!string.IsNullOrEmpty(prm)) Parameter.Add(new KeyValuePair<string, DataType>(prm, tmp.Key.Value), tmp.Value);
                        }
                        else  if (t == typeof(string))
                            Val += ",'" + tmp.Value.ToString() + "'";
                        else if (t == typeof(DateTime))
                            Val += "," + ColumnAttributeEX.DBDate(Convert.ToDateTime(tmp.Value), Mode);
                        else if (t == typeof(bool))
                        {
                            if ((bool)tmp.Value)
                                Val += ",1";
                            else
                                Val += ",0";
                        }
                        else if (t.IsEnum)
                        {
                            string ss = tmp.Value.ToString().ToUpper().Trim();
                            FieldInfo[] fields = tmp.Value.GetType().GetFields();
                            if (fields != null)
                            {
                                if (fields.Length > 0)
                                {
                                    foreach (var field in fields)
                                    {
                                        string EnumName = field.Name.ToUpper().Trim();
                                        if (ss == EnumName)
                                        {
                                            Val += "," + field.GetRawConstantValue().ToString();
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                        else if (t.IsValueType)
                            Val += "," + tmp.Value.ToString();
                    }
                }
            }
            if (string.IsNullOrEmpty(Col) || string.IsNullOrEmpty(Val))
                return "";
            if (Val.StartsWith(",")) Col = Col.Substring(1);
            if (Val.StartsWith(",")) Val = Val.Substring(1);
            sql = sql + "(" + Col + ")VALUES(" + Val + ")";
            return sql;
        }
         
        /// <summary>
        /// 合成更改记录T-SQL
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Mode">数据库类型</param>
        /// <param name="Column">字段，字段值</param>
        /// <returns>T-SQL</returns>
        public static string UpdateToSQL(this string TableName, DBType Mode, Dictionary<string, object> Column)
        {
            if (string.IsNullOrEmpty(TableName)) return "";
            string sql = "UPDATE " + TableName + " SET ";
            string ColVal = "";
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    ColVal += "," + tmp.Key + "=";
                    if (tmp.Value == null)
                        ColVal += "NULL";
                    else
                    {
                        Type t = tmp.Value.GetType();
                        if (t == typeof(string))
                            ColVal += "'" + tmp.Value.ToString() + "'";
                        else if (t == typeof(DateTime))
                            ColVal += ColumnAttributeEX.DBDate(Convert.ToDateTime(tmp.Value), Mode);
                        else if (t == typeof(bool))
                        {
                            if ((bool)tmp.Value)
                                ColVal += "1";
                            else
                                ColVal += "0";
                        }
                        else if (t.IsEnum)
                        {
                            string ss = tmp.Value.ToString().ToUpper().Trim();
                            FieldInfo[] fields = tmp.Value.GetType().GetFields();
                            if (fields != null)
                            {
                                if (fields.Length > 0)
                                {
                                    foreach (var field in fields)
                                    {
                                        string EnumName = field.Name.ToUpper().Trim();
                                        if (ss == EnumName)
                                            ColVal += field.GetRawConstantValue().ToString();
                                    }
                                }
                            }
                        }
                        else if (t.IsValueType)
                            ColVal += tmp.Value.ToString();
                    }
                }
            }
            if (string.IsNullOrEmpty(ColVal)) return "";
            if (ColVal.StartsWith(",")) ColVal = ColVal.Substring(1);
             sql = sql + ColVal;
            return sql;
        }

        /// <summary>
        /// 合成更改记录T-SQL
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Mode">数据库类型</param>
        /// <param name="Column">字段，字段值</param>
        /// <param name="Parameter">参数</param>
        /// <returns></returns>
        public static string UpdateToSQLEX(this string TableName, DBType Mode, Dictionary<KeyValuePair<string, DataType>, object> Column, out Dictionary<KeyValuePair<string, DataType>, object> Parameter)
        {
            Parameter = new Dictionary<KeyValuePair<string, DataType>, object>(); 
            if (string.IsNullOrEmpty(TableName)) return "";
            string sql = "UPDATE " + TableName + " SET ";
            string ColVal = "";
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    ColVal += "," + tmp.Key.Key + "=";
                    if (tmp.Value == null)
                        ColVal += "NULL";
                    else
                    {
                        Type t = tmp.Value.GetType();
                        if (tmp.Key.Value == DataType.Blob)
                        {
                            string prm = "";
                            ColVal +=  ColumnAttributeEX.DBClob(tmp.Key.Key, tmp.Value.ToString(), Mode, out prm);
                            if (!string.IsNullOrEmpty(prm)) Parameter.Add(new KeyValuePair<string, DataType>(prm, tmp.Key.Value), tmp.Value);
                        }
                       else if (t == typeof(string))
                            ColVal += "'" + tmp.Value.ToString() + "'";
                        else if (t == typeof(DateTime))
                            ColVal += ColumnAttributeEX.DBDate(Convert.ToDateTime(tmp.Value), Mode);
                        else if (t == typeof(bool))
                        {
                            if ((bool)tmp.Value)
                                ColVal += "1";
                            else
                                ColVal += "0";
                        }
                        else if (t.IsEnum)
                        {
                            string ss = tmp.Value.ToString().ToUpper().Trim();
                            FieldInfo[] fields = tmp.Value.GetType().GetFields();
                            if (fields != null)
                            {
                                if (fields.Length > 0)
                                {
                                    foreach (var field in fields)
                                    {
                                        string EnumName = field.Name.ToUpper().Trim();
                                        if (ss == EnumName)
                                            ColVal += field.GetRawConstantValue().ToString();
                                    }
                                }
                            }
                        }
                        else if (t.IsValueType)
                            ColVal += tmp.Value.ToString();
                    }
                }
            }
            if (string.IsNullOrEmpty(ColVal)) return "";
            if (ColVal.StartsWith(",")) ColVal = ColVal.Substring(1);
            sql = sql + ColVal;
            return sql;
        }


        /// <summary>
        /// 合成删除记录T-SQL
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>T-SQL</returns>
        public static string DeleteToSQL(this string TableName)
        {
            if (string.IsNullOrEmpty(TableName)) return "";
            string sql = "DELETE FROM " + TableName;
            return sql;
        }

        /// <summary>
        /// 合成创建表T-SQL
        /// </summary>
        /// <param name="TableName">表</param>
        /// <param name="Mode">数据库类型</param>
        /// <param name="Columns">字段</param>
        /// <returns>T-SQL</returns>
        public static List<string> CreatToSql(this string TableName, DBType Mode, List<ColumnAttribute> Columns)
        {
            List<string> res = new List<string>();
            if (string.IsNullOrEmpty(TableName)) return res;
            if (Mode == DBType.None) return res;
            if (Columns == null) return res;
            if (Columns.Count <= 0) return res;
            string sql = "Create Table " + TableName + " (";
            string temp = "";
            string Column = "";
            string PKColumn = "";
            if (Columns != null)
            {
                if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                {
                    int index = -1;
                    index = Columns.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX") || c.IsIdentity);
                    if(index>-1)
                      Columns[index].IsPrimaryKey = true;
                    if (Columns.Where(c => c.IsIdentity).Count() <= 0)
                    {
                        index = Columns.FindIndex(c => c.IsPrimaryKey);
                        if (index > -1)
                            Columns[index].IsPrimaryKey = true;
                    }
                } 
                foreach (var Col in Columns)
                {
                    if (Col == null) continue;
                    bool CanBeNull = Col.CanBeNull;
                    string ColumnName = Col.Name;
                    bool IsPrimaryKey = Col.IsPrimaryKey;
                    bool IsIdentity = Col.IsIdentity;
                    LinqToDB.DataType Types = Col.DataType;
                    string ColType = Types.DBType(Mode);
                    if (string.IsNullOrEmpty(ColType))
                        continue;
                    temp = "";
                    ColType = " " + ColType + " ";
                    switch (Mode)
                    {

                        #region Access
                        case DBType.Access: 
                            temp = ",[" + ColumnName + "]";
                            PKColumn = "";
                            if (IsIdentity)
                                temp += " Counter ";
                            else
                                temp += ColType;
                            if (IsPrimaryKey)
                                temp += "  primary key ";
                            else
                                temp += (CanBeNull ? " default NULL " : "  NOT NULL "); 
                            break;
                        #endregion

                        #region MYSQL
                        case DBType.MYSQL:
                            temp = ",`" + ColumnName + "` ";
                            temp += ColType;
                            if (IsIdentity)
                                temp += " NOT NULL auto_increment ";
                            else
                                temp += " " + (CanBeNull ? " default NULL " : "  NOT NULL "); 
                            if (IsPrimaryKey)
                                PKColumn = ",PRIMARY KEY  (`" + ColumnName + "`)";
                            break;
                        #endregion

                        #region MSSQL
                        case DBType.MSSQL:
                            temp = ",[" + ColumnName + "]";
                            temp += ColType;
                            if (IsIdentity)
                                temp += " not null identity(1,1) ";
                            else
                                temp += (CanBeNull ? " default NULL " : "  NOT NULL ");
                            if (IsPrimaryKey)
                                PKColumn = ",CONSTRAINT " + TableName + "_PK PRIMARY KEY (" + ColumnName + ")"; 
                            break;
                        #endregion

                        #region Oracle
                        case DBType.Oracle:
                            temp = "," + ColumnName  + " ";
                            temp += ColType;
                            if (IsIdentity)
                            {
                                string tmp = "create sequence " + TableName + "_seq INCREMENT BY 1 START WITH 1 NOMAXVALUE NOCYCLE CACHE 10";
                                res.Add(tmp);
                                tmp = "CREATE OR REPLACE TRIGGER " + TableName + "_Increase BEFORE "
                                          + " INSERT ON  " + TableName + "  FOR EACH ROW "
                                          + "BEGIN "
                                          + " SELECT " + TableName + "_seq.NEXTVAL INTO:NEW." + ColumnName + " FROM DUAL;"
                                          + "END; ";
                                res.Add(tmp);
                            }
                            if (IsPrimaryKey)
                                temp += " PRIMARY KEY NOT NULL";
                            else
                                temp+= (CanBeNull ? " default NULL " : "  NOT NULL "); 
                            break;
                        #endregion

                        #region SQLite
                        case DBType.SQLite:
                            PKColumn = "";
                            temp += "," + ColumnName + " ";
                            temp += ColType; 
                            if (IsPrimaryKey)
                                temp += " PRIMARY KEY ";
                            if (IsIdentity)
                                temp += " autoincrement ";
                            else
                                temp += (CanBeNull ? " default NULL " : "  NOT NULL ");
                            break;
                        #endregion

                        #region PostgreSQL
                        case DBType.PostgreSQL:
                            PKColumn = "";
                            Column += "," + ColumnName + " ";
                            temp += ColType;
                            if (IsIdentity)
                                temp += " SERIAL ";
                            if (IsPrimaryKey)
                                temp += " PRIMARY KEY ";
                            else
                                temp += (CanBeNull ? " default NULL " : "  NOT NULL "); 
                            break;
                        #endregion

                        #region ODBC,OLEDB
                        case DBType.ODBC:
                        case DBType.OleDB:
                            Column += "," + ColumnName + "";
                            temp += ColType;
                            if (IsPrimaryKey)
                            {
                                CanBeNull = false;
                                PKColumn = "PRIMARY KEY( " + ColumnName + " )";
                            }
                            temp += (CanBeNull ? " default NULL " : "  NOT NULL "); 
                            break;
                            #endregion

                    }
                    if (!string.IsNullOrEmpty(temp))
                        Column += temp+" ";
                }
            }
            if (Column.StartsWith(","))
                Column = Column.Substring(1, Column.Length - 1);
            sql += Column + PKColumn + " )";
            res.Insert(0, sql);
            return res;
        }

        /// <summary>
        /// 删除表T-SQL
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <returns>T-SQL</returns>
        public static List<string> DropToSql(this string TableName)
        {
            List<string> res = new List<string>();
            if (!string.IsNullOrEmpty(TableName))
            {
                res.Add("DROP TABLE " + TableName);
            }
            return res;
        }

        /// <summary>
        /// 修改表T-SQL
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Mode"></param>
        /// <param name="Columns"></param>
        /// <param name="OrgCol"></param>
        /// <returns></returns>
        public static List<string> ModifyToSql(this string TableName, DBType Mode, List<ColumnAttribute> Columns, List<ColumnAttribute> OrgCol)
        {
            List<string> res = new List<string>();
            if (string.IsNullOrEmpty(TableName)) return res;
            if (Mode == DBType.None) return res;
            if (Columns == null) return res;
            if (Columns.Count <= 0) return res;
            if (OrgCol == null) return res;
            if (OrgCol.Count <= 0) return res; 
            if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
            {
                int index = -1;
                index = Columns.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX") || c.IsIdentity);
                if (index > -1)
                    Columns[index].IsPrimaryKey = true;
                if (Columns.Where(c => c.IsIdentity).Count() <= 0)
                {
                    index = Columns.FindIndex(c => c.IsPrimaryKey);
                    if (index > -1)
                        Columns[index].IsPrimaryKey = true;
                }
            }
            if (OrgCol.Where(c => c.IsPrimaryKey).Count() <= 0)
            {
                int index = -1;
                index = OrgCol.FindIndex(c => c.Name.ToUpper().Contains("ID") || c.Name.ToUpper().Contains("INDEX") || c.IsIdentity);
                if (index > -1)
                    OrgCol[index].IsPrimaryKey = true;
                if (OrgCol.Where(c => c.IsIdentity).Count() <= 0)
                {
                    index = OrgCol.FindIndex(c => c.IsPrimaryKey);
                    if (index > -1)
                        OrgCol[index].IsPrimaryKey = true;
                }
            }
            var orgpri = OrgCol.Find(c => c.IsPrimaryKey);
            foreach (var col in Columns)
            {
                string sql = "";
                bool Mod = true;
                bool AddMod = true;
                string ColumnName = col.Name;
                bool CanBeNull = col.CanBeNull;
                bool IsPrimaryKey = col.IsPrimaryKey;
                bool IsIdentity = col.IsIdentity;
                LinqToDB.DataType Types = col.DataType; 
                if (OrgCol.Exists(c => c.Name.ToUpper().Trim() == ColumnName.ToUpper().Trim()))
                {
                    AddMod = false;
                    var org = OrgCol.Find(c => c.Name.ToUpper().Trim() == ColumnName.ToUpper().Trim());
                    if (Types == org.DataType   && org.IsPrimaryKey == IsPrimaryKey)
                        Mod = false;

                } 
                if (Mod)
                {
                    sql = " ALTER TABLE " + TableName;
                    string ColType =" "+ Types.DBType(Mode)+" "; 
                    switch (Mode)
                    {
                        #region Access
                        case DBType.Access:
                            if (AddMod)
                                sql += " ADD  " + ColumnName + " ";
                            else
                                sql += " ALTER COLUMN " + ColumnName + " "; 
                            if(IsIdentity)
                                sql += " Counter ";
                            else
                                sql += ColType;
                            if (IsPrimaryKey)
                            { 
                                if (orgpri!=null)
                                  res.Add(" ALTER TABLE " + TableName + " DROP CONSTRAINT " + TableName + "_PK ");
                                res.Add(" ALTER TABLE " + TableName + " ADD CONSTRAINT " + TableName + "_PK primary key(" + ColumnName + ") ");
                            } 
                            break;
                        #endregion

                        #region MYSQL
                        case DBType.MYSQL:
                            if (AddMod)
                                sql += " ADD " + ColumnName + " ";
                            else
                                sql += " MODIFY " + ColumnName + " ";
                            sql += ColType;
                            if (IsIdentity)
                                sql += " auto_increment ";
                            if (IsPrimaryKey)
                            { 
                                if (orgpri != null)
                                    res.Add("  ALTER TABLE " + TableName + " drop primary key ");
                                res.Add(" ALTER TABLE " + TableName + " ADD  primary key(" + ColumnName + ") ");
                            } 
                            break;
                        #endregion

                        #region MSSQL
                        case DBType.MSSQL:
                            if (AddMod)
                                sql += " ADD " + ColumnName + " ";
                            else
                                sql += " ALTER COLUMN " + ColumnName + " ";
                            sql += ColType;
                            if (IsIdentity)
                                sql += " NOT NULL identity(1,1) ";
                            if (IsPrimaryKey)
                            { 
                                if (orgpri != null)
                                    res.Add(" ALTER TABLE " + TableName + " DROP CONSTRAINT " + TableName + "_PK ");
                                res.Add(" ALTER TABLE " + TableName + " ADD CONSTRAINT " + TableName + "_PK primary key(" + ColumnName + ") ");
                            } 
                            break;
                        #endregion

                        #region Oracle
                        case DBType.Oracle:
                            if (AddMod)
                                sql += " ADD " + ColumnName + " ";
                            else
                                sql += " MODIFY " + ColumnName + " ";
                            sql += ColType;
                            if (IsPrimaryKey)
                            { 
                                if (orgpri != null)
                                    res.Add(" ALTER TABLE " + TableName + " DROP CONSTRAINT PK_" + TableName + " ");
                                res.Add(" ALTER TABLE " + TableName + " ADD (CONSTRAINT PK_" + TableName + " KEY(" + ColumnName + ") "); 
                            } 
                            break;
                        #endregion

                        #region SQLite
                        case DBType.SQLite:
                            if (AddMod)
                                sql += " ADD " + ColumnName + " ";
                            sql += ColType; 
                            break;
                        #endregion

                        #region PostgreSQL
                        case DBType.PostgreSQL:
                            if (AddMod)
                                sql += " ADD " + ColumnName + " ";
                            else
                                sql += " ALTER " + ColumnName + " ";
                            sql += ColType;
                            if (IsIdentity)
                                sql += " SERIAL ";
                            if (IsPrimaryKey)
                            { 
                                if (orgpri != null)
                                    res.Add(" ALTER TABLE " + TableName + " DROP CONSTRAINT " + TableName + "_pkey ");
                                res.Add(" ALTER TABLE " + TableName + " ADD primary key ("+ ColumnName + ")");
                            } 
                            break;
                        #endregion

                        #region ODBC,OLEDB
                        case DBType.ODBC:
                        case DBType.OleDB:
                            if (AddMod)
                                sql += " ADD " + ColumnName + " ";
                            else
                                sql += " MODIFY " + ColumnName + " ";
                            sql += ColType; 
                            break;
                       #endregion

                    }
                    res.Insert(0,sql);
                } 
            }
            return res;
        }

        /// <summary>
        /// 创建视图
        /// </summary>
        /// <param name="ViewName">视图名</param>
        /// <param name="tsql">查询语句</param>
        /// <returns>TSQL</returns>
        public static string CreatViewToSql(this string ViewName,string tsql)
        {
            return "CREATE VIEW " + ViewName + " AS " + tsql;
        }

        /// <summary>
        /// 删除视图
        /// </summary>
        /// <param name="ViewName">视图名</param>
        /// <returns>TSQL</returns>
        public static string DropViewToSql(this string ViewName)
        {
            return "DROP VIEW " + ViewName;
        }

        /// <summary>
        /// 查询语句模拟为视图
        /// </summary>
        /// <param name="TSQL">查询语句</param>
        /// <returns></returns>
        public static string SimViewToSql(this string TSQL)
        {
            return "SELECT * FROM ("+ TSQL+ ") View_Sim ";
        }

        /// <summary>
        /// 拼接视图合成T-SQL
        /// </summary>
        /// <param name="Table">表名</param>
        /// <param name="Column">显示字段</param>
        /// <param name="IsUnique">数据是否重复</param>
        /// <returns>T-SQL</returns>
        public static string UnionToSql(this List<string> Table, List<string> Column,bool IsUnique=false)
        {
            if (Table==null) return "";
            if (Table.Count<=0) return "";
            string sql = "";
            if (Column != null)
            {
                foreach (var tmp in Column)
                {
                    if (string.IsNullOrEmpty(sql))
                        sql = tmp;
                    else
                        sql += "," + tmp;
                }
            }
            if (string.IsNullOrEmpty(sql))
                sql = "*";

            if (Table.Count == 1)
                return "SELECT " + sql + " FROM " + Table[0];
            else
            {
                string strs = "";
                foreach (var tmp in Table)
                {
                    if (string.IsNullOrEmpty(strs))
                        strs = "SELECT " + sql + " FROM " + tmp + " \r\n";
                    else
                        strs = strs + (IsUnique ? " UNION \r\n" : " UNION ALL \r\n") + "SELECT " + sql + " FROM " + tmp + " \r\n";
                }
                return strs;
            }
        }

    }
}
