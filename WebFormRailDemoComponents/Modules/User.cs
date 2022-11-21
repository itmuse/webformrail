using System;
using System.Collections.Generic;
using System.Text;

namespace WebFormRailDemoComponents
{
    public class User
    {
        public User(int userID, string login, string password)
        {
            UserID = userID;
            Login = login;
            Password = password;
        }

        public int UserID;
        public string Login;
        public string Password;

        public User Clone()
        {
            return (User)MemberwiseClone();
        }
    }
}
