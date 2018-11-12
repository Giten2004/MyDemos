using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace NetworkSerializeDemo
{
    public static class ByteExtensions
    {
        public static string ToHex(this byte b)
        {
            return b.ToString("X2");
        }

        public static byte[] HexStrToBytes(this string hexString, string split)
        {
            hexString = hexString.Replace(split, "");

            if ((hexString.Length % 2) != 0)
                hexString += " ";

            byte[] returnBytes = new byte[hexString.Length / 2];

            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);

            return returnBytes;
        }

        public static bool GetBit(this byte b, int index)
        {
            return (b & (1 << index)) > 0;
        }

        public static byte SetBit(this byte b, int index)
        {
            b |= (byte)(1 << index);
            return b;
        }

        public static byte ClearBit(this byte b, int index)
        {
            b &= (byte)((1 << 8) - 1 - (1 << index));
            return b;
        }

        public static byte ReverseBit(this byte b, int index)
        {
            b ^= (byte)(1 << index);
            return b;
        }

        public static byte[] ToBytes(this string str, int size, Encoding encoding)
        {
            var bytes = encoding.GetBytes(str);
            byte[] ret = new byte[size];
            bytes.CopyTo(ret, 0);
            return ret;
        }

        public static byte[] ToBytesUTF8(this string str, int size)
        {
            var bytes = Encoding.UTF8.GetBytes(str);
            byte[] ret = new byte[size];
            bytes.CopyTo(ret, 0);
            return ret;
        }
        
        public static string ToBase64String(this byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }
        
        public static string Decode(this byte[] data, Encoding encoding)
        {
            return encoding.GetString(data);
        }
        
        public static MemoryStream ToMemoryStream(this byte[] data)
        {
            return new MemoryStream(data);
        }
        
        public static Int16 ConvertInt16(this byte[] data)
        {
            var datas = ConvertBytesToLength(data, 2);
            return BitConverter.ToInt16(datas, 0);
        }
        
        public static Int32 ConvertInt32(this byte[] data)
        {
            var datas = ConvertBytesToLength(data, 4);
            return BitConverter.ToInt32(datas, 0);
        }

        public static Int64 ConvertInt64(this byte[] data)
        {
            var datas = ConvertBytesToLength(data, 8);
            return BitConverter.ToInt64(datas, 0);
        }

        private static byte[] ConvertBytesToLength(byte[] data, int dataSize)
        {
            if (data.Length == dataSize)
                return data;
            var bytes = new byte[dataSize];
            for (int i = 0; i < dataSize; i++)
            {
                if (data.Length == i + 1)
                {
                    bytes[i] = data[i];
                }
                else
                {
                    bytes[i] = 0;
                }
            }
            return bytes;
        }

        public static byte[] ToBytes<TObject>(this TObject obj)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, obj);
                stream.Flush();
                return stream.ToArray();
            }
        }
        
        public static TObject ToObject<TObject>(this byte[] bytes) where TObject : class
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length, false))
            {
                var formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                stream.Flush();
                return data as TObject;
            }
        }
        
        public static TObject ToObjectAsStruct<TObject>(this byte[] bytes) where TObject : struct
        {
            using (var stream = new MemoryStream(bytes, 0, bytes.Length, false))
            {
                var formatter = new BinaryFormatter();
                var data = formatter.Deserialize(stream);
                stream.Flush();
                return (TObject)data;
            }
        }

        public static byte[] StructToBytes<TObject>(this TObject structObj) where TObject : struct
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }

        public static object BytesToStuct(this byte[] bytes, Type type)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > bytes.Length)
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            object obj = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj;
        }
        
        public static void CopyIndex(this byte[] src, byte[] target, long srcIndex)
        {
            int p = 0;
            for (long i = srcIndex; i < src.Length; i++)
            {
                target[p] = src[i];
                p++;
            }
        }
    }
}
