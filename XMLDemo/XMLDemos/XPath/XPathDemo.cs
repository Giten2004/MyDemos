using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLDemos.XPath
{
    public class XPathDemo
    {
        public static void ParsePetsXml(string xmlFilePath)
        {
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlFilePath);

            //取所有pets节点下的dog字节点
            XmlNodeList nodeListAllDog = xdoc.SelectNodes("/pets/dog");

            //所有的price节点
            XmlNodeList allPriceNodes = xdoc.SelectNodes("//price");

            //取最后一个price节点
            XmlNode lastPriceNode = xdoc.SelectSingleNode("//price[last()]");

            //用双点号取price节点的父节点
            XmlNode lastPriceParentNode = lastPriceNode.SelectSingleNode("..");

            //
            var descNode = lastPriceParentNode.SelectSingleNode("./desc");

            //选择weight*count=40的所有动物，使用通配符*
            XmlNodeList nodeList = xdoc.SelectNodes("/pets/*[@weight*@count=40]");

            //选择除了pig之外的所有动物,使用name()函数返回节点名字
            XmlNodeList animalsExceptPigNodes = xdoc.SelectNodes("/pets/*[name() != 'pig']");


            //选择价格大于100而不是pig的动物
            XmlNodeList priceGreaterThan100s = xdoc.SelectNodes("/pets/*[price div @weight >10 and name() != 'pig']");
            foreach (XmlNode item in priceGreaterThan100s)
            {
                Console.WriteLine(item.OuterXml);
            }

            //选择第二个dog节点
            XmlNode theSecondDogNode = xdoc.SelectSingleNode("//dog[position() = 2]");

            //使用xpath ，axes 的 parent 取父节点
            XmlNode parentNode = theSecondDogNode.SelectSingleNode("parent::*");

            //使用xPath选择第二个dog节点前面的所有dog节点
            XmlNodeList dogPresibling = theSecondDogNode.SelectNodes("preceding::dog");

            //取文档的所有子孙节点price
            XmlNodeList childrenNodes = xdoc.SelectNodes("descendant::price");
        }

        public static void ParsePetsXmlWithNamespace()
        {
            string xmlFilePath = "./Xpath/FuturesTrade.xml";
            XmlDocument xdoc = new XmlDocument();
            xdoc.Load(xmlFilePath);

            var rootNode = xdoc.DocumentElement;

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xdoc.NameTable);
            nsmgr.AddNamespace("ns", "www.eurexchange.com/technology");
            //or the next line method
            //nsmgr.AddNamespace("ns", xdoc.DocumentElement.NamespaceURI);

            var trdCapRptNode = rootNode.SelectSingleNode("./ns:TrdCaptRpt", nsmgr);
            if (trdCapRptNode != null)
            {
                var bizDt = trdCapRptNode.Attributes["BizDt"].Value;
                var ccy = trdCapRptNode.Attributes["Ccy"].Value;
            }
        }
    }
}
