using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Collections.Concurrent;
using System.Net.Http.Headers;
using SSharing.Frameworks.Common.Extends;
using System.Net;

namespace ConcurrentTools
{
    public partial class Form1 : Form
    {
        private string Url = "http://www.gtb.life/index/index/lingqu.html";
        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;//fix从不是创建控件的线程访问它

            this.txtUrl.Text = "http://www.5hkok7.cn/user_ajax4.asp?tjnc=tjkjgm";
            this.txtBody.Text = "id=4";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var url = GetUrl();
            var headers = GetHeader();
            var body = GetBody();
            var comparssMethod = GetDecomparessMethod();

            var taskArr = GetTaskArr();
            for (int i = 0; i < taskArr.Length; i++)
            {
                taskArr[i] = HttpHelper.Post(url, headers, body, comparssMethod, AppendMsg);
            }
            Task.WhenAll(taskArr);
        }

        private void btnGet_Click(object sender, EventArgs e)
        {
            var url = GetUrl();
            var headers = GetHeader();
            var body = GetBody();
            var comparssMethod = GetDecomparessMethod();

            var taskArr = GetTaskArr();
            for (int i = 0; i < taskArr.Length; i++)
            {
                taskArr[i] = HttpHelper.Get(url, headers, comparssMethod, AppendMsg);
            }
            Task.WhenAll(taskArr);
        }

        private int GetQty()
        {
            return txtQty.Text.Trim().ToInt(500);
        }

        private Task[] GetTaskArr()
        {
            Task[] taskArr = new Task[GetQty()];
            return taskArr;
        }

        private string GetUrl()
        {
            return this.txtUrl.Text.Trim();
        }
        private Dictionary<string, string> GetHeader()
        {
            var dics = new Dictionary<string, string>();

            var headerStr = this.txtHeader.Text.Trim();
            var lines = headerStr.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line)) continue;

                var index = line.IndexOf(":");
                if (index < 0) continue;

                var key = line.Substring(0, index);

                var val = line.Substring(index + 1, line.Length - index - 1);
                dics[key] = val.Trim().TrimEnd(';');
            }
            return dics;
        }
        private string GetBody()
        {
            return this.txtBody.Text.Trim();
        }
        private DecompressionMethods GetDecomparessMethod()
        {
            return (DecompressionMethods)this.cmbCompress.SelectedIndex;
        }

        private void AppendMsg(string msg)
        {
            richTextBox1.AppendText(msg);
            richTextBox1.AppendText(Environment.NewLine);
        }
    }
}
