using System;
using MvvmLight1.Model;

namespace MvvmLight1.Design
{
    public class DesignDataService : IDataService
    {
        private IProxy _proxy;
        public DesignDataService(IProxy proxy)
        {
            _proxy = proxy;
        }

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to create design time data

            var item = new DataItem("Welcome to MVVM Light [design]");
            callback(item, null);
        }
    }
}