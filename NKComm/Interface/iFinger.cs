
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using NK.ENum;
using NK.Entity;

namespace NK.Interface
{
    /// <summary>
    /// 指纹接口
    /// </summary>
    public interface iFinger
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
        /// 指纹图像
        /// </summary>
        /// <returns></returns>
        Image Image();
        /// <summary>
        /// 获取特征码
        /// </summary>
        /// <returns></returns>
        string Feature(Image img = null);
        /// <summary>
        /// 比对值
        /// </summary>
        /// <param name="Feature">特征</param>
        /// <param name="Temple">模板</param>
        /// <returns></returns>
        float Matching(string Feature, string Temple);
        /// <summary>
        /// 异步1对N
        /// </summary>
        /// <param name="Feature">特征</param>
        /// <param name="Temple">模板,key认证信息，value特征</param> 
        /// <returns>最高分</returns>
        Dictionary<object,float> Matching(string Feature, Dictionary<object, string> Temple);
    }
}
