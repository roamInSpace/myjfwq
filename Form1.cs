using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using mshtml;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

namespace myjfwq
{
    public partial class Form1 : Form
    {
        string jsFunctionNext = "StartPlay";
        MyWebBrowser webBrowser1 = new MyWebBrowser();
        string myjHTML = "";
        private void Start()
        {
            string JSCode = "HMF = new Array();";
            string txtPath = Application.StartupPath + "\\" + this.Text + ".txt";
            if (File.Exists(txtPath))
            {
                StreamReader sr = new StreamReader(txtPath, System.Text.Encoding.Default);
                JSCode += sr.ReadToEnd();
                sr.Close();
            }
            txtPath = Application.StartupPath + "\\basics.txt";
            if (File.Exists(txtPath))
            {
                StreamReader sr = new StreamReader(txtPath, System.Text.Encoding.Default);
                string sTemporary = sr.ReadToEnd();
                JSCode += sTemporary;
                sr.Close();
            }
            HtmlElement head = webBrowser1.Document.GetElementsByTagName("html")[0];
            //创建script标签
            HtmlElement scriptEl = webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
            //给script标签加js内容

            element.text = JSCode;
            //将script标签添加到head标签中
            head.AppendChild(scriptEl);
            //执行js代码'
            webBrowser1.Document.InvokeScript(jsFunctionNext);
            jsFunctionNext = "StartPlay";
            timer1.Enabled = true;
        }

        //主窗口
        public Form1()
        {
            InitializeComponent();
        }

        //新标签页
        void Form1_NewWindow2(ref object ppDisp, ref bool Cancel)
        {
            WebBrowser webBrowserX = new WebBrowser();
            webBrowserX.Dock = DockStyle.Fill;
            webBrowserX.ScriptErrorsSuppressed = true;
            webBrowserX.Navigated += new System.Windows.Forms.WebBrowserNavigatedEventHandler(this.WebBrowser_Navigated);
            (webBrowserX.ActiveXInstance as SHDocVw.WebBrowser).NewWindow2 += new SHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(Form1_NewWindow2);
            ppDisp = webBrowserX.ActiveXInstance;
            tabControl1.TabPages.Add("加载中");
            tabControl1.TabPages[tabControl1.TabPages.Count - 1].Controls.Add(webBrowserX);
            if (buttonStart.Visible == false)
            {
                webBrowserX.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
            }
            else
            {
                tabControl1.SelectedIndex = tabControl1.TabPages.Count - 1;
            }
        }
        private void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            ((WebBrowser)sender).Parent.Text = ((WebBrowser)sender).Url.ToString();
        }

        //自动竞技场
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            timer3.Enabled = false;
            string JSCode = "";
            string txtPath = Application.StartupPath + "\\" + this.Text + ".txt";
            if (File.Exists(txtPath))
            {
                StreamReader sr = new StreamReader(txtPath, System.Text.Encoding.Default);
                JSCode = sr.ReadToEnd();
                sr.Close();
            }
            HtmlElement head = ((WebBrowser)sender).Document.GetElementsByTagName("html")[0];
            //创建script标签
            HtmlElement scriptEl = ((WebBrowser)sender).Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
            element.text = JSCode;
            //将script标签添加到head标签中
            head.AppendChild(scriptEl);

            if (((WebBrowser)sender).Url.ToString().Contains("shop"))
            {
                IHTMLWindow2 win = (IHTMLWindow2)(((WebBrowser)sender).Document.Window.DomWindow);
                string s = @"function confirm() {";
                s += @"return true;";
                s += @"}";
                win.execScript(s, "javascript");
                ((WebBrowser)sender).Document.InvokeScript("duihuan");
            }
            else
            {
                ((WebBrowser)sender).Document.InvokeScript("jiancha");
            }
            timer3.Enabled = true;
        }
        private void Timer3_Tick(object sender, EventArgs e)
        {
            while (tabControl1.TabPages.Count != 1)
            {
                tabControl1.TabPages[1].Controls[0].Dispose();
                tabControl1.TabPages.RemoveAt(1);
            }
        }

        //开始挂机
        private void ButtonStart_Click(object sender, EventArgs e)
        {
            buttonStart.Visible = false;
            Start();
        }
        //停止挂机
        private void ButtonStop_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            buttonStart.Visible = true;
            webBrowser1.Navigate(myjHTML);
        }

        //双击关闭标签页
        private void TabControl1_DoubleClick(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex > 0)
            {
                tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0].Dispose();
                tabControl1.TabPages.RemoveAt(tabControl1.SelectedIndex);
            }
        }

        //页面加载
        private void Form1_Load(object sender, EventArgs e)
        {
            StreamReader sr = new StreamReader(Application.StartupPath + "\\logInRecord.txt", System.Text.Encoding.Default);
            myjHTML = sr.ReadLine();
            string logInLast = sr.ReadLine();
            List<string> logInRecord = new List<string>();
            string ss = sr.ReadLine();
            while (ss != null)
            {
                logInRecord.Add(ss);
                ss = sr.ReadLine();
            }
            sr.Close();
            for (int i = 0; i < logInRecord.ToArray().Length; ++i)
            {
                comboBox1.Items.Add(logInRecord[i]);
            }
            if (int.Parse(logInLast) < logInRecord.ToArray().Length - 1)
            {
                comboBox1.SelectedIndex = int.Parse(logInLast) + 1;
            }
            else
            {
                comboBox1.SelectedIndex = 0;
            }
            textBox4.Text = myjHTML;
            this.tabPage1.Controls.Add(webBrowser1);
            webBrowser1.Dock = DockStyle.Fill;
            webBrowser1.ScriptErrorsSuppressed = true;
            webBrowser1.AllowWebBrowserDrop = false;
            webBrowser1.Visible = true;
            webBrowser1.Navigate(myjHTML);
            (this.webBrowser1.ActiveXInstance as SHDocVw.WebBrowser).NewWindow2 += new SHDocVw.DWebBrowserEvents2_NewWindow2EventHandler(Form1_NewWindow2);
        }

        //登录
        private void ButtonLogin_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string password = textBox2.Text;
            string checkcode = textBox3.Text;
            HtmlElement head = this.webBrowser1.Document.GetElementsByTagName("html")[0];
            //创建script标签
            HtmlElement scriptEl = this.webBrowser1.Document.CreateElement("script");
            IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
            element.text = "function logIn() {var main=document.frames[0];var username=main.document.getElementById(\"user_name\");var password=main.document.getElementById(\"password\");var checkcode=main.document.getElementById(\"checkcode\");username.innerText=\"" + username + "\";password.innerText=\"" + password + "\";checkcode.innerText=\"" + checkcode + "\";main.formSubmit();}";
            //将script标签添加到head标签中
            head.AppendChild(scriptEl);
            //执行js代码'
            this.webBrowser1.Document.InvokeScript("logIn");
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\logInRecord.txt", false, System.Text.Encoding.Default);
            sw.WriteLine(myjHTML);
            sw.WriteLine(comboBox1.SelectedIndex.ToString());
            for (int i = 0; i < comboBox1.Items.Count; ++i)
            {
                sw.WriteLine(comboBox1.GetItemText(comboBox1.Items[i]));
            }
            sw.Close();
            this.Text = textBox1.Text;
            tabControl1.TabPages[0].Text = textBox1.Text;
            label1.Dispose();
            label2.Dispose();
            label3.Dispose();
            buttonLogin.Dispose();
            buttonSave.Dispose();
            textBox1.Dispose();
            textBox2.Dispose();
            textBox3.Dispose();
            comboBox1.Dispose();
            button1.Visible = false;
            textBox4.Visible = false;
        }

        //保存
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            string ss = textBox1.Text + " " + textBox2.Text;
            comboBox1.Items.Add(ss);
            StreamReader sr = new StreamReader(Application.StartupPath + "\\logInRecord.txt", System.Text.Encoding.Default);
            sr.ReadLine();
            int logInLast = int.Parse(sr.ReadLine());
            sr.Close();
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\logInRecord.txt", false, System.Text.Encoding.Default);
            sw.WriteLine(myjHTML);
            sw.WriteLine(logInLast.ToString());
            for (int i = 0; i < comboBox1.Items.Count; ++i)
            {
                sw.WriteLine(comboBox1.GetItemText(comboBox1.Items[i]));
            }
            sw.Close();
            comboBox1.SelectedIndex = comboBox1.Items.Count - 1;
        }

        //红色显示上一次登录账号
        private void ComboBox1_DrawItem(object sender, DrawItemEventArgs e)
        {
            StreamReader sr = new StreamReader(Application.StartupPath + "\\logInRecord.txt", System.Text.Encoding.Default);
            sr.ReadLine();
            string logInLast = sr.ReadLine();
            sr.Close();
            Pen[] redColor = new Pen[comboBox1.Items.Count];
            for (int i = 0; i < comboBox1.Items.Count; ++i)
            {
                redColor[i] = new Pen(Color.Black);
            }
            redColor[int.Parse(logInLast)] = new Pen(Color.Red);

            e.Graphics.DrawString((string)comboBox1.Items[e.Index], this.Font, redColor[e.Index].Brush, e.Bounds);
        }

        //选择账号
        private void ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string[] ss = comboBox1.Text.Split(' ');
            textBox1.Text = ss[0];
            textBox2.Text = ss[1];
        }

        //检测刷新
        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            if (webBrowser1.Url.ToString() != myjHTML)
            {
                webBrowser1.Navigate(myjHTML);
                timer2.Enabled = true;
            }
            else
            {
                jsFunctionNext = (string)(webBrowser1.Document.InvokeScript("Shuaxin"));
                if (jsFunctionNext != "no")
                {
                    if (jsFunctionNext == null)
                    {
                        jsFunctionNext = "StartPlay";
                    }
                    webBrowser1.Navigate(myjHTML);
                    timer2.Enabled = true;
                }
                else
                {
                    jsFunctionNext = "StartPlay";
                    timer1.Enabled = true;
                }
            }
        }
        private void Timer2_Tick(object sender, EventArgs e)
        {
            timer2.Enabled = false;
            Start();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            myjHTML = textBox4.Text;
            webBrowser1.Navigate(myjHTML);
            StreamReader sr = new StreamReader(Application.StartupPath + "\\logInRecord.txt", System.Text.Encoding.Default);
            sr.ReadLine();
            int logInLast = int.Parse(sr.ReadLine());
            sr.Close();
            StreamWriter sw = new StreamWriter(Application.StartupPath + "\\logInRecord.txt", false, System.Text.Encoding.Default);
            sw.WriteLine(myjHTML);
            sw.WriteLine(logInLast.ToString());
            for (int i = 0; i < comboBox1.Items.Count; ++i)
            {
                sw.WriteLine(comboBox1.GetItemText(comboBox1.Items[i]));
            }
            sw.Close();
        }
    }

    public class MyWebBrowser : WebBrowser
    {
        #region ExtendedWebBrowserSite
        class ExtendedWebBrowserSite : WebBrowser.WebBrowserSite, UnsafeNativeMethods.IDocHostShowUI
        {
            public ExtendedWebBrowserSite(WebBrowser host)
              : base(host)
            {
            }
            void UnsafeNativeMethods.IDocHostShowUI.ShowMessage(ref UnsafeNativeMethods._RemotableHandle hwnd, string lpstrText, string lpstrCaption, uint dwType, string lpstrHelpFile, uint dwHelpContext, out int plResult)
            {
                plResult = 0;
                return;
            }

            void UnsafeNativeMethods.IDocHostShowUI.ShowHelp(ref UnsafeNativeMethods._RemotableHandle hwnd, string pszHelpFile, uint uCommand, uint dwData, UnsafeNativeMethods.tagPOINT ptMouse, object pDispatchObjectHit)
            {
                return;
            }
        }

        protected override WebBrowserSiteBase CreateWebBrowserSiteBase()
        {
            return new ExtendedWebBrowserSite(this);
        }
        #endregion
    }

    public class UnsafeNativeMethods
    {
        #region IDocHostShowUI
        [StructLayout(LayoutKind.Explicit, Pack = 4)]
        public struct __MIDL_IWinTypes_0009
        {
            // Fields 
            [FieldOffset(0)]
            public int hInproc;
            [FieldOffset(0)]
            public int hRemote;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct _RemotableHandle
        {
            public int fContext;
            public __MIDL_IWinTypes_0009 u;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 4)]
        public struct tagPOINT
        {
            public int x;
            public int y;
        }

        [ComImport, Guid("C4D244B0-D43E-11CF-893B-00AA00BDCE1A"), InterfaceType((short)1)]
        public interface IDocHostShowUI
        {
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void ShowMessage([In, ComAliasName("ExtendedWebBrowser2.UnsafeNativeMethods.wireHWND")] ref _RemotableHandle hwnd, [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrText, [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrCaption, [In] uint dwType, [In, MarshalAs(UnmanagedType.LPWStr)] string lpstrHelpFile, [In] uint dwHelpContext, [ComAliasName("ExtendedWebBrowser2.UnsafeNativeMethods.LONG_PTR")] out int plResult);
            [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
            void ShowHelp([In, ComAliasName("ExtendedWebBrowser2.UnsafeNativeMethods.wireHWND")] ref _RemotableHandle hwnd, [In, MarshalAs(UnmanagedType.LPWStr)] string pszHelpFile, [In] uint uCommand, [In] uint dwData, [In] tagPOINT ptMouse, [Out, MarshalAs(UnmanagedType.IDispatch)] object pDispatchObjectHit);
        }
        #endregion
    }
}