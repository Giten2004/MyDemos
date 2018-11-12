using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    public interface IListProvider
    {
        IEnumerable<ListItem> GetListItems(string listName);
    }
}