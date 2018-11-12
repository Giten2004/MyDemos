using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApp
{
    [ModelBinder(typeof(FooModelBinder))]
    public class Foo
    { }

    [ModelBinder(typeof(BarModelBinder))]
    public class Bar
    { }

    public class Baz
    { }
}