using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace DatabaseConnectionPool
{
    public class ConnectionPool
    {
        private static readonly string connectionString = "Server=localhost;Database=electricbusiness;Uid=abc;Pwd=123456;";
        private static readonly int poolSize = 10; // 连接池大小
        private static readonly Queue<MySqlConnection> connectionPool = new Queue<MySqlConnection>();

        static ConnectionPool()
        {
            InitializeConnectionPool();
        }
        /// <summary>
        /// 初始化数据库
        /// </summary>
        private static void InitializeConnectionPool()
        {
            for (int i = 0; i < poolSize; i++)
            {
                MySqlConnection connection = new MySqlConnection(connectionString);
                connectionPool.Enqueue(connection);
            }
        }
        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static MySqlConnection GetConnection()
        {
            if (connectionPool.Count > 0)
            {
                MySqlConnection connection = connectionPool.Dequeue();
                return connection;
            }

            throw new Exception("Connection pool is empty.");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connection"></param>
        public static void ReleaseConnection(MySqlConnection connection)
        {
            if (connection != null)
            {
                connectionPool.Enqueue(connection);
            }
        }
    }
}
