using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRailDemoComponents
{
    public class Department
    {
        private int _departmentID;
        private int _branchID;
        private string _name;

        public Department(int departmentID, string name, int branchID)
        {
            DepartmentID = departmentID;
            Name = name;
            BranchID = branchID;
        }

        public int DepartmentID
        {
            get { return _departmentID; }
            set { _departmentID = value; }
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int BranchID
        {
            get { return _branchID; }
            set { _branchID = value; }
        }

        public Branch Branch
        {
            get { return new DataService().LoadBranch(BranchID); }
        }

        public Department Clone()
        {
            return (Department)MemberwiseClone();
        }
    }
}
