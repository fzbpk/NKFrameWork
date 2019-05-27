using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NK.ENum;
using NK.Entity;

namespace NK.Interface
{
  
    /// <summary>
    /// 卡接口
    /// </summary>
    public interface iCard
    {
        #region 基本信息

        /// <summary>
        /// 驱动名称
        /// </summary>
        string DriverName { get; }
        /// <summary>
        /// 驱动描述
        /// </summary>
        string DriverDescription { get; }
        /// <summary>
        /// 公司
        /// </summary>
        string Company { get; }
        /// <summary>
        /// 版本
        /// </summary>
        string Version { get; }

        #endregion
        /// <summary>
        /// 连接模式
        /// </summary>
        ReferForUse Mode { get; }
        /// <summary>
        /// 连接信息
        /// </summary>
        string Connection { get; set; }
        /// <summary>
        /// 性能
        /// </summary>
        ReferSet Refer { get; set; }
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 释放
        /// </summary>
        void Dispose(); 
        /// <summary>
        /// 参数
        /// </summary>
        Dictionary<string, Type> Parameter { get; }
        /// <summary>
        /// 值
        /// </summary>
        Dictionary<string, object> Config { set; } 
        /// <summary>
        /// 获得物理卡号
        /// </summary>
        /// <returns></returns>
        string PhyCard();
        /// <summary>
        /// 读取卡内数据
        /// </summary>
        /// <returns></returns>
        string Read();
        /// <summary>
        /// 写入卡内数据
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns></returns>
        bool Write(string data);
    }
}
