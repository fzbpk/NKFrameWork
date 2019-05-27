using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
namespace NK.OS
{
    public class DLLLoader : IDisposable
    {
        #region 定义

        private Assembly DLL = null;
        private object DyncClass = null;
        private Type DyncType = null;
        private bool m_disposed;
        #endregion

        #region 构造函数

        /// <summary>
        /// 加载DLL
        /// </summary>
        /// <param name="FilePath">DLL位置</param>
        public DLLLoader(string FilePath)
        {
            if(!string.IsNullOrEmpty(FilePath))
              DLL = Assembly.Load(FilePath);
        }

        /// <summary>
        /// 加载DLL
        /// </summary>
        /// <param name="FilePath">文件信息</param>
        public DLLLoader(FileInfo FilePath)
        {
           if(FilePath!=null)
             DLL = Assembly.Load(FilePath.FullName);
        }
        /// <summary>
        /// 释放资源
        /// </summary>
        ~DLLLoader()
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

        /// <summary>
        /// 释放连接
        /// </summary>
        /// <param name="disposing">是否释放</param>
        protected virtual void Dispose(bool disposing)
        {
            lock (this)
            {
                if (disposing && !m_disposed)
                {
                    if (DLL == null || DyncClass == null || DyncType == null)
                        return; 
                    DyncClass = null;
                    DyncType = null;
                    m_disposed = true;
                }
            }
        }


        #endregion

        #region 属性

        /// <summary>
        /// DLL全名
        /// </summary>
        public string FullName
        {
            get
            {
                if (DLL == null)
                    return "";
                return DLL.FullName;
            }
        }

        /// <summary>
        /// 版本
        /// </summary>
        public string RuntimeVersion
        {
            get
            {
                if (DLL == null)
                    return "";
                return DLL.ImageRuntimeVersion;
            }
        }

        /// <summary>
        /// 是否动态
        /// </summary>
        public bool IsDynamic
        {
            get
            {
                if (DLL == null)
                    return false;
                return DLL.IsDynamic;
            }
        }


        /// <summary>
        /// 是否信任方式
        /// </summary>
        public bool IsFullyTrusted
        {
            get
            {
                if (DLL == null)
                    return false;
                return DLL.IsFullyTrusted;
            }
        }

        /// <summary>
        /// 枚举
        /// </summary>
        public List<Type> PublicEnum
        {
            get
            {
                if (DLL == null)
                    return new List<Type>();
                return DLL.GetTypes().ToList().FindAll(c => c.IsEnum && c.IsPublic);
            }

        }

        /// <summary>
        /// 类
        /// </summary>
        public List<Type> PublicClass
        {
            get
            {
                if (DLL == null)
                    return new List<Type>();
                return DLL.GetTypes().ToList().FindAll(c => c.IsClass && c.IsPublic);
            }
        }

        /// <summary>
        /// 接口
        /// </summary>
        public List<Type> PublicInterface
        {
            get
            {
                if (DLL == null)
                    return new List<Type>();
                return DLL.GetTypes().ToList().FindAll(c => c.IsInterface && c.IsPublic);
            }
        }


        public List<Type> TypeList
        {
            get
            {
                if (DLL != null)
                    return new List<Type>();
                return DLL.GetTypes().ToList();
            }
        }

        /// <summary>
        /// 类全名
        /// </summary>
        public List<string> ClassFullName
        {
            get
            {
                List<string> ClassList = new List<string>();
                if (DLL != null)
                {
                    foreach (Type Class in DLL.GetTypes())
                    {
                        if (Class.IsClass && Class.IsPublic)
                            ClassList.Add(Class.FullName);
                    }
                }
                return ClassList;
            }
        }

        /// <summary>
        /// 枚举全名
        /// </summary>
        public List<string> EnumFullName
        {
            get
            {
                List<string> ClassList = new List<string>();
                if (DLL != null)
                {
                    foreach (Type Class in DLL.GetTypes())
                    {
                        if (Class.IsEnum && Class.IsPublic)
                            ClassList.Add(Class.FullName);
                    }
                }
                return ClassList;
            }
        }

        /// <summary>
        /// 接口全名
        /// </summary>
        public List<string> InterfaceFullName
        {
            get
            {
                List<string> ClassList = new List<string>();
                if (DLL != null)
                {
                    foreach (Type Class in DLL.GetTypes())
                    {
                        if (Class.IsInterface && Class.IsPublic)
                            ClassList.Add(Class.FullName);
                    }
                }
                return ClassList;
            }
        }


        #endregion

        #region 类

        /// <summary>
        /// 根据名称查找类
        /// </summary>
        /// <param name="ClassName">类名</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>类</returns>
        public Type FindClass(string ClassName, bool ignoreCase = true)
        {
            if (DLL == null)
                return null;
            return DLL.GetType(ClassName, false, ignoreCase);
        }

        #endregion

        #region 构造函数

        /// <summary>
        /// 获取构造函数
        /// </summary>
        /// <param name="ClassName">类型信息</param>
        /// <returns>构造函数列表</returns>
        public List<ConstructorInfo> GetClassConstructors(Type ClassName)
        {
            if (ClassName == null)
                return new List<ConstructorInfo>();
            return ClassName.GetConstructors().ToList();
        }

        /// <summary>
        /// 获取构造函数
        /// </summary>
        /// <param name="ClassName">类型名</param>
        /// <param name="ignoreCase">忽略大小写</param>
        /// <returns>构造函数列表</returns>
        public List<ConstructorInfo> GetClassConstructors(string ClassName, bool ignoreCase = true)
        {
            if (DLL == null)
                return new List<ConstructorInfo>();
            return DLL.GetType(ClassName, false, ignoreCase).GetConstructors().ToList();
        }

        #endregion

        #region 类方法

        /// <summary>
        /// 获取指定类的方法
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public List<MethodInfo> GetClassMethod(Type ClassName)
        {
            if (ClassName == null)
                return new List<MethodInfo>();
            return ClassName.GetMethods().ToList();
        }

        /// <summary>
        /// 获取指定类的方法
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public List<MethodInfo> GetClassMethod(string ClassName, bool ignoreCase = true)
        {
            if (DLL == null)
                return new List<MethodInfo>();
            return DLL.GetType(ClassName, false, ignoreCase).GetMethods().ToList();
        }

        /// <summary>
        /// 获取指定类的方法名获取方法
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public MethodInfo GetMethod(Type ClassName, string MethodName, params object[] Refer)
        {
            if (ClassName == null || MethodName == null)
                return null;
            else if (MethodName.Trim() == "" || MethodName.Trim() == "")
                return null;
            Type[] ReferType = new Type[(Refer == null ? 0 : Refer.Length)];
            for (int i = 0; i < Refer.Length; i++)
                ReferType[i] = Refer[i].GetType();
            return ClassName.GetMethod(MethodName, ReferType);
        }

        /// <summary>
        /// 获取指定类的方法名获取方法
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="MethodName"></param>
        /// <param name="Refer"></param>
        /// <returns></returns>
        public MethodInfo GetMethod(string ClassName, string MethodName, params object[] Refer)
        {
            if (DLL == null)
                return null;
            Type[] ReferType = new Type[(Refer == null ? 0 : Refer.Length)];
            for (int i = 0; i < Refer.Length; i++)
                ReferType[i] = Refer[i].GetType();
            Type Class = DLL.GetType(ClassName, false, false);
            if (Class == null)
                return null;
            return Class.GetMethod(MethodName, ReferType);
        }

        #endregion

        #region 类属性

        /// <summary>
        /// 获取指定类的属性
        /// </summary>
        /// <param name="ClassName"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetClassProperty(Type ClassName)
        {
            if (ClassName == null)
                return new List<PropertyInfo>();
            return ClassName.GetProperties().ToList();
        }

        /// <summary>
        /// 获取指定类的属性
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public List<PropertyInfo> GetClassProperty(string ClassName, bool ignoreCase = true)
        {
            if (DLL == null)
                return new List<PropertyInfo>();
            return DLL.GetType(ClassName, false, ignoreCase).GetProperties().ToList();
        }

        /// <summary>
        /// 通过指定类的属性名获取属性
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="PropertyName"></param>
        /// <returns></returns>
        public PropertyInfo GetProperty(Type ClassName, string PropertyName)
        {
            if (ClassName == null || PropertyName == null)
                return null;
            else if (PropertyName.Trim() == "")
                return null;
            return ClassName.GetProperty(PropertyName);
        }

        /// <summary>
        /// 通过指定类的属性名获取属性
        /// </summary>
        /// <param name="ClassName"></param>
        /// <param name="PropertyName"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public PropertyInfo GetProperty(string ClassName, string PropertyName, bool ignoreCase = true)
        {
            if (DLL == null)
                return null;
            return DLL.GetType(ClassName, false, ignoreCase).GetProperty(PropertyName);
        }

        #endregion
 
 

        #region 反射调用方法

        public object StaticMethod(Type Class, string MethodName, params object[] Refer)
        {
            if (Class == null)
                return null;
            Type[] ReferType = new Type[(Refer == null ? 0 : Refer.Length)];
            for (int i = 0; i < Refer.Length; i++)
                ReferType[i] = Refer[i].GetType();
            MethodInfo mi = Class.GetMethod(MethodName, ReferType);
            return mi.Invoke(null, Refer);
        }

        public object StaticMethod(string ClassName, string MethodName, params object[] Refer)
        {
            if (DLL == null)
                return null;
            Type Class = DLL.GetType(ClassName);
            if (Class == null)
                return null;
            Type[] ReferType = new Type[(Refer == null ? 0 : Refer.Length)];
            for (int i = 0; i < Refer.Length; i++)
                ReferType[i] = Refer[i].GetType();
            MethodInfo mi = Class.GetMethod(MethodName, ReferType);
            if (mi == null)
                return null;
            return mi.Invoke(null, Refer);
        }

        public object NewClass(Type Class, params object[] Refer)
        {
            if (Class == null)
                return null;
            DyncClass = Activator.CreateInstance(Class, Refer);
            return DyncClass;
        }

        public object NewClass(string ClassName, params object[] Refer)
        {
            if (DLL == null)
                return null;
            Type Class = DLL.GetType(ClassName);
            if (Class == null)
                return null;
            DyncClass = Activator.CreateInstance(Class, Refer);
            DyncType = Class;
            return DyncClass;
        }

        public object ExecMethod(string MethodName, params object[] Refer)
        {
            if (MethodName == null)
                return null;
            else if (MethodName.Trim() == "")
                return null;
            if (DLL == null || DyncClass == null || DyncType == null)
                return null;
            Type[] ReferType = new Type[(Refer == null ? 0 : Refer.Length)];
            for (int i = 0; i < Refer.Length; i++)
                ReferType[i] = Refer[i].GetType();
            MethodInfo mi = DyncType.GetMethod(MethodName, ReferType);
            if (mi == null)
                return null;
            return mi.Invoke(DyncClass, Refer);
        }

        public object GetPropertyValue(string PropertyName)
        {
            if (DLL == null || DyncClass == null || DyncType == null)
                return null;
            if (PropertyName == null)
                return null;
            else if (PropertyName.Trim() == "")
                return null;
            PropertyInfo proper = DyncType.GetProperty(PropertyName);
            if (proper == null)
                return null;
            return proper.GetValue(DyncClass, null);
        }

        public void GetPropertyValue(string PropertyName, object value)
        {
            if (DLL == null || DyncClass == null || DyncType == null)
                return;
            PropertyInfo proper = DyncType.GetProperty(PropertyName);
            if (proper == null)
                return;
            proper.SetValue(DyncClass, value, null);
        }

        public object GetPropertyValue(PropertyInfo PropertyName)
        {
            if (DLL == null || DyncClass == null || DyncType == null)
                return null;
            if (PropertyName == null)
                return null;
            return PropertyName.GetValue(DyncClass, null);
        }

        public void GetPropertyValue(PropertyInfo PropertyName, object value)
        {
            if (DLL == null || DyncClass == null || DyncType == null)
                return;
            PropertyName.SetValue(DyncClass, value, null);
        }




        #endregion

    }
}
