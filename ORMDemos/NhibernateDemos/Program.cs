using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NhibernateDemos.DAL;
using NhibernateDemos.DAL.Model;

namespace NhibernateDemos
{
    class Program
    {
        static void Main(string[] args)
        {
            var tt = new Program();
            var restult = tt.GetForumUserProfile("20161115", "20161122");

            Console.ReadLine();
        }

        public IEnumerable<ElectricityTermStructureModel> GetForumUserProfile(string startDate, string endDate)
        {
            var query =  NHibernateHelper
                .OpenSession()
                .GetNamedQuery("GetElectricityTermStructure")
                .SetString("FileStartDate", startDate)
                .SetString("FileEndDate", endDate)
                .SetResultTransformer(new NHibernate.Transform.AliasToBeanConstructorResultTransformer(typeof(ElectricityTermStructureModel).GetConstructors()[0]));

            return query.List<ElectricityTermStructureModel>();
        }
    }
}
