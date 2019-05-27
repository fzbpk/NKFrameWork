using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security;
using System.Security.Cryptography;

namespace NK.DataWork
{
    /// <summary>
    /// 加解密
    /// </summary>
    public static partial class Security
    {

        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="code">被加密字符串</param>
        /// <param name="Key">密匙</param>
        /// <returns>加密字符串</returns>
        public static string MD5(string code, string Key = "",Encoding CharSet=null)
        {
            if (CharSet == null)
                CharSet = Encoding.Default;
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            return  BitConverter.ToString(hashmd5.ComputeHash(CharSet.GetBytes(code + Key))).Replace("-", "");
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data">原始数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param> 
        /// <returns></returns>
        public static byte[] AesEncrypt(byte[] Data, byte[] Key, byte[] IV = null)
        {
            if (IV == null)
                IV = Key;
            if (Data == null )
                throw new NullReferenceException("Data  IS NULL ");
            else if(Data.Length==0)
                throw new NullReferenceException("Data  IS EMPTY ");
            else if (Key == null)
                throw new NullReferenceException("Key  IS NULL ");
            else if (Key.Length == 0)
                throw new NullReferenceException("Key  IS EMPTY ");
            byte[] KeyBuf = Key;
            byte[] IVBuf = IV;
            byte[] buf = null;
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 256)
                {
                    aesAlg.KeySize = 256;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else if (KeyBuf.Length >= 128)
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = IVBUF;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(Data);
                        }
                        StringBuilder ret = new StringBuilder();
                        buf = msEncrypt.ToArray();
                        }
                }
            }
            return buf;
        }

        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="Data">原始数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param> 
        /// <param name="CharSet">编码</param> 
        /// <returns></returns>
        public static string AesEncrypt( string Data, string Key, string IV = "",Encoding CharSet=null)
        {
            if (string.IsNullOrEmpty(IV))
                IV = Key;
            if (CharSet == null)
                CharSet = Encoding.UTF8;
            byte[] KeyBuf = CharSet.GetBytes(Key);
            byte[] IVBuf = CharSet.GetBytes(IV);
            byte[] Datas= CharSet.GetBytes(Data);
            byte[] buf= AesEncrypt(Datas, KeyBuf, IVBuf);
            StringBuilder ret = new StringBuilder();
            foreach (byte b in buf)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }


        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">加密数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param>
        /// <returns></returns>
        public static byte[] AesDecrypt(byte[] Data, byte[] Key, byte[] IV = null)
        {
            if (IV == null)
                IV = Key;
            if (Data == null)
                throw new NullReferenceException("Data  IS NULL ");
            else if (Data.Length == 0)
                throw new NullReferenceException("Data  IS EMPTY ");
            else if (Key == null)
                throw new NullReferenceException("Key  IS NULL ");
            else if (Key.Length == 0)
                throw new NullReferenceException("Key  IS EMPTY ");
            byte[] KeyBuf = Key;
            byte[] IVBuf = IV;
            List<byte> buf = new List<byte>();
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 256)
                {
                    aesAlg.KeySize = 256;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else if (KeyBuf.Length >= 128)
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = IVBUF;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            int res = 0;
                            do
                            {
                                res = srDecrypt.Read();
                                if (res != -1)
                                    buf.Add((byte)res);
                            }
                            while (res != -1); 
                        }
                    }
                }
            }
            return buf.ToArray(); 
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="Data">加密数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param>
        /// <param name="CharSet">编码</param>
        /// <returns></returns>
        public static string AesDecrypt(this string Data, string Key, string IV = "", Encoding CharSet = null)
        {
            if (string.IsNullOrEmpty(Data) || string.IsNullOrEmpty(Key))
                return "";
            if (string.IsNullOrEmpty(IV))
                IV = Key;
            if (CharSet == null)
                CharSet = Encoding.UTF8;
            string res = "";
            byte[] KeyBuf = CharSet.GetBytes(Key);
            byte[] IVBuf = CharSet.GetBytes(IV);
            byte[] inputByteArray = new byte[Data.Length / 2];
            for (int x = 0; x < Data.Length / 2; x++)
            {
                int i = (Convert.ToInt32(Data.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            using (AesCryptoServiceProvider aesAlg = new AesCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 256)
                {
                    aesAlg.KeySize = 256;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else if (KeyBuf.Length >= 128)
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = IVBUF;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(inputByteArray))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            res = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return res;
        }
         
        /// <summary>
        /// TripeDes加密
        /// </summary>
        /// <param name="Data">原始数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param> 
        /// <param name="mode">块密码模式</param>
        /// <param name="type">填充</param>
        /// <returns></returns>
        public static byte[] TripeDesEncrypt(byte[] Data, byte[] Key, byte[] IV = null,  CipherMode mode = CipherMode.CBC, PaddingMode type = PaddingMode.PKCS7)
        {
            if (IV == null)
                IV = Key;
            if (Data == null)
                throw new NullReferenceException("Data  IS NULL ");
            else if (Data.Length == 0)
                throw new NullReferenceException("Data  IS EMPTY ");
            else if (Key == null)
                throw new NullReferenceException("Key  IS NULL ");
            else if (Key.Length == 0)
                throw new NullReferenceException("Key  IS EMPTY ");
            byte[] KeyBuf = Key;
            byte[] IVBuf = IV;
            byte[] buf = null;
            using (TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 192)
                {
                    des.KeySize = 192;
                    KEYBUF = new byte[des.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[des.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else if (KeyBuf.Length >= 128)
                {
                    des.KeySize = 128;
                    KEYBUF = new byte[des.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[des.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    des.KeySize = 128;
                    KEYBUF = new byte[des.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[des.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                des.Mode = mode;
                des.Padding = type;
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, des.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(Data);
                        }
                        StringBuilder ret = new StringBuilder();
                        buf = msEncrypt.ToArray();
                    }
                }
            }
            return buf;
        }

        /// <summary>
        /// TripeDes加密
        /// </summary>
        /// <param name="Data">原始数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param> 
        /// <param name="mode">块密码模式</param>
        /// <param name="type">填充</param>
        /// <param name="CharSet">编码</param> 
        /// <returns></returns>
        public static string TripeDesEncrypt(string Data, string Key, string IV = "", CipherMode mode = CipherMode.CBC, PaddingMode type = PaddingMode.PKCS7, Encoding CharSet = null)
        {
            if (string.IsNullOrEmpty(IV))
                IV = Key;
            if (CharSet == null)
                CharSet = Encoding.UTF8;
            byte[] KeyBuf = CharSet.GetBytes(Key);
            byte[] IVBuf = CharSet.GetBytes(IV);
            byte[] Datas = CharSet.GetBytes(Data);
            byte[] buf = TripeDesEncrypt(Datas, KeyBuf, IVBuf, mode, type);
            StringBuilder ret = new StringBuilder();
            foreach (byte b in buf)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }


        /// <summary>
        /// TripeDes解密
        /// </summary>
        /// <param name="Data">加密数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param>
        /// <param name="mode">块密码模式</param>
        /// <param name="type">填充</param>
        /// <returns></returns>
        public static byte[] TripeDesDecrypt(byte[] Data, byte[] Key, byte[] IV = null, CipherMode mode = CipherMode.CBC, PaddingMode type = PaddingMode.PKCS7)
        {
            if (IV == null)
                IV = Key;
            if (Data == null)
                throw new NullReferenceException("Data  IS NULL ");
            else if (Data.Length == 0)
                throw new NullReferenceException("Data  IS EMPTY ");
            else if (Key == null)
                throw new NullReferenceException("Key  IS NULL ");
            else if (Key.Length == 0)
                throw new NullReferenceException("Key  IS EMPTY ");
            byte[] KeyBuf = Key;
            byte[] IVBuf = IV;
            List<byte> buf = new List<byte>();
            using (TripleDESCryptoServiceProvider aesAlg = new TripleDESCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 192)
                {
                    aesAlg.KeySize = 192;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else if (KeyBuf.Length >= 128)
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = KEYBUF;
                aesAlg.Mode = mode;
                aesAlg.Padding = type;
                 using (MemoryStream msDecrypt = new MemoryStream(Data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            int res = 0;
                            do
                            {
                                res = srDecrypt.Read();
                                if (res != -1)
                                    buf.Add((byte)res);
                            }
                            while (res != -1);
                        }
                    }
                }
            }
            return buf.ToArray();
        }

        /// <summary>
        /// TripeDes解密
        /// </summary>
        /// <param name="Data">加密数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param>
        /// <param name="mode">块密码模式</param>
        /// <param name="type">填充</param>
        /// <param name="CharSet">编码</param>
        /// <returns></returns>
        public static string TripeDesDecrypt(this string Data, string Key, string IV = "", CipherMode mode = CipherMode.CBC, PaddingMode type = PaddingMode.PKCS7, Encoding CharSet = null)
        {
            if (string.IsNullOrEmpty(Data) || string.IsNullOrEmpty(Key))
                return "";
            if (string.IsNullOrEmpty(IV))
                IV = Key;
            if (CharSet == null)
                CharSet = Encoding.UTF8;
            string res = "";
            byte[] KeyBuf = CharSet.GetBytes(Key);
            byte[] IVBuf = CharSet.GetBytes(IV);
            byte[] inputByteArray = new byte[Data.Length / 2];
            for (int x = 0; x < Data.Length / 2; x++)
            {
                int i = (Convert.ToInt32(Data.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            using (TripleDESCryptoServiceProvider aesAlg = new TripleDESCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 192)
                {
                    aesAlg.KeySize = 192;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else if (KeyBuf.Length >= 128)
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 128;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = KEYBUF;
                aesAlg.Mode = mode;
                aesAlg.Padding = type;
                   using (MemoryStream msDecrypt = new MemoryStream(inputByteArray))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, aesAlg.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            res = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return res;
        }

        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="Data">原始数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param> 
        /// <returns></returns>
        public static byte[] DesEncrypt(byte[] Data, byte[] Key, byte[] IV = null)
        {
            if (IV == null)
                IV = Key;
            if (Data == null)
                throw new NullReferenceException("Data  IS NULL ");
            else if (Data.Length == 0)
                throw new NullReferenceException("Data  IS EMPTY ");
            else if (Key == null)
                throw new NullReferenceException("Key  IS NULL ");
            else if (Key.Length == 0)
                throw new NullReferenceException("Key  IS EMPTY ");
            byte[] KeyBuf = Key;
            byte[] IVBuf = IV;
            byte[] buf = null;
            using (DESCryptoServiceProvider aesAlg = new DESCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                if (KeyBuf.Length >= 64)
                {
                    aesAlg.KeySize = 64;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 64;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = IVBUF;
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(Data);
                        }
                        StringBuilder ret = new StringBuilder();
                        buf = msEncrypt.ToArray();
                    }
                }
            }
            return buf;
        }

        /// <summary>
        /// Des加密
        /// </summary>
        /// <param name="Data">原始数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param> 
        /// <param name="CharSet">编码</param> 
        /// <returns></returns>
        public static string DesEncrypt(string Data, string Key, string IV = "", Encoding CharSet = null)
        {
            if (string.IsNullOrEmpty(IV))
                IV = Key;
            if (CharSet == null)
                CharSet = Encoding.UTF8;
            byte[] KeyBuf = CharSet.GetBytes(Key);
            byte[] IVBuf = CharSet.GetBytes(IV);
            byte[] Datas = CharSet.GetBytes(Data);
            byte[] buf = AesEncrypt(Datas, KeyBuf, IVBuf);
            StringBuilder ret = new StringBuilder();
            foreach (byte b in buf)
            {
                ret.AppendFormat("{0:X2}", b);
            }
            return ret.ToString();
        }
        
        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="Data">加密数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param>
        /// <returns></returns>
        public static byte[] DesDecrypt(byte[] Data, byte[] Key, byte[] IV = null)
        {
            if (IV == null)
                IV = Key;
            if (Data == null)
                throw new NullReferenceException("Data  IS NULL ");
            else if (Data.Length == 0)
                throw new NullReferenceException("Data  IS EMPTY ");
            else if (Key == null)
                throw new NullReferenceException("Key  IS NULL ");
            else if (Key.Length == 0)
                throw new NullReferenceException("Key  IS EMPTY ");
            byte[] KeyBuf = Key;
            byte[] IVBuf = IV;
            List<byte> buf = new List<byte>();
            using (DESCryptoServiceProvider aesAlg = new DESCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                 if (KeyBuf.Length >= 64)
                {
                    aesAlg.KeySize = 64;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 64;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = IVBUF;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(Data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            int res = 0;
                            do
                            {
                                res = srDecrypt.Read();
                                if (res != -1)
                                    buf.Add((byte)res);
                            }
                            while (res != -1);
                        }
                    }
                }
            }
            return buf.ToArray();
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="Data">加密数据</param>
        /// <param name="Key">密匙</param>
        /// <param name="IV">IV密匙</param>
        /// <param name="CharSet">编码</param>
        /// <returns></returns>
        public static string DesDecrypt(this string Data, string Key, string IV = "", Encoding CharSet = null)
        {
            if (string.IsNullOrEmpty(Data) || string.IsNullOrEmpty(Key))
                return "";
            if (string.IsNullOrEmpty(IV))
                IV = Key;
            if (CharSet == null)
                CharSet = Encoding.UTF8;
            string res = "";
            byte[] KeyBuf = CharSet.GetBytes(Key);
            byte[] IVBuf = CharSet.GetBytes(IV);
            byte[] inputByteArray = new byte[Data.Length / 2];
            for (int x = 0; x < Data.Length / 2; x++)
            {
                int i = (Convert.ToInt32(Data.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }
            using (DESCryptoServiceProvider aesAlg = new DESCryptoServiceProvider())
            {
                byte[] KEYBUF = null;
                byte[] IVBUF = null;
                 if (KeyBuf.Length >= 64)
                {
                    aesAlg.KeySize = 64;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KEYBUF.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, IVBUF.Length);
                }
                else
                {
                    aesAlg.KeySize = 64;
                    KEYBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(KeyBuf, 0, KEYBUF, 0, KeyBuf.Length);
                    IVBUF = new byte[aesAlg.Key.Length];
                    Array.Copy(IVBuf, 0, IVBUF, 0, KeyBuf.Length);
                }
                aesAlg.Key = KEYBUF;
                aesAlg.IV = IVBUF;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msDecrypt = new MemoryStream(inputByteArray))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            res = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return res;
        }

    }

}
