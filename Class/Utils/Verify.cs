using Electric.Class.DAO;
using Electric.Class.Factory;
using Sunny.UI.Win32;
using System;

namespace Electric.Class.Utils
{
    internal class Verify
    {
        public static Boolean verifyUser(string username,string password)
        {
            foreach (var item in DAOFactor.users) 
            {
                if(item.name.Equals(username))
                {
                    if (item.password.Equals(password))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static EleUser getUser(string username, string password)
        {
            foreach (var item in DAOFactor.users)
            {
                EleUser itema = (EleUser)item;
                if (itema.name.Equals(username))
                {
                    if (itema.password.Equals(password))
                    {
                        return itema;
                    }
                }
            }
            return null;
        }

        public static int sTi(string it)
        {
            if(int.TryParse(it, out var t))
            {
                return t;
            }
            return 0;
        }

    }
}
