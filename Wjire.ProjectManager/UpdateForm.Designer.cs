namespace Wjire.ProjectManager
{
    partial class UpdateForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.lbl_appName = new System.Windows.Forms.Label();
            this.lbl_appDir = new System.Windows.Forms.Label();
            this.tbx_localPath = new System.Windows.Forms.TextBox();
            this.btn_chooseProjectDir = new System.Windows.Forms.Button();
            this.tbx_appName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(96, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 0;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(236, 137);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 30);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbl_appName
            // 
            this.lbl_appName.AutoSize = true;
            this.lbl_appName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_appName.Location = new System.Drawing.Point(15, 24);
            this.lbl_appName.Name = "lbl_appName";
            this.lbl_appName.Size = new System.Drawing.Size(72, 16);
            this.lbl_appName.TabIndex = 2;
            this.lbl_appName.Text = "站点名称";
            // 
            // lbl_appDir
            // 
            this.lbl_appDir.AutoSize = true;
            this.lbl_appDir.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_appDir.Location = new System.Drawing.Point(15, 79);
            this.lbl_appDir.Name = "lbl_appDir";
            this.lbl_appDir.Size = new System.Drawing.Size(72, 16);
            this.lbl_appDir.TabIndex = 3;
            this.lbl_appDir.Text = "本地路径";
            // 
            // tbx_localPath
            // 
            this.tbx_localPath.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbx_localPath.Location = new System.Drawing.Point(98, 76);
            this.tbx_localPath.Name = "tbx_localPath";
            this.tbx_localPath.Size = new System.Drawing.Size(379, 26);
            this.tbx_localPath.TabIndex = 6;
            // 
            // btn_chooseProjectDir
            // 
            this.btn_chooseProjectDir.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_chooseProjectDir.Location = new System.Drawing.Point(493, 76);
            this.btn_chooseProjectDir.Name = "btn_chooseProjectDir";
            this.btn_chooseProjectDir.Size = new System.Drawing.Size(46, 26);
            this.btn_chooseProjectDir.TabIndex = 10;
            this.btn_chooseProjectDir.Text = "...";
            this.btn_chooseProjectDir.UseVisualStyleBackColor = true;
            this.btn_chooseProjectDir.Click += new System.EventHandler(this.btn_chooseProjectDir_Click);
            // 
            // tbx_appName
            // 
            this.tbx_appName.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbx_appName.Location = new System.Drawing.Point(98, 21);
            this.tbx_appName.Name = "tbx_appName";
            this.tbx_appName.ReadOnly = true;
            this.tbx_appName.Size = new System.Drawing.Size(379, 26);
            this.tbx_appName.TabIndex = 11;
            // 
            // UpdateForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(569, 181);
            this.Controls.Add(this.tbx_appName);
            this.Controls.Add(this.btn_chooseProjectDir);
            this.Controls.Add(this.tbx_localPath);
            this.Controls.Add(this.lbl_appDir);
            this.Controls.Add(this.lbl_appName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "UpdateForm";
            this.Text = "编辑项目";
            this.Load += new System.EventHandler(this.UpdateForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbl_appName;
        private System.Windows.Forms.Label lbl_appDir;
        private System.Windows.Forms.TextBox tbx_localPath;
        private System.Windows.Forms.Button btn_chooseProjectDir;
        private System.Windows.Forms.TextBox tbx_appName;
    }
}