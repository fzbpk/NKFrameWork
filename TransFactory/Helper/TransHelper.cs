using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace NK.Communicate
{
    internal static  class TransHelper
    {
        public static string ToHex(this byte[] data, bool SpaceSplit = true)
        {
            if (data == null)
                return "";
            else if (data.Length == 0)
                return "";
            string info = "";
            for (int i = 0; i < data.Length; i++)
            {
                info += " " + (data[i].ToString("X2").Length == 1 ? "0" + data[i].ToString("X2") : data[i].ToString("X2"));
            }
            info = info.TrimStart();
            if (!SpaceSplit)
                info = info.Replace(" ", "");
            return info;
        }

        public static byte[] FromHex(this string data)
        {
            if (string.IsNullOrEmpty(data))
                return null;
            data = data.Replace(" ", "");
            if ((data.Length % 2) != 0)
                data += " ";
            byte[] returnBytes = new byte[data.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(data.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static string Serialize(this object obj)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, obj);
            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            return Encoding.UTF8.GetString(dataBytes);
        }

        public static T Deserialize<T>(this string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }

    }
}
