namespace Wjire.ProjectManager
{
    partial class ProjectAddForm
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
            this.lbl_projectName = new System.Windows.Forms.Label();
            this.lbl_projectDir = new System.Windows.Forms.Label();
            this.tbx_projectName = new System.Windows.Forms.TextBox();
            this.tbx_projectDir = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbn_exe = new System.Windows.Forms.RadioButton();
            this.rbn_iis = new System.Windows.Forms.RadioButton();
            this.btn_chooseProjectDir = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(293, 147);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "确认";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(399, 147);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "取消";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // lbl_projectName
            // 
            this.lbl_projectName.AutoSize = true;
            this.lbl_projectName.Location = new System.Drawing.Point(39, 39);
            this.lbl_projectName.Name = "lbl_projectName";
            this.lbl_projectName.Size = new System.Drawing.Size(53, 12);
            this.lbl_projectName.TabIndex = 2;
            this.lbl_projectName.Text = "项目名称";
            // 
            // lbl_projectDir
            // 
            this.lbl_projectDir.AutoSize = true;
            this.lbl_projectDir.Location = new System.Drawing.Point(27, 89);
            this.lbl_projectDir.Name = "lbl_projectDir";
            this.lbl_projectDir.Size = new System.Drawing.Size(65, 12);
            this.lbl_projectDir.TabIndex = 3;
            this.lbl_projectDir.Text = "项目文件夹";
            // 
            // tbx_projectName
            // 
            this.tbx_projectName.Location = new System.Drawing.Point(122, 36);
            this.tbx_projectName.Name = "tbx_projectName";
            this.tbx_projectName.Size = new System.Drawing.Size(100, 21);
            this.tbx_projectName.TabIndex = 5;
            // 
            // tbx_projectDir
            // 
            this.tbx_projectDir.Location = new System.Drawing.Point(122, 86);
            this.tbx_projectDir.Name = "tbx_projectDir";
            this.tbx_projectDir.Size = new System.Drawing.Size(379, 21);
            this.tbx_projectDir.TabIndex = 6;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbn_exe);
            this.groupBox1.Controls.Add(this.rbn_iis);
            this.groupBox1.Location = new System.Drawing.Point(293, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(245, 59);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "项目类型";
            // 
            // rbn_exe
            // 
            this.rbn_exe.AutoSize = true;
            this.rbn_exe.Location = new System.Drawing.Point(125, 23);
            this.rbn_exe.Name = "rbn_exe";
            this.rbn_exe.Size = new System.Drawing.Size(41, 16);
            this.rbn_exe.TabIndex = 1;
            this.rbn_exe.TabStop = true;
            this.rbn_exe.Text = "EXE";
            this.rbn_exe.UseVisualStyleBackColor = true;
            // 
            // rbn_iis
            // 
            this.rbn_iis.AutoSize = true;
            this.rbn_iis.Checked = true;
            this.rbn_iis.Location = new System.Drawing.Point(7, 23);
            this.rbn_iis.Name = "rbn_iis";
            this.rbn_iis.Size = new System.Drawing.Size(41, 16);
            this.rbn_iis.TabIndex = 0;
            this.rbn_iis.TabStop = true;
            this.rbn_iis.Text = "IIS";
            this.rbn_iis.UseVisualStyleBackColor = true;
            // 
            // btn_chooseProjectDir
            // 
            this.btn_chooseProjectDir.Location = new System.Drawing.Point(507, 84);
            this.btn_chooseProjectDir.Name = "btn_chooseProjectDir";
            this.btn_chooseProjectDir.Size = new System.Drawing.Size(31, 23);
            this.btn_chooseProjectDir.TabIndex = 10;
            this.btn_chooseProjectDir.Text = "...";
            this.btn_chooseProjectDir.UseVisualStyleBackColor = true;
            this.btn_chooseProjectDir.Click += new System.EventHandler(this.btn_chooseProjectDir_Click);
            // 
            // ProjectAddForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(555, 209);
            this.Controls.Add(this.btn_chooseProjectDir);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbx_projectDir);
            this.Controls.Add(this.tbx_projectName);
            this.Controls.Add(this.lbl_projectDir);
            this.Controls.Add(this.lbl_projectName);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Name = "ProjectAddForm";
            this.Text = "添加项目";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label lbl_projectName;
        private System.Windows.Forms.Label lbl_projectDir;
        private System.Windows.Forms.TextBox tbx_projectName;
        private System.Windows.Forms.TextBox tbx_projectDir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbn_exe;
        private System.Windows.Forms.RadioButton rbn_iis;
        private System.Windows.Forms.Button btn_chooseProjectDir;
    }
}