using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electric.Class.DAO
{
    public class EleBooks:EleDAO
    {
        public override int id { get; set; }
        public int quarter { get; set; }
        public int year { get; set; }   
        public decimal recorded { get; set; }
        public decimal billing { get;set; }
        public EleBooks() { }   
    }
}
