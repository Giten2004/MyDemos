using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NhibernateDemos.DAL.Model
{
    public class ElectricityTermStructureModel
    {
        //http://www.codeproject.com/Articles/37425/Execute-Stored-Procedure-in-SQL-Server-using-nHibe
        public ElectricityTermStructureModel(string fileDate, string fileTime, string fileType, string productYear, string productMonth, string @base, string peak, string cap, string basePointValue, string peakPointValue, string capPointValue, string traderId)
        {
            FileDate = fileDate;
            FileTime = fileTime;
            FileType = fileType;
            ProductYear = productYear;
            ProductMonth = productMonth;
            Base = @base;
            Peak = peak;
            Cap = cap;
            BasePointValue = basePointValue;
            PeakPointValue = peakPointValue;
            CapPointValue = capPointValue;
            TraderId = traderId;
        }

        public virtual string FileDate { get; set; }
        public virtual string FileTime { get; set; }
        public virtual string FileType { get; set; }
        public virtual string ProductYear { get; set; }
        public virtual string ProductMonth { get; set; }
        public virtual string Base { get; set; }
        public virtual string Peak { get; set; }
        public virtual string Cap { get; set; }
        public virtual string BasePointValue { get; set; }
        public virtual string PeakPointValue { get; set; }
        public virtual string CapPointValue { get; set; }
        public virtual string TraderId { get; set; }
    }
}
