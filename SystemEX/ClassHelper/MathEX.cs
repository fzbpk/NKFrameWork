using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace   NK
{
    /// <summary>
    /// 算术扩展类
    /// </summary>
    public static partial class MathEX
    {

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Abs(this double val)
        {
            return Math.Abs(val);
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static float Abs(this float  val)
        {
            return Math.Abs(val);
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int  Abs(this int val)
        {
            return Math.Abs(val);
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static long Abs(this long val)
        {
            return Math.Abs(val);
        }

        /// <summary>
        /// 绝对值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static short Abs(this short val)
        {
            return Math.Abs(val);
        }

        /// <summary>
        /// 正弦
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Sin(this double val)
        {
            return Math.Sin(val);
        }

        /// <summary>
        /// 反正弦
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Asin(this double val)
        {
            return Math.Asin(val);
        }
        
        /// <summary>
        /// 余弦
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Cos(this double val)
        {
            return Math.Cos(val);
        }

        /// <summary>
        /// 反余弦
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Acos(this double val)
        {
            return Math.Acos(val);
        }

        /// <summary>
        /// 正切
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double tan(this double val)
        {
            return Math.Tan(val);
        }

        /// <summary>
        /// 反正切
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Atan(this double val)
        {
            return Math.Atan(val);
        }

        /// <summary>
        /// 次方
        /// </summary>
        /// <param name="val"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double pow(this double val, double n)
        {
            return Math.Pow(val,n);
        }

        /// <summary>
        /// 底
        /// </summary>
        /// <param name="val"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double log(this double val, double n)
        {
            return Math.Log(val, n);
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Avg(this List<int> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                int sum = 0;
                foreach (var tmp in val)
                    sum += tmp;
                return sum / val.Count;
            } 
        }

        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double  Avg(this List<double> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                double sum = 0;
                foreach (var tmp in val)
                    sum += tmp;
                return sum / val.Count;
            }
        }

        /// <summary>
        /// 统计值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Sum(this List<int> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                int sum = 0;
                foreach (var tmp in val)
                    sum += tmp;
                return sum  ;
            }
        }

        /// <summary>
        /// 统计值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Sum(this List<double> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                double sum = 0;
                foreach (var tmp in val)
                    sum += tmp;
                return sum  ;
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Max(this List<int> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                int sum = val[0];
                foreach (var tmp in val)
                {
                    if (tmp > sum)
                        sum = tmp;
                }
                return sum;
            }
        }

        /// <summary>
        /// 最大值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Max(this List<double> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                double sum = val[0];
                foreach (var tmp in val)
                {
                    if (tmp > sum)
                        sum = tmp;
                }
                return sum;
            }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static int Min(this List<int> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                int sum = val[0];
                foreach (var tmp in val)
                {
                    if (tmp < sum)
                        sum = tmp;
                }
                return sum;
            }
        }

        /// <summary>
        /// 最小值
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static double Min(this List<double> val)
        {
            if (val == null)
                return 0;
            else if (val.Count <= 0)
                return 0;
            else
            {
                double sum =val[0];
                foreach (var tmp in val)
                {
                    if (tmp < sum)
                        sum = tmp;
                }
                return sum;
            }
        }

        /// <summary>
        /// 公式计算
        /// </summary>
        /// <param name="Formula">公式</param>
        /// <returns></returns>
        public static object Cacl(this string Formula)
        {
            if (string.IsNullOrEmpty(Formula))
                return 0; 
            else
            {
                Microsoft.JScript.Vsa.VsaEngine ve = Microsoft.JScript.Vsa.VsaEngine.CreateEngine();
                return  Microsoft.JScript.Eval.JScriptEvaluate(Formula, ve);
            }
        }

    }
}
