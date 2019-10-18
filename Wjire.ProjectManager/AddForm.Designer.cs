namespace Wjire.ProjectManager
{
    partial class AddForm
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
            this.tbx_appDir = new System.Windows.Forms.TextBox();
            this.btn_chooseProjectDir = new System.Windows.Forms.Button();
            this.cbx_app = new System.Windows.Forms.ComboBox();
            this.rbn_iis = new System.Windows.Forms.RadioButton();
            this.rbn_exe = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(91, 220);
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
            this.button2.Location = new System.Drawing.Point(231, 220);
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
            this.lbl_appName.Location = new System.Drawing.Point(10, 107);
            this.lbl_appName.Name = "lbl_appName";
            this.lbl_appName.Size = new System.Drawing.Size(72, 16);
            this.lbl_appName.TabIndex = 2;
            this.lbl_appName.Text = "站点名称";
            // 
            // lbl_appDir
            // 
            this.lbl_appDir.AutoSize = true;
            this.lbl_appDir.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbl_appDir.Location = new System.Drawing.Point(10, 162);
            this.lbl_appDir.Name = "lbl_appDir";
            this.lbl_appDir.Size = new System.Drawing.Size(72, 16);
            this.lbl_appDir.TabIndex = 3;
            this.lbl_appDir.Text = "本地路径";
            // 
            // tbx_appDir
            // 
            this.tbx_appDir.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbx_appDir.Location = new System.Drawing.Point(93, 159);
            this.tbx_appDir.Name = "tbx_appDir";
            this.tbx_appDir.Size = new System.Drawing.Size(644, 26);
            this.tbx_appDir.TabIndex = 6;
            // 
            // btn_chooseProjectDir
            // 
            this.btn_chooseProjectDir.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_chooseProjectDir.Location = new System.Drawing.Point(752, 159);
            this.btn_chooseProjectDir.Name = "btn_chooseProjectDir";
            this.btn_chooseProjectDir.Size = new System.Drawing.Size(46, 26);
            this.btn_chooseProjectDir.TabIndex = 10;
            this.btn_chooseProjectDir.Text = "...";
            this.btn_chooseProjectDir.UseVisualStyleBackColor = true;
            this.btn_chooseProjectDir.Click += new System.EventHandler(this.btn_chooseProjectDir_Click);
            // 
            // cbx_app
            // 
            this.cbx_app.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbx_app.FormattingEnabled = true;
            this.cbx_app.Location = new System.Drawing.Point(94, 104);
            this.cbx_app.Name = "cbx_app";
            this.cbx_app.Size = new System.Drawing.Size(378, 24);
            this.cbx_app.TabIndex = 11;
            // 
            // rbn_iis
            // 
            this.rbn_iis.AutoSize = true;
            this.rbn_iis.Checked = true;
            this.rbn_iis.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbn_iis.Location = new System.Drawing.Point(12, 20);
            this.rbn_iis.Name = "rbn_iis";
            this.rbn_iis.Size = new System.Drawing.Size(50, 20);
            this.rbn_iis.TabIndex = 12;
            this.rbn_iis.TabStop = true;
            this.rbn_iis.Text = "IIS";
            this.rbn_iis.UseVisualStyleBackColor = true;
            // 
            // rbn_exe
            // 
            this.rbn_exe.AutoSize = true;
            this.rbn_exe.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbn_exe.Location = new System.Drawing.Point(102, 20);
            this.rbn_exe.Name = "rbn_exe";
            this.rbn_exe.Size = new System.Drawing.Size(50, 20);
            this.rbn_exe.TabIndex = 13;
            this.rbn_exe.Text = "EXE";
            this.rbn_exe.UseVisualStyleBackColor = true;
            this.rbn_exe.CheckedChanged += new System.EventHandler(this.rbn_exe_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbn_exe);
            this.groupBox1.Controls.Add(this.rbn_iis);
            this.groupBox1.Location = new System.Drawing.Point(94, 23);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(162, 57);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 15;
            this.label1.Text = "程序类型";
            // 
            // AddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 298);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cbx_app);
            this.Controls.Add(this.btn_chooseProjectDir);
            this.Controls.Add(this.tbx_appDir);
            this.Controls.Add(this.lbl_appDir);
            this.Controls.Add(this.lbl_appName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "AddForm";
            this.Text = "添加项目";
            this.Load += new System.EventHandler(this.ProjectAddForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbl_appName;
        private System.Windows.Forms.Label lbl_appDir;
        private System.Windows.Forms.TextBox tbx_appDir;
        private System.Windows.Forms.Button btn_chooseProjectDir;
        private System.Windows.Forms.ComboBox cbx_app;
        private System.Windows.Forms.RadioButton rbn_iis;
        private System.Windows.Forms.RadioButton rbn_exe;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
    }
}