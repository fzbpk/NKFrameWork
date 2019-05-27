using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
namespace NK.Communicate
{
    /// <summary>
    /// 设备桥接器
    /// </summary>
    public partial  class NetDriver:iNet
    {
        #region 定义
        private iNet conn = null;
        private string ClassName = "";
        #endregion

        #region 事件

        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        public CommEvent.HasErrorEven HasError { get; set; }
        /// <summary>
        /// 连接事件
        /// </summary>
        public NetEvent.Connect Connect { get; set; }
        /// <summary>
        /// 连接断开
        /// </summary>
        public NetEvent.DisConnect DisConnect { get; set; }

        #endregion

        #region 构造
 
        private string Serialize(object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }


        /// <summary>
        /// 设备桥接器
        /// </summary>
        public NetDriver()
        { }

        public NetDriver(NetSet connection)
        {
            if (connection != null)
                this.Connection = Serialize(connection);
            conn = new SocketSDK(this.Connection);
        }

        public NetDriver(PortsSet connection)
        {
            if (connection != null)
                this.Connection = Serialize(connection);
            conn = new SerialPortSDK(this.Connection);
        }

        public NetDriver(FileInfo  connection)
        {
            if (connection != null)
                this.Connection = Serialize(connection);
            conn = new FileIOSDK(this.Connection);
        }

        public NetDriver(USBSet connection)
        {
            if (connection != null)
                this.Connection = Serialize(connection);
            conn = new USBSDK (this.Connection);
        }

        #endregion

        #region 基本属性
        /// <summary>
        /// 连接方式
        /// </summary>
        public ReferForUse IMode { get; set; }
        /// <summary>
        ///  网络参数,JSON
        /// </summary>
        public string Connection { get; set; }
        /// <summary>
        /// 性能参数
        /// </summary>
        public ReferSet Refer_Prama { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool IsConnected { get { return conn != null? conn.IsConnected:false; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion

        #region 方法

        /// <summary>
        /// 连接
        /// </summary>
        public void Open()
        {
            if (conn == null)
            {
                switch (IMode)
                {
                    case ReferForUse.File:
                        conn = new FileIOSDK(this.Connection);
                        break;
                    case ReferForUse.UartSet:
                        conn = new SerialPortSDK(this.Connection);
                        break;
                    case ReferForUse.NetSet :
                        conn = new SocketSDK(this.Connection);
                        break;
                    case ReferForUse.USBSet:
                        conn = new USBSDK(this.Connection);
                        break;
                    default:
                        if (HasError != null)
                            HasError(ClassName, "Open", new NotSupportedException());
                        else
                            throw new NotSupportedException();
                        break;
                }
            }
            if (conn != null)
            {
                if (HasError != null)
                    conn.HasError += HasError;
                if (Connect != null)
                    conn.Connect += Connect;
                if (DisConnect != null)
                    conn.DisConnect += DisConnect;
                conn.language = this.language;
                conn.Open();
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close()
        {
            if (conn != null)
                conn.Close();
        }

        /// <summary>
        /// 数据读取
        /// </summary>
        /// <param name="Index">读取起始位置</param>
        /// <param name="Datalen">数据量,0为全部读取</param>
        /// <returns></returns>
        public byte[] Read(int Index = 0, int Datalen = 0)
        {
            byte[] res = new byte[0];
            if (conn != null)
                res= conn.Read(Index, Datalen);
            return res;
        }

        /// <summary>
        /// 数据写入
        /// </summary>
        /// <param name="Data">数据</param>
        /// <param name="Index">写入起始位置</param>
        /// <returns></returns>
        public bool Write(byte[] Data, int Index = 0)
        {
            if (conn != null)
                return conn.Write(Data, Index);
            else
                return false;
        }

        #endregion
         
    }
}
