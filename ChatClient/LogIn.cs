using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChatClient
{
    public partial class LogIn : Form
    {
        public LogIn()
        {
            InitializeComponent();
            string folderPath = @"D:\ChatIO文件存储";
            Directory.CreateDirectory(folderPath);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string UserName = textBox1.Text;
            string Password = textBox2.Text;
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("请输入账号"); return;
            }
            if (string.IsNullOrWhiteSpace(textBox2.Text))
            {
                MessageBox.Show("请输入密码"); return;
            }
            ChatUser U = new ChatUser();
            using (MyDBContext dbContext = new MyDBContext())
            {
                U = dbContext.Users.FirstOrDefault(x => x.Uname == UserName && x.Pwd == Password);
            }
            if (U == null)
            {
                MessageBox.Show("未查询到此用户，请注册"); return;
            }
            MInterface mInterface = new MInterface(U);
            mInterface.Show();
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Enroll enroll = new Enroll();
            enroll.ShowDialog();
        }
    }
}
