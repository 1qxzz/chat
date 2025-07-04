namespace ChatClient
{
    partial class Enroll
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            label1 = new Label();
            textBoxUname = new TextBox();
            textBoxName = new TextBox();
            label2 = new Label();
            textBoxPwd = new TextBox();
            label3 = new Label();
            textBoxCpwd = new TextBox();
            label4 = new Label();
            button1 = new Button();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(180, 81);
            label1.Name = "label1";
            label1.Size = new Size(44, 17);
            label1.TabIndex = 0;
            label1.Text = "账号：";
            // 
            // textBoxUname
            // 
            textBoxUname.Location = new Point(241, 78);
            textBoxUname.Name = "textBoxUname";
            textBoxUname.Size = new Size(150, 23);
            textBoxUname.TabIndex = 1;
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(241, 124);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(150, 23);
            textBoxName.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(168, 124);
            label2.Name = "label2";
            label2.Size = new Size(56, 17);
            label2.TabIndex = 2;
            label2.Text = "用户名：";
            // 
            // textBoxPwd
            // 
            textBoxPwd.Location = new Point(241, 172);
            textBoxPwd.Name = "textBoxPwd";
            textBoxPwd.PasswordChar = '*';
            textBoxPwd.Size = new Size(150, 23);
            textBoxPwd.TabIndex = 5;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(180, 175);
            label3.Name = "label3";
            label3.Size = new Size(44, 17);
            label3.TabIndex = 4;
            label3.Text = "密码：";
            // 
            // textBoxCpwd
            // 
            textBoxCpwd.Location = new Point(241, 218);
            textBoxCpwd.Name = "textBoxCpwd";
            textBoxCpwd.PasswordChar = '*';
            textBoxCpwd.Size = new Size(150, 23);
            textBoxCpwd.TabIndex = 7;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(156, 221);
            label4.Name = "label4";
            label4.Size = new Size(68, 17);
            label4.TabIndex = 6;
            label4.Text = "确认密码：";
            // 
            // button1
            // 
            button1.Cursor = Cursors.Hand;
            button1.Location = new Point(241, 319);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 8;
            button1.Text = "注册";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Enroll
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(588, 382);
            Controls.Add(button1);
            Controls.Add(textBoxCpwd);
            Controls.Add(label4);
            Controls.Add(textBoxPwd);
            Controls.Add(label3);
            Controls.Add(textBoxName);
            Controls.Add(label2);
            Controls.Add(textBoxUname);
            Controls.Add(label1);
            Name = "Enroll";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Enroll";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxUname;
        private TextBox textBoxName;
        private Label label2;
        private TextBox textBoxPwd;
        private Label label3;
        private TextBox textBoxCpwd;
        private Label label4;
        private Button button1;
    }
}