using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NK.Interface;
using NK.Entity;
using NK.ENum;
using System.Drawing;

namespace NK.Identification
{
    /// <summary>
    /// 车牌识别
    /// </summary>
    public class CarDriver : AuthDriver, iCar
    {
        #region 定义 
        iCar iDriver = null;
        #endregion

        #region 构造函数

        /// <summary>
        /// 车牌识别
        /// </summary>
        /// <param name="FilePath">DLL路径</param>
        /// <param name="ClassFullName">类名</param>
        public CarDriver(string FilePath, string ClassFullName) :base(FilePath)
        {
            Type sType = typeof(iCar);
            if (string.IsNullOrEmpty(ClassFullName))
            {
                foreach (var type in DLL.GetTypes())
                {
                    if (type.GetInterfaces().Contains(sType))
                    {
                        iDriver = (iCar)Activator.CreateInstance(type, null);
                        break;
                    }
                }
            }
            else
            {
                Type type = DLL.GetType(ClassFullName, false, true);
                if (type != null)
                {
                    if (type.GetInterfaces().Contains(sType))
                        iDriver = (iCar)Activator.CreateInstance(type, null);
                }
            }
            if (iDriver == null)
                throw new NotSupportedException(DLL.FullName);
        }

        /// <summary>
        /// 车牌识别
        /// </summary>
        /// <param name="FilePath">DLL路径</param>
        /// <param name="ClassFullName">类名</param>
        public CarDriver(FileInfo FilePath, string ClassFullName) : base(FilePath)
        {
            Type sType = typeof(iCar);
            if (string.IsNullOrEmpty(ClassFullName))
            {
                foreach (var type in DLL.GetTypes())
                {
                    if (type.GetInterfaces().Contains(sType))
                    {
                        iDriver = (iCar)Activator.CreateInstance(type, null);
                        break;
                    }
                }
            }
            else
            {
                Type type = DLL.GetType(ClassFullName, false, true);
                if (type != null)
                {
                    if (type.GetInterfaces().Contains(sType))
                        iDriver = (iCar)Activator.CreateInstance(type, null);
                }
            }
            if (iDriver == null)
                throw new NotSupportedException(DLL.FullName);
        }

        /// <summary>
        /// 车牌识别
        /// </summary>
        /// <param name="Server"></param>
        public CarDriver(Type Server) : base(Server)
        {
            Type sType = typeof(iCar);
            if (Class.GetInterfaces().Contains(sType))
                iDriver = (iCar)Activator.CreateInstance(Class, null);
            else
                throw new NotSupportedException(Server.Name);
        }

        protected override void dispose()
        {
            iDriver.Dispose();
            iDriver = null;
        }

        #endregion

        #region 属性

        /// <summary>
        /// 连接模式
        /// </summary>
        public ReferForUse Mode { get { return (iDriver == null) ? ReferForUse.None : iDriver.Mode; } }

        /// <summary>
        /// 连接信息
        /// </summary>
        public string Connection
        {
            get { return (iDriver == null) ? "" : iDriver.Connection; }
            set { if (iDriver != null) iDriver.Connection = value; }
        }
        /// <summary>
        /// 性能
        /// </summary>
        public ReferSet Refer
        {
            get { return (iDriver == null) ? null : iDriver.Refer; }
            set { if (iDriver != null) iDriver.Refer = value; }
        }
        /// <summary>
        /// 获取设置参数
        /// </summary>
        public Dictionary<string, Type> Parameter { get { return (iDriver == null) ? new Dictionary<string, Type>() : iDriver.Parameter; } }
        /// <summary>
        /// 设置参数
        /// </summary>
        public Dictionary<string, object> Config { set { if (iDriver != null) iDriver.Config = value; } }

        #endregion
      
        #region 基本信息

        /// <summary>
        /// 驱动名称
        /// </summary>
        public string DriverName { get { return (iDriver == null) ? "" : iDriver.DriverName; } }
        /// <summary>
        /// 驱动描述
        /// </summary>
        public string DriverDescription { get { return (iDriver == null) ? "" : iDriver.DriverDescription; } }
        /// <summary>
        /// 公司
        /// </summary>
        public string Company { get { return (iDriver == null) ? "" : iDriver.Company; } }
        /// <summary>
        /// 版本
        /// </summary>
        public string Version { get { return (iDriver == null) ? "" : iDriver.Version; } }

        #endregion

        #region 操作

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if (iDriver != null) iDriver.Init();
        }

        /// <summary>
        /// 车牌图像
        /// </summary>
        /// <returns></returns>
        public Image Image()
        {
            if (iDriver != null) return iDriver.Image();
            return null;
        }

        /// <summary>
        /// 车牌
        /// </summary>
        /// <returns></returns>
        public  string Feature(Image img = null)
        {
            if (iDriver != null) return iDriver.Feature(img);
            return null;
        }

        #endregion

    }
}
