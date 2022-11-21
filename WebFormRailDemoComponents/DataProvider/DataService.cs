using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRailDemoComponents
{
    public class DataService
    {
        private static readonly Dictionary<int, Department> _departments = new Dictionary<int, Department>();
        private static readonly Dictionary<int, Employee> _employees = new Dictionary<int, Employee>();
        private static readonly Dictionary<int, User> _users = new Dictionary<int, User>();
        private static readonly Dictionary<int, Branch> _branches = new Dictionary<int, Branch>();

        // Setup test data
        static DataService()
        {
            Random rnd = new Random();

            _branches.Add(100, new Branch(100, "Chicago"));
            _branches.Add(200, new Branch(200, "New York"));
            _branches.Add(300, new Branch(300, "Los Angeles"));

            _departments.Add(1, new Department(1, "Accounting", 100));
            _departments.Add(2, new Department(2, "Accounting", 200));
            _departments.Add(3, new Department(3, "Accounting", 300));
            _departments.Add(4, new Department(4, "IT", 300));
            _departments.Add(5, new Department(5, "Sales", 200));
            _departments.Add(6, new Department(6, "Sales", 300));
            _departments.Add(7, new Department(7, "Marketing", 100));

            _employees.Add(1, new Employee(1, "Phil Baxter", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(2, new Employee(2, "John Miles", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(3, new Employee(3, "Mark Jones", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(4, new Employee(4, "Kate Moss", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(5, new Employee(5, "Dan Jameson", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(6, new Employee(6, "Eric Sanders", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(7, new Employee(7, "Dana Phillips", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));
            _employees.Add(8, new Employee(8, "Andy Waldorf", rnd.Next(20, 300) * 1000, rnd.Next(1, 7)));

            _users.Add(5, new User(5, "demo", "promesh"));
            _users.Add(10, new User(10, "activa", "coolstorage"));
        }

        public List<Employee> GetAllEmployees()
        {
            List<Employee> list = new List<Employee>();

            foreach (Employee employee in _employees.Values)
                list.Add(employee.Clone());

            return list;
        }

        public List<Employee> GetAllEmployeesForDepartment(Department department)
        {
            List<Employee> employees = new List<Employee>();

            foreach (Employee employee in _employees.Values)
                if (employee.DepartmentID == department.DepartmentID)
                    employees.Add(employee);

            return employees;
        }

        public List<Department> GetAllDepartmentsForBranch(Branch branch)
        {
            List<Department> departments = new List<Department>();

            foreach (Department department in _departments.Values)
                if (department.BranchID == branch.BranchID)
                    departments.Add(department);

            return departments;
        }


        public List<Department> GetAllDepartments()
        {
            List<Department> list = new List<Department>();

            foreach (Department department in _departments.Values)
                list.Add(department.Clone());

            return list;
        }

        public List<User> GetAllUsers()
        {
            List<User> list = new List<User>();

            foreach (User user in _users.Values)
                list.Add(user.Clone());

            return list;
        }

        public void Save(Employee employee)
        {
            if (employee.EmployeeID == 0)
            {
                foreach (Employee e in _employees.Values)
                    if (e.EmployeeID > employee.EmployeeID)
                        employee.EmployeeID = e.EmployeeID;

                employee.EmployeeID++;
            }

            _employees[employee.EmployeeID] = employee.Clone();
        }

        public void Save(Department department)
        {
            if (department.DepartmentID == 0)
            {
                foreach (Department d in _departments.Values)
                    if (d.DepartmentID > department.DepartmentID)
                        department.DepartmentID = d.DepartmentID;

                department.DepartmentID++;
            }

            _departments[department.DepartmentID] = department.Clone();
        }

        public Employee LoadEmployee(int id)
        {
            if (_employees.ContainsKey(id))
                return _employees[id].Clone();
            else
                return null;
        }

        public Department LoadDepartment(int id)
        {
            if (_departments.ContainsKey(id))
                return _departments[id].Clone();
            else
                return null;
        }

        public User FindUser(string login, string password)
        {
            foreach (User user in _users.Values)
                if (user.Login == login && user.Password == password)
                    return user.Clone();

            return null;
        }

        public Branch LoadBranch(int id)
        {
            if (_branches.ContainsKey(id))
                return _branches[id].Clone();
            else
                return null;

        }

        public List<Branch> GetAllBranches()
        {
            List<Branch> list = new List<Branch>();

            foreach (Branch branch in _branches.Values)
                list.Add(branch.Clone());

            return list;
        }
    }
}
