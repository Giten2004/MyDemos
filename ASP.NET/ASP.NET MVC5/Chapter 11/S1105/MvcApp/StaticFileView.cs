using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    public class StaticFileView : IView
    {
        public string FileName { get; private set; }
        public StaticFileView(string fileName)
        {
            this.FileName = fileName;
        }
        public void Render(ViewContext viewContext, TextWriter writer)
        {
            byte[] buffer;
            using (FileStream fs = new FileStream(this.FileName, FileMode.Open))
            {
                buffer = new byte[fs.Length];
                fs.Read(buffer, 0, buffer.Length);
            }
            writer.Write(Encoding.UTF8.GetString(buffer));
        }
    }
}