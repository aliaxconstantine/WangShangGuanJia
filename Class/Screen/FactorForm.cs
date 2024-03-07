using Electric.Class.API;
using Electric.Class.DAO;
using Electric.Class.Factory;
using Electric.Class.Utils;
using Google.Protobuf.WellKnownTypes;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Electric.Class.Screen
{
    public partial class FactorForm : UIAsideHeaderMainFrame
    {
        TreeNode index;
        Logfin lin;

        Size oldSize;
        Size num;
        bool iffirst;
        int i;
        Dictionary<ListView,string> keys = new Dictionary<ListView, string>();
        public FactorForm(Logfin lin)
        {
            this.lin = lin;
            InitializeComponent();
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            uiLabel1.Text = DAOFactor.user.name;
            iffirst = true;
            init();
            oldSize = this.Size;
            num = new Size(1,1);
            this.WindowState = FormWindowState.Maximized;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result = MessageBox.Show("确定要关闭窗口吗？", "确认关闭", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                lin.Close();
            }
        }

        public void DeleteClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ListView listView = (ListView)button.Parent;
            if (keys[listView] == "商品")
            {
                EleCommodity ele = (EleCommodity)DAOFactor.commodities.FirstOrDefault(T => T.id == (int)button.Tag);
                DAOControl.DeleteDAO(ele);
                ShowManager.takeDown(ele);
            }
            if (keys[listView] == "订单")
            {
                EleOrder ele = (EleOrder)DAOFactor.orders.FirstOrDefault(T => T.id == (int)button.Tag);
                DAOControl.DeleteDAO(ele);
                ShowManager.orderDeletion(ele);
            }
            if (keys[listView] == "账单")
            {
                EleBooks ele = (EleBooks)DAOFactor.books.FirstOrDefault(T => T.id == (int)button.Tag);
                DAOControl.DeleteDAO(ele);
                DAOFactor.books.Remove(ele);
            }

            init();
        }

        public void editClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ListView listView = (ListView)button.Parent;
            EleCommodity commodity = (EleCommodity)DAOFactor.commodities.FirstOrDefault(T => T.id == (int)button.Tag);
            EditFrom editFrom = new EditFrom(commodity,"编辑");
            editFrom.Tag = this;
            editFrom.Show();
        }

        public void sendClick(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            ListView listView = (ListView)button.Parent;
            EleOrder order = (EleOrder)DAOFactor.orders.FirstOrDefault(T => T.id == (int)button.Tag);
            if(order.state == "未发货")
            {
                order.state = "已发货";
                ShowManager.sendShop(order,DAOFactor.commodities.FirstOrDefault(T => T.name == order.commoidty));
                init();
            }
            else
            {
                MessageBox.Show("请勿重复发货");
            }
        }

        private void CreateListViewButton(ListView listView, string what)
        {
            for (int i = 0; i < listView.Items.Count; i++)
            {
                Button button = new Button
                {
                    Tag = int.Parse(listView.Items[i].SubItems[1].Text)
                };
                int buttonWidth = 70;
                int buttonHeight = listView.GetItemRect(i).Height;
                if (what.Equals("删除"))
                {
                    button.Text = what;
                    button.Click += DeleteClick;
                    button.SetBounds(70, listView.GetItemRect(i).Y, buttonWidth, buttonHeight);
                }
                if (what.Equals("编辑"))
                {
                    button.Text = what;
                    button.Click += editClick;
                    button.SetBounds(0, listView.GetItemRect(i).Y, buttonWidth, buttonHeight);
                }
                if (what.Equals("发货"))
                {
                    button.Text = what;
                    button.Click += sendClick;
                    button.SetBounds(0, listView.GetItemRect(i).Y, buttonWidth, buttonHeight);
                }

                listView.Controls.Add(button);
            }
        }


        public System.Windows.Forms.ListView CreateListView<T>(HashSet<T> objList, T obj,Control control)
        {
            System.Windows.Forms.ListView listView = new System.Windows.Forms.ListView();
            listView.View = System.Windows.Forms.View.Details;
            listView.Parent = control;
            listView.Size = listView.Parent.Size;
            PropertyInfo[] properties = obj.GetType().GetProperties();
            listView.Columns.Add("属性");
            foreach (PropertyInfo property in properties)
            {
                listView.Columns.Add(property.Name);
            }
            foreach (object item in objList)
            {
                ListViewItem listItem = new ListViewItem();

                foreach (PropertyInfo property in properties)
                {
                    object value = property.GetValue(item);
                    listItem.SubItems.Add(value != null ? value.ToString() : "");
                }
                listView.Items.Add(listItem);
            }
            for (int i = 0; i < listView.Columns.Count; i++)
            {
                listView.Columns[i].Width = 150; // 设置为所需的宽度
            }
            ColumnHeader buttonColumn = new ColumnHeader();
            buttonColumn.Text = "操作";
            listView.Columns.Add(buttonColumn);
            CreateListViewButton(listView, "删除");
            if (obj is EleCommodity)
            {
                CreateListViewButton(listView, "编辑");
            }
            if(obj is EleOrder)
            {
                CreateListViewButton(listView, "发货");
            }
            listView.Location = new Point(0, 0);
            listView.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            listView.View = View.Details;
            listView.GridLines = true;
            return listView;
        }
        private void freshDictionary(ListView listView,string key)
        {
            var a = keys.FirstOrDefault(T => T.Value == key).Key;
            if (a != null)
            {
                keys.Remove(keys.FirstOrDefault(T => T.Value == key).Key);
            }
            keys.Add(listView, key);
        }

        public static UIControl CreateBarChartControl(HashSet<EleBooks> books)
        {
            if (books == null || books.Count == 0)
            {
                return null;
            }
            UIBarChart chart = new UIBarChart();
            chart.ChartStyleType = UIChartStyleType.Default;
            // 创建柱状图控件
            UIBarOption metaChart = new UIBarOption();
            // 配置标题
            metaChart.Title = new UITitle();
            // 主标题
            metaChart.Title.Text = "收支表一览";
            // 副标题
            metaChart.Title.SubText = "BarChart";

            // 设置图例
            metaChart.Legend = new UILegend();
            // 图例水平布局
            metaChart.Legend.Orient = UIOrient.Horizontal;
            metaChart.Legend.Top = UITopAlignment.Top;
            metaChart.Legend.Left = UILeftAlignment.Left;
            // 两个图例分别是Bar1和Bar2
            metaChart.Legend.AddData("入账");
            metaChart.Legend.AddData("出账");

            // 设置系列
            UIBarSeries seriesB = new UIBarSeries();
            UIBarSeries seriesA = new UIBarSeries();
            seriesA.Name = "入账";
            seriesB.Name = "出账";
            foreach (EleBooks eleBooks in DAOFactor.books)
            {
                seriesA.AddData((double)eleBooks.recorded);
                seriesB.AddData((double)eleBooks.billing);
                metaChart.XAxis.Data.Add("第"+eleBooks.quarter.ToString()+"季度");
            }

            metaChart.Series.Add(seriesA);
            metaChart.Series.Add(seriesB);

            // 辅助ToolTip是否可见
            metaChart.ToolTip.Visible = true;
            // Y轴的刻度
            metaChart.YAxis.Scale = true;
            // XY轴的单位
            metaChart.XAxis.Name = "季度";
            metaChart.YAxis.Name = "金额";
            // 标记处上下限（数值超过了也没事）
            metaChart.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Red, Name = "上限", Value = 5000 });
            metaChart.YAxisScaleLines.Add(new UIScaleLine() { Color = Color.Gold, Name = "下限", Value = 5000 });
            chart.SetOption(metaChart);
            return chart;
        }

        private Panel CreateSet(Control control)
        {
            Panel panel = new Panel
            {
                Parent = control
            };
            UserSet userSet = new UserSet();
            userSet.Location = new Point(0, 0);
            panel.Size = new Size(861, 507);
            userSet.Parent = panel;
            panel.Controls.Add(userSet);
            userSet.Size = userSet.Parent.Size;
            panel.Anchor = AnchorStyles.Right | AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            return panel;
        }
        private UIPanel CreateDataMap()
        {
            UIPanel uIPanel = new UIPanel();
            uIPanel.Size = new Size(861, 507);
            var A = CreateBarChartControl(DAOFactor.books);
            if(A != null)
            {
                A.Parent = uIPanel;
                uIPanel.Controls.Add(A);
                A.Size = uIPanel.Size;
            }
            uIPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            return uIPanel;
        }

        private void setAnchor(List<Control> controls)
        {
            foreach(Control control in controls)
            {
                control.Size = new Size(1700,900);
                control.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            }
        }
        public void init()
        {
            Aside.ClearAll();
            uiButton2.Hide();
            uiButton4.Hide();
            MainTabControl.Controls.Clear();
            UIPage page = AddPage(new UIPage(), 1);
            TreeNode parPage = Aside.CreateNode("仓储管理", 1);
            UIPage page2 = AddPage(new UIPage(), 2);
            TreeNode pricePage = Aside.CreateChildNode(parPage, "库存", 2);
            UIPage page3 = AddPage(new UIPage(), 3);
            TreeNode faPage = Aside.CreateChildNode(parPage, "订单", 3);
            UIPage page4 = AddPage(new UIPage(), 4);
            TreeNode zhPage = Aside.CreateChildNode(parPage, "收支", 4);
            UIPage page5 = AddPage(new UIPage(), 5);
            TreeNode daPage = Aside.CreateNode("数据统计", 5);
            UIPage page6 = AddPage(new UIPage(), 6);
            TreeNode setPage = Aside.CreateNode("平台设置", 6);
            setAnchor(new List<Control> { page, page2, page3, page4, page5, page6 });
            ListView CommodityListView = CreateListView(DAOFactor.commodities, new EleCommodity(),page2);
            ListView OrderListView = CreateListView(DAOFactor.orders, new EleOrder(),page3);
            ListView BookListView = CreateListView(DAOFactor.books, new EleBooks(),page4);
            page2.Controls.Add(CommodityListView);
            page3.Controls.Add(OrderListView);
            page4.Controls.Add(BookListView);
            page5.Controls.Add(CreateDataMap());
            page6.Controls.Add(CreateSet(page6));
            freshDictionary(CommodityListView,"商品");
            freshDictionary(OrderListView, "订单");
            freshDictionary(BookListView,"账单");
            uiButton2.Tag = CommodityListView;
            if (iffirst)
            {
                Aside.SelectedNode = pricePage;
                Aside.SelectPage(2);
                i = 2;
            }
            else
            {
                Aside.SelectedNode = index;
                Aside.SelectPage(index.Index);
            }
        }
        //订单管理，库存清单，账务记账，数据统计，平台设置
        private void Aside_MenuItemClick(TreeNode node, NavMenuItem item, int pageIndex)
        {
            index = node;
            i = pageIndex;
            uiButton2.Hide();
            uiButton4.Hide();
            uiButton5.Hide();
            uiTextBox1.Hide();
            uiButton1.Hide();
            if (node.Text == "库存")
            {
                uiButton2.Show();
                uiButton5.Show();
                uiTextBox1.Show();
                uiButton1.Show();
            }
            if(node.Text == "订单")
            {
                uiButton4.Show();
                uiTextBox1.Show();
            }
            if(node.Text == "收支")
            {
                uiButton4.Show();
                uiTextBox1.Show();
            }
        }

        private void uiLabel1_Click(object sender, System.EventArgs e)
        {

        }

        private HashSet<T> GetTs<T>(HashSet<T> values,string name) where T : EleDAO
        {
            HashSet<T> ts = new HashSet<T>();
            if(values is HashSet<EleCommodity> v)
            {
                ts = v.Where(Ts => Ts.name.Contains(name)).OfType<T>().ToHashSet();
            }
            if(values is HashSet<EleOrder> o)
            {
                ts = o.Where(Ts => Ts.commoidty.Contains(name)).OfType<T>().ToHashSet();
            }
            if (values is HashSet<EleBooks> b)
            {
                ts = b.Where(Ts => Ts.quarter == Verify.sTi(name)).OfType<T>().ToHashSet();
            }
            return ts;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            UIPage uIPage = MainTabControl.GetPage(i);
            var c = uIPage.Controls;
            c.Clear();
            if(index.Text == "库存")
            {
                var h = CreateListView(GetTs(DAOFactor.commodities, uiTextBox1.Text), new EleCommodity(),uIPage);
                c.Add(h);
            }
            if(index.Text == "收支")
            {
                var h = CreateListView(GetTs(DAOFactor.books, uiTextBox1.Text), new EleBooks(), uIPage);
                c.Add(h);

            }
            if(index.Text == "订单")
            {
                var h = CreateListView(GetTs(DAOFactor.orders, uiTextBox1.Text), new EleOrder(), uIPage);
                c.Add(h);
            }
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            UIButton button = (UIButton)sender;
            ListView listView = (ListView)button.Tag;
            EleCommodity eleCommodity = new EleCommodity();
            DAOFactor.commodities.Add(eleCommodity);
            EditFrom editFrom = new EditFrom(eleCommodity, "新建");
            editFrom.Tag = this;
            editFrom.Show();
        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            init();
        }

        private void uiButton4_Click(object sender, EventArgs e)
        {
           var a = DAOFactor.orders.Where(T => T.state == "未发货").ToHashSet();
            if(a.Count < 1)
            {
                MessageBox.Show("没有需要发货的订单");
            }
            foreach(var item in a)
            {
                EleCommodity ele = DAOFactor.commodities.Where(T => T.name == item.commoidty).First();
                if(ele != null && ele.num > item.num)
                {
                    ShowManager.sendShop(item, ele);
                }
                else
                {
                    MessageBox.Show("货物短缺");
                }
            }
            
        }

        private void FactorForm_SizeChanged(object sender, EventArgs e)
        {
            num.Width = this.Size.Width/oldSize.Width;
            num.Height = this.Size.Height/oldSize.Height;
            oldSize = this.Size;
            init();
        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel 文件 (*.xlsx)|*.xlsx";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = false;

            DialogResult result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                TableControl.RoadTFormTable(DAOFactor.commodities,new EleCommodity(),selectedFile);
                MessageBox.Show("文件路径为"+selectedFile);
            }
        }
    }
}
