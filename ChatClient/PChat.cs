using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class PChat : Form
    {
        Dictionary<string, List<string>> userAndmessage = new Dictionary<string, List<string>>();
        List<string>IOpath = new List<string>();
        string self = null;
        string chooseuser = null;
        TcpClient tcpClient;
        NetworkStream stream;
        public PChat(string myselfname, string chooseUser, Dictionary<string, List<string>> uAndm,List<string>IOpath, TcpClient client)
        {
            InitializeComponent();
            this.tcpClient = client;
            stream = tcpClient.GetStream();
            self = myselfname;
            chooseuser = chooseUser;
            userAndmessage = uAndm;
            label2.Text = self;
            this.IOpath=IOpath;
            StartReading(chooseuser);

        }
        private void StartReading(string chooseuser)
        {
            if (userAndmessage.TryGetValue(chooseuser, out List<string> list))
            {
                foreach (var item in list)
                {
                    listBox1.Items.Add(item);
                }
            }
        }
        //发送按钮
        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;
            try
            {
                ChatMessage message = new ChatMessage()
                {
                    Type = "P",
                    Sender = self,
                    Receiver = chooseuser,
                    Content = textBox1.Text,
                    Timestamp = DateTime.Now
                };
                SendMessage(message);
                Invoke((Action)(() => listBox1.Items.Add($"{message.Timestamp}          {message.Sender}:{message.Content}")));
                textBox1.Clear();
            }
            catch (Exception ex)
            {
                Invoke((Action)(() => listBox1.Items.Add(($"发送失败: {ex.Message}"))));
            }

        }
        //发送消息
        private void SendMessage(ChatMessage message)
        {
            string json = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(data.Length);
            stream.WriteAsync(length, 0, 4);
            stream.WriteAsync(data, 0, data.Length);
        }
        //打印消息到控件
        public void ToListBox(string mag)
        {
            Invoke((Action)(() => listBox1.Items.Add(mag)));
        }
        private void PChat_Load(object sender, EventArgs e)
        {
            if (tcpClient.Connected)
            {
                return;
            }
            else
            {
                listBox1.Items.Add("我方离线：连接服务器失败");
            }
        }
        private void SendIO(ChatIO chatIO)
        {
            string json = JsonConvert.SerializeObject(chatIO);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(data.Length);
            stream.WriteAsync(length, 0, 4);
            stream.WriteAsync(data, 0, data.Length);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择要发送的文件";
                openFileDialog.Filter = "所有文件(*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        byte[] bytes = File.ReadAllBytes(filePath);
                        ChatIO chatIO = new ChatIO()
                        {
                            FileName = Path.GetFileName(filePath),
                            FileSize = bytes.Length,
                            FileType = Path.GetExtension(filePath),
                            FileContentBase64 = Convert.ToBase64String(bytes),
                            Timestamp = DateTime.Now
                        };
                        SendIO(chatIO);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("处理文件出错");
                        return;
                    }
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (var i in IOpath)
            {
                if ((string)listBox1.SelectedItem == i)
                {
                    string filePath = i.ToString();
                    Process.Start("explorer.exe", $"\"{filePath}\""); // 使用资源管理器打开
                }
            }
        }
    }
}
