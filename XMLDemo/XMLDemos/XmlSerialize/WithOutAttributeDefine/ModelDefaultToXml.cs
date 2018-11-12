using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace XMLDemos.XmlSerialize.WithOutAttributeDefine
{
    public class ModelDefaultToXml
    {
        [Serializable]
        public class CatCollection2
        {
            public Cat2[] Cats { get; set; }
        }

        [Serializable]
        public class Cat2
        {
            public string Color { get; set; }

            public int Speed { get; set; }

            public string Saying { get; set; }
        }

        public static void DoDefaultXmlSerializeTest()
        {
            //声明一个猫咪对象
            var cWhite = new Cat2 { Color = "White", Speed = 10, Saying = "White or black,  so long as the cat can catch mice,  it is a good cat" };
            var cBlack = new Cat2 { Color = "Black", Speed = 10, Saying = "White or black,  so long as the cat can catch mice,  it is a good cat" };

            CatCollection2 cc = new CatCollection2 { Cats = new Cat2[] { cWhite, cBlack } };

            //序列化这个对象
            XmlSerializer serializer = new XmlSerializer(typeof(CatCollection2));

            //将对象序列化输出到控制台
            serializer.Serialize(Console.Out, cc);
//<? xml version = "1.0" encoding = "gb2312" ?>
//   < CatCollection2 xmlns:xsi = "http://www.w3.org/2001/XMLSchema-instance" xmlns:
//            xsd = "http://www.w3.org/2001/XMLSchema" >
//    < Cats >
//        < Cat2 >
//            < Color > White </ Color >
//            < Speed > 10 </ Speed >
//            < Saying > White or black, so long as the cat can catch mice,  it is a good cat</ Saying >
//        </ Cat2 >
//        < Cat2 >
//            < Color > Black </ Color >
//            < Speed > 10 </ Speed >
//            < Saying > White or black, so long as the cat can catch mice,  it is a good cat</ Saying >
//        </ Cat2 >
//    </ Cats >
//</ CatCollection2 >

            //compare the format 
           var xmlStr = XmlHelper.XmlSerialize(cc, Encoding.UTF8);


Console.ReadLine();
        }
    }
}
