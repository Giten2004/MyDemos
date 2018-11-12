using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class HomeController : ControllerBase
    {
        public ActionResult Index(SimpleModel model)
        {
            Action<TextWriter> callback = writer =>
            {
                writer.Write(string.Format("Controller: {0}<br/>Action: {1}<br/><br/>", model.Controller, model.Action));
                writer.Write(string.Format("Foo: {0}<br/>Bar: {1}<br/>Baz: {2}", model.Foo, model.Bar, model.Baz));
            };
            return new RawContentResult(callback);
        }
    }
}