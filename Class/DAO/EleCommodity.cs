using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Electric.Class.DAO
{
    public class EleCommodity:EleDAO
    {
        public override int id { get; set; }
        public String name { get; set; }
        public String description { get; set; }
        public Decimal price { get; set; }
        public int num { get; set; }
        public String state { get;set; }
        public String platform { get;set; }
        public EleCommodity() { }
    }
}
