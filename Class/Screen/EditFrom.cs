using Electric.Class.API;
using Electric.Class.DAO;
using Electric.Class.Factory;
using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Electric.Class.Screen
{
    public partial class EditFrom : UIForm
    {
        public EleCommodity commodity;
        private List<string> states = new List<string>() {"已上架","未上架"};
        private List<string> plateFrom = new List<string> { "淘宝", "拼多多", "天猫", "闲鱼" };

        public string eletype {  get; set; }
        public EditFrom(EleCommodity ele,string eletype)
        {
            InitializeComponent();
            commodity = ele;
            if(eletype == "新建")
            {
                ele.id = DAOFactor.commodities.Count + 1;
            }
            this.eletype = eletype;
            uiComboBox1.DataSource = states;
            uiComboBox2.DataSource = plateFrom;
            uiComboBox1.SelectedItem = ele.state;
            uiComboBox2.SelectedItem = ele.platform;
            uiTextBox1.Text = ele.name;
            uiLabel1.Text = "ID:" + ele.id.ToString();
            numericUpDown1.Value = ele.price;
            numericUpDown2.Value = ele.num;
            richTextBox1.Text = ele.description;
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            commodity.name = uiTextBox1.Text;
            commodity.state = uiComboBox1.SelectedItem.ToString();
            commodity.price = numericUpDown1.Value;
            commodity.description = richTextBox1.Text;
            commodity.platform = uiComboBox2.SelectedItem.ToString();
            commodity.num = (int)numericUpDown2.Value;
            if(eletype == "新建")
            {
                commodity.id = DAOFactor.commodities.Count + 1;
                DAOControl.AddDAO(commodity);
            }
            if(eletype == "修改")
            {
                DAOControl.UpdateDAO(commodity);
            }
            FactorForm a = (FactorForm)this.Tag;
            a.init(); 
            if(commodity.state == "已上架")
            {
                ShowManager.Shelves(commodity);
            }
            this.Close();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void uiComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
