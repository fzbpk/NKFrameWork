using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NK.ENum;
using NK.Entity;
using System.Drawing;

namespace NK.Interface
{
    /// <summary>
    /// 车牌接口
    /// </summary>
    public interface iCar
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
        /// 参数
        /// </summary>
        Dictionary<string, Type> Parameter { get; }
        /// <summary>
        /// 值
        /// </summary>
        Dictionary<string, object> Config { set; }
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();
        /// <summary>
        /// 车牌图像
        /// </summary>
        /// <returns></returns>
        Image Image();
        /// <summary>
        /// 车牌
        /// </summary>
        /// <returns></returns>
        string Feature(Image img=null);
    }
}
