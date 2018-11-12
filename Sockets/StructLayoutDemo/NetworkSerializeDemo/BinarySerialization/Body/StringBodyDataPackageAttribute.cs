using System;
using System.Text;

namespace NetworkSerializeDemo.Body
{


    /// <summary>
    /// 针对String数据类型
    /// </summary>
    public sealed class StringBodyDataPackageAttribute : BodyDataPackageAttribute
    {

        public StringBodyDataPackageAttribute(int byteLength, int sortLocation)
            : base(byteLength, sortLocation)
        {

        }

        public override void ConvertObjectToByteArray<T>(T obj)
        {
            string str;
            if (obj as string == null)
                str = "\0";
            else
                str = Convert.ToString(obj);

            var strarray = Encoding.UTF8.GetBytes(str);
            for (var i = 0; i < DataSize; i++)
            {
                if (strarray.Length > i)
                {
                    DataPackage[i] = strarray[i];
                }
            }
        }

        public override T ConvertByteArrayToObject<T, TK>(byte[] bytes, TK obj)
        {
            string str = Encoding.UTF8.GetString(bytes);
            //string unicode = Encoding.Unicode.GetString(bytes);
            //string ascii = Encoding.ASCII.GetString(bytes);
            //string bigEndianUnicode = Encoding.BigEndianUnicode.GetString(bytes);
            //string utf32 = Encoding.UTF32.GetString(bytes);
            //string utf7 = Encoding.UTF7.GetString(bytes);

            //Logger.Info("T2", string.Format("utf7:{0}, utf8:{1}, utf32:{2}, unicode:{3}, ascii:{4}, bigE:{5}", utf7, str, utf32, unicode, ascii, bigEndianUnicode));
            
            if (!string.IsNullOrEmpty(str))
            {
                var indexOf = str.IndexOf("\0", StringComparison.Ordinal);
                if (indexOf != -1)
                {
                    str = str.Substring(0, indexOf);
                }
            }

            return (T)(object)str;
        }
    }
}
