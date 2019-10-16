namespace Wjire.ProjectManager
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_addProject = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.dgv = new System.Windows.Forms.DataGridView();
            this.btn_deleteApp = new System.Windows.Forms.Button();
            this.btn_clearAll = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.AppId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppTypeString = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AppType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).BeginInit();
            this.SuspendLayout();
            // 
            // btn_addProject
            // 
            this.btn_addProject.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_addProject.Location = new System.Drawing.Point(12, 368);
            this.btn_addProject.Name = "btn_addProject";
            this.btn_addProject.Size = new System.Drawing.Size(100, 30);
            this.btn_addProject.TabIndex = 1;
            this.btn_addProject.Text = "添加项目";
            this.btn_addProject.UseVisualStyleBackColor = true;
            this.btn_addProject.Click += new System.EventHandler(this.button2_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button2.Location = new System.Drawing.Point(702, 368);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(100, 30);
            this.button2.TabIndex = 4;
            this.button2.Text = "发布项目";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // dgv
            // 
            this.dgv.ColumnHeadersHeight = 30;
            this.dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.AppId,
            this.AppName,
            this.LocalPath,
            this.AppTypeString,
            this.AppType});
            this.dgv.Location = new System.Drawing.Point(12, 12);
            this.dgv.MultiSelect = false;
            this.dgv.Name = "dgv";
            this.dgv.RowTemplate.Height = 30;
            this.dgv.RowTemplate.ReadOnly = true;
            this.dgv.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv.Size = new System.Drawing.Size(790, 336);
            this.dgv.TabIndex = 2;
            // 
            // btn_deleteApp
            // 
            this.btn_deleteApp.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_deleteApp.Location = new System.Drawing.Point(258, 368);
            this.btn_deleteApp.Name = "btn_deleteApp";
            this.btn_deleteApp.Size = new System.Drawing.Size(100, 30);
            this.btn_deleteApp.TabIndex = 5;
            this.btn_deleteApp.Text = "删除项目";
            this.btn_deleteApp.UseVisualStyleBackColor = true;
            this.btn_deleteApp.Click += new System.EventHandler(this.btn_deleteApp_Click);
            // 
            // btn_clearAll
            // 
            this.btn_clearAll.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_clearAll.Location = new System.Drawing.Point(381, 368);
            this.btn_clearAll.Name = "btn_clearAll";
            this.btn_clearAll.Size = new System.Drawing.Size(100, 30);
            this.btn_clearAll.TabIndex = 6;
            this.btn_clearAll.Text = "清除所有";
            this.btn_clearAll.UseVisualStyleBackColor = true;
            this.btn_clearAll.Click += new System.EventHandler(this.btn_clearAll_Click);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.button1.Location = new System.Drawing.Point(135, 368);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 30);
            this.button1.TabIndex = 7;
            this.button1.Text = "编辑项目";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AppId
            // 
            this.AppId.DataPropertyName = "AppId";
            this.AppId.Frozen = true;
            this.AppId.HeaderText = "项目Id";
            this.AppId.Name = "AppId";
            this.AppId.Visible = false;
            this.AppId.Width = 60;
            // 
            // AppName
            // 
            this.AppName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AppName.DataPropertyName = "AppName";
            this.AppName.Frozen = true;
            this.AppName.HeaderText = "项目名称";
            this.AppName.Name = "AppName";
            this.AppName.ReadOnly = true;
            this.AppName.Width = 200;
            // 
            // LocalPath
            // 
            this.LocalPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.LocalPath.DataPropertyName = "LocalPath";
            this.LocalPath.Frozen = true;
            this.LocalPath.HeaderText = "本地路径";
            this.LocalPath.Name = "LocalPath";
            this.LocalPath.ReadOnly = true;
            this.LocalPath.Width = 450;
            // 
            // AppTypeString
            // 
            this.AppTypeString.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AppTypeString.DataPropertyName = "AppTypeString";
            this.AppTypeString.Frozen = true;
            this.AppTypeString.HeaderText = "项目类型";
            this.AppTypeString.Name = "AppTypeString";
            this.AppTypeString.ReadOnly = true;
            // 
            // AppType
            // 
            this.AppType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.AppType.DataPropertyName = "AppType";
            this.AppType.Frozen = true;
            this.AppType.HeaderText = "项目类型";
            this.AppType.Name = "AppType";
            this.AppType.ReadOnly = true;
            this.AppType.Visible = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(806, 410);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btn_clearAll);
            this.Controls.Add(this.btn_deleteApp);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.dgv);
            this.Controls.Add(this.btn_addProject);
            this.Name = "Form1";
            this.Text = "项目管理";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgv)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btn_addProject;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgv;
        private System.Windows.Forms.Button btn_deleteApp;
        private System.Windows.Forms.Button btn_clearAll;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppId;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppName;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppTypeString;
        private System.Windows.Forms.DataGridViewTextBoxColumn AppType;
    }
}

