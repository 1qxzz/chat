using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace ChatClient
{
    public partial class MInterface : Form
    {
        
        public static readonly Dictionary<string,List<string>> userAndmessage = new Dictionary<string,List< string>>();//私聊的消息存储
        public static readonly Dictionary<string, byte[]> GsIO = new Dictionary<string,byte[]>();//文件信息 文件名＋后缀  内容
        List<string> IOpath = new List<string>();
        ArrayList PchatIOs = new ArrayList();
        List<string> users=new List<string>();
        List<PChat> PFormList=new List<PChat>();
        PChat pchat;
        Gchat gchat;
        List<string> GList=new List<string> ();
        List<string>PList=new List<string> ();
        TcpClient tcpClient;
        NetworkStream _stream;
        private CancellationTokenSource _cts;
        public MInterface(ChatUser chatUser)
        {
            InitializeComponent();
            labelSelfName.Text = chatUser.Name;
        }
        
        
        //打开群聊
        private void button2_Click(object sender, EventArgs e)
        {
            if (gchat == null || gchat.IsDisposed)
            {
                if (GList.Count==0)
                {
                    gchat = new Gchat(labelSelfName.Text, tcpClient);
                    gchat.Show();
                }
                else
                {
                    gchat=new Gchat(GList,IOpath, tcpClient);
                    GList.Clear();
                    gchat.Show();
                }
            }
        }
        //私聊按钮
        private void button4_Click(object sender, EventArgs e)
        {
            if (listBoxUsers.SelectedItem == null)
            {
                MessageBox.Show("请选择私聊对象");
                return;
            }
            string chooseuser = listBoxUsers.SelectedItem.ToString();
            bool isOpen=false;
            if (PFormList.Count != 0)
            {
                foreach (var pform in PFormList)
                {
                    if (pform.Text == chooseuser)
                    {
                        isOpen = true;
                        break;
                    }
                }
            }
            if (!isOpen)
            {
                pchat = new PChat(labelSelfName.Text, chooseuser, userAndmessage,IOpath, tcpClient);
                pchat.Text = chooseuser;
                PFormList.Add(pchat);
                pchat.Show();
                if (userAndmessage.ContainsKey(chooseuser))
                {
                    userAndmessage.Remove(chooseuser);
                }
            }
            else
            {
                MessageBox.Show($"已打开与用户{chooseuser}的对话框");
            }
        }
        private async void MInterface_Load(object sender, EventArgs e)
        {
            try
            {
                if (tcpClient?.Connected == true)
                {
                    listBox1.Items.Clear();
                    listBox1.Items.Add("在线");
                    return;
                }
                _cts = new CancellationTokenSource();
                tcpClient = new TcpClient();
                tcpClient.ConnectAsync("192.168.1.13", 9500);
                _stream = tcpClient.GetStream();

                listBox1.Items.Clear();
                listBox1.Items.Add("在线");
                LoginMessage();
                /*_ = Task.Run(() => StartReadingAsync(_cts.Token));*///读取消息的方法
                while (true)
                {
                    ChatMessage message = await ReceptionMessage(_stream);
                    if (message.Receiver == labelSelfName.Text || message.Receiver == "All".ToString())
                    {
                        await ClassifyType(message);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("未连接上服务器" + ex.Message);
                listBox1.Items.Clear();
                listBox1.Items.Add("离线");
            }
        }
        //给服务器发送登入消息
        private void LoginMessage()
        {
            var Lmessage = new ChatMessage()
            {
                Type = "Login",
                Sender = labelSelfName.Text,
                Receiver = "System",
                Content = $"我是{labelSelfName.Text}我上线了",
                Timestamp = DateTime.Now
            };
            SendMessageAsync(_stream, Lmessage);
        }
        //发送消息
        private void SendMessageAsync(NetworkStream stream, ChatMessage message)
        {
            string json = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(json);
            byte[] length = BitConverter.GetBytes(data.Length);
            stream.WriteAsync(length, 0, 4);
            stream.WriteAsync(data, 0, data.Length);
        }
        //处理接收到的信息标签 
        private async Task ClassifyType(ChatMessage message)
        {
            if (message == null) return;
            switch (message.Type)
            {
                case "G":
                    //判断是否打开群聊，没打开即存储，打开后遍历、显示
                    if (gchat == null || gchat.IsDisposed)
                    {
                        //把chatmessage存储起来
                        string DeliverMssage = $"{message.Timestamp}          {message.Sender}:{message.Content}";
                        GList.Add( DeliverMssage );
                    }
                    else
                    {
                        //把数据传送给gchat
                        string DeliverMssage = $"{message.Timestamp}          {message.Sender}:{message.Content}";
                        await gchat.PrintToListBox(DeliverMssage);
                    }
                    break;
                case "P":
                    //遍历所有私聊窗口，查询是否已创建对应用户的窗口，如有即把数据传递并打印到对应的窗口，反之存储
                    foreach (var pform in PFormList)
                    {
                        if (pform.Text != message.Sender || (pchat == null || pchat.IsDisposed))
                        {
                            //把chatmessage存储起来
                            string DeliverMessage = $"{message.Timestamp}          {message.Sender}:{message.Content}";
                            List<string> newlist =new List<string>();
                            //newuserAndmessage.Add
                            //PList.Add( DeliverMessage);
                            if (userAndmessage.TryGetValue(message.Sender, out List<string> list))
                            {
                                newlist= list;
                                newlist.Add( DeliverMessage );
                                userAndmessage.Remove(message.Sender);
                                userAndmessage.Add(message.Sender, newlist);
                                return;
                            }
                            newlist.Add(DeliverMessage);
                            userAndmessage.Add(message.Sender, newlist);
                        }
                        else
                        {
                            //把数据传送给pchat
                            string DeliverMssage = $"{message.Timestamp}          {message.Sender}:{message.Content}";
                            pform.ToListBox(DeliverMssage);
                        }
                    }
                    break;
                case "UserList":
                    UpdateUserList(message.Content);
                    break;
                case "System":
                    //判断是否打开群聊，没打开即存储，打开后遍历、显示
                    if (gchat == null || gchat.IsDisposed)
                    {
                        //把chatmessage存储起来
                    }
                    else
                    {
                        //把数据传送给gchat
                    }
                    break;
                case "G-IO":
                    try
                    {
                        ChatIO cIO = JsonConvert.DeserializeObject<ChatIO>(message.Content);
                        byte[] filebyte = Convert.FromBase64String(cIO.FileContentBase64);
                        //string Fname = cIO.FileName + cIO.FileType;
                        int bytrSize = 1024;
                        int filebytelength = filebyte.Length;
                        int offset = 0;
                        await using (FileStream fs = new FileStream(@$"D:\ChatIO文件存储\{cIO.FileName}", FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            while (offset < filebytelength)
                            {
                                int byteTowrite = Math.Min(bytrSize, filebytelength - offset);
                                fs.Write(filebyte, offset, byteTowrite);
                                offset += byteTowrite;
                            }
                        }
                        IOpath.Add(@$"D:\ChatIO文件存储\{cIO.FileName}");
                        if (gchat == null || gchat.IsDisposed)
                        {
                            string DeliverMssage = $"{message.Timestamp}          {message.Sender}: 文件：";
                            string DeliverMssage1 = $"{cIO.FileName}";
                            GList.Add(DeliverMssage);
                            GList.Add(DeliverMssage1);
                        }
                        else
                        {
                            string DeliverMssage = $"{message.Timestamp}          {message.Sender}: 文件：";
                            string DeliverMssage1 = $"{cIO.FileName}";
                            await gchat.PrintToListBox(DeliverMssage);
                            await gchat.PrintToListBox(DeliverMssage1);
                        }
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show($"文件处理时出错: {ex.Message}");
                    }
                    
                    break;
                case "P-IO":
                    try
                    {
                        ChatIO PIO = JsonConvert.DeserializeObject<ChatIO>(message.Content);
                        byte[] pbyte = Convert.FromBase64String(PIO.FileContentBase64);
                        string finame = PIO.FileName + PIO.FileType;
                        int bSize = 1024;
                        int pbytelength = pbyte.Length;
                        int off = 0;
                        await using (FileStream fs = new FileStream(@$"D:\ChatIO文件存储\{PIO.FileName}", FileMode.OpenOrCreate, FileAccess.Write))
                        {
                            while (off < pbytelength)
                            {
                                int byteTowrite = Math.Min(bSize, pbytelength - off);
                                fs.Write(pbyte, off, byteTowrite);
                                off += byteTowrite;
                            }
                        }
                        IOpath.Add(@$"D:\ChatIO文件存储\{finame}");
                        //遍历所有私聊窗口，查询是否已创建对应用户的窗口，如有即把数据传递并打印到对应的窗口，反之存储
                        foreach (var pform in PFormList)
                        {
                            if (pform.Text != message.Sender || (pchat == null || pchat.IsDisposed))
                            {
                                //把chatmessage存储起来
                                string DeliverMssage = $"{message.Timestamp}          {message.Sender}: 文件：";
                                string DeliverMssage1 = $"{finame}";
                                List<string> newlist = new List<string>();
                                //newuserAndmessage.Add
                                //PList.Add( DeliverMessage);
                                if (userAndmessage.TryGetValue(message.Sender, out List<string> list))
                                {
                                    newlist = list;
                                    newlist.Add(DeliverMssage);
                                    newlist.Add(DeliverMssage1);
                                    userAndmessage.Remove(message.Sender);
                                    userAndmessage.Add(message.Sender, newlist);
                                    return;
                                }
                                newlist.Add(DeliverMssage);
                                newlist.Add(DeliverMssage1);
                                userAndmessage.Add(message.Sender, newlist);
                            }
                            else
                            {
                                //把数据传送给pchat
                                string DeliverMssage = $"{message.Timestamp}          {message.Sender}: 文件：";
                                string DeliverMssage1 = $"{finame}";
                                pform.ToListBox(DeliverMssage);
                                pform.ToListBox(DeliverMssage1);
                            }
                        }
                    }catch(Exception ex)
                    {
                        MessageBox.Show($"文件处理时出错: {ex.Message}");
                    }
                    
                    break;
            }
        }

        //更新在线用户列表UI
        private void UpdateUserList(string jsonList)
        {
            users = JsonConvert.DeserializeObject<List<string>>(jsonList);
            listBoxUsers.Items.Clear();
            foreach (var usr in users)
            {
                //if (usr != labelSelfName.Text)
                //{
                listBoxUsers.Items.Add(usr);
                //}
            }
        }
        //接收消息并编译
        private async Task<ChatMessage> ReceptionMessage(NetworkStream stream)
        {
            byte[] lengthbyte = new byte[4];
            await ReadFullAsync(stream, lengthbyte, 4);
            int messagelength = BitConverter.ToInt32(lengthbyte, 0);
            byte[] messagebuffer = new byte[messagelength];
            await ReadFullAsync(stream, messagebuffer, messagelength);
            string json = Encoding.UTF8.GetString(messagebuffer);
            return JsonConvert.DeserializeObject<ChatMessage>(json);
        }
        //防粘黏
        private static async Task ReadFullAsync(NetworkStream stream, byte[] buffer, int count)
        {
            int totalRead = 0;
            while (totalRead < count)
            {
                int read = await stream.ReadAsync(buffer, totalRead, count - totalRead);
                if (read == 0) throw new Exception("客户端断开连接");
                totalRead += read;
            }
        }
    
        
        //关闭程序
        private void MInterface_FormClosing(object sender, FormClosingEventArgs e)
        {
            System.Environment.Exit(0);
        }
    }
}
