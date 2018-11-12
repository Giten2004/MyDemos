using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetworkSerialize
{
    public interface IObjectDataField:IDataField
    {
        bool IsComplexObject { get; }
    }
}
