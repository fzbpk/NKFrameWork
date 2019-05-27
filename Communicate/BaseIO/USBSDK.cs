using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using NK.Message;
using System.Runtime.Serialization.Json;
using NK.Entity;
using NK.ENum;
using NK.Event;
using NK.Interface;
using USBHIDDevice;
namespace NK.Communicate
{
    /// <summary>
    /// USB通信
    /// </summary>
     public partial  class USBSDK : IDisposable,iNet
    {

        #region 定义
        private long m_session = 0;
        private UsbHidDevice hid=null ;
        private bool m_disposed; 
        private string ClassName = "";
        private List<byte> Recv = new List<byte>();
        private const byte cmd = 0;
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

        #region 构造函数

        /// <summary>
        /// USB HID
        /// </summary>
        /// <param name="connction"></param>
        public USBSDK(string connction = "")
        {
            this.Connection = connction;
            ClassName = this.GetType().ToString();
            this.language = Language.Chinese;
        }

        public USBSDK(string vid, string pid)
        {
            if (!string.IsNullOrEmpty(vid) && !string.IsNullOrEmpty(pid))
            {
                USBSet usb = new USBSet();
                usb.Mode = Net_Mode.Local;
                usb.PID = pid;
                usb.VID = vid;
                usb.devPath = "";
                this.Connection = Serialize(usb);
            }
            this.language = Language.Chinese;
            ClassName = this.GetType().ToString();
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        ~USBSDK()
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
                    if (hid  != null)
                    {
                        try
                        {
                            hid.Disconnect();
                        }
                        catch { }
                        hid = null;
                    }
                    m_disposed = true;
                }
            }
        }

        #endregion

        #region 基本属性
        /// <summary>
        /// 连接方式
        /// </summary>
        public ReferForUse IMode { get { return ReferForUse.USBSet; } }
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
        public bool IsConnected { get { return hid != null?hid.IsDeviceConnected:false ; } }
        /// <summary>
        /// 显示语言
        /// </summary>
        public Language language { get; set; }
        #endregion

        #region 扩展属性

        /// <summary>
        /// 获取所有HID路径
        /// </summary>
        public List<string> HIDS {
            get
            {
                List<string> ss = new List<string>();
                try
                {
                    if (hid != null)
                        hid.GetDeviceList(ref ss);
                }
                catch
                { }
                return ss;
            }
        }

        /// <summary>
        /// 设备路径
        /// </summary>
        public string devPath
        {
            get
            {
                if (hid == null)
                    return "";
                else if (hid.IsDeviceConnected)
                    return hid.DevicePath;
                else
                    return "";
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    USBSet usb = new USBSet();
                    usb.Mode = Net_Mode.Local;
                    usb.PID = "";
                    usb.VID = "";
                    usb.devPath = value;
                    this.Connection = Serialize(usb);
                }
            }
        }

        #endregion

        #region 基本方法

        private T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

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
        /// 连接
        /// </summary>
        public void Open()
        {
            if (string.IsNullOrEmpty(this.Connection))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language)));
                else
                    throw new NullReferenceException(SystemMessage.RefNullOrEmpty("Connection", language));
                return;
            }
            USBSet port = null;
            try
            { port = Deserialize<USBSet>(this.Connection); }
            catch
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new InvalidCastException(SystemMessage.CastError("Connection", language)));
                else
                    throw new InvalidCastException(SystemMessage.CastError("Connection", language));
                return;
            }
            if (string.IsNullOrEmpty(port.devPath) &&  string.IsNullOrEmpty(port.VID) && string.IsNullOrEmpty(port.VID))
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", new NullReferenceException(SystemMessage.CastError("devPath,VID", language)));
                else
                    throw new NullReferenceException(SystemMessage.CastError("devPath,VID", language));
                return;
            }
            try
            {
                if (hid == null)
                {
                    if (!string.IsNullOrEmpty(port.VID) && !string.IsNullOrEmpty(port.PID))
                    {
                        int vendorID = ConvertHelper.ToInt(ConvertHelper.Hex2Ten(port.VID));
                        int productID = ConvertHelper.ToInt(ConvertHelper.Hex2Ten(port.PID));
                        hid = new UsbHidDevice(vendorID, productID);
                    }
                    else if(!string.IsNullOrEmpty(port.devPath) )
                    {
                        hid = new UsbHidDevice();
                        hid.devicePath = port.devPath;
                    } 
                    hid.OnConnected += Hid_OnConnected;
                    hid.OnDisConnected += Hid_OnDisConnected;
                    hid.DataReceived += Hid_DataReceived;
                    hid.Connect();
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Open", ex);
                else
                    throw ex;
            }
        }

        private void Hid_DataReceived(byte[] data)
        {
            Recv.AddRange(data);
        }

        private void Hid_OnDisConnected()
        {
            if (this.DisConnect != null)
                this.DisConnect(Connection, "", ReferForUse.USBSet, m_session);
        }

        private void Hid_OnConnected()
        {
            System.Random Random = new System.Random();
            m_session = Random.Next(1, int.MaxValue);
            if (this.Connect != null)
                this.Connect(Connection, "", ReferForUse.USBSet, m_session);
        }

        /// <summary>
        /// 断开连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (hid != null)
                {
                    hid.Disconnect();
                    hid.Dispose();
                    hid = null;
                }
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Close", ex);
                else
                    throw ex;
            } 
        }

        /// <summary>
        /// 数据读取
        /// </summary>
        /// <param name="Index">读取起始位置</param>
        /// <param name="Datalen">数据量,0为全部读取</param>
        /// <returns></returns>
        public byte[] Read(int Index = 0, int Datalen = 0)
        {
            List<byte> pack = new List<byte>();
            if (hid == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            }
            else if (!hid.IsDeviceConnected)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return pack.ToArray();
            }
            try
            { 
                int len = 0;
                int  size = 1024,n=0;
                int Tout = 0;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.ReceiveBufferSize > 0)
                        size = this.Refer_Prama.ReceiveBufferSize;
                    if (this.Refer_Prama.ReceiveBufferSize > 0)
                        Tout = this.Refer_Prama.ReceiveTimeout;
                }
                byte[] buf = new byte[size];
                if (Recv.Count <= 0)
                {
                    if (Tout > 0)
                        System.Threading.Thread.Sleep(Tout);
                    else
                        while (Recv.Count <= 0) ; 
                    if (Recv.Count <= 0)
                        return pack.ToArray();
                }
                if (Datalen > 0)
                {
                    n = 0;
                    while (Datalen > 0)
                    {
                        if (Datalen > (Recv.Count - n * size))
                            len = size;
                        else
                            len = Datalen;
                        Recv.CopyTo(n * buf.Length, buf, 0, len);
                        byte[] recv = new byte[len];
                        Array.Copy(buf, recv, len);
                        pack.AddRange(recv);
                        Datalen -= (int)len;
                    }
                    if (Recv.Count > pack.Count)
                    {
                        byte[] res = new byte[Recv.Count - pack.Count];
                        Recv.CopyTo(pack.Count, res, 0, res.Length);
                        Recv.Clear();
                        Recv.AddRange(res);
                    }
                    else
                        Recv.Clear(); 
                }
                else
                {
                    byte[] res = new byte[Recv.Count];
                    Recv.CopyTo(pack.Count, res, 0, res.Length);
                    Recv.Clear();
                    pack.AddRange(res);
                } 
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", ex);
                else
                    throw ex;
            }
            return pack.ToArray();
        }

        /// <summary>
        /// 数据写入
        /// </summary>
        /// <param name="Data">数据</param>
        /// <param name="Index">写入起始位置</param>
        /// <returns></returns>
        public bool Write(byte[] Data, int Index = 0)
        {
            if (hid == null)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return false;
            }
            else if (!hid.IsDeviceConnected)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Read", new NullReferenceException(SystemMessage.Badsequencecommands(language)));
                else
                    throw new NullReferenceException(SystemMessage.Badsequencecommands(language));
                return false;
            }
            try
            {
                if (Data == null)
                    return false;
                else if (Data.Length == 0)
                    return false;
                uint len = 0;
                int size = 64;
                if (this.Refer_Prama != null)
                {
                    if (this.Refer_Prama.SendBufferSize > 0)
                        size = this.Refer_Prama.SendBufferSize;
                }
                byte[] buf = null;
                int Datalen = Data.Length;
                int n = 0;
                int index = 0;
                while (Datalen > 0)
                {
                    if (Datalen >= size)
                        len = (uint )size;
                    else
                        len = (uint)Datalen;
                    buf = new byte[len];
                    Array.Copy(Data, index, buf, 0, len); 
                    bool res=  hid.SendBytes(buf);
                    Datalen -= (int)len;
                    index += (int)len;
                    n++;
                }
                return true;
            }
            catch (Exception ex)
            {
                if (this.HasError != null)
                    this.HasError(ClassName, "Write", ex);
                else
                    throw ex;
                return false;
            }
        }

        #endregion
 
    }
}
