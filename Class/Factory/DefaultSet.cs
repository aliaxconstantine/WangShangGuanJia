using Electric.Properties;
using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;

namespace Electric.Class.Factory
{
    public class DefaultSet
    {
        public static string startReturn = "亲，需要什么商品直接下单，机器人自动发货。";
        public static string endReturn = "谢谢惠顾亲。";
        public static string orderReturn = "24小时之内工作自动发货亲。";
        public static int uidnum = 10;

       public static void init()
        {
            try
            {
                startReturn = ElectSet.Default.开始回复;
                endReturn = ElectSet.Default.结束回复;
                orderReturn = ElectSet.Default.订单回复;
                uidnum = int.Parse(ElectSet.Default.uid位数);

                DAOFactor.users.Add(new DAO.EleUser()
                {
                    id = 1,
                    name = ElectSet.Default.默认管理员账号,
                    password = ElectSet.Default.默认管理员密码
                }) ;
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public static void save()
        {
            try
            {

                Dictionary<String, String> V = new Dictionary<string, string>()
                {
                    {"开始回复",startReturn},
                    {"结束回复",endReturn },
                    {"订单回复",orderReturn },
                    {"uid位数" ,uidnum.ToString() }
                };

                ElectSet.Default.开始回复 = startReturn;
                ElectSet.Default.结束回复 = endReturn;
                ElectSet.Default.订单回复 = orderReturn;
                ElectSet.Default.uid位数 = uidnum.ToString();
            }
            catch (ConfigurationErrorsException ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
