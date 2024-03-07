using Electric.Class.Factory;
using Electric.Class.Utils;
using Sunny.UI;
using System;
using System.Windows.Forms;

namespace Electric.Class.Screen
{
    public partial class Logfin : UIForm
    {
        public Logfin()
        {
            InitializeComponent();
            DefaultSet.init();
            textBox2.PasswordChar = '*';
        }

        private void uiButton1_Click(object sender, EventArgs e)
        {
            if(Verify.verifyUser(textBox1.Text, textBox2.Text))
            {
                DAOFactor.user = Verify.getUser(textBox1.Text, textBox2.Text);
                FactorForm factorForm = new FactorForm(this);
                factorForm.Parent = null;
                if (uiRadioButton1.Checked)
                {
                    DAOFactor.ifDataBase = true;
                    DAOControl.Init();
                }
                else
                {
                    DAOFactor.ifDataBase = false;
                }
                factorForm.Show();
            }
            else
            {
                MessageBox.Show("用户名或者密码错误");
            }
            this.Hide();
        }

        private void uiButton2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void uiLabel3_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
