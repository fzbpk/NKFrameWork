using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NK
{
    /// <summary>
    /// byte数组帮助类
    /// </summary>
    public static partial class ByteEX
    {
        /// <summary>
        /// 转为BCD
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        public static byte ToBCD(this byte b)
        {
            byte b1 = (byte)(b / 10);
            byte b2 = (byte)(b % 10);
            return (byte)((b1 << 4) | b2);
        }

        /// <summary>  
        /// 将BCD一字节数据转换到byte 十进制数据  
        /// </summary>  
        /// <param name="b" />字节
        /// <returns>返回转换后的BCD码</returns>  
        public static byte FromBCD(this byte b)
        {
            byte b1 = (byte)((b >> 4) & 0xF);
            byte b2 = (byte)(b & 0xF);
            return (byte)(b1 * 10 + b2);
        }
         
        /// <summary>
        /// 高4位
        /// </summary>
        /// <param name="b">字节</param>
        /// <returns></returns>
        public static byte HiByte(this byte b)
        {
            return (byte)((b >> 4) & 0xF);
        }

        /// <summary>
        /// 低4位
        /// </summary>
        /// <param name="b">字节</param>
        /// <returns></returns>
        public static byte LoByte(this byte b)
        {
            return (byte)(b & 0xF);
        }

        /// <summary>
        /// 数组转INT
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns></returns>
        public static int ToInt(this byte[] data)
        {
            return System.BitConverter.ToInt32(data, 0);
        }

        /// <summary>
        /// INT 转数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToByte(this int data)
        {
            return System.BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 数组转Short
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns></returns>
        public static short ToShort(this byte[] data)
        {
            return System.BitConverter.ToInt16(data, 0);
        }

        /// <summary>
        /// Short转数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToByte(this short data)
        {
            return System.BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 数组转long
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns></returns>
        public static long ToLong(this byte[] data)
        {
            return System.BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        /// long转数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToByte(this long data)
        {
            return System.BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 数组转float
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns></returns>
        public static float Tofloat(this byte[] data)
        {
            return System.BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        /// float转数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToByte(this float data)
        {
            return System.BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 数组转double
        /// </summary>
        /// <param name="data">数组</param>
        /// <returns></returns>
        public static double ToDouble(this byte[] data)
        {
            return System.BitConverter.ToInt64(data, 0);
        }

        /// <summary>
        /// double转数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] ToByte(this double data)
        {
            return System.BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 数组是否为NULL或空
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this byte[] data)
        {
            if (data == null)
                return true;
            else if (data.Length <= 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 数组清零
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] BZero(this byte[] data)
        {
            if (data != null)
                Array.Clear(data,0,data.Length);
            return data;
        }

        /// <summary>
        /// 数组复制
        /// </summary>
        /// <param name="org">源数组</param>
        /// <param name="dest">目标数组</param>
        /// <param name="srcindex">源数组起始位置</param>
        /// <param name="destindex">目标数组起始位置</param>
        /// <param name="len">数据长度</param>
        public static void CopyTo(this byte[] org,byte[] dest, int srcindex=0,int destindex=0,int len=0)
        {
            if (org != null)
                return;
            else if (dest == null)
            {
                dest = org;
                return;
            }
            if(len==0)
              len = dest.Length;
            Array.Copy(org, srcindex, dest, destindex, len); 
        }

        /// <summary>
        /// 查找起始位置
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="comparison">比对数组</param>
        /// <param name="sourceindex">起始搜索位置</param>
        /// <returns></returns>
        public static int IndexofArray(this byte[] source, byte[] comparison, int sourceindex = 0)
        {
            int index = sourceindex;
            int count = 0;
            if (source.Length <= 0)
                return -1;
            if (source.Length < comparison.Length)
                return -1;
            if (0 >= comparison.Length)
                return -1;
            if (sourceindex >= source.Length)
                return -1;
            List<byte> pack = new List<byte>();
            pack.AddRange(source);
            byte key = comparison[0];
            do
            {
                index = pack.IndexOf(key, index);
                if (index != -1)
                {
                    count = 0;
                    for (int i = 0; i < comparison.Length; i++)
                    {
                        if (pack[index + i] != comparison[i])
                            break;
                        else
                            count++;
                    }
                    if (count > 0 && count == comparison.Length)
                        return index;
                    else if (index + 1 >= pack.Count)
                        return -1;
                    else
                        index++;
                }
            }
            while (index != -1);
            return -1;
        }

        /// <summary>
        /// 是否包含数组
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="comparison">查找数组</param>
        /// <param name="sourceindex">查找起始位置</param>
        /// <returns></returns>
        public static bool ContainArray(this byte[] source, byte[] comparison, int sourceindex = 0)
        {
            bool res = false;
            int index = sourceindex;
            int count = 0;
            if (source.Length <= 0)
                return res;
            if (source.Length < comparison.Length)
                return res;
            if (0 >= comparison.Length)
                return res;
            if (sourceindex >= comparison.Length)
                return res;
            List<byte> pack = new List<byte>();
            pack.AddRange(source);
            byte key = comparison[0];
            do
            {
                index = pack.IndexOf(key, index);
                if (index != -1)
                {
                    count = 0;
                    for (int i = 0; i < comparison.Length; i++)
                    {
                        if (pack[index + i] != comparison[i])
                            break;
                        else
                            count++;
                    }
                    if (count > 0 && count == comparison.Length)
                        return true;
                    else if (index + 1 >= pack.Count)
                        return false ;
                    else
                        index++;
                }
            }
            while (index != -1);
            return false ;
        }

        /// <summary>
        /// 分隔数组
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="comparison">分隔数组</param>
        /// <param name="sourceindex"></param>
        /// <returns></returns>
        public static List<byte[]> Spilt(this byte[] source, byte[] comparison, int sourceindex = 0)
        {
            List<byte[]> res = new List<byte[]>();
            List<byte> pack = new List<byte>();
            pack.AddRange(source);
            int index = IndexofArray(pack.ToArray(), comparison, 0);
            if (index == -1)
                res.Add(pack.ToArray());
            else
            {
                if (index != 0)
                {
                    byte[] buffer = new byte[index];
                    pack.CopyTo(0, buffer, 0, buffer.Length);
                    res.Add(buffer);
                }
                int len = pack.Count;
                do
                {
                    int lastindex = IndexofArray(pack.ToArray(), comparison, index + comparison.Length);
                    if (lastindex == -1)
                    {
                        byte[] buffer = new byte[len - (index + comparison.Length)];
                        pack.CopyTo((index + comparison.Length), buffer, 0, buffer.Length);
                        res.Add(buffer);
                        index = -1;
                    }
                    else
                    {
                        byte[] buffer = new byte[lastindex - (index + comparison.Length)];
                        pack.CopyTo((index + comparison.Length), buffer, 0, buffer.Length);
                        res.Add(buffer);
                        index = lastindex;
                    }
                }
                while (index != -1);
            }
            return res;
        }

        /// <summary>
        /// 分隔数组
        /// </summary>
        /// <param name="source">源数组</param>
        /// <param name="comparison">包头</param>
        /// <param name="endcomparison">包尾</param>
        /// <param name="sourceindex">起始位置</param>
        /// <returns></returns>
        public static List<byte[]> Spilt(this byte[] source, byte[] comparison, byte[] endcomparison, int sourceindex = 0)
        {
            List<byte[]> res = new List<byte[]>();
            List<byte> pack = new List<byte>();
            if (source == null || comparison==null || endcomparison==null)
                return res;
            if (comparison != null)
            {
                if (comparison.Length == 0)
                    return res;
            }
            if (endcomparison != null)
            {
                if (endcomparison.Length == 0)
                    return res;
            }
            pack.AddRange(source);
            int index = 0;
            int lastindex= 0;
            do
            {
                index = IndexofArray(pack.ToArray(), comparison, index);
                if (index != -1)
                {
                    if (lastindex == 0 && index != 0)
                    {
                        byte[] buffer = new byte[index];
                        pack.CopyTo(0, buffer, 0, buffer.Length);
                        res.Add(buffer);
                    }
                    lastindex = IndexofArray(pack.ToArray(), endcomparison, index);
                    if (lastindex != -1)
                    {
                        byte[] buffer = new byte[lastindex - index + endcomparison.Length];
                        pack.CopyTo(index , buffer, 0, buffer.Length);
                        res.Add(buffer);
                        index = lastindex + endcomparison.Length;
                    }
                    else
                    {
                        byte[] buffer = new byte[pack.Count - index];
                        pack.CopyTo(index, buffer, 0, buffer.Length);
                        res.Add(buffer);
                        index = -1;
                    }
                }
                else
                {
                    if (lastindex == 0)
                        res.Add(pack.ToArray());
                    else if(pack.Count > (lastindex + endcomparison.Length))
                    {
                        byte[] buffer = new byte[pack.Count - (lastindex + endcomparison.Length)];
                        pack.CopyTo((lastindex + endcomparison.Length), buffer, 0, buffer.Length);
                        res.Add(buffer);
                    }  
                }
            }
            while (index != -1); 
            return res;
        }


        /// <summary>
        /// 倒叙
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static byte[] Flashback(this byte[] data)
        {
            List<byte> pack = new List<byte>();
            if (data != null)
                pack.AddRange(data);
            byte[] buf = new byte[pack.Count];
            for (int i = 0; i < pack.Count; i++)
                buf[pack.Count - 1 - i] = pack[i];
            return buf;
        }


    }
}
