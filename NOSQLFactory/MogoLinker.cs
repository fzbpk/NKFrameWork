using NK.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using NK.ENum;
using System.Reflection;
using System.Linq.Expressions;
using Newtonsoft.Json;
using MongoDB.Driver.Linq;

namespace NK.Data
{
    /// <summary>
    /// Mongo数据库
    /// </summary>
    public class MongoLinker :NoSQLHelper
    {
        protected IMongoDatabase context = null;

        #region 构造


        public MongoLinker()
        {
            DB = new DBInfo();
            DB.ConfigName = "default";
            DB.Enable = true;
            NEW();
        }

        public MongoLinker(DBInfo info)
        {
            DB = info;
            NEW();
        }

        public MongoLinker(string ConnectionString, int Timeouts = 60)
        {
            DB = new DBInfo();
            DB.ConfigName = "default";
            DB.Enable = true;
            DB.ConnStr = ConnectionString;
            DB.Mode = DBType.MongoDB;
            DB.TimeOut = Timeouts;
            NEW();
        }
        #endregion

        #region 私有方法

        protected override void Initialization()
        {
            if (context == null)
            {
                var conventionPack = new ConventionPack { new IgnoreExtraElementsConvention(true) };
                ConventionRegistry.Register("IgnoreExtraElements", conventionPack, type => true);
                MongoClientSettings mongoSetting = MongoClientSettings.FromConnectionString(connstr);
                if (DB.TimeOut > 0)
                    mongoSetting.ConnectTimeout = new TimeSpan(DB.TimeOut * TimeSpan.TicksPerSecond);
                var client = new MongoClient(mongoSetting);
                context = client.GetDatabase(DB.DataBaseName);
            }
        }

        protected override void dispose()
        {
            if (context != null)
                context = null;
        }

        internal Dictionary<ColumnAttribute, object> PropertyInfoToCol(object entity)
        {
            Dictionary<ColumnAttribute, object> Col = new Dictionary<ColumnAttribute, object>();
            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (p != null)
                {
                    string ColumnName = p.Name;
                    ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                    bool IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                    bool IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                    bool CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                    ColumnName = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name : p.Name));
                    ColumnAttribute res = new ColumnAttribute();
                    res.Name = ColumnName;
                    res.CanBeNull = CanBeNull;
                    res.IsPrimaryKey = IsPrimaryKey;
                    res.IsIdentity = IsIdentity;
                    Type t = p.PropertyType;
                    if (t.SupportedType())
                        Col.Add(res, p.GetValue(entity, null));
                }
            }
            return Col;
        }

        protected Dictionary<PropertyInfo,ColumnAttribute> PropertyInfoToCol(Type entity)
        {
            Dictionary<PropertyInfo, ColumnAttribute> Col = new Dictionary<PropertyInfo, ColumnAttribute>();
            PropertyInfo[] properties = entity.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                string ColumnName = p.Name;
                ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                bool IsPrimaryKey = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsPrimaryKey : false));
                bool IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                bool CanBeNull = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().CanBeNull : false));
                ColumnName = (ColumnAttributes == null ? p.Name : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().Name : p.Name));
                ColumnAttribute res = new ColumnAttribute();
                res.Name = ColumnName;
                res.CanBeNull = CanBeNull;
                res.IsPrimaryKey = IsPrimaryKey;
                res.IsIdentity = IsIdentity;
                Type t = p.PropertyType;
                if (t.SupportedType())
                    Col.Add(p,res);
            }
            return Col;
        }

        protected void UpdatePK<T>(ref T entity, out string TableName, IMongoDatabase context)
        {
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
            PropertyInfo[] properties = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo p in properties)
            {
                if (p != null)
                {
                    ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                    bool IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                    if (!IsIdentity) continue;
                    Type t = p.PropertyType;
                    if (t.IsValueType)
                    {
                        var sort = Builders<T>.Sort.Descending(p.Name);
                        IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                        var RM = dat.Find<T>(c => true).Sort(sort).FirstOrDefault();
                        var Pr = entity.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(c => c.Name == p.Name);
                        int ids = 0;
                        try
                        { ids = Convert.ToInt32(Pr.GetValue(RM, null)); }
                        catch { };
                        ids += 1;
                        p.SetValue(entity, ids);
                    }
                    else if (t == typeof(Guid))
                        p.SetValue(entity, Guid.NewGuid());
                    else if (t == typeof(string))
                        p.SetValue(entity, Guid.NewGuid().ToString());
                    break;
                }
            }
        }

        protected Dictionary<string, object> EToSqlUpdateEX(object entity, out string TableName, out KeyValuePair<string, object> where)
        {
            where = new KeyValuePair<string, object>("", null);
            Dictionary<string, object> ColSVal = new Dictionary<string, object>();
            TableAttribute[] TableAttributes = (TableAttribute[])entity.GetType().GetCustomAttributes(typeof(TableAttribute), false);
            TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = entity.GetType().Name;
            Dictionary<ColumnAttribute, object> ColS = PropertyInfoToCol(entity);
            if (ColS.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
            {
                for (int i = 0; i < ColS.Count; i++)
                {
                    if (ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "ID" || ColS.ElementAt(i).Key.Name.ToUpper().Trim() == "INDEX")
                        ColS.ElementAt(i).Key.IsPrimaryKey = true;
                }
            }
            if (ColS.Where(c => c.Key.IsPrimaryKey).Count() <= 0)
                throw new NullReferenceException("PrimaryKey");
            foreach (var col in ColS)
            {
                if (col.Key.IsPrimaryKey)
                {
                    if (col.Value == null)
                        throw new NullReferenceException(col.Key.Name);
                    where = new KeyValuePair<string, object>(col.Key.Name, col.Value);
                }
                else if (col.Key.IsIdentity)
                    continue;
                else if (col.Value != null)
                    ColSVal.Add(col.Key.Name, col.Value);
                else if (col.Key.CanBeNull)
                    ColSVal.Add(col.Key.Name, null);
                else
                    throw new NullReferenceException(col.Key.Name);
            }
            return ColSVal;
        }

        protected virtual string ToTableName<T>()
        {
            TableAttribute[] TableAttributes = (TableAttribute[])typeof(T).GetCustomAttributes(typeof(TableAttribute), false);
            string TableName = (TableAttributes == null ? "" : (TableAttributes.Length > 0 ? TableAttributes.ToList().First().Name.Trim() : ""));
            if (string.IsNullOrEmpty(TableName))
                TableName = typeof(T).Name;
            return TableName;
        }

        protected dynamic BsonTodynamic(BsonDocument bs)
        {
            dynamic ss = new DynamicData(bs.ToDictionary());
            return ss;
        }

        protected string BsonToJson(BsonDocument bs)
        {
            var ss = Newtonsoft.Json.JsonConvert.SerializeObject(BsonTodynamic(bs).ToObject());
            return ss;
        }

        protected string BsonToJson(List<BsonDocument> bs)
        {
            List<dynamic> res = new List<dynamic>();
            foreach (var b in bs)
                res.Add(BsonToJson(b));
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        protected dynamic BsonTodynamic(List<BsonDocument> bs)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(BsonToJson(bs));
        }


        #endregion

        #region 表

        /// <summary>
        /// 测试数据库连接
        /// </summary>
        /// <param name="errmsg"></param>
        /// <returns></returns>
        public   bool CheckDataBase(out string errmsg)
        {
            init();
            try
            {
                context.ListCollections();
                errmsg = "";
                return true;
            }
            catch (Exception ex)
            {
                errmsg = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 执行命令
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public   string StoredProcedure(string cmd)
        {
            init();
            try
            {
                return BsonToJson(context.RunCommand(new JsonCommand<BsonDocument>("{ eval: \"" + cmd + "\" }")));
            }
            catch (Exception ex)
            { throw ex; }
        }

        #endregion

        #region 实体

        /// <summary>
        /// 实体表名
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public string TableName<T>()
        {
            return ToTableName<T>();
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>执行结果</returns> 
        public   void CreatTable<T>(string TableName="")
        {
            init();
            try
            {
                if(string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                context.GetCollection<T>(TableName);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删表
        /// </summary>
        /// <returns></returns> 
        public   void DropTable<T>(string TableName = "")
        {
            init();
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                context.DropCollection(TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns> 
        public   bool TableIsExist<T>()
        {
            init();
            try
            {
                string TableName = ToTableName<T>();
                var options = new ListCollectionsOptions
                {
                    Filter = Builders<BsonDocument>.Filter.Eq("name", TableName)
                };
                return context.ListCollections(options).ToEnumerable().Any(); ;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>符合条件的记录</returns>
        public IMongoQueryable<T> GetTable<T>(string TableName = "")
        {
            init();
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                return dat.AsQueryable();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <returns>符合条件的记录</returns>
        public IMongoCollection<T> GetCollection<T>(string TableName = "")
        {
            init();
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                return context.GetCollection<T>(TableName); 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Entity">条件</param>
        /// <returns></returns>
        public   void Insert<T>(T Entity,string TableNames = "")
        {
            init();
            try
            {
                string TableName = "";
                UpdatePK<T>(ref Entity, out TableName, context);
                if (string.IsNullOrEmpty(TableNames))
                    TableName = ToTableName<T>();
                else
                    TableName = TableNames;
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                dat.InsertOne(Entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public void InsertBat<T>(List<T> Entity, string TableName = "")
        {
            init();
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName); 
                PropertyInfo[] properties =typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                PropertyInfo pk = null;
                foreach (PropertyInfo p in properties)
                {
                    if (p != null)
                    {
                        ColumnAttribute[] ColumnAttributes = (ColumnAttribute[])p.GetCustomAttributes(typeof(ColumnAttribute), false);
                        bool IsIdentity = (ColumnAttributes == null ? false : (ColumnAttributes.Length > 0 ? ColumnAttributes.ToList().First().IsIdentity : false));
                        if (!IsIdentity) continue;
                        pk = p;  
                        break;
                    }
                }
                List<T> bat = new List<T>();
                if (pk != null)
                {
                    int count = 0;
                    Type t = pk.PropertyType;
                    if (t.IsValueType)
                    {
                        var sort = Builders<T>.Sort.Descending(pk.Name);
                        var RM = dat.Find<T>(c => true).Sort(sort).FirstOrDefault(); 
                        try
                        { count = Convert.ToInt32(pk.GetValue(RM, null)); }
                        catch { };
                        count += 1; 
                    }
                    Entity.ForEach(m => {
                        if (t.IsValueType)
                        {
                            pk.SetValue(m, count);
                            count+=1;
                        }
                        else if (t == typeof(Guid))
                            pk.SetValue(m, Guid.NewGuid());
                        else if (t == typeof(string))
                            pk.SetValue(m, Guid.NewGuid().ToString());
                        bat.Add(m);
                    });
                    dat.InsertMany(bat);
                }
                else
                    dat.InsertMany(Entity);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="Entity">实体</param>
        public   void Update<T>(T Entity, string TableNames = "")
        {
            init();
            try
            {
                string TableName = ""; 
                KeyValuePair<string, object> Pkey = new KeyValuePair<string, object>("", null);
                var ColSVal = EToSqlUpdateEX(Entity, out TableName, out Pkey);
                if (string.IsNullOrEmpty(TableNames))
                    TableName = ToTableName<T>();
                else
                    TableName = TableNames;
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                dat.ReplaceOne(Builders<T>.Filter.Eq(Pkey.Key, Pkey.Value), Entity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public T Find<T>(Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false, string TableName = "")
        {
            init();
            try
            {
                if (whereLambda == null) whereLambda = c => true;
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                if (orderLambda == null)
                    return dat.Find(whereLambda).FirstOrDefault();
                else if (ASCDESC)
                    return dat.Find(whereLambda).Sort(Builders<T>.Sort.Ascending(orderLambda)).FirstOrDefault();
                else
                    return dat.Find(whereLambda).Sort(Builders<T>.Sort.Descending(orderLambda)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="whereLambda"></param>
        /// <param name="orderLambda"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public T Find<T>(FilterDefinition<T> whereLambda  , Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false, string TableName = "")
        {
            init();
            try
            { 
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                if (orderLambda == null)
                    return dat.Find(whereLambda).FirstOrDefault();
                else if (ASCDESC)
                    return dat.Find(whereLambda).Sort(Builders<T>.Sort.Ascending(orderLambda)).FirstOrDefault();
                else
                    return dat.Find(whereLambda).Sort(Builders<T>.Sort.Descending(orderLambda)).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Entity"></param>
        public   void Delete<T>(T Entity, string TableNames = "")
        {
            init();
            try
            {
                string TableName = "";
                KeyValuePair<string, object> Pkey = new KeyValuePair<string, object>("", null);
                var ColSVal = EToSqlUpdateEX(Entity, out TableName, out Pkey);
                if (string.IsNullOrEmpty(TableNames))
                    TableName = ToTableName<T>();
                else
                    TableName = TableNames;
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                dat.DeleteOne(Builders<T>.Filter.Eq(Pkey.Key, Pkey.Value));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">查询条件</param>
        /// <param name="ASCDESC">查询条件</param>
        /// <returns>符合条件的记录</returns>
        public   List<T> Query<T>(Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false, string TableName = "")
        {
            init();
            if (whereLambda == null) whereLambda = c => true;
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                if (orderLambda == null)
                    return dat.Find<T>(whereLambda).ToList();
                else if (ASCDESC)
                    return dat.Find<T>(whereLambda).Sort(Builders<T>.Sort.Ascending(orderLambda)).ToList();
                else
                    return dat.Find<T>(whereLambda).Sort(Builders<T>.Sort.Descending(orderLambda)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        /// <summary>
        /// 查询符合条件的记录
        /// </summary>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">查询条件</param>
        /// <param name="ASCDESC">查询条件</param>
        /// <returns>符合条件的记录</returns>
        public List<T> Query<T>(FilterDefinition<T> whereLambda  , Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false, string TableName = "")
        {
            init(); 
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                if (orderLambda == null)
                    return dat.Find(whereLambda).ToList();
                else if (ASCDESC)
                    return dat.Find(whereLambda).Sort(Builders<T>.Sort.Ascending(orderLambda)).ToList();
                else
                    return dat.Find(whereLambda).Sort(Builders<T>.Sort.Descending(orderLambda)).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="PageIndex">当前页，从1开始</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">记录数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序字段</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>符合条件的记录</returns> 
        public   List<T> Select<T>(int PageIndex, int PageSize, out int PageCount, out int RecordCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false, string TableName = "")
        {
            init();
            PageCount = 0;
            RecordCount = 0;
            if (whereLambda == null) whereLambda = c => true;
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                RecordCount = (int)dat.CountDocuments(whereLambda);
                if (PageSize == 0)
                    PageCount = (RecordCount > 0 ? 1 : 0);
                else
                    PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1);
                if (orderLambda == null)
                    return dat.Find(whereLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                else if (ASCDESC)
                    return dat.Find(whereLambda).SortBy(orderLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                else
                    return dat.Find(whereLambda).SortByDescending(orderLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页查询数据
        /// </summary>
        /// <param name="PageIndex">当前页，从1开始</param>
        /// <param name="PageSize">页面大小</param>
        /// <param name="PageCount">总页数</param>
        /// <param name="RecordCount">记录数</param>
        /// <param name="whereLambda">查询条件</param>
        /// <param name="orderLambda">排序字段</param>
        /// <param name="ASCDESC">顺序倒叙</param>
        /// <returns>符合条件的记录</returns> 
        public List<T> Select<T>(FilterDefinition<T> whereLambda,int PageIndex, int PageSize, out int PageCount, out int RecordCount, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false, string TableName = "")
        {
            init();
            PageCount = 0;
            RecordCount = 0; 
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                RecordCount = (int)dat.CountDocuments(whereLambda);
                if (PageSize == 0)
                    PageCount = (RecordCount > 0 ? 1 : 0);
                else
                    PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1);
                if (orderLambda == null)
                    return dat.Find(whereLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                else if (ASCDESC)
                    return dat.Find(whereLambda).SortBy(orderLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                else
                    return dat.Find(whereLambda).SortByDescending(orderLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public long Count<T>(Expression<Func<T, bool>> whereLambda, string TableName = "")
        {
            init();
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                return dat.Count(whereLambda);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public long Count<T>(FilterDefinition<T> whereLambda, string TableName = "")
        {
            init();
            try
            {
                if (string.IsNullOrEmpty(TableName))
                    TableName = ToTableName<T>();
                IMongoCollection<T> dat = context.GetCollection<T>(TableName);
                return dat.Count(whereLambda);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         
        public List<T> History<T>(DateTime SDT, DateTime EDT, int PageIndex, int PageSize, out long PageCount, out long RecordCount, Expression<Func<T, bool>> whereLambda = null, Expression<Func<T, object>> orderLambda = null, bool ASCDESC = false )
        {
            init();
            PageCount = 0;
            RecordCount = 0;
            if (whereLambda == null) whereLambda = c => true;
            DateTime DT = DateTime.Now;
            List<string> Mothtable = new List<string>();
            List<string> tabls = new List<string>();
            string TableName = ToTableName<T>();
             context.ListCollections(new ListCollectionsOptions
            {
                Filter = Builders<BsonDocument>.Filter.Regex("name", new BsonRegularExpression(TableName))
            }).ToList().ForEach(m=> {
                tabls.Add(m["name"].ToString());
            });
            int i = 0;
            string MainTable ="";
            while ((DT = SDT.AddMonths(i)) <= EDT.AddMonths(1))
            {
                string tablename = TableName + DT.ToString("yyyyMM");
                if (tabls.FirstOrDefault(c => c == tablename) != null)
                {
                    if (string.IsNullOrEmpty(MainTable) && context.GetCollection<T>(tablename).CountDocuments(c => true) > 0)
                        MainTable = tablename;
                    else  
                        Mothtable.Add(tablename);
                } 
                i++;
            }
            if (string.IsNullOrEmpty(MainTable)) MainTable = TableName;
            string projectjson = "";
            foreach (var col in PropertyInfoToCol(typeof(T)))
                projectjson += "," + col.Key.Name + ":\"$allValue." + col.Value.Name+"\"";
            projectjson = "{_id:\"any\"" + projectjson + "}";
            IAggregateFluent<T> qt = context.GetCollection<T>(MainTable).Aggregate(new AggregateOptions() {
                AllowDiskUse = true,
                UseCursor = true,
                BatchSize=100
            }) ; 
            Mothtable.ForEach(m => {
                qt = qt.Group<T>("{_id:\"any\",tabA:{ $push: \"$$ROOT\"}}").Lookup<T>(m, "invalidField", "testField", "tabB").Project<T>("{ _id:0, allValue:{ $setUnion:[\"$tabA\", \"$tabB\"] } }").Unwind<T>("allValue").Project<T>(projectjson);
            }); 
            qt = qt.Match(whereLambda);
            var ic = qt.Out("tempreport");
            var sss = ic.Current;


            var rec = qt.Count();
            if (rec.ToList().FirstOrDefault() == null)
                RecordCount = qt.ToList().Count;
            else
                RecordCount = rec.ToList().FirstOrDefault().Count;
            if (PageSize == 0)
                PageCount = (RecordCount > 0 ? 1 : 0);
            else
                PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1); 
            if(orderLambda==null)
              return qt.Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();  
            else if(ASCDESC)
                return qt.SortByDescending<T>(orderLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
            else
                return qt.SortBy<T>(orderLambda).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
        }
         
        #endregion

        #region JSON

        /// <summary>
        /// 插入JSON
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="json"></param>
        public void Insert(string TableName, string json)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("json");
            try
            {
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.InsertOne(BsonDocument.Parse(json));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新JSON
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="json"></param>
        /// <param name="Keys"></param>
        public   void Update(string TableName, string json, Dictionary<string, object> Keys)
        {
            init();
            if (Keys != null)
                throw new NullReferenceException("Keys");
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            if (string.IsNullOrEmpty(json))
                throw new NullReferenceException("json");
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);

                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                foreach (var Key in Keys)
                    where.Add(builderFilter.Eq(Key.Key, Key.Value));

                dat.ReplaceOne(builderFilter.And(where), BsonDocument.Parse(json));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public   string FindJson(string TableName, Dictionary<string, object> Keys, string OrderKey = "", bool ASCDESC = false)
        {
            init();
            if (Keys != null)
                throw new NullReferenceException("Keys");
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                foreach (var Key in Keys)
                    where.Add(builderFilter.Eq(Key.Key, Key.Value));
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                if (string.IsNullOrEmpty(OrderKey))
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return BsonToJson(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return BsonToJson(result);
                }
                else
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return BsonToJson(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public   string QueryJson(string TableName, Dictionary<string, object> Keys = null, string OrderKey = "", bool ASCDESC = false)
        {
            init();
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                if (Keys != null)
                {
                    foreach (var Key in Keys)
                        where.Add(builderFilter.Eq(Key.Key, Key.Value));
                }
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                if (string.IsNullOrEmpty(OrderKey))
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return BsonToJson(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return BsonToJson(result);
                }
                else
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return BsonToJson(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageCount"></param>
        /// <param name="RecordCount"></param>
        /// <param name="Keys"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public   string SelectJson(string TableName, int PageIndex, int PageSize, out int PageCount, out int RecordCount, Dictionary<string, object> Keys = null, string OrderKey = "", bool ASCDESC = false)
        {
            init();
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                if (Keys != null)
                {
                    foreach (var Key in Keys)
                        where.Add(builderFilter.Eq(Key.Key, Key.Value));
                }
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                RecordCount = (int)dat.CountDocuments((where.Count > 0 ? builderFilter.And(where) : new BsonDocument()));
                if (PageSize == 0)
                    PageCount = (RecordCount > 0 ? 1 : 0);
                else
                    PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1);
                if (string.IsNullOrEmpty(OrderKey))
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonToJson(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonToJson(result);
                }
                else
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonToJson(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Object

        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>执行结果</returns> 
        public   void CreatTable(string TableName)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                context.GetCollection<object>(TableName);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删表
        /// </summary>
        /// <returns></returns> 
        public   void DropTable(string TableName)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                context.DropCollection(TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns> 
        public   bool TableIsExist(string TableName)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                var options = new ListCollectionsOptions
                {
                    Filter = Builders<BsonDocument>.Filter.Eq("name", TableName)
                };
                return context.ListCollections(options).ToEnumerable().Any(); ;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Obj"></param>
        public   void Insert(string TableName, object Obj)
        {
            init();
            try
            {
                BsonDocument bd = BsonDocument.Parse(JsonConvert.SerializeObject(Obj));
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.InsertOne(bd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Obj"></param>
        /// <param name="Keys"></param>
        public   void Update(string TableName, object Obj, Dictionary<string, object> Keys)
        {
            init();
            if (Keys != null)
                throw new NullReferenceException("Keys");
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                BsonDocument bd = BsonDocument.Parse(JsonConvert.SerializeObject(Obj));
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                foreach (var Key in Keys)
                    where.Add(builderFilter.Eq(Key.Key, Key.Value));
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.ReplaceOne(builderFilter.And(where), bd);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        public   void Delete(string TableName, Dictionary<string, object> Keys)
        {
            init();
            if (Keys != null)
                throw new NullReferenceException("Keys");
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                foreach (var Key in Keys)
                    where.Add(builderFilter.Eq(Key.Key, Key.Value));
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.DeleteOne(builderFilter.And(where));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public   dynamic Find(string TableName, Dictionary<string, object> Keys, string OrderKey = "", bool ASCDESC = false)
        {
            init();
            if (Keys != null)
                throw new NullReferenceException("Keys");
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                foreach (var Key in Keys)
                    where.Add(builderFilter.Eq(Key.Key, Key.Value));
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                if (string.IsNullOrEmpty(OrderKey))
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return BsonTodynamic(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return BsonTodynamic(result);
                }
                else
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return BsonTodynamic(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///查询记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public   dynamic Query(string TableName, Dictionary<string, object> Keys = null, string OrderKey = "", bool ASCDESC = false)
        {
            init();
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                if (Keys != null)
                {
                    foreach (var Key in Keys)
                        where.Add(builderFilter.Eq(Key.Key, Key.Value));
                }
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                if (string.IsNullOrEmpty(OrderKey))
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return BsonTodynamic(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return BsonTodynamic(result);
                }
                else
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return BsonTodynamic(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageCount"></param>
        /// <param name="RecordCount"></param>
        /// <param name="Keys"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public   dynamic Select(string TableName, int PageIndex, int PageSize, out int PageCount, out int RecordCount, Dictionary<string, object> Keys = null, string OrderKey = "", bool ASCDESC = false)
        {
            init();
            try
            {
                FilterDefinitionBuilder<BsonDocument> builderFilter = Builders<BsonDocument>.Filter;
                List<FilterDefinition<BsonDocument>> where = new List<FilterDefinition<BsonDocument>>();
                if (Keys != null)
                {
                    foreach (var Key in Keys)
                        where.Add(builderFilter.Eq(Key.Key, Key.Value));
                }
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                RecordCount = (int)dat.CountDocuments((where.Count > 0 ? builderFilter.And(where) : new BsonDocument()));
                if (PageSize == 0)
                    PageCount = (RecordCount > 0 ? 1 : 0);
                else
                    PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1);
                if (string.IsNullOrEmpty(OrderKey))
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonTodynamic(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonTodynamic(result);
                }
                else
                {
                    var result = dat.Find((where.Count > 0 ? builderFilter.And(where) : new BsonDocument())).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonTodynamic(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Mongo

        public IMongoDatabase DbConnection { get {
                if (context == null) init();
                return context;
            }
        }

        /// <summary>
        /// 创建表
        /// </summary>
        /// <returns>执行结果</returns> 
        public void CreatBson(string TableName)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                context.GetCollection<object>(TableName);
            }
            catch (Exception ex)
            { throw ex; }
        }

        /// <summary>
        /// 删表
        /// </summary>
        /// <returns></returns> 
        public void DropBson(string TableName)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                context.DropCollection(TableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <returns>是否存在</returns> 
        public bool BsonIsExist(string TableName)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                var options = new ListCollectionsOptions
                {
                    Filter = Builders<BsonDocument>.Filter.Eq("name", TableName)
                };
                return context.ListCollections(options).ToEnumerable().Any(); ;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="TableName">表名</param>
        /// <param name="Bson"></param>
        public void Insert(string TableName, BsonDocument Bson)
        {
            init();
            try
            {
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.InsertOne(Bson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Bson"></param>
        /// <param name="where"></param>
        public void Update(string TableName, BsonDocument Bson, BsonDocument where)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.ReplaceOne(where, Bson);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        public void Delete(string TableName, BsonDocument where)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                dat.DeleteOne(where);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public BsonDocument Find(string TableName, BsonDocument where, Expression<Func<BsonDocument, object>> OrderKey = null, bool ASCDESC = false)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                if (where == null) where = new BsonDocument();
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                if (OrderKey == null)
                {
                    var result = dat.Find(where).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return result;
                }
                else if (ASCDESC)
                {
                    var result = dat.Find(where).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return result;
                }
                else
                {
                    var result = dat.Find(where).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).FirstOrDefault();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        ///查询记录
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="Keys"></param>
        /// <returns></returns>
        public List<BsonDocument> Query(string TableName, BsonDocument where, Expression<Func<BsonDocument, object>> OrderKey = null, bool ASCDESC = false)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                if (where == null) where = new BsonDocument();
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                if (OrderKey == null)
                {
                    var result = dat.Find(where).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return result;
                }
                else if (ASCDESC)
                {
                    var result = dat.Find(where).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return result;
                }
                else
                {
                    var result = dat.Find(where).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).ToList();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="TableName"></param>
        /// <param name="PageIndex"></param>
        /// <param name="PageSize"></param>
        /// <param name="PageCount"></param>
        /// <param name="RecordCount"></param>
        /// <param name="Keys"></param>
        /// <param name="OrderKey"></param>
        /// <param name="ASCDESC"></param>
        /// <returns></returns>
        public List<BsonDocument> Select(string TableName, int PageIndex, int PageSize, out int PageCount, out int RecordCount, BsonDocument where, Expression<Func<BsonDocument, object>> OrderKey = null, bool ASCDESC = false)
        {
            init();
            if (string.IsNullOrEmpty(TableName))
                throw new NullReferenceException("TableName");
            try
            {
                if (where == null) where = new BsonDocument();
                IMongoCollection<BsonDocument> dat = context.GetCollection<BsonDocument>(TableName);
                RecordCount = (int)dat.CountDocuments(where);
                if (PageSize == 0)
                    PageCount = (RecordCount > 0 ? 1 : 0);
                else
                    PageCount = (RecordCount % PageSize == 0 ? RecordCount / PageSize : RecordCount / PageSize + 1);
                if (OrderKey == null)
                {
                    var result = dat.Find(where).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonTodynamic(result);
                }
                else if (ASCDESC)
                {
                    var result = dat.Find(where).Sort(Builders<BsonDocument>.Sort.Ascending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonTodynamic(result);
                }
                else
                {
                    var result = dat.Find(where).Sort(Builders<BsonDocument>.Sort.Descending(OrderKey)).Project(Builders<BsonDocument>.Projection.Exclude("_id")).Skip((PageIndex - 1) * PageSize).Limit(PageSize).ToList();
                    return BsonTodynamic(result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }

}
