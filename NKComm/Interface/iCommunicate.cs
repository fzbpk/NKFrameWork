using NK.Entity;
using NK.Event;
using NK.ENum;
using NK.Class;
using System.Collections.Generic;
namespace NK.Interface
{
    /// <summary>
    /// 通信连接接口
    /// </summary>
    public interface iCommunicate
    {

        #region 属性
        /// <summary>
        /// 连接方式
        /// </summary>
        ReferForUse IMode { get; }
        /// <summary>
        ///  网络参数 
        /// </summary>
        string Connection { get; set; }
        /// <summary>
        /// 性能参数
        /// </summary>
        ReferSet Refer { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsRuning { get; }
        /// <summary>
        /// 心跳时间
        /// </summary>
        int HeartBeatTime { get; set; }
        /// <summary>
        /// 查询时间
        /// </summary>
        int InquiryTime { get; set; }
        /// <summary>
        /// 标识获取次数
        /// </summary>
        int FlagCount { get; set; }
        /// <summary>
        /// 注册执行次数
        /// </summary>
        int RegeditCount { get; set; }
        /// <summary>
        /// 心跳执行次数
        /// </summary>
        int HeartBeatCount { get; set; }
        /// <summary>
        /// 获取数据执行次数
        /// </summary>
        int DataCount { get; set; }
        /// <summary>
        /// 连接在线状态
        /// </summary>
        Dictionary<CommunicateSession,  string> Online { get; }
        /// <summary>
        /// 描述语言
        /// </summary>
        Language language { get; set; }
        /// <summary>
        /// 整点数据
        /// </summary>
        bool IntegralPoint { get; set; }

        #endregion

        #region 基本方法

        /// <summary>
        /// 连接
        /// </summary>
        void Start();
        /// <summary>
        /// 断开连接
        /// </summary>
        void Stop();
        /// <summary>
        /// 数据写入读取
        /// </summary>
        /// <param name="flags">标识</param>
        /// <param name="Data">数据</param>
        /// <returns></returns>
        byte[] IO(string flags, byte[] Data);
        /// <summary>
        /// 清除SESSION
        /// </summary>
        /// <param name="flags">标识</param>
        void ClearSession(string flags);
        /// <summary>
        /// 获取SESSION
        /// </summary>
        /// <param name="flags">标识</param>
        /// <returns></returns>
        Dictionary<string, object> GetSession(string flags);
        /// <summary>
        /// 设置SESSION
        /// </summary>
        /// <param name="flags">标识</param>
        /// <param name="session">Session</param>
        /// <returns></returns>
        void SetSession(string flags, Dictionary<string, object> session);

        #endregion

        #region 基本事件

        /// <summary>
        /// 调试信息事件
        /// </summary>
        event CommEvent.LogEven log;
        /// <summary>
        /// 连接调试信息
        /// </summary>
        event NetEvent.LogEven Connectlog ;
        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        event CommEvent.HasErrorEven HasError;
        /// <summary>
        /// 连接事件
        /// </summary>
        event NetEvent.Connect Connect;
        /// <summary>
        /// 连接断开
        /// </summary>
        event NetEvent.DisConnect DisConnect;
        /// <summary>
        /// 请求注册事件
        /// </summary>
        event NetEvent.RequestInitEven RequestInit;
        /// <summary>
        /// 返回注册事件
        /// </summary>
        event NetEvent.ResponseInitEven ResponseInit;
        /// <summary>
        /// 请求数据事件
        /// </summary>
        event NetEvent.RequestDataEven RequestData;
        /// <summary>
        /// 返回数据事件
        /// </summary>
        event NetEvent.ResponsetDataEven ResponsetData;
        /// <summary>
        /// 接收到数据
        /// </summary>
        event NetEvent.ReceiveDataEven ReceiveData;
        /// <summary>
        /// 请求心跳事件
        /// </summary>
        event NetEvent.RequestHeartBeatEven RequestHeartBeat;
        /// <summary>
        /// 返回心跳事件
        /// </summary>
        event NetEvent.ResponeHeartBeatEven ResponeHeartBeat;
        /// <summary>
        /// 请求获取FLAGS
        /// </summary>
        event NetEvent.RequestFlagsEven RequestFlags;
        /// <summary>
        /// 返回Flags
        /// </summary>
        event NetEvent.ResponseFlagsEven ResponseFlags;
        /// <summary>
        /// 请求命令
        /// </summary>
        event NetEvent.RequestCMDEven RequestCMD;
        /// <summary>
        /// 返回命令
        /// </summary>
        event NetEvent.ResponseCMDEven ResponseCMD; 
        #endregion

    }
}
