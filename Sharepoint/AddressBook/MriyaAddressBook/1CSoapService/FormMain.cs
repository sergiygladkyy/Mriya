using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace _1CSoapService
{
    public partial class FormMain : Form
    {
        private test1c_AddressBook.AddressBook _ab = new test1c_AddressBook.AddressBook();
        private UserProfilesDAL.TableProfiles tableProfiles = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }


        private void configureConnectionTo1CSOAPServerToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        
        private void buttonGetSoapData_Click(object sender, EventArgs e)
        {
            _ab.Url = Properties.Settings.Default.soapServerUrl;
            _ab.Credentials = new System.Net.NetworkCredential(
                Properties.Settings.Default.soapServerUser,
                Properties.Settings.Default.soapServerPassword);
            _ab.GetListCompleted += new test1c_AddressBook.GetListCompletedEventHandler(_ab_GetListCompleted);
            _ab.GetListAsync();
            Cursor = Cursors.WaitCursor;
        }

        void _ab_GetListCompleted(object sender, test1c_AddressBook.GetListCompletedEventArgs e)
        {
            Cursor = Cursors.Default;

            if (e.Error!= null)
            {
                MessageBox.Show(e.Error.Message.ToString(), "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (e.Result != null)
            {
                dataGridViewABSoap.DataSource = e.Result;
                dataGridViewABSoap.Refresh();
            }
            else
            {
                MessageBox.Show("No data was received");
            }
        }

        private void buttonGetDALData_Click(object sender, EventArgs e)
        {
            try
            {
                tableProfiles = UserProfilesDAL.Import1CUserProfiles.ImportRecords(
                    Properties.Settings.Default.soapServerUrl,
                    Properties.Settings.Default.soapServerUser,
                    Properties.Settings.Default.soapServerPassword
                    );
                dataGridViewABDal.DataSource = tableProfiles.GetProfiles();
                dataGridViewABDal.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("The error was occured while importing data from {0}\nThe error message:{1}",
                    Properties.Settings.Default.soapServerUrl, ex.ToString()), "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
