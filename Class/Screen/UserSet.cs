using Electric.Class.Factory;
using Electric.Properties;
using Sunny.UI.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Electric.Class.Screen
{
    public partial class UserSet : UserControl
    {
        private static Process browserProcess;

        public UserSet()
        {
            InitializeComponent();
            this.Size = new Size(500, 400);
            init();
        }

        public void init()
        {
            uiLabel2.Text = DAOFactor.user.name;
            uiTextBox3.Text = DefaultSet.startReturn;
            uiTextBox1.Text = DefaultSet.endReturn;
            uiTextBox2.Text = DefaultSet.orderReturn;
            numericUpDown1.Value = 10;
            pictureBox1.Image = Resources._99675346_p0;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {

        }

        private void uiButton4_Click(object sender, EventArgs e)
        {

        }

        private void uiButton5_Click(object sender, EventArgs e)
        {
            DefaultSet.startReturn = uiTextBox3.Text;
            DefaultSet.endReturn = uiTextBox1.Text ;
            DefaultSet.orderReturn = uiTextBox2.Text;
            DefaultSet.save();

            MessageBox.Show("修改成功");
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {

        }

        private void uiButton3_Click(object sender, EventArgs e)
        {
            WebFrom webFrom = new WebFrom();
            webFrom.Show();
        }

        private static void BrowserProcess_Exited(object sender, EventArgs e)
        {
            MessageBox.Show("正在打开支付宝");
        }




    }
}
