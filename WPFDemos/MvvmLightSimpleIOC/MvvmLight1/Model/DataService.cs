using System;

namespace MvvmLight1.Model
{
    public class DataService : IDataService
    {
        private IProxy _proxy;
        public DataService(IProxy proxy)
        {
            _proxy = proxy;
        }

        public void GetData(Action<DataItem, Exception> callback)
        {
            // Use this to connect to the actual data service

            var item = new DataItem("Welcome to MVVM Light");
            callback(item, null);
        }
    }
}