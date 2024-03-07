using Sunny.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;

namespace Electric.Class.Screen
{
    public partial class WebFrom : UIForm
    {

        public WebFrom()
        {
            this.WindowState = FormWindowState.Maximized;
            InitializeComponent();
            InitializeComponents();
            LoadTaobaoHomePage();
        }

        private void InitializeComponents()
        {
            this.Text = "访问支付宝主页";

            webBrowser.Dock = DockStyle.Fill;
            webBrowser.ScriptErrorsSuppressed = true; // 忽略脚本错误

            webBrowser.Navigating += WebBrowser_Navigating;
            webBrowser.DocumentCompleted += WebBrowser_DocumentCompleted;

            this.Controls.Add(webBrowser);
        }

        private void LoadTaobaoHomePage()
        {
            webBrowser.Navigate("https://auth.alipay.com/login/index.htm");
        }

        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            // 当从网页退出时弹窗提示
            if (e.Url.AbsolutePath == "blank")
            {
                MessageBox.Show("您正在离开支付宝主页！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            // 注册 JavaScript 事件处理函数，用于当用户点击页面链接时触发
            webBrowser.Document.Click += Document_Click;
        }

        // 当用户点击页面链接时触发
        private void Document_Click(object sender, HtmlElementEventArgs e)
        {
            HtmlElement targetElement = webBrowser.Document.GetElementFromPoint(e.ClientMousePosition);
            if (targetElement != null && !string.IsNullOrEmpty(targetElement.GetAttribute("href")))
            {
                string url = targetElement.GetAttribute("href");

                // 在这里处理点击链接的操作，比如加载新页面等
                // 这里只是一个示例，当点击链接时显示链接的地址
                MessageBox.Show($"您点击了一个链接：{url}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            if (e.CloseReason == CloseReason.UserClosing)
            {
                MessageBox.Show("获取支付宝账户失败","提示");
            }
        }
    }
}
