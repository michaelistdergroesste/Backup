namespace BackupWork
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.buttonHide = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.progressBarSuccess = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(104, 80);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "BackupWork";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(104, 112);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon.Text = "notifyIcon1";
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseClick_1);
            // 
            // buttonHide
            // 
            this.buttonHide.Location = new System.Drawing.Point(234, 21);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(49, 23);
            this.buttonHide.TabIndex = 2;
            this.buttonHide.Text = "Hide";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(125, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "label2";
            // 
            // progressBarSuccess
            // 
            this.progressBarSuccess.Location = new System.Drawing.Point(64, 52);
            this.progressBarSuccess.MarqueeAnimationSpeed = 500;
            this.progressBarSuccess.Name = "progressBarSuccess";
            this.progressBarSuccess.Size = new System.Drawing.Size(162, 23);
            this.progressBarSuccess.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 194);
            this.Controls.Add(this.progressBarSuccess);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonHide);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Label label1;
        private Button button1;
        private NotifyIcon notifyIcon;
        private Button buttonHide;
        private Label label2;
        private ProgressBar progressBarSuccess;
    }
}