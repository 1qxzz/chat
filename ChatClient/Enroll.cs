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
    public partial class Enroll : Form
    {
        public Enroll()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string uname=textBoxUname.Text;
            string Name=textBoxName.Text;
            string Pwd=textBoxPwd.Text;
            string cPwd=textBoxCpwd.Text;
            if (string.IsNullOrWhiteSpace(textBoxUname.Text))
            {
                MessageBox.Show("请输入账号");
                return;
            }if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                MessageBox.Show("请输入用户名");
                return;
            }if (string.IsNullOrWhiteSpace(textBoxPwd.Text))
            {
                MessageBox.Show("请输入密码");
                return;
            }if (Pwd!=cPwd)
            {
                MessageBox.Show("两次密码输入不一致");
                return;
            }
            ChatUser user = new ChatUser();
            user.Uname = uname;
            user.Name = Name;
            user.Pwd = Pwd;
            int i = -1;
            using(MyDBContext dbContext = new MyDBContext())
            {
                int count=  dbContext.Users.Count(x=>x.Uname==uname);
                if (count>0)
                {
                    MessageBox.Show("该账号已被注册");
                    return ;
                }
                dbContext.Users.Attach(user);
                dbContext.Entry(user).State=System.Data.Entity.EntityState.Added;
                i=dbContext.SaveChanges();
            }
            if (i<1)
            {
                MessageBox.Show("注册失败");
            } else
            { 
                MessageBox.Show("注册成功");
                this.Close();
            }
        }
    }
}
