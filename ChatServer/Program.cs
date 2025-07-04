using ChatClient;
using Newtonsoft.Json;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ChatServer
{
    internal class Program
    {
        public static readonly Dictionary<string, TcpClient> users = new Dictionary<string, TcpClient>();
        static readonly List<TcpClient> clients = new List<TcpClient>();
        private readonly static object _lock = new object();
        static async Task Main(string[] args)
        {
            const int port = 9500;
            TcpListener server = new TcpListener(IPAddress.Any, port);
            server.Server.SetSocketOption(
            SocketOptionLevel.Socket,
            SocketOptionName.ReuseAddress,
            true);

            server.ExclusiveAddressUse = false;
            server.Start();
            Console.WriteLine("服务器启动，监听端口：" + port);
            while (true)
            {
                TcpClient client =await server.AcceptTcpClientAsync();
                //lock (_lock)
                Console.WriteLine($"客户端连接：{client.Client.RemoteEndPoint},当前连接数: {clients.Count}");
                _ = Task.Run(() => HandleClientAsync(client));
            }
        }
        //用户登入后加入字典
        public static void AddUser(string username, TcpClient Client)
        {
            lock (_lock)
            {
                if (!users.ContainsKey(username))
                {
                    users[username] = Client;
                    Console.WriteLine($"{username}已加入聊天室");
                }
            }
        }
        //用户退出后移除字典
        public static void RemoveUser(string username)
        {
            lock (_lock)
            {
                if (users.ContainsKey(username))
                {
                    users.Remove(username);
                    Console.WriteLine($"{username}已退出聊天室");
                }
            }
        }
        //是否连接到服务器
        public static bool TryGetClient(string username, out TcpClient client)
        {
            lock (_lock)
            {
                return users.TryGetValue(username, out client);
            }
        }
        //得到在线的用户名称
        public static List<string> GetOnlineUsers()
        {
            lock (_lock)
            {
                return new List<string>(users.Keys);
            }
        }
        //向在线用户发送在线用户列表
        public static async Task SendOnlineUsersList(NetworkStream stream)
        {
            try
            {
                List<string> onlineUsers = GetOnlineUsers();
                var Msg = new ChatMessage()
                {
                    Type = "UserList",
                    Sender = "Server",
                    Receiver = "All",
                    Content = JsonConvert.SerializeObject(onlineUsers),
                    Timestamp = DateTime.Now
                };
                await SendMessageAsync(stream, Msg);
                Console.WriteLine("已向用户发送在线列表");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"发送用户列表失败：{ex.Message}");
            }
        }
        //发送消息
        private static async Task SendMessageAsync(NetworkStream stream, ChatMessage message)
        {
            // 序列化消息
            string json = JsonConvert.SerializeObject(message);
            byte[] data = Encoding.UTF8.GetBytes(json);

            // 添加长度前缀
            byte[] lengthBytes = BitConverter.GetBytes(data.Length);

            // 先发送长度
            await stream.WriteAsync(lengthBytes, 0, 4);
            // 再发送数据
            await stream.WriteAsync(data, 0, data.Length);
        }
        //接收消息并反序列成chatmessage
        private static async Task<ChatMessage> ReceiveMessageAsync(NetworkStream stream)
        {
            byte[] lengthByte = new byte[4];
            await ReadFullAsync(stream, lengthByte, 4);
            int messageLength = BitConverter.ToInt32(lengthByte, 0);
            byte[] meassageBuffer = new byte[messageLength];
            await ReadFullAsync(stream, meassageBuffer, messageLength);
            string json = Encoding.UTF8.GetString(meassageBuffer);
            return JsonConvert.DeserializeObject<ChatMessage>(json);
        }
        //用户连接进来，异步接收用户消息并发送
        static async Task HandleClientAsync(TcpClient client)
        {
            //获取登入消息
            string username = null;
            try
            {
                var login = await ReceiveMessageAsync(client.GetStream());
                username = login.Sender;
                AddUser(username, client);
                //向所有用户发送在线用户列表
                if(TryGetClient(username,out TcpClient clients))
                {
                    await BroadcastOnlineUsersUpdate();
                } 
                

                while (true)
                {
                    var message = await ReceiveMessageAsync(client.GetStream());
                    
                    if (message != null)
                    {
                        if (message.Type == "G"||message.Type=="G-IO")
                        {//发送群聊消息
                            Console.WriteLine($"收到用户群聊消息:{message.Content}");
                            await BroadcastMessageAsync(message, client);//提供消息和发送用户
                        }
                        else if (message.Type == "P"||message.Type=="P-IO")
                        {//发送私聊消息
                            await SendPrivateMessageAsync(message);
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"客户端错误：+{ex.Message}");
            }
            finally
            {
                if (username != null)
                {
                    RemoveUser(username);
                    //群发到所有在线用户更新在线用户列表
                    await BroadcastOnlineUsersUpdate();
                }
                client.Close();
            }

            //using (TcpClient tcpClient = (TcpClient)obj)
            //using (NetworkStream stream = tcpClient.GetStream())
            //{
            //    byte[] lengthBuffer = new byte[4];
            //    try
            //    {
            //        while (true)
            //        {

            //            await ReadFullAsync(stream, lengthBuffer, 4);
            //            int messageLength = BitConverter.ToInt32(lengthBuffer, 0);

            //            // 2. 读取消息内容
            //            byte[] messageBuffer = new byte[messageLength];
            //            await ReadFullAsync(stream, messageBuffer, messageLength);
            //            string message = Encoding.UTF8.GetString(messageBuffer);
            //            Console.WriteLine($"收到: {message}");

            //            // 3. 广播给其他客户端
            //            await Broadcast(message, tcpClient);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        Console.WriteLine($"错误：{ex.Message}");
            //    }
            //    finally
            //    {
            //        lock (_lock) clients.Remove(tcpClient);
            //        Console.WriteLine("客户端断开连接");
            //    }
            //}


        }
        //群发消息
        private static async Task BroadcastMessageAsync(ChatMessage message, TcpClient client)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            List<string> u = GetOnlineUsers();
            foreach (string us in u)
            {
                if (TryGetClient(us, out TcpClient clients))
                {
                    await SendMessageAsync(clients.GetStream(), message);
                }
                else
                {
                    Console.WriteLine("用户未连接");
                }
            }
            Console.WriteLine("发送用户群聊消息");
        }
        //私聊消息
        private static async Task SendPrivateMessageAsync(ChatMessage message)
        {
            if (TryGetClient(message.Receiver, out TcpClient client))
            {
                NetworkStream stream = client.GetStream();
                await SendMessageAsync(stream, message);
            }
            else
            {
                var offMsg = new ChatMessage()
                {
                    Type = "System",
                    Sender = message.Receiver,
                    Receiver = message.Sender,
                    Content = $"{message.Receiver} 不在线",
                    Timestamp = DateTime.Now
                };
                if(TryGetClient(message.Sender,out TcpClient sendClient))
                {
                    await SendMessageAsync(sendClient.GetStream(),offMsg);
                }
            }
        }
        //通知所有用户更新在线列表
        private static async Task BroadcastOnlineUsersUpdate()
        {
            var users = GetOnlineUsers();
            var Msg = new ChatMessage()
            {
                Type = "UserList",
                Sender = "Server",
                Receiver = "All",
                Content = JsonConvert.SerializeObject(users),
                Timestamp = DateTime.Now
            };
            foreach (var user in users)
            {
                if (TryGetClient(user, out TcpClient client))
                {
                    await SendMessageAsync(client.GetStream(), Msg);
                }
            }
            Console.WriteLine("已发送列表");
        }
        //读取防粘黏
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
        //static async Task Broadcast(string message,TcpClient client)
        //{
        //    //byte[] buffer = Encoding.UTF8.GetBytes(message);
        //    //lock (_lock)
        //    //{
        //    //    foreach (TcpClient i in clients)
        //    //    {
        //    //        try
        //    //        {
        //    //            NetworkStream stream = i.GetStream();
        //    //            stream.Write(buffer, 0, buffer.Length);
        //    //        }
        //    //        catch
        //    //        {
        //    //            clients.Remove(i);
        //    //        }
        //    //    }
        //    //}
        //    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
        //    byte[] lengthBytes = BitConverter.GetBytes(messageBytes.Length);

        //    List<Task> tasks = new List<Task>();

        //    lock (_lock)
        //    {
        //        foreach (TcpClient client1 in clients)
        //        {

        //            try
        //            {
        //                NetworkStream stream = client1.GetStream();
        //                tasks.Add(stream.WriteAsync(lengthBytes, 0, 4));
        //                tasks.Add(stream.WriteAsync(messageBytes, 0, messageBytes.Length));
        //            }
        //            catch
        //            {
        //                // 移除断开连接的客户端
        //                clients.Remove(client);
        //            }
        //        }
        //    }

        //    await Task.WhenAll(tasks);

        //}
    }
}
