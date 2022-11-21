using System;
using WebFormRail;

namespace WebFormRailDemoComponents
{
    public class DataObjectCreator : ICustomObjectCreator
    {
        public bool TryConvert(string value, Type objectType, out object obj)
        {
            if (objectType == typeof(Employee))
            {
                int id = WebFormRailUtil.ConvertString<int>(value);

                obj = new DataService().LoadEmployee(id);

                return (obj != null);
            }

            if (objectType == typeof(Department))
            {
                int id = WebFormRailUtil.ConvertString<int>(value);

                obj = new DataService().LoadDepartment(id);

                return (obj != null);
            }

            obj = null;

            return false;
        }
    }
}
