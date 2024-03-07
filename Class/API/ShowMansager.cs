using Electric.Class.DAO;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Policy;
using System.Windows;

namespace Electric.Class.API
{
    public class ShowManager
    {
        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="commodity"></param>
        /// <returns></returns>
        public static Boolean sendShop(EleOrder ele, EleCommodity commodity)
        {
            MessageBox.Show($"{ele.state}平台发货成功", "提示");
            return true;
        }    
        /// <summary>
        /// 下架商品
        /// </summary>
        /// <param name="commodity"></param>
        /// <returns></returns>
        public static Boolean takeDown(EleCommodity commodity)
        {
            MessageBox.Show($"已从平台下架{commodity.name}", "提示");
            return true;
        }
        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="commodity"></param>
        /// <returns></returns>
        public static Boolean orderDeletion(EleOrder order)
        {
            MessageBox.Show($"已从{order.state}平台删除订单", "提示");
            return true;
        }

        /// <summary>
        /// 获取指定文件URL
        /// </summary>
        /// <param name="eleCommodity"></param>
        /// <returns></returns>
        public static Boolean GetURL(EleCommodity eleCommodity)
        {
            return true;
        }
        /// <summary>
        /// 上架货物
        /// </summary>
        /// <param name="eleCommodity"></param>
        /// <returns></returns>
        public static Boolean Shelves(EleCommodity eleCommodity)
        {
            MessageBox.Show($"已在{eleCommodity.platform}平台上架货物{eleCommodity.name}","提示");
            return true;
        }
    }
}
