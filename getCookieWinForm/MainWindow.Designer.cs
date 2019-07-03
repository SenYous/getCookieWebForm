using System;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace getCookieWinForm
{
    partial class MainWindow
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        private string WebCookie;
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.LoginPageWebBrowser = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();//悬挂式布局
            //初始化WebBrowser控件
            this.LoginPageWebBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LoginPageWebBrowser.Location = new System.Drawing.Point(0, 0);
            this.LoginPageWebBrowser.MinimumSize = new System.Drawing.Size(20, 20);
            this.LoginPageWebBrowser.Name = "LoginPageWebBrowser";
            this.LoginPageWebBrowser.ScriptErrorsSuppressed = true;
            this.LoginPageWebBrowser.Size = new System.Drawing.Size(1223, 575);
            this.LoginPageWebBrowser.TabIndex = 0;
            this.LoginPageWebBrowser.Url = new System.Uri("https://login.tmall.com/?redirectURL=https%3A%2F%2Fwww.tmall.com%2F", System.UriKind.Absolute);
            this.LoginPageWebBrowser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.LoginPageWebBrowser_DocumentCompleted);

            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 575);
            this.Controls.Add(this.LoginPageWebBrowser);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "网站";
            this.ResumeLayout(false);
        }

        // 获取当前WebBrowser登录后的Cookie字符串
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool InternetGetCookieEx(string pchUrl, string pchCookieName, StringBuilder pchCookieData, ref System.UInt32 pcchCookieData, int dwFlags, IntPtr lpReserved);

        /// <summary>
        /// 获取某 URL 的 Cookie 返回字符串
        /// </summary>
        /// <param name="Url"></param>
        /// <returns></returns>
        private static string GetCookieString(string Url)
        {
            uint Datasize = 1024;
            StringBuilder CookieData = new StringBuilder((int)Datasize);
            //CookieData = new StringBuilder((int)Datasize);
            if (!InternetGetCookieEx(Url, null, CookieData, ref Datasize, 0x2000, IntPtr.Zero))
            {
                if (Datasize < 0)
                    return null;
                CookieData = new StringBuilder((int)Datasize);
                if (!InternetGetCookieEx(Url, null, CookieData, ref Datasize, 0x00002000, IntPtr.Zero))
                    return null;
            }
            return CookieData.ToString();
        }
        private void LoginPageWebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser web = (WebBrowser)sender;
            if (web.ReadyState == WebBrowserReadyState.Complete)
            {
                Console.WriteLine("trun");
            }
            // e.Url 是当前加载的页面 URL
            if (e.Url.ToString().Contains("login"))
            {
                // 自动填充
                try
                {

                  //var  htmlDoc = this.LoginPageWebBrowser.Document.Window.Frames[0].Document;
                    var userName = this.LoginPageWebBrowser.Document.GetElementById("TPL_username_1");
                    if (userName != null)
                        userName.SetAttribute("value", "");
                    var passWord = this.LoginPageWebBrowser.Document.GetElementById("TPL_password_1");
                    if (passWord != null)
                        passWord.SetAttribute("value", "123456");

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
            // 如果跳转到了 URL包含 https://www.taobao.com 的地方
            else if (e.Url.ToString().Contains("https://subway.simba.taobao.com"))
            {
                string Cookie = GetCookieString(e.Url.ToString());
                MessageBox.Show(Cookie, "Cookie获取成功", MessageBoxButtons.OK, MessageBoxIcon.None);
                // 然后你可以执行其他操作...
                // 例如：
                this.WebCookie = Cookie;
                // 假如这个窗口有爸爸 →_→
                this.Close(); // 必须随即关闭此窗口停止执行啦
            }
            // 否则...
            else
            {
                //MessageBox.Show("登录失败？！", "Cookie获取失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

        private System.Windows.Forms.WebBrowser LoginPageWebBrowser;//WebBrowser控件
    }
}

