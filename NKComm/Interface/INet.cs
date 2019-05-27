using NK.Entity;
using NK.Event;
using NK.ENum;
using System.Collections.Generic;
namespace NK.Interface
{
    /// <summary>
    ///  连接服务接口
    /// </summary>
    public interface iNet
    {

        #region 属性
        /// <summary>
        /// 连接方式
        /// </summary>
        ReferForUse IMode { get; }
        /// <summary>
        ///  网络参数,JSON
        /// </summary>
        string Connection { get; set; }
        /// <summary>
        /// 性能参数
        /// </summary>
        ReferSet Refer_Prama { get; set; }
        /// <summary>
        /// 是否已连接
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 显示语言
        /// </summary>
        Language language { get; set; }
        #endregion

        #region 方法

        /// <summary>
        /// 连接
        /// </summary>
        void Open();
        /// <summary>
        /// 断开连接
        /// </summary>
        void Close();

        /// <summary>
        /// 数据读取
        /// </summary>
        /// <param name="Index">读取起始位置</param>
        /// <param name="Datalen">数据量,0为全部读取</param>
        /// <returns></returns>
        byte[] Read(int Index=0,int Datalen=0);

        /// <summary>
        /// 数据写入
        /// </summary>
        /// <param name="Data">数据</param>
        /// <param name="Index">写入起始位置</param>
        /// <returns></returns>
        bool Write(byte[] Data, int Index = 0);

        #endregion

        #region 事件

        /// <summary>
        /// 错误出现事件，性能参数内DEBUG设置为EVENT有效
        /// </summary>
        CommEvent.HasErrorEven HasError { get; set; }
        /// <summary>
        /// 连接事件
        /// </summary>
        NetEvent.Connect Connect { get; set; }
        /// <summary>
        /// 连接断开
        /// </summary>
        NetEvent.DisConnect DisConnect { get; set; } 

        #endregion

    }
}
