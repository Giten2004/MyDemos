using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WebApp
{
    public class DepartmentSelectedEventArgs : EventArgs
    {
        public string Department { get; private set; }
        public DepartmentSelectedEventArgs(string department)
        {
            this.Department = department;
        }
    }
}
