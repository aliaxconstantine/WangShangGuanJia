using Electric.Class.DAO;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electric.Class.Factory
{
    public class DAOFactor
    {
        public static EleUser user;
        public static bool ifDataBase = true;
        public static HashSet<EleCommodity> commodities = new HashSet<EleCommodity>();
        public static HashSet<EleUser> users = new HashSet<EleUser>();
        public static HashSet<EleOrder> orders = new HashSet<EleOrder>();   
        public static HashSet<EleBooks> books = new HashSet<EleBooks>();
    }
}
