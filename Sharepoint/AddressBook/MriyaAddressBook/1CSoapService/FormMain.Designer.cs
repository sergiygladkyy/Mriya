namespace _1CSoapService
{
    partial class FormMain
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureConnectionTo1CSOAPServerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewABSoap = new System.Windows.Forms.DataGridView();
            this.buttonGetSoapData = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.dataGridViewABDal = new System.Windows.Forms.DataGridView();
            this.buttonGetDALData = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.dataGridViewSP = new System.Windows.Forms.DataGridView();
            this.buttonbuttonGetSPData = new System.Windows.Forms.Button();
            this.dataGridViewSPDal = new System.Windows.Forms.DataGridView();
            this.buttonbuttonGetSPDataDal = new System.Windows.Forms.Button();
            this.labelSoapStatus = new System.Windows.Forms.Label();
            this.labelABDalStatus = new System.Windows.Forms.Label();
            this.labelSPStatus = new System.Windows.Forms.Label();
            this.labelSPDalStatus = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewABSoap)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewABDal)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSP)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSPDal)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(611, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.exitToolStripMenuItem.Text = "E&xit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // settingsToolStripMenuItem
            // 
            this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configureConnectionTo1CSOAPServerToolStripMenuItem});
            this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
            this.settingsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.settingsToolStripMenuItem.Text = "Settings";
            // 
            // configureConnectionTo1CSOAPServerToolStripMenuItem
            // 
            this.configureConnectionTo1CSOAPServerToolStripMenuItem.Name = "configureConnectionTo1CSOAPServerToolStripMenuItem";
            this.configureConnectionTo1CSOAPServerToolStripMenuItem.Size = new System.Drawing.Size(269, 22);
            this.configureConnectionTo1CSOAPServerToolStripMenuItem.Text = "Configure connection to 1C SOAP server";
            this.configureConnectionTo1CSOAPServerToolStripMenuItem.Click += new System.EventHandler(this.configureConnectionTo1CSOAPServerToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 379);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(611, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.tabPage1);
            this.tabControlMain.Controls.Add(this.tabPage2);
            this.tabControlMain.Controls.Add(this.tabPage3);
            this.tabControlMain.Controls.Add(this.tabPage4);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 24);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(611, 355);
            this.tabControlMain.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.labelSoapStatus);
            this.tabPage1.Controls.Add(this.dataGridViewABSoap);
            this.tabPage1.Controls.Add(this.buttonGetSoapData);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(603, 329);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "SOAP data";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewABSoap
            // 
            this.dataGridViewABSoap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewABSoap.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewABSoap.Location = new System.Drawing.Point(7, 37);
            this.dataGridViewABSoap.Name = "dataGridViewABSoap";
            this.dataGridViewABSoap.Size = new System.Drawing.Size(593, 289);
            this.dataGridViewABSoap.TabIndex = 1;
            // 
            // buttonGetSoapData
            // 
            this.buttonGetSoapData.Location = new System.Drawing.Point(7, 7);
            this.buttonGetSoapData.Name = "buttonGetSoapData";
            this.buttonGetSoapData.Size = new System.Drawing.Size(75, 23);
            this.buttonGetSoapData.TabIndex = 0;
            this.buttonGetSoapData.Text = "Get data";
            this.buttonGetSoapData.UseVisualStyleBackColor = true;
            this.buttonGetSoapData.Click += new System.EventHandler(this.buttonGetSoapData_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.labelABDalStatus);
            this.tabPage2.Controls.Add(this.dataGridViewABDal);
            this.tabPage2.Controls.Add(this.buttonGetDALData);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(603, 329);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "DAL data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // dataGridViewABDal
            // 
            this.dataGridViewABDal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewABDal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewABDal.Location = new System.Drawing.Point(9, 37);
            this.dataGridViewABDal.Name = "dataGridViewABDal";
            this.dataGridViewABDal.Size = new System.Drawing.Size(586, 289);
            this.dataGridViewABDal.TabIndex = 1;
            // 
            // buttonGetDALData
            // 
            this.buttonGetDALData.Location = new System.Drawing.Point(9, 7);
            this.buttonGetDALData.Name = "buttonGetDALData";
            this.buttonGetDALData.Size = new System.Drawing.Size(75, 23);
            this.buttonGetDALData.TabIndex = 0;
            this.buttonGetDALData.Text = "Get Data";
            this.buttonGetDALData.UseVisualStyleBackColor = true;
            this.buttonGetDALData.Click += new System.EventHandler(this.buttonGetDALData_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.labelSPStatus);
            this.tabPage3.Controls.Add(this.dataGridViewSP);
            this.tabPage3.Controls.Add(this.buttonbuttonGetSPData);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(603, 329);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "SP Profiles (API)";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.labelSPDalStatus);
            this.tabPage4.Controls.Add(this.dataGridViewSPDal);
            this.tabPage4.Controls.Add(this.buttonbuttonGetSPDataDal);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(603, 329);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "SP Profiles (SQL)";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // dataGridViewSP
            // 
            this.dataGridViewSP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSP.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSP.Location = new System.Drawing.Point(5, 35);
            this.dataGridViewSP.Name = "dataGridViewSP";
            this.dataGridViewSP.Size = new System.Drawing.Size(593, 289);
            this.dataGridViewSP.TabIndex = 3;
            // 
            // buttonbuttonGetSPData
            // 
            this.buttonbuttonGetSPData.Location = new System.Drawing.Point(5, 5);
            this.buttonbuttonGetSPData.Name = "buttonbuttonGetSPData";
            this.buttonbuttonGetSPData.Size = new System.Drawing.Size(75, 23);
            this.buttonbuttonGetSPData.TabIndex = 2;
            this.buttonbuttonGetSPData.Text = "Get data";
            this.buttonbuttonGetSPData.UseVisualStyleBackColor = true;
            this.buttonbuttonGetSPData.Click += new System.EventHandler(this.buttonbuttonGetSPData_Click);
            // 
            // dataGridViewSPDal
            // 
            this.dataGridViewSPDal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewSPDal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSPDal.Location = new System.Drawing.Point(5, 35);
            this.dataGridViewSPDal.Name = "dataGridViewSPDal";
            this.dataGridViewSPDal.Size = new System.Drawing.Size(593, 289);
            this.dataGridViewSPDal.TabIndex = 5;
            // 
            // buttonbuttonGetSPDataDal
            // 
            this.buttonbuttonGetSPDataDal.Location = new System.Drawing.Point(5, 5);
            this.buttonbuttonGetSPDataDal.Name = "buttonbuttonGetSPDataDal";
            this.buttonbuttonGetSPDataDal.Size = new System.Drawing.Size(75, 23);
            this.buttonbuttonGetSPDataDal.TabIndex = 4;
            this.buttonbuttonGetSPDataDal.Text = "Get data";
            this.buttonbuttonGetSPDataDal.UseVisualStyleBackColor = true;
            this.buttonbuttonGetSPDataDal.Click += new System.EventHandler(this.buttonbuttonGetSPDataDal_Click);
            // 
            // labelSoapStatus
            // 
            this.labelSoapStatus.AutoSize = true;
            this.labelSoapStatus.Location = new System.Drawing.Point(88, 12);
            this.labelSoapStatus.Name = "labelSoapStatus";
            this.labelSoapStatus.Size = new System.Drawing.Size(0, 13);
            this.labelSoapStatus.TabIndex = 2;
            // 
            // labelABDalStatus
            // 
            this.labelABDalStatus.AutoSize = true;
            this.labelABDalStatus.Location = new System.Drawing.Point(90, 12);
            this.labelABDalStatus.Name = "labelABDalStatus";
            this.labelABDalStatus.Size = new System.Drawing.Size(0, 13);
            this.labelABDalStatus.TabIndex = 2;
            // 
            // labelSPStatus
            // 
            this.labelSPStatus.AutoSize = true;
            this.labelSPStatus.Location = new System.Drawing.Point(86, 10);
            this.labelSPStatus.Name = "labelSPStatus";
            this.labelSPStatus.Size = new System.Drawing.Size(0, 13);
            this.labelSPStatus.TabIndex = 4;
            // 
            // labelSPDalStatus
            // 
            this.labelSPDalStatus.AutoSize = true;
            this.labelSPDalStatus.Location = new System.Drawing.Point(86, 10);
            this.labelSPDalStatus.Name = "labelSPDalStatus";
            this.labelSPDalStatus.Size = new System.Drawing.Size(0, 13);
            this.labelSPDalStatus.TabIndex = 6;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 401);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormMain";
            this.Text = "Test 1C SOAP servie";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewABSoap)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewABDal)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSP)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSPDal)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridViewABSoap;
        private System.Windows.Forms.Button buttonGetSoapData;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureConnectionTo1CSOAPServerToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridViewABDal;
        private System.Windows.Forms.Button buttonGetDALData;
        private System.Windows.Forms.Label labelSoapStatus;
        private System.Windows.Forms.Label labelABDalStatus;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label labelSPStatus;
        private System.Windows.Forms.DataGridView dataGridViewSP;
        private System.Windows.Forms.Button buttonbuttonGetSPData;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Label labelSPDalStatus;
        private System.Windows.Forms.DataGridView dataGridViewSPDal;
        private System.Windows.Forms.Button buttonbuttonGetSPDataDal;

    }
}

