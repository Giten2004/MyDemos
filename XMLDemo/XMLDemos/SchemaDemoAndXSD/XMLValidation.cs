using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace XMLDemos.SchemaDemoAndXSD
{
    public class XMLValidation
    {
        //c# 使用xsd文件验证xml格式
        /// <summary>
        /// 通过xsd验证xml格式是否正确，正确返回空字符串，错误返回提示
        /// </summary>
        /// <param name="xmlfile">xml文件</param>
        /// <param name="xsdfile">xsd文件</param>
        /// <param name="namespaceurl">命名空间，无则默认为null</param>
        /// <returns></returns>
        public static string XmlValidationByXSD(string xmlfile, string xsdfile, string namespaceurl = null)
        {
            StringBuilder sb = new StringBuilder();

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ValidationType = ValidationType.Schema;
            settings.Schemas.Add(namespaceurl, xsdfile);
            settings.ValidationEventHandler += (x, y) =>
            {
                sb.AppendFormat("{0}|", y.Message);
            };

            using (XmlReader reader = XmlReader.Create(xmlfile, settings))
            {
                try
                {
                    while (reader.Read());
                }
                catch (XmlException ex)
                {
                    sb.AppendFormat("{0}|", ex.Message);
                }
            }
            return sb.ToString();
        }
    }
}
