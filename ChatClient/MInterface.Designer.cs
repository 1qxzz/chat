namespace ChatClient
{
    partial class MInterface
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
            label2 = new Label();
            button2 = new Button();
            labelSelfName = new Label();
            listBox1 = new ListBox();
            button4 = new Button();
            listBoxUsers = new ListBox();
            label1 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(12, 462);
            label2.Name = "label2";
            label2.Size = new Size(92, 17);
            label2.TabIndex = 2;
            label2.Text = "多人共享聊天室";
            // 
            // button2
            // 
            button2.Location = new Point(256, 456);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 3;
            button2.Text = "进入群聊";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // labelSelfName
            // 
            labelSelfName.AutoSize = true;
            labelSelfName.Location = new Point(74, 9);
            labelSelfName.Name = "labelSelfName";
            labelSelfName.Size = new Size(43, 17);
            labelSelfName.TabIndex = 7;
            labelSelfName.Text = "label4";
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new Point(279, 12);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(65, 21);
            listBox1.TabIndex = 8;
            // 
            // button4
            // 
            button4.Location = new Point(256, 409);
            button4.Name = "button4";
            button4.Size = new Size(75, 23);
            button4.TabIndex = 10;
            button4.Text = "私聊";
            button4.UseVisualStyleBackColor = true;
            button4.Click += button4_Click;
            // 
            // listBoxUsers
            // 
            listBoxUsers.FormattingEnabled = true;
            listBoxUsers.ItemHeight = 17;
            listBoxUsers.Location = new Point(10, 67);
            listBoxUsers.Name = "listBoxUsers";
            listBoxUsers.Size = new Size(240, 378);
            listBoxUsers.TabIndex = 11;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 12;
            label1.Text = "用户名：";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(12, 47);
            label3.Name = "label3";
            label3.Size = new Size(68, 17);
            label3.TabIndex = 13;
            label3.Text = "在线用户：";
            // 
            // MInterface
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(356, 611);
            Controls.Add(label3);
            Controls.Add(label1);
            Controls.Add(listBoxUsers);
            Controls.Add(button4);
            Controls.Add(listBox1);
            Controls.Add(labelSelfName);
            Controls.Add(button2);
            Controls.Add(label2);
            Name = "MInterface";
            Text = "Form2";
            FormClosing += MInterface_FormClosing;
            Load += MInterface_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Label label2;
        private Button button2;
        private Label labelSelfName;
        private ListBox listBox1;
        private Button button4;
        private ListBox listBoxUsers;
        private Label label1;
        private Label label3;
    }
}