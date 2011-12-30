namespace ViewXmlApplication
{
    partial class FormViewXml
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
            this.buttonGo = new System.Windows.Forms.Button();
            this.textBoxSiteAddress = new System.Windows.Forms.TextBox();
            this.textBoxViewXMLContent = new System.Windows.Forms.TextBox();
            this.labelViewXML = new System.Windows.Forms.Label();
            this.textBoxMessages = new System.Windows.Forms.TextBox();
            this.labelMessages = new System.Windows.Forms.Label();
            this.comboBoxLists = new System.Windows.Forms.ComboBox();
            this.buttonGet = new System.Windows.Forms.Button();
            this.comboBoxListViews = new System.Windows.Forms.ComboBox();
            this.labelListView = new System.Windows.Forms.Label();
            this.labelList = new System.Windows.Forms.Label();
            this.buttonMessagesClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonGo
            // 
            this.buttonGo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGo.Location = new System.Drawing.Point(533, 13);
            this.buttonGo.Name = "buttonGo";
            this.buttonGo.Size = new System.Drawing.Size(71, 23);
            this.buttonGo.TabIndex = 1;
            this.buttonGo.Text = "Go";
            this.buttonGo.UseVisualStyleBackColor = true;
            this.buttonGo.Click += new System.EventHandler(this.buttonGo_Click);
            // 
            // textBoxSiteAddress
            // 
            this.textBoxSiteAddress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxSiteAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSiteAddress.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.textBoxSiteAddress.Location = new System.Drawing.Point(12, 15);
            this.textBoxSiteAddress.Name = "textBoxSiteAddress";
            this.textBoxSiteAddress.Size = new System.Drawing.Size(515, 20);
            this.textBoxSiteAddress.TabIndex = 0;
            this.textBoxSiteAddress.Text = "Site Address";
            this.textBoxSiteAddress.TextChanged += new System.EventHandler(this.textBoxListAddress_TextChanged);
            this.textBoxSiteAddress.Enter += new System.EventHandler(this.textBoxListAddress_Enter);
            this.textBoxSiteAddress.Leave += new System.EventHandler(this.textBoxListAddress_Leave);
            // 
            // textBoxViewXMLContent
            // 
            this.textBoxViewXMLContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxViewXMLContent.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxViewXMLContent.Location = new System.Drawing.Point(12, 141);
            this.textBoxViewXMLContent.Multiline = true;
            this.textBoxViewXMLContent.Name = "textBoxViewXMLContent";
            this.textBoxViewXMLContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxViewXMLContent.Size = new System.Drawing.Size(592, 193);
            this.textBoxViewXMLContent.TabIndex = 4;
            // 
            // labelViewXML
            // 
            this.labelViewXML.AutoSize = true;
            this.labelViewXML.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelViewXML.Location = new System.Drawing.Point(8, 126);
            this.labelViewXML.Name = "labelViewXML";
            this.labelViewXML.Size = new System.Drawing.Size(83, 13);
            this.labelViewXML.TabIndex = 6;
            this.labelViewXML.Text = "List ViewXML";
            // 
            // textBoxMessages
            // 
            this.textBoxMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxMessages.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxMessages.Location = new System.Drawing.Point(12, 365);
            this.textBoxMessages.Multiline = true;
            this.textBoxMessages.Name = "textBoxMessages";
            this.textBoxMessages.ReadOnly = true;
            this.textBoxMessages.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxMessages.Size = new System.Drawing.Size(592, 164);
            this.textBoxMessages.TabIndex = 5;
            // 
            // labelMessages
            // 
            this.labelMessages.AutoSize = true;
            this.labelMessages.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMessages.Location = new System.Drawing.Point(12, 346);
            this.labelMessages.Name = "labelMessages";
            this.labelMessages.Size = new System.Drawing.Size(63, 13);
            this.labelMessages.TabIndex = 7;
            this.labelMessages.Text = "Messages";
            // 
            // comboBoxLists
            // 
            this.comboBoxLists.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxLists.FormattingEnabled = true;
            this.comboBoxLists.Location = new System.Drawing.Point(12, 55);
            this.comboBoxLists.Name = "comboBoxLists";
            this.comboBoxLists.Size = new System.Drawing.Size(515, 21);
            this.comboBoxLists.TabIndex = 2;
            this.comboBoxLists.SelectionChangeCommitted += new System.EventHandler(this.comboBoxLists_SelectionChangeCommitted);
            // 
            // buttonGet
            // 
            this.buttonGet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonGet.Location = new System.Drawing.Point(533, 55);
            this.buttonGet.Name = "buttonGet";
            this.buttonGet.Size = new System.Drawing.Size(71, 23);
            this.buttonGet.TabIndex = 3;
            this.buttonGet.Text = "Get";
            this.buttonGet.UseVisualStyleBackColor = true;
            this.buttonGet.Click += new System.EventHandler(this.buttonGet_Click);
            // 
            // comboBoxListViews
            // 
            this.comboBoxListViews.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxListViews.FormattingEnabled = true;
            this.comboBoxListViews.Location = new System.Drawing.Point(12, 101);
            this.comboBoxListViews.Name = "comboBoxListViews";
            this.comboBoxListViews.Size = new System.Drawing.Size(515, 21);
            this.comboBoxListViews.TabIndex = 8;
            this.comboBoxListViews.SelectionChangeCommitted += new System.EventHandler(this.comboBoxListViews_SelectionChangeCommitted);
            // 
            // labelListView
            // 
            this.labelListView.AutoSize = true;
            this.labelListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelListView.Location = new System.Drawing.Point(8, 87);
            this.labelListView.Name = "labelListView";
            this.labelListView.Size = new System.Drawing.Size(58, 13);
            this.labelListView.TabIndex = 9;
            this.labelListView.Text = "List View";
            // 
            // labelList
            // 
            this.labelList.AutoSize = true;
            this.labelList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelList.Location = new System.Drawing.Point(8, 40);
            this.labelList.Name = "labelList";
            this.labelList.Size = new System.Drawing.Size(27, 13);
            this.labelList.TabIndex = 10;
            this.labelList.Text = "List";
            // 
            // buttonMessagesClear
            // 
            this.buttonMessagesClear.Font = new System.Drawing.Font("Arial", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonMessagesClear.Location = new System.Drawing.Point(529, 340);
            this.buttonMessagesClear.Name = "buttonMessagesClear";
            this.buttonMessagesClear.Size = new System.Drawing.Size(75, 22);
            this.buttonMessagesClear.TabIndex = 11;
            this.buttonMessagesClear.Text = "Clear";
            this.buttonMessagesClear.UseVisualStyleBackColor = true;
            this.buttonMessagesClear.Click += new System.EventHandler(this.buttonMessagesClear_Click);
            // 
            // FormViewXml
            // 
            this.AcceptButton = this.buttonGo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(616, 533);
            this.Controls.Add(this.buttonMessagesClear);
            this.Controls.Add(this.labelList);
            this.Controls.Add(this.labelListView);
            this.Controls.Add(this.comboBoxListViews);
            this.Controls.Add(this.buttonGet);
            this.Controls.Add(this.comboBoxLists);
            this.Controls.Add(this.labelMessages);
            this.Controls.Add(this.textBoxMessages);
            this.Controls.Add(this.labelViewXML);
            this.Controls.Add(this.textBoxViewXMLContent);
            this.Controls.Add(this.textBoxSiteAddress);
            this.Controls.Add(this.buttonGo);
            this.Name = "FormViewXml";
            this.Text = "Sharepoint View Explorer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormViewXml_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonGo;
        private System.Windows.Forms.TextBox textBoxSiteAddress;
        private System.Windows.Forms.TextBox textBoxViewXMLContent;
        private System.Windows.Forms.Label labelViewXML;
        private System.Windows.Forms.TextBox textBoxMessages;
        private System.Windows.Forms.Label labelMessages;
        private System.Windows.Forms.ComboBox comboBoxLists;
        private System.Windows.Forms.Button buttonGet;
        private System.Windows.Forms.ComboBox comboBoxListViews;
        private System.Windows.Forms.Label labelListView;
        private System.Windows.Forms.Label labelList;
        private System.Windows.Forms.Button buttonMessagesClear;
    }
}

