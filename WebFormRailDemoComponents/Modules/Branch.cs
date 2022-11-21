using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRailDemoComponents
{
    public class Branch
    {
        public Branch(int branchID, string name)
        {
            BranchID = branchID;
            Name = name;
        }

        public int BranchID;
        public string Name;

        public Branch Clone()
        {
            return (Branch)MemberwiseClone();
        }
    }
}
