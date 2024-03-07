using Electric.Class.DAO;
using Electric.Class.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Electric.Class.Utils
{
    public class MathUtils
    {

        public static Decimal numInFlow(int num,string name)
        {
            EleCommodity ele = null;
            List <EleCommodity> eles = DAOFactor.commodities.Where(t => t.name == name).ToList();
            if (eles.Count > 0) 
            {
                ele = eles[0];
            }
            return num * ele.price;
        }
        public static HashSet<EleBooks> getBook(HashSet<EleOrder> eleOrders)
        { 
            Dictionary<(int, int), decimal> groupedOrders = new Dictionary<(int, int), decimal>();
            HashSet<EleBooks> eles = new HashSet<EleBooks>();
            foreach (EleOrder eleOrder in eleOrders)
            {
                (int year, int quarter) key = (eleOrder.year, eleOrder.quarter);
                Decimal inflow = 0;
                if (groupedOrders.ContainsKey(key))
                {
                    inflow = numInFlow(eleOrder.num, eleOrder.commoidty);
                    decimal recordedSum = groupedOrders[key];
                    recordedSum += inflow;
                    groupedOrders[key] = recordedSum;
                }
                else
                {
                    groupedOrders.Add(key,inflow);
                }
            }


            // 计算总入账金额并赋值给EleBooks对象
            foreach (var entry in groupedOrders)
            {
                EleBooks eleBooks = new EleBooks();
                eleBooks.recorded += entry.Value;
                eleBooks.year = entry.Key.Item1;
                eleBooks.quarter = entry.Key.Item2;
                eles.Add(eleBooks);
            }
            return eles;
        }

    }
}
