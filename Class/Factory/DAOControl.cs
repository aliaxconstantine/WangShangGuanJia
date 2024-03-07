using DatabaseConnectionPool;
using Electric.Class.DAO;
using Electric.Class.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Electric.Class.Factory
{
    public class DAOControl
    {
        public static void Init()
        {
            if (DAOFactor.ifDataBase)
            {
                SqlFactor.Init();
            }
            else
            {
                TableControl.Init();
            }
        }

        /// <summary>
        /// 获取DAOFactor的静态字段种类，并返回对应参数的HashSet
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eleDAO"></param>
        /// <returns>HashSet<T></returns>
        private static object GetHashSet<T>(T eleDAO)
        {
            Type type = typeof(DAOFactor);
            FieldInfo[] fields = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            foreach (FieldInfo field in fields)
            {
                // 检查字段类型是否为 HashSet<>
                if (field.FieldType.IsGenericType && field.FieldType.GetGenericTypeDefinition() == typeof(HashSet<>))
                {
                    Type elementType = field.FieldType.GetGenericArguments()[0];
                    if(eleDAO.GetType() == elementType)
                    {
                        return field.GetValue(null);
                    }
                }
            }
            return null;
        }


        public static void AddDAO(EleDAO eleDAO)
        {
            if(eleDAO is EleCommodity)
            {
                DAOFactor.commodities.Add((EleCommodity)eleDAO);
            }
            if (DAOFactor.ifDataBase)
            {
                SqlFactor.InsertSql(eleDAO);
            }
            else
            {
                TableControl.ExcelActControl(eleDAO, "添加");
            }
        }

        public static void UpdateDAO(EleDAO eleDAO)
        {
            if (eleDAO is EleCommodity)
            {
                DAOFactor.commodities.Add((EleCommodity)eleDAO);
            }
            if (eleDAO is EleOrder)
            {
                DAOFactor.orders.Add((EleOrder)eleDAO);
            }
            if (DAOFactor.ifDataBase)
            {
                SqlFactor.UpdateSql(eleDAO);
            }
            else
            {
                TableControl.ExcelActControl(eleDAO, "修改");
            }
        }

        public static void DeleteDAO(EleDAO eleDAO)
        {
            if (eleDAO is EleCommodity)
            {
                DAOFactor.commodities.Remove((EleCommodity)eleDAO);
            }
            if(eleDAO is EleOrder)
            {
                DAOFactor.orders.Remove((EleOrder)eleDAO);
            }
            if(eleDAO is EleBooks)
            {
                DAOFactor.books.Remove((EleBooks)eleDAO);
            }
            if (DAOFactor.ifDataBase)
            {
                SqlFactor.DeleteSql(eleDAO);
            }
            else
            {
                TableControl.ExcelActControl(eleDAO, "删除");
            }
        }
    }
}
