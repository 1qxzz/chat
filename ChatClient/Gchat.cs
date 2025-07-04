using Newtonsoft.Json;
using System.Collections;
using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class Gchat : Form
    {
        List<string>IOpath = new List<string>();
        List<string> messages = new List<string>();
        string self = null;
        TcpClient tcpClient;
        NetworkStream stream;
        //NetworkStream _stream;
        private CancellationTokenSource _cts;

        public Gchat(List<string> lsmessage,List<string>IOpath,TcpClient client)
        {
            InitializeComponent();
            this.tcpClient = client;
            stream = tcpClient.GetStream();
            foreach (string s in lsmessage)
            {
                messages.Add(s);
            }
            this.IOpath = IOpath;
            StartReading();
        }
        public Gchat(string SelfName, TcpClient client)
        {
            try
            {
                InitializeComponent();
                self = SelfName;
                this.tcpClient = client;
                stream = tcpClient.GetStream();
                label2.Text = self;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"未连接上服务器,{ex.Message}");
            }

        }


        //连接服务器
        private void Form1_Load(object sender, EventArgs e)
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

        //发送消息
        private void SendMessageAsync(ChatMessage message)
        {
            try
            {
                string json = JsonConvert.SerializeObject(message);
                byte[] data = Encoding.UTF8.GetBytes(json);
                byte[] length = BitConverter.GetBytes(data.Length);
                stream.WriteAsync(length, 0, 4);
                stream.WriteAsync(data, 0, data.Length);
            }
            catch (Exception ex)
            {
                Invoke((Action)(() => listBox1.Items.Add(($"发送失败:{message.Timestamp}          {message.Content}"))));
            }

        }
        //点击发送按钮
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) return;
            try
            {
                ChatMessage chatMessage = new ChatMessage()
                {
                    Type = "G",
                    Sender = self,
                    Receiver = "All",
                    Content = textBox1.Text,
                    Timestamp = DateTime.Now
                };
                SendMessageAsync(chatMessage);
                textBox1.Clear();
            }

            catch (Exception ex)
            {
                MessageBox.Show($"发送失败: {ex.Message}");
                //Disconnect();
            }
        }
        //读取集合里存储的消息 并更新UI
        public void StartReading()
        {
            foreach (var a in messages)
            {
                listBox1.Items.Add(a);
            }
        }
        public async Task PrintToListBox(string mag)
        {
            Invoke((Action)(() => listBox1.Items.Add(mag)));
        }
        //发送文件到服务器
        private void SendIO(ChatIO chatIO)
        {
            //文件先编译成json
            string jsonIO = JsonConvert.SerializeObject(chatIO);
            ChatMessage chatMessage = new ChatMessage()
            {
                Type = "G-IO",
                Sender = self,
                Receiver = "All",
                Content = jsonIO,
                Timestamp = DateTime.Now
            };
            //消息再编译成json，并发送
            string json = JsonConvert.SerializeObject(chatMessage);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(data.Length);
            stream.WriteAsync(length, 0, 4);
            stream.WriteAsync(data, 0, data.Length);
        }
        //发送文件
        private void button2_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "选择要发送的文件";
                openFileDialog.Filter = "所有文件 (*.*)|*.*";
                openFileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    try
                    {
                        byte[] bytes = File.ReadAllBytes(filePath); // 读取文件内容，确保文件存在
                        ChatIO chatIO = new ChatIO()
                        {
                            FileSender = self,
                            FileReceiver = "All",
                            FileName = Path.GetFileName(filePath),
                            FileSize = bytes.Length,
                            FileType = Path.GetExtension(filePath),
                            FileContentBase64 = Convert.ToBase64String(bytes),
                            Timestamp = DateTime.Now,
                        };
                        SendIO(chatIO);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"文件处理时出错: {ex.Message}");
                        return;
                    }

                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string tempPath = $@"D:\ChatIO文件存储\{listBox1.SelectedItem}";
            foreach(var i in IOpath)
            {
                if (tempPath == i)
                {
                    string filePath = i.ToString();
                    try
                    {
                        Process.Start("explorer", $"\"{filePath}\""); // 使用资源管理器打开
                    }//有些文件无法打开
                    catch(Exception ex2)
                    {
                        Process.Start(@"D:\ChatIO文件存储\");
                    }
                }
            }
        }
    }
}
