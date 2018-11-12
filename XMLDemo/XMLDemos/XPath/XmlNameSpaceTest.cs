using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLDemos.XPath
{
    public class XmlNameSpaceTest
    {
        //Why does this not work?"
        public static void DoesNotWork_TestSelectWithDefaultNamespace()
        {
            // xml to parse with defaultnamespace
            string xml = @"<a xmlns='urn:test.Schema'><b/><b/></a>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            // fails because xpath does not have the namespace
            //!!!!
            Debug.Assert(doc.SelectNodes("//b").Count == 2);


            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("", "urn:test.Schema");

            // This will fail. Why?
            //This is a FAQ.In XPath any unprefixed name is assumed to be in "no namespace".
            //In order to select elements that belong to a namespace, 
            //in any XPath expression their names must be prefixed with a prefix that is associated with this namespace.
            //The AddNamespace() method serves exactly this purpose.It creates a binding between a specific namespace and a specific prefix.
            //Then, if this prefix is used in an XPath expression, the element prefixed by it can be selected.

            //It is written in the XPath W3C spec: "A QName in the node test is expanded into an expanded-name using the namespace declarations from the expression context. 
            //This is the same way expansion is done for element type names in start and end-tags except that the default namespace declared with xmlns is not used: 
            //if the QName does not have a prefix, then the namespace URI is null".

            //See this at: w3.org/TR/xpath/#node-tests .

            //So, any unprefixed name is considered to be in "no namespace". 
            //In the provided XML document there are no b elements in "no namespace" and this is why the XPath expression //b selects no nodes at all.
            // using XPath defaultnamespace 
            //!!!!
            Debug.Assert(doc.SelectNodes("//b", nsmgr).Count == 2);
        }

        public static void TestSelectWithoutNamespaces_Ok()
        {
            // xml to parse without namespace
            string xml = @"<a><b/><b/></a>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            // works ok
            Debug.Assert(doc.SelectNodes("//b").Count == 2);

            // works ok
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            Debug.Assert(doc.SelectNodes("//b", nsmgr).Count == 2);
        }

        public static void TestSelectWithNamespacesPrefixed_Ok()
        {
            // xml to parse with defaultnamespace
            string xml = @"<a xmlns='urn:test.Schema'><b/><b/></a>";

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);

            // using XPath namespace via alias "t". works ok but xpath is to complicated
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("t", doc.DocumentElement.NamespaceURI);

            Debug.Assert(doc.SelectNodes("//t:b", nsmgr).Count == 2);
        }
    }
}
