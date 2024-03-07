using DatabaseConnectionPool;
using Electric.Class.DAO;
using Electric.Class.Utils;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Electric.Class.Factory
{
    public class SqlFactor
    {
        public static String selectUserSql = "SELECT * FROM userSet";
        public static String selectCommodity = "SELECT * FROM commoidity";
        public static String selectOrder = "SELECT * FROM orders";
        public static String selectBook = "SELECT * FROM Books";


        public static bool DeleteSql<T>(T obj)
        {
            using (var connection = ConnectionPool.GetConnection())
            {
                String sqlDelete = "";
                connection.Open();
                var id = 0;
                if (obj is EleCommodity eleCommodity)
                {
                    sqlDelete = "DELETE FROM commoidity WHERE id = @Id";
                    id = eleCommodity.id;
                }
                if (obj is EleOrder order)
                {
                    sqlDelete = "DELETE FROM orders WHERE id = @Id";
                    id = order.id;
                }
                if (obj is EleUser user)
                {
                    sqlDelete = "DELETE FROM userset WHERE id = @Id";
                    id = user.id;
                }
                if (obj is EleBooks books)
                {
                    sqlDelete = "DELETE FROM books WHERE ID = @Id";
                    id = books.id;
                }
                using (MySqlCommand command = new MySqlCommand(sqlDelete, connection))
                {
                    command.Parameters.AddWithValue("@Id", id.ToString());
                    command.ExecuteNonQuery();
                    return true;
                }
            }
        }

        public static void Init()
        {
            InitUtli(SqlFactor.selectUserSql, new EleUser(), DAOFactor.users);
            InitUtli(SqlFactor.selectCommodity, new EleCommodity(), DAOFactor.commodities);
            InitUtli(SqlFactor.selectOrder,new EleOrder(),DAOFactor.orders);
            InitUtli(SqlFactor.selectBook, new EleBooks(), DAOFactor.books);
        }

        public static void InitUtli<T>(string sql,T obj,HashSet<T> list)
        {
            
            using (MySqlConnection mySqlConnection = ConnectionPool.GetConnection())
            {
                mySqlConnection.Open();

                using (MySqlCommand command = new MySqlCommand(sql, mySqlConnection))
                {
                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            
                            Type objectType = obj.GetType();
                            var o = Activator.CreateInstance(objectType,true);
                            Type oType = o.GetType();
                            PropertyInfo[] properties = oType.GetProperties();
                            foreach (PropertyInfo property in properties)
                            {
                                var data = reader.GetString(property.Name);

                                if (property.PropertyType == typeof(int))
                                {
                                    int intValue = Verify.sTi(data);
                                    property.SetValue(o, intValue);
                                }
                                else
                                {
                                    var convertedValue = Convert.ChangeType(data, property.PropertyType);
                                    property.SetValue(o, convertedValue);
                                }
                            }
                            list.Add((T)o);
                        }
                    }
                }
            }
        }

        public static bool InsertSql<T>(T obj)
        {
            using (var connection = ConnectionPool.GetConnection())
            {
                connection.Open();
                using (var command = new MySqlCommand("", connection))
                {
                    var tableName = "";
                    if (obj is EleCommodity)
                    {
                        tableName = "commoidity";
                    }
                    else if (obj is EleOrder)
                    {
                        tableName = "orders";
                    }
                    else if (obj is EleUser)
                    {
                        tableName = "userset";
                    }
                    else if (obj is EleBooks)
                    {
                        tableName = "books";
                    }

                    var properties = obj.GetType().GetProperties();
                    var columns = string.Join(", ", properties.Select(p => p.Name));
                    var values = string.Join(", ", properties.Select(p => $"@{p.Name}"));

                    var sqlInsert = $"INSERT INTO {tableName} ({columns}) VALUES ({values})";
                    command.CommandText = sqlInsert;

                    foreach (var property in properties)
                    {
                        var value = property.GetValue(obj);
                        command.Parameters.AddWithValue($"@{property.Name}", value);
                    }

                    command.ExecuteNonQuery();

                    return true;
                }
            }
        }

        public static bool UpdateSql<T>(T obj)
        {
            using (var connection = ConnectionPool.GetConnection())
            {
                string tableName = "";
                connection.Open();
                using (MySqlCommand command = new MySqlCommand("", connection))
                {
                    var id = 0;
                    if (obj is EleCommodity eleCommodity)
                    {
                        tableName = "commoidity";
                        id = eleCommodity.id;
                    }
                    if (obj is EleOrder order)
                    {
                        tableName = "orders";
                        id = order.id;
                    }
                    if (obj is EleUser user)
                    {
                        tableName = "userset";
                        id = user.id;
                    }
                    if (obj is EleBooks books)
                    {
                        tableName = "books";
                        id = books.id;
                    }

                    StringBuilder sqlUpdate = new StringBuilder($"UPDATE {tableName} SET ");

                    var properties = obj.GetType().GetProperties();
                    foreach (var property in properties)
                    {
                        var value = property.GetValue(obj);
                        if (value != null)
                        {
                            sqlUpdate.Append($"{property.Name} = @Value{property.Name}, ");
                            command.Parameters.AddWithValue($"@Value{property.Name}", value);
                        }
                    }

                    sqlUpdate.Remove(sqlUpdate.Length - 2, 2); // 移除最后的逗号和空格
                    sqlUpdate.Append($"WHERE id = @Id");
                    command.Parameters.AddWithValue("@Id", id);

                    command.CommandText = sqlUpdate.ToString();
                    command.ExecuteNonQuery();

                    return true;
                }
            }
        }


    }
}
