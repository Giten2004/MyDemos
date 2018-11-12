using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp
{
    public interface IModelBinder
    {
        object BindModel(ControllerContext controllerContext, string modelName,Type modelType);
    }
}
