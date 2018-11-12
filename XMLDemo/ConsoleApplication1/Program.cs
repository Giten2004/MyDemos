using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XMLDemos.XmlSerialize;
using XMLDemos.XmlSerialize.WithOutAttributeDefine;
using XMLDemos.XPath;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            //XmlNameSpaceTest.DoesNotWork_TestSelectWithDefaultNamespace();

            //XPathDemo.ParsePetsXmlWithNamespace();
            //test XPath demo
            //XPathDemo.ParsePetsXml("./Xpath/Pets.xml");

            Model2XML.Obj2Xml();
            //ModelDefaultToXml.DoDefaultXmlSerializeTest();

            Console.ReadLine();
        }
    }
}
