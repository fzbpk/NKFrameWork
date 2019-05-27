using System;
using System.Collections.Generic;
using System.Dynamic;

namespace NK.Data
{
    /// <summary>
    /// 数据库可变类型
    /// </summary>
    public class DynamicData : DynamicObject
    {
        //保存对象动态定义的属性值  
        public Dictionary<string, object> Property;

        /// <summary>
        /// 数据库可变类型
        /// </summary>
        public DynamicData()
        {
            Property = new Dictionary<string, object>();
        }

        /// <summary>
        /// 数据库可变类型
        /// </summary>
        /// <param name="obj"></param>
        public DynamicData(Dictionary<string, object> obj)
        {
            Property = new Dictionary<string, object>();
            if (obj != null)
            {
                foreach (var dic in obj)
                    Property.Add(dic.Key, dic.Value);
            }
        }

        /// <summary>  
        /// 获取属性值  
        /// </summary>  
        /// <param name="propertyName"></param>  
        /// <returns></returns>  
        public object GetPropertyValue(string propertyName)
        {
            if (Property.ContainsKey(propertyName) == true)
            {
                return Property[propertyName];
            }
            return null;
        }
        /// <summary>  
        /// 设置属性值  
        /// </summary>  
        /// <param name="propertyName"></param>  
        /// <param name="value"></param>  
        public void SetPropertyValue(string propertyName, object value)
        {
            if (Property.ContainsKey(propertyName) == true)
            {
                Property[propertyName] = value;
            }
            else
            {
                Property.Add(propertyName, value);
            }
        }
        /// <summary>  
        /// 实现动态对象属性成员访问的方法，得到返回指定属性的值  
        /// </summary>  
        /// <param name="binder"></param>  
        /// <param name="result"></param>  
        /// <returns></returns>  
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            result = GetPropertyValue(binder.Name);
            return result == null ? false : true;
        }
        /// <summary>  
        /// 实现动态对象属性值设置的方法。  
        /// </summary>  
        /// <param name="binder"></param>  
        /// <param name="value"></param>  
        /// <returns></returns>  
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            SetPropertyValue(binder.Name, value);
            return true;
        }
        /// <summary>
        /// 属性数
        /// </summary>
        public int Count { get { return Property.Count; } }
        /// <summary>
        /// 属性
        /// </summary>
        public Dictionary<string, Type> GetProperty
        {
            get
            {
                Dictionary<string, Type> res = new Dictionary<string, Type>();
                foreach (var dic in Property)
                {
                    if (dic.Value == null)
                        res.Add(dic.Key, null);
                    else
                        res.Add(dic.Key, dic.Value.GetType());
                }
                return res;
            }
        }

        /// <summary>
        /// 转对象
        /// </summary>
        public object ToObject()
        {
            return (object)Property;
        }

    }

}
