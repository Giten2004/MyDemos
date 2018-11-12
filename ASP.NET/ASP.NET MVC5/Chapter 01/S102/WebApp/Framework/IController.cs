using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp
{
    public interface IController
    {
        void Execute(RequestContext requestContext);
    }
}
