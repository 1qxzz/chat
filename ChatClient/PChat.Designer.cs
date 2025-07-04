namespace ChatClient
{
    partial class PChat
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
            textBox1 = new TextBox();
            listBox1 = new ListBox();
            button1 = new Button();
            label1 = new Label();
            label2 = new Label();
            button2 = new Button();
            SuspendLayout();
            // 
            // textBox1
            // 
            textBox1.Location = new Point(10, 316);
            textBox1.Multiline = true;
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(959, 112);
            textBox1.TabIndex = 5;
            // 
            // listBox1
            // 
            listBox1.FormattingEnabled = true;
            listBox1.ItemHeight = 17;
            listBox1.Location = new Point(10, 17);
            listBox1.Name = "listBox1";
            listBox1.Size = new Size(959, 293);
            listBox1.TabIndex = 4;
            listBox1.SelectedIndexChanged += listBox1_SelectedIndexChanged;
            // 
            // button1
            // 
            button1.Location = new Point(894, 441);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 3;
            button1.Text = "发送";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 441);
            label1.Name = "label1";
            label1.Size = new Size(56, 17);
            label1.TabIndex = 6;
            label1.Text = "用户名：";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(58, 441);
            label2.Name = "label2";
            label2.Size = new Size(43, 17);
            label2.TabIndex = 7;
            label2.Text = "label2";
            // 
            // button2
            // 
            button2.Location = new Point(134, 441);
            button2.Name = "button2";
            button2.Size = new Size(75, 23);
            button2.TabIndex = 8;
            button2.Text = "发送文件";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // PChat
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(982, 477);
            Controls.Add(button2);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(listBox1);
            Controls.Add(button1);
            Name = "PChat";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "私聊";
            Load += PChat_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TextBox textBox1;
        private ListBox listBox1;
        private Button button1;
        private Label label1;
        private Label label2;
        private Button button2;
    }
}