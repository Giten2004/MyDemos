using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerialize
{
    public interface IDataField
    {
        uint DataFieldIndex { get; }
        uint DataFieldSize { get; }
        Type DataFieldType { get; }
        byte[] ConvertToBytes<T>(T dataFieldValue);
        object ConvertToObject(byte[] bytes);
        object ConvertToObject(byte[] bytes, object parentObj);
    }
}
