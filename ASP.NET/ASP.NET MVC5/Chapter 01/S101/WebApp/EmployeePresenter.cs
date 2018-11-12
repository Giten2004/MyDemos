using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApp
{
    public class EmployeePresenter
    {
        public IEmployeeView View { get; private set; }
        public EmployeeRepository Repository { get; private set; }

        public EmployeePresenter(IEmployeeView view)
        {
            this.View = view;
            this.Repository = new EmployeeRepository();
            this.View.DepartmentSelected += OnDepartmentSelected;
        }

        public void Initialize()
        {
            IEnumerable<Employee> employees = this.Repository.GetEmployees();
            this.View.BindEmployees(employees);
            string[] departments = new string[] { "", "销售部", "采购部", "人事部", "IT部" };
            this.View.BindDepartments(departments);
        }

        protected void OnDepartmentSelected(object sender,  DepartmentSelectedEventArgs args)
        {
            string department = args.Department;
            var employees = this.Repository.GetEmployees(department);
            this.View.BindEmployees(employees);
        }
    }
}