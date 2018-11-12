using MvcApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MvcApp
{
    public interface IEmployeeRepository
    {
        IEnumerable<Employee> GetEmployees(string id = "");
    }
}