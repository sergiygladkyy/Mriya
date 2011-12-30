using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.SharePoint;

namespace ViewXmlApplication
{
    public partial class FormViewXml : Form
    {
        private bool dirtyFlag = false;
        private List<Guid> allLists = new List<Guid>(); //List IDs of Lists
        private List<Guid> listViews = new List<Guid>(); // View IDs of the selected list
        
        private SPSite site;
        private SPWeb web;


        public FormViewXml()
        {
            InitializeComponent();
            textBoxSiteAddress.Text = Properties.Settings.Default.SharepointUrl;
            buttonGet.Enabled = false;                        
        }



        private void buttonGo_Click(object sender, EventArgs e)
        {
            try
            {
                buttonGo.Enabled = false;
                string siteUrl = textBoxSiteAddress.Text;
                
                //using (SPWeb web = new SPSite(siteUrl).OpenWeb())
                //{
                site = new SPSite(siteUrl);
                web = site.OpenWeb();

                ShowMessage(string.Format("Connected to {0}",siteUrl));
                    
                comboBoxLists.Items.Clear();
                allLists.Clear();

                SPListCollection collSiteLists = web.Lists;
                foreach (SPList oList in collSiteLists)
                {
                    comboBoxLists.Items.Add(string.Format("{0} - {1}", web.Title, oList.Title ));
                    allLists.Add(oList.ID);
                            
                }

                comboBoxLists.DroppedDown = true;


                //}
            }
            catch (Exception ex)
            {
                buttonGo.Enabled = true;
                   
                string sError = string.Format("buttonGo_Click(): Error was occured while processing your request:{0}",
                    ex.ToString());
                ShowMessage(sError);
                
            }

        }


        private void buttonGet_Click(object sender, EventArgs e)
        {
            try
            {
                SPList selectedList = web.Lists.GetList(allLists[comboBoxLists.SelectedIndex],true);
                ClearListData();

                foreach (SPView spView in selectedList.Views)
                {
                    comboBoxListViews.Items.Add(spView.Title);
                    listViews.Add(spView.ID);
                }

                comboBoxListViews.DroppedDown = true;



            }
            catch (Exception ex)
            {
                string sError = string.Format("buttonGet_Click(): Error was occured while getting the list:{0}",
                    ex.ToString());
                ShowMessage(sError);
            }

        }

        #region Boilerplate
        private void ShowMessage(string message)
        {
            textBoxMessages.Text += message;
            textBoxMessages.Text += System.Environment.NewLine;
            textBoxMessages.Text += "------------------------------";
            textBoxMessages.Text += System.Environment.NewLine;
        }
            

        private void textBoxListAddress_Enter(object sender, EventArgs e)
        {

            this.textBoxSiteAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxSiteAddress.ForeColor = System.Drawing.SystemColors.ControlText;


        }

        private void textBoxListAddress_Leave(object sender, EventArgs e)
        {
            if (!dirtyFlag)
            {
                this.textBoxSiteAddress.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                this.textBoxSiteAddress.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            }

            if (textBoxSiteAddress.Text.Length == 0)   
                textBoxSiteAddress.Text = Properties.Settings.Default.SharepointUrl;
        }

        private void textBoxListAddress_TextChanged(object sender, EventArgs e)
        {
            if ((textBoxSiteAddress.Text.Length == 0) | (textBoxSiteAddress.Text == Properties.Settings.Default.SharepointUrl))
                dirtyFlag = false;
            else
                dirtyFlag = true;
        }



        /// <summary>
        /// Called when new list is going to be processed to clear the controls
        /// </summary>
        private void ClearListData()
        {
            comboBoxListViews.Items.Clear();
            comboBoxListViews.Text = string.Empty;
            textBoxViewXMLContent.Clear();
            listViews.Clear();
        }

        private void FormViewXml_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (web != null)
                web.Dispose();

            if (site != null)
                site.Dispose();
        }

        #endregion

        private void comboBoxLists_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedIndex == -1)
            {
                buttonGet.Enabled = false;
                return;
            }

            buttonGet.Enabled = true;
            ShowMessage("Selected List with ID:" + allLists[comboBox.SelectedIndex]);
            ClearListData();

        }

        /// <summary>
        /// Here we go with getting the View information
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void comboBoxListViews_SelectionChangeCommitted(object sender, EventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            if (comboBox.SelectedIndex == -1)
            {
                return;
            }

            try
            {            
                SPList selectedList = web.Lists.GetList(allLists[comboBoxLists.SelectedIndex],true);
                SPView spView = selectedList.Views[listViews[comboBoxListViews.SelectedIndex]];

                textBoxViewXMLContent.Text = spView.GetViewXml();

                ShowMessage(string.Format("View Url: {0}:", spView.Url));
                ShowMessage(string.Format("View XSL: {0}:", spView.Xsl));
                ShowMessage(string.Format("View XslLink: {0}:", spView.XslLink));
                ShowMessage(string.Format("View Query: {0}:", spView.Query));
                ShowMessage(string.Format("View Data: {0}:", spView.ViewData));
                ShowMessage(string.Format("View Body: {0}:", spView.ViewBody));
                ShowMessage(string.Format("View Fields: {0}:", spView.ViewFields));
                ShowMessage(string.Format("View SchemaXml: {0}:", spView.SchemaXml));
                ShowMessage(string.Format("View HtmlSchemaXml: {0}:", spView.HtmlSchemaXml));
                ShowMessage(string.Format("View CssStyleSheet: {0}:", spView.CssStyleSheet));
                ShowMessage(string.Format("View ParameterBindings: {0}:", spView.ParameterBindings));
                ShowMessage(string.Format("View BaseViewID: {0}:", spView.BaseViewID));
                ShowMessage(string.Format("View ContentTypeId: {0}:", spView.ContentTypeId));
                ShowMessage(string.Format("View Formats: {0}:", spView.Formats));
                ShowMessage(string.Format("View Empty: {0}:", spView.ViewEmpty));
                ShowMessage(string.Format("View as Html: {0}:", spView.RenderAsHtml()));
            }
            catch (Exception ex)
            {
                string sError = string.Format("comboBoxListViews_SelectionChangeCommitted(): Error was occured while getting the view:{0}",
                    ex.ToString());
                ShowMessage(sError);
            }

        }

        private void buttonMessagesClear_Click(object sender, EventArgs e)
        {
            textBoxMessages.Clear();
        }


  
    }
}
