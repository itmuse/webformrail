using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRailDemoComponents
{
    public class Employee
    {
        public Employee()
        {
        }

        public Employee(int employeeID, string name, decimal salary, int departmentID)
        {
            EmployeeID = employeeID;
            Name = name;
            Salary = salary;
            DepartmentID = departmentID;
        }

        public int EmployeeID;
        public string Name;
        public decimal? Salary;
        public int DepartmentID;


        public Department Department
        {
            get { return new DataService().LoadDepartment(DepartmentID); }
        }

        public Employee Clone()
        {
            return (Employee)MemberwiseClone();
        }
    }
}
