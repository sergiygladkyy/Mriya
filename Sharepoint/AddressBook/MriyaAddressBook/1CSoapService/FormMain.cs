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
        private UserProfilesDAL.TableProfiles tableProfilesSP = new UserProfilesDAL.TableProfiles();
        private UserProfilesDAL.TableProfiles tableProfilesSPDal = new UserProfilesDAL.TableProfiles();
        private DateTime t1Async = DateTime.Now;
        private DateTime t2Async = DateTime.Now;

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
            t1Async = t2Async = DateTime.Now;
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

                t2Async = DateTime.Now;
                labelSoapStatus.Text = string.Format("Received {0} records, t = {1:N2} sec",
                    e.Result.Count(), (double)((t2Async - t1Async).TotalMilliseconds / 1000)); 
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
                DateTime t1 = DateTime.Now;
                
                tableProfiles = UserProfilesDAL.Import1CUserProfiles.ImportRecords(
                    Properties.Settings.Default.soapServerUrl,
                    Properties.Settings.Default.soapServerUser,
                    Properties.Settings.Default.soapServerPassword
                    );
                dataGridViewABDal.DataSource = tableProfiles.GetProfiles();
                dataGridViewABDal.Refresh();

                DateTime t2 = DateTime.Now;
                labelABDalStatus.Text = string.Format("Received {0} records, t = {1:N2} sec",
                    tableProfiles.Count, (double)((t2 - t1).TotalMilliseconds / 1000));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("The error was occured while importing data from {0}\nThe error message:{1}\n",
                    Properties.Settings.Default.soapServerUrl, ex.Message), "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonbuttonGetSPData_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime t1 = DateTime.Now;

                tableProfilesSP.ReadSPProfiles(Properties.Settings.Default.spSiteUrl);
                dataGridViewSP.DataSource = tableProfilesSP.GetProfiles();
                dataGridViewSP.Refresh();

                DateTime t2 = DateTime.Now;
                labelSPStatus.Text = string.Format("Received {0} records, t = {1:N2} sec",
                    tableProfilesSP.Count, (double)((t2 - t1).TotalMilliseconds / 1000));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("The error was occured while importing from sharepoint server '{0}'\nThe error message:{1}\n",
                    Properties.Settings.Default.spSiteUrl, ex.Message), "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void buttonbuttonGetSPDataDal_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime t1 = DateTime.Now;

                tableProfilesSPDal.ReadSqlSPProfiles(Properties.Settings.Default.spSqlConnectionString);
                dataGridViewSPDal.DataSource = tableProfilesSPDal.GetProfiles();
                dataGridViewSPDal.Refresh();

                DateTime t2 = DateTime.Now;
                labelSPDalStatus.Text = string.Format("Received {0} records, t = {1:N2} sec",
                    tableProfilesSPDal.Count, (double)((t2 - t1).TotalMilliseconds / 1000));
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format("The error was occured while importing from sharepoint server '{0}'\nThe error message:{1}\n",
                    Properties.Settings.Default.spSiteUrl, ex.Message), "Error!", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }
    }
}
