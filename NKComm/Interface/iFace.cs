using NK.Entity;
using NK.ENum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace NK.Interface
{
    /// <summary>
    /// 人面识别接口
    /// </summary>
    public interface iFace
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
        /// 获取设置参数
        /// </summary>
        Dictionary<string, Type> Parameter { get; }
        /// <summary>
        /// 设置参数
        /// </summary>
        Dictionary<string, object> Config { set; }
        /// <summary>
        /// 连接
        /// </summary>
        void Connect();
        /// <summary>
        /// 断开
        /// </summary>
        void DisConnect();
        /// <summary>
        /// 初始化
        /// </summary>
        void Init();
        /// <summary>
        /// 释放
        /// </summary>
        void Dispose();
        /// <summary>
        /// 获取人面矩阵
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        List<KeyValuePair<Point, Point>> Detection(Image img);
        /// <summary>
        /// 获取人面特征
        /// </summary>
        /// <param name="img"></param>
        /// <returns>人面特征，性别（0未知，1男，2，女）</returns>
        Dictionary<string, int> Feature(Image img);
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
        Dictionary<object, float> Matching(string Feature, Dictionary<object, string> Temple);

    }

}
