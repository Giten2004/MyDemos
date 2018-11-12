using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;

namespace VM.Models
{
public class PagingInfo
{
    public static int PageSize
    {
        get { return int.Parse(ConfigurationManager.AppSettings["pageSize"]); }
    }
    public int RecordCount { get; set; }       
    public int PageIndex { get; set; }
    public int PageCount
    {
        get { return (int)Math.Ceiling((decimal)RecordCount / PageSize); }
    }
}
}