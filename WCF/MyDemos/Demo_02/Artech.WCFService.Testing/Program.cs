using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using System.IO;
using Artech.WCFService.Contract;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace Artech.WCFService.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Runtime.Serialization.DataContractSerializer serializer = new DataContractSerializer(typeof(MessageContractOrder));
            MessageContractOrder order = new MessageContractOrder();
            order.OrderNo = new Guid();
            order.OrderDate = DateTime.Today;
            order.ProductSupplier = "HP";
            order.Product = "PC";
            order.Quantity = 200;

            Message message = order as Message;

            XmlDictionaryWriter writer = new XmlDictionaryWriter();

            using (FileStream fs = new FileStream(@"c:\order.xml", FileMode.Create, FileAccess.ReadWrite, FileShare.Write))
            {
                serializer.WriteObject(fs, order);
            }
            
            System.Diagnostics.Process.Start(@"c:\order.xml");
        }
    }
}
