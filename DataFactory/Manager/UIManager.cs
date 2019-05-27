using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.Common;
using LinqToDB;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.ComponentModel;
using NK.Message;
using NK.ENum;
using NK.Entity;
using NK.Event;
using NK.Attribut;
using NK.Interface;
using LinqToDB.Mapping;
using System.Reflection;

namespace NK.Data.Manager
{
    /// <summary>
    /// 描述记录
    /// </summary>
    [DisplayName("UIManager")]
    [Description("描述记录")]
    public class UIManager : DataHelper, IDisposable
    {

        #region 构造函数
 
        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        public UIManager():base()
        {
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        ///  Linq数据库处理
        /// </summary>
        /// <param name="info">数据库参数</param>
        public UIManager(DBInfo info = null):base(info)
        { 
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// Linq数据库处理
        /// </summary>
        /// <param name="ConnectionType">数据库类型</param>
        /// <param name="ConnectionString">连接串</param>
        /// <param name="Timeout">超时时间，毫秒</param>
        public UIManager(DBType ConnectionType, string ConnectionString, int Timeout = 60):base(ConnectionType, ConnectionString, Timeout)
        { 
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~UIManager()
        {
            Dispose(false);
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        } 
        #endregion

        #region 私有方法

        private string Serialize(object obj)
        {
            try
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                MemoryStream stream = new MemoryStream();
                serializer.WriteObject(stream, obj);
                byte[] dataBytes = new byte[stream.Length];
                stream.Position = 0;
                stream.Read(dataBytes, 0, (int)stream.Length);
                return Encoding.UTF8.GetString(dataBytes);
            }
            catch (Exception ex)
            { return ex.Message; }
        }

        private void Init()
        {
            init();
            if (!TableIsExist())
                CreatTable();
        }

        private void CreatTable()
        {
            string TableName = "";
            List<ColumnAttribute> Columns = EToSqlCreat<DisplayColumnAttribute>(out TableName);
            if (Columns.Where(c => c.IsPrimaryKey).Count() <= 0)
                throw new NullReferenceException(SystemMessage.RefNullOrEmpty("PrimaryKey", language));
            List<string> sqlbat = TableName.CreatToSql(DB.Mode, Columns);
            foreach (var tsql in sqlbat)
            {
                if (Execute(tsql) <= 0)
                    throw new Exception(tsql);
            }
        }

        private void DropTable()
        {
            string TableName = Table<DisplayColumnAttribute>();
            List<string> sqlbat = TableName.DropToSql();
            foreach (var tsql in sqlbat)
            {
                if (Execute(tsql) <= 0)
                    throw new Exception(tsql);
            }
        }

        private bool TableIsExist()
        { 
            string TableName = Table<DisplayColumnAttribute>();
            if (TableIsExist(TableName))
            {
                try
                {
                    var tmp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault();
                    return true;
                }
                catch
                {
                    DropTable();
                    return false;
                }
            }
            else
                return false;
        }

        #endregion

        #region 方法
 
        /// <summary>
        /// 获取字段描述
        /// </summary>
        /// <param name="UI">表或视图名</param>
        /// <returns></returns>
        [DisplayName("GetDispColumn")]
        [Description("获取字段描述")]
        public List<DisplayColumnAttribute> GetDispColumn(string UI)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<DisplayColumnAttribute> res = new List<DisplayColumnAttribute>();
            if (string.IsNullOrEmpty(UI))
                return res;
            try
            {
                res = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == UI.ToUpper().Trim() && c.Displaylanguage == this.language).ToList();
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }

        /// <summary>
        /// 设置字段描述
        /// </summary>
        /// <param name="UI">表或视图名称</param>
        ///  <param name="Column">修改的字段</param>
        /// <returns></returns>
        [DisplayName("SetDispColumn")]
        [Description("设置字段描述")]
        public bool SetDispColumn(string UI, List<DisplayColumnAttribute> Column)
        {
            if (string.IsNullOrEmpty(UI))
                return false;
            else if (Column == null)
                return false;
            else if (Column.Count <= 0)
                return false;
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                var ColumnLS = Column.Where(c => c.Table.ToUpper().Trim() == UI.ToUpper().Trim() && c.Displaylanguage == this.language); 
                foreach (var col in ColumnLS)
                {
                    var tmp = context.GetTable<DisplayColumnAttribute>().FirstOrDefault(c => c.Table.ToUpper().Trim() == UI.ToUpper().Trim() && col.Column.ToUpper().Trim() == c.Column.ToUpper().Trim() && c.Displaylanguage == this.language);
                    if (tmp != null)
                    {
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.index = col.index;
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Displaylanguage = col.Displaylanguage;
                        tmp.Caption = col.Caption;
                        context.Update(tmp);
                    }
                    else
                    {
                        tmp = new DisplayColumnAttribute();
                        tmp.JS = (string.IsNullOrEmpty(col.JS) ? "" : col.JS);
                        tmp.CSS = (string.IsNullOrEmpty(col.CSS) ? "" : col.CSS);
                        tmp.Format = (string.IsNullOrEmpty(col.Format) ? "" : col.Format);
                        tmp.Unit = (string.IsNullOrEmpty(col.Unit) ? "" : col.Unit);
                        tmp.index = col.index;
                        tmp.Seqencing = col.Seqencing;
                        tmp.CanCount = col.CanCount;
                        tmp.CanHead = col.CanHead;
                        tmp.CanSearch = col.CanSearch;
                        tmp.CanImpExp = col.CanImpExp;
                        tmp.IsUnique = col.IsUnique;
                        tmp.CanDeitail = col.CanDeitail;
                        tmp.Displaylanguage = col.Displaylanguage;
                        tmp.Caption = col.Caption;
                        context.Insert(col);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// 删除设置
        /// </summary>
        /// <param name="UI">表或视图名称</param>
        /// <returns></returns>
        [DisplayName("DelDispColumn")]
        [Description("删除设置")]
        public bool DelDispColumn(string UI)
        {
            if (string.IsNullOrEmpty(UI))
                return false;
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            try
            {
                var LS = context.GetTable<DisplayColumnAttribute>().Where(c => c.Table.ToUpper().Trim() == UI.ToUpper().Trim() && c.Displaylanguage==this.language).ToList();
                foreach (var col in LS)
                    context.Delete(col);
                return true;
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
                return false;
            }
        }

        /// <summary>
        /// UI显示
        /// </summary>
        /// <param name="Name"></param>
        /// <returns></returns>
        public List<DisplayColumnAttribute> UIDisplay(string Name)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<DisplayColumnAttribute> res = null;
            try
            { 
                res = GetDispColumn(Name);
                if (res == null) res = new List<DisplayColumnAttribute>();
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }

        /// <summary>
        /// UI显示
        /// </summary> 
        /// <param name="Name"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Dictionary<DisplayColumnAttribute, object> UIDisplay(string Name, Dictionary<string, object> Value)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            List<DisplayColumnAttribute> res = null;
            try
            {
                res = GetDispColumn(Name);
                if (res == null) res = new List<DisplayColumnAttribute>();
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return UIColumn(res,Value);
        }

        /// <summary>
        /// UI显示带显示值
        /// </summary>
        /// <param name="Column"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Dictionary<DisplayColumnAttribute, object> UIColumn(List<DisplayColumnAttribute> Column, Dictionary<string, object> Value)
        {
            Init();
            MethodName = "";
            try
            {
                System.Reflection.MethodBase method = new System.Diagnostics.StackTrace().GetFrame(0).GetMethod();
                MethodName = method.Name;
            }
            catch { }
            Dictionary<DisplayColumnAttribute, object> res = new Dictionary<DisplayColumnAttribute, object>();
            if (Value == null) Value = new Dictionary<string, object>();
            if (Column == null) Column = new List<DisplayColumnAttribute>();
            try
            {
                foreach (var col in Column)
                {
                    if (Value.Where(c => c.Key == col.Column).Count() > 0)
                    {
                        var dic = Value.FirstOrDefault(c => c.Key == col.Name);
                        res.Add(col, dic.Value);
                    }
                    else
                        res.Add(col, null);
                }
            }
            catch (Exception ex)
            {
                if (HasError != null)
                    HasError(ClassName, MethodName, ex);
                else
                    throw ex;
            }
            return res;
        }


        #endregion

    }
}
