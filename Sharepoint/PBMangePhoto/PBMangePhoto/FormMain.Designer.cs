namespace PBMangePhoto
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelRec = new System.Windows.Forms.ToolStripStatusLabel();
            this.groupBoxSearch = new System.Windows.Forms.GroupBox();
            this.buttonShowAll = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxCity = new System.Windows.Forms.ComboBox();
            this.comboBoxDepartment = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxSearch = new System.Windows.Forms.TextBox();
            this.timerFind = new System.Windows.Forms.Timer(this.components);
            this.groupBoxPhoto = new System.Windows.Forms.GroupBox();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.buttonSaveImageAs = new System.Windows.Forms.Button();
            this.buttonUpload = new System.Windows.Forms.Button();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.labelPIB = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.dataGridViewPeople = new System.Windows.Forms.DataGridView();
            this.phoneBookBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.omniTrackerDataSet = new PBMangePhoto.OmniTrackerDataSet();
            this.openFileDialogImage = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialogImage = new System.Windows.Forms.SaveFileDialog();
            this.phoneBookTableAdapter = new PBMangePhoto.OmniTrackerDataSetTableAdapters.PhoneBookTableAdapter();
            this.pIBDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dobDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.departmentDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.jobtitleDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cityDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1.SuspendLayout();
            this.groupBoxSearch.SuspendLayout();
            this.groupBoxPhoto.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeople)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.phoneBookBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.omniTrackerDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelRec});
            this.statusStrip1.Location = new System.Drawing.Point(0, 531);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(792, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelRec
            // 
            this.toolStripStatusLabelRec.Name = "toolStripStatusLabelRec";
            this.toolStripStatusLabelRec.Size = new System.Drawing.Size(77, 17);
            this.toolStripStatusLabelRec.Text = "Not connected";
            // 
            // groupBoxSearch
            // 
            this.groupBoxSearch.Controls.Add(this.buttonShowAll);
            this.groupBoxSearch.Controls.Add(this.label3);
            this.groupBoxSearch.Controls.Add(this.label2);
            this.groupBoxSearch.Controls.Add(this.comboBoxCity);
            this.groupBoxSearch.Controls.Add(this.comboBoxDepartment);
            this.groupBoxSearch.Controls.Add(this.label1);
            this.groupBoxSearch.Controls.Add(this.textBoxSearch);
            this.groupBoxSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxSearch.Location = new System.Drawing.Point(0, 0);
            this.groupBoxSearch.Name = "groupBoxSearch";
            this.groupBoxSearch.Size = new System.Drawing.Size(792, 131);
            this.groupBoxSearch.TabIndex = 5;
            this.groupBoxSearch.TabStop = false;
            this.groupBoxSearch.Text = "Пошук";
            // 
            // buttonShowAll
            // 
            this.buttonShowAll.Enabled = false;
            this.buttonShowAll.Location = new System.Drawing.Point(73, 100);
            this.buttonShowAll.Name = "buttonShowAll";
            this.buttonShowAll.Size = new System.Drawing.Size(102, 23);
            this.buttonShowAll.TabIndex = 6;
            this.buttonShowAll.Text = "Показати всі";
            this.buttonShowAll.UseVisualStyleBackColor = true;
            this.buttonShowAll.Click += new System.EventHandler(this.buttonShowAll_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Місто";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Підрозділ";
            // 
            // comboBoxCity
            // 
            this.comboBoxCity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxCity.FormattingEnabled = true;
            this.comboBoxCity.Location = new System.Drawing.Point(73, 73);
            this.comboBoxCity.Name = "comboBoxCity";
            this.comboBoxCity.Size = new System.Drawing.Size(426, 21);
            this.comboBoxCity.TabIndex = 3;
            this.comboBoxCity.SelectedIndexChanged += new System.EventHandler(this.comboBoxCity_SelectedIndexChanged);
            // 
            // comboBoxDepartment
            // 
            this.comboBoxDepartment.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxDepartment.FormattingEnabled = true;
            this.comboBoxDepartment.Location = new System.Drawing.Point(73, 46);
            this.comboBoxDepartment.Name = "comboBoxDepartment";
            this.comboBoxDepartment.Size = new System.Drawing.Size(426, 21);
            this.comboBoxDepartment.TabIndex = 2;
            this.comboBoxDepartment.SelectedIndexChanged += new System.EventHandler(this.comboBoxDepartment_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ПІБ";
            // 
            // textBoxSearch
            // 
            this.textBoxSearch.Location = new System.Drawing.Point(73, 19);
            this.textBoxSearch.Name = "textBoxSearch";
            this.textBoxSearch.Size = new System.Drawing.Size(426, 20);
            this.textBoxSearch.TabIndex = 0;
            this.textBoxSearch.TextChanged += new System.EventHandler(this.textBoxSearch_TextChanged);
            // 
            // timerFind
            // 
            this.timerFind.Interval = 800;
            this.timerFind.Tick += new System.EventHandler(this.timerFind_Tick);
            // 
            // groupBoxPhoto
            // 
            this.groupBoxPhoto.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxPhoto.Controls.Add(this.buttonDelete);
            this.groupBoxPhoto.Controls.Add(this.buttonSaveImageAs);
            this.groupBoxPhoto.Controls.Add(this.buttonUpload);
            this.groupBoxPhoto.Controls.Add(this.buttonOpen);
            this.groupBoxPhoto.Controls.Add(this.labelPIB);
            this.groupBoxPhoto.Controls.Add(this.pictureBox1);
            this.groupBoxPhoto.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBoxPhoto.Location = new System.Drawing.Point(507, 131);
            this.groupBoxPhoto.Name = "groupBoxPhoto";
            this.groupBoxPhoto.Size = new System.Drawing.Size(285, 397);
            this.groupBoxPhoto.TabIndex = 7;
            this.groupBoxPhoto.TabStop = false;
            this.groupBoxPhoto.Text = "Фото";
            // 
            // buttonDelete
            // 
            this.buttonDelete.Image = global::PBMangePhoto.Properties.Resources.DeleteHS;
            this.buttonDelete.Location = new System.Drawing.Point(167, 113);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(106, 23);
            this.buttonDelete.TabIndex = 5;
            this.buttonDelete.Text = "Видалити";
            this.buttonDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // buttonSaveImageAs
            // 
            this.buttonSaveImageAs.Image = global::PBMangePhoto.Properties.Resources.saveHS;
            this.buttonSaveImageAs.Location = new System.Drawing.Point(167, 142);
            this.buttonSaveImageAs.Name = "buttonSaveImageAs";
            this.buttonSaveImageAs.Size = new System.Drawing.Size(106, 23);
            this.buttonSaveImageAs.TabIndex = 4;
            this.buttonSaveImageAs.Text = "Зберегти як";
            this.buttonSaveImageAs.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonSaveImageAs.UseVisualStyleBackColor = true;
            this.buttonSaveImageAs.Click += new System.EventHandler(this.buttonSaveImageAs_Click);
            // 
            // buttonUpload
            // 
            this.buttonUpload.Image = global::PBMangePhoto.Properties.Resources.import;
            this.buttonUpload.Location = new System.Drawing.Point(167, 84);
            this.buttonUpload.Name = "buttonUpload";
            this.buttonUpload.Size = new System.Drawing.Size(106, 23);
            this.buttonUpload.TabIndex = 3;
            this.buttonUpload.Text = "Завантажити";
            this.buttonUpload.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonUpload.UseVisualStyleBackColor = true;
            this.buttonUpload.Click += new System.EventHandler(this.buttonUpload_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Image = global::PBMangePhoto.Properties.Resources.OIpenHS;
            this.buttonOpen.Location = new System.Drawing.Point(167, 55);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(106, 23);
            this.buttonOpen.TabIndex = 2;
            this.buttonOpen.Text = "Вибрати файл";
            this.buttonOpen.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // labelPIB
            // 
            this.labelPIB.AutoSize = true;
            this.labelPIB.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPIB.Location = new System.Drawing.Point(8, 22);
            this.labelPIB.Name = "labelPIB";
            this.labelPIB.Size = new System.Drawing.Size(218, 13);
            this.labelPIB.TabIndex = 1;
            this.labelPIB.Text = "Виберіть потрібний запис у таблиці";
            this.labelPIB.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Silver;
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Location = new System.Drawing.Point(11, 55);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Padding = new System.Windows.Forms.Padding(5);
            this.pictureBox1.Size = new System.Drawing.Size(150, 150);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // dataGridViewPeople
            // 
            this.dataGridViewPeople.AllowUserToAddRows = false;
            this.dataGridViewPeople.AllowUserToDeleteRows = false;
            this.dataGridViewPeople.AllowUserToOrderColumns = true;
            this.dataGridViewPeople.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewPeople.AutoGenerateColumns = false;
            this.dataGridViewPeople.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPeople.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pIBDataGridViewTextBoxColumn,
            this.dobDataGridViewTextBoxColumn,
            this.departmentDataGridViewTextBoxColumn,
            this.jobtitleDataGridViewTextBoxColumn,
            this.cityDataGridViewTextBoxColumn});
            this.dataGridViewPeople.DataSource = this.phoneBookBindingSource;
            this.dataGridViewPeople.Location = new System.Drawing.Point(0, 137);
            this.dataGridViewPeople.Name = "dataGridViewPeople";
            this.dataGridViewPeople.ReadOnly = true;
            this.dataGridViewPeople.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewPeople.Size = new System.Drawing.Size(499, 391);
            this.dataGridViewPeople.TabIndex = 8;
            this.dataGridViewPeople.SelectionChanged += new System.EventHandler(this.dataGridViewPeople_SelectionChanged);
            // 
            // phoneBookBindingSource
            // 
            this.phoneBookBindingSource.DataMember = "PhoneBook";
            this.phoneBookBindingSource.DataSource = this.omniTrackerDataSet;
            // 
            // omniTrackerDataSet
            // 
            this.omniTrackerDataSet.DataSetName = "OmniTrackerDataSet";
            this.omniTrackerDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // openFileDialogImage
            // 
            this.openFileDialogImage.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|All files (*.*)|*" +
    ".*";
            // 
            // saveFileDialogImage
            // 
            this.saveFileDialogImage.Filter = "Images (*.BMP;*.JPG;*.GIF,*.PNG)|*.BMP;*.JPG;*.JPEG;*.GIF;*.PNG|All files (*.*)|*" +
    ".*";
            // 
            // phoneBookTableAdapter
            // 
            this.phoneBookTableAdapter.ClearBeforeFill = true;
            // 
            // pIBDataGridViewTextBoxColumn
            // 
            this.pIBDataGridViewTextBoxColumn.DataPropertyName = "PIB";
            this.pIBDataGridViewTextBoxColumn.HeaderText = "ПІБ";
            this.pIBDataGridViewTextBoxColumn.Name = "pIBDataGridViewTextBoxColumn";
            this.pIBDataGridViewTextBoxColumn.ReadOnly = true;
            this.pIBDataGridViewTextBoxColumn.Width = 200;
            // 
            // dobDataGridViewTextBoxColumn
            // 
            this.dobDataGridViewTextBoxColumn.DataPropertyName = "dob";
            dataGridViewCellStyle1.Format = "d";
            dataGridViewCellStyle1.NullValue = null;
            this.dobDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle1;
            this.dobDataGridViewTextBoxColumn.HeaderText = "Дата народження";
            this.dobDataGridViewTextBoxColumn.Name = "dobDataGridViewTextBoxColumn";
            this.dobDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // departmentDataGridViewTextBoxColumn
            // 
            this.departmentDataGridViewTextBoxColumn.DataPropertyName = "department";
            this.departmentDataGridViewTextBoxColumn.HeaderText = "Підрозділ";
            this.departmentDataGridViewTextBoxColumn.Name = "departmentDataGridViewTextBoxColumn";
            this.departmentDataGridViewTextBoxColumn.ReadOnly = true;
            this.departmentDataGridViewTextBoxColumn.Width = 200;
            // 
            // jobtitleDataGridViewTextBoxColumn
            // 
            this.jobtitleDataGridViewTextBoxColumn.DataPropertyName = "job_title";
            this.jobtitleDataGridViewTextBoxColumn.HeaderText = "Посада";
            this.jobtitleDataGridViewTextBoxColumn.Name = "jobtitleDataGridViewTextBoxColumn";
            this.jobtitleDataGridViewTextBoxColumn.ReadOnly = true;
            this.jobtitleDataGridViewTextBoxColumn.Width = 200;
            // 
            // cityDataGridViewTextBoxColumn
            // 
            this.cityDataGridViewTextBoxColumn.DataPropertyName = "city";
            this.cityDataGridViewTextBoxColumn.HeaderText = "Місто";
            this.cityDataGridViewTextBoxColumn.Name = "cityDataGridViewTextBoxColumn";
            this.cityDataGridViewTextBoxColumn.ReadOnly = true;
            this.cityDataGridViewTextBoxColumn.Width = 150;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(792, 553);
            this.Controls.Add(this.dataGridViewPeople);
            this.Controls.Add(this.groupBoxPhoto);
            this.Controls.Add(this.groupBoxSearch);
            this.Controls.Add(this.statusStrip1);
            this.Name = "FormMain";
            this.Text = "Mriya Phone Book - Manage photo";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.groupBoxSearch.ResumeLayout(false);
            this.groupBoxSearch.PerformLayout();
            this.groupBoxPhoto.ResumeLayout(false);
            this.groupBoxPhoto.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPeople)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.phoneBookBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.omniTrackerDataSet)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.GroupBox groupBoxSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxCity;
        private System.Windows.Forms.ComboBox comboBoxDepartment;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxSearch;
        private OmniTrackerDataSet omniTrackerDataSet;
        private System.Windows.Forms.BindingSource phoneBookBindingSource;
        private OmniTrackerDataSetTableAdapters.PhoneBookTableAdapter phoneBookTableAdapter;
        private System.Windows.Forms.Button buttonShowAll;
        private System.Windows.Forms.Timer timerFind;
        private System.Windows.Forms.GroupBox groupBoxPhoto;
        private System.Windows.Forms.DataGridView dataGridViewPeople;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRec;
        private System.Windows.Forms.Button buttonUpload;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.Label labelPIB;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonSaveImageAs;
        private System.Windows.Forms.OpenFileDialog openFileDialogImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialogImage;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn pIBDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn dobDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn departmentDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn jobtitleDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cityDataGridViewTextBoxColumn;
    }
}

