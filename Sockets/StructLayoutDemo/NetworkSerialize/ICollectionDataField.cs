using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerialize
{
    public interface ICollectionDataField:IDataField
    {
        bool IsCollection { get; }
        uint SingleDataSize { get; }
        string CollectionSizePropertyName { get; }
        string CollectionPropertyName { get; }
        Type CollectionElementType { get; }
    }
}
