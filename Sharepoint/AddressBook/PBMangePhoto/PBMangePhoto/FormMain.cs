using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace PBMangePhoto
{
    public partial class FormMain : Form
    {
        private bool bIgnoreNotification = false;
        private int nRecordID = 0;
        private bool bUploadPending = false;
        MriyaUserDataDataSet.UserPhotoRow rowPicture = null;

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                this.phoneBookTableAdapter.Fill(this.omniTrackerDataSet.PhoneBook);
                FillSearchCombos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при отриманні даних:\r\n" + ex.Message, "Підключення...",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pictureBox1.Refresh();
                return;
            }
            toolStripStatusLabelRec.Text = String.Format("Знайдено {0} записів", this.omniTrackerDataSet.PhoneBook.Count);
        }
        
        private void buttonShowAll_Click(object sender, EventArgs e)
        {
            timerFind.Enabled = false;
            bIgnoreNotification = true;
            textBoxSearch.Text = "";
            comboBoxDepartment.SelectedIndex = 0;
            comboBoxCity.SelectedIndex = 0;
            bIgnoreNotification = false;
            ShowAllrecords();
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            timerFind.Enabled = false;

            if (bIgnoreNotification)
                return;

            timerFind.Enabled = true;
        }

        private void comboBoxDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bIgnoreNotification)
                return;
            ShowFilteredRecords();
        }

        private void comboBoxCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bIgnoreNotification)
                return;
            ShowFilteredRecords();
        }

        private void timerFind_Tick(object sender, EventArgs e)
        {
            timerFind.Enabled = false;
            ShowFilteredRecords();
        }


        private void dataGridViewPeople_SelectionChanged(object sender, EventArgs e)
        {
            EnablePictureControls(false);
            if (dataGridViewPeople.CurrentRow == null)
                return;

            DataRowView rw = dataGridViewPeople.CurrentRow.DataBoundItem as DataRowView;

            if (rw == null)
                return;

            nRecordID = Convert.ToInt32(rw["id"]);
            labelPIB.Text = rw["PIB"].ToString();

            ReadPictureFromDB();

            EnablePictureControls(true);
        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            if (openFileDialogImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                pictureBox1.Image = System.Drawing.Image.FromFile(openFileDialogImage.FileName);
                bUploadPending = true;
                buttonUpload.Enabled = true & (nRecordID > 0);
                buttonDelete.Enabled = true;
                buttonSaveImageAs.Enabled = false;
            }
        }

        private void buttonUpload_Click(object sender, EventArgs e)
        {
            UploadPictureToDB();
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            DeletePicture();
        }

        private void buttonSaveImageAs_Click(object sender, EventArgs e)
        {
            SavePicture();
        }

        private void FillSearchCombos()
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionStringOmniTrackerDb);
            SqlCommand sqlDepartments = new SqlCommand("SELECT DISTINCT u407.f16_abteilun AS department " +
                "FROM " +
                "[OmniTracker].[dbo].[UserFields407] AS u407 " +
                "WHERE " +
                "u407.f16_abteilun IS NOT NULL AND u407.f16_abteilun > '' " +
                "ORDER BY [department]", connection);
            SqlCommand sqlCities = new SqlCommand("SELECT DISTINCT u407.f32_ad AS city " +
                "FROM " +
                "[OmniTracker].[dbo].[UserFields407] AS u407 " +
                "WHERE " +
                "u407.f32_ad IS NOT NULL AND u407.f32_ad > '' " +
                "ORDER BY [city]", connection);

            comboBoxDepartment.Items.Clear();
            comboBoxDepartment.Items.Add("");
            comboBoxCity.Items.Clear();
            comboBoxCity.Items.Add("");

            connection.Open();

            SqlDataReader readerDepartments = sqlDepartments.ExecuteReader();
            if (readerDepartments != null)
            {
                while (readerDepartments.Read())
                    comboBoxDepartment.Items.Add(readerDepartments[0]);
                readerDepartments.Close();
            }

            SqlDataReader readerCities = sqlCities.ExecuteReader();
            if (readerCities != null)
            {
                while (readerCities.Read())
                    comboBoxCity.Items.Add(readerCities[0]);
                readerCities.Close();
            }

            connection.Close();
        }

        void ShowAllrecords()
        {
            UseWaitCursor = true;
            this.phoneBookTableAdapter.Fill(this.omniTrackerDataSet.PhoneBook);
            buttonShowAll.Enabled = false;
            toolStripStatusLabelRec.Text = String.Format("Знайдено {0} записів", this.omniTrackerDataSet.PhoneBook.Count);
            UseWaitCursor = false;
        }

        private void ShowFilteredRecords()
        {
            UseWaitCursor = true;
            string sFilterPIB = (textBoxSearch.Text.Trim().Length > 0) ?
                (String.Format("%{0}%", SanitizeInput(textBoxSearch.Text))) : ("%");
            string sFilterCity = (comboBoxCity.SelectedIndex > 0) ?
                (comboBoxCity.SelectedItem.ToString()) : ("%");
            string sFilterDepartment = (comboBoxDepartment.SelectedIndex > 0) ?
                (comboBoxDepartment.SelectedItem.ToString()) : ("%");
            this.phoneBookTableAdapter.FillFiltered(this.omniTrackerDataSet.PhoneBook, pib: sFilterPIB,
                department: sFilterDepartment, city: sFilterCity);
            toolStripStatusLabelRec.Text = String.Format("Знайдено {0} записів", this.omniTrackerDataSet.PhoneBook.Count);
            buttonShowAll.Enabled = true;
            UseWaitCursor = false;
        }

        private string SanitizeInput(string input)
        {
            string sSanitized = input.ToLower().Trim();
            sSanitized = sSanitized.Replace("%", "\\%");
            sSanitized = sSanitized.Replace("'", "\\'");
            sSanitized = sSanitized.Replace("'", "\\\"");
            return sSanitized;
        }

        private void EnablePictureControls(bool enable)
        {
            if (enable == false)
            {
                labelPIB.Text = "Виберіть потрібний запис у таблиці";
                pictureBox1.Image = null;
                bUploadPending = false;
                rowPicture = null;
                nRecordID = 0;
            }
            pictureBox1.Enabled = enable;
            buttonOpen.Enabled = enable;
            buttonUpload.Enabled = enable & bUploadPending;
            buttonDelete.Enabled = enable & (rowPicture != null);
            buttonSaveImageAs.Enabled = enable & (rowPicture != null);
        }

        private void ReadPictureFromDB()
        {
            MriyaUserDataDataSet.UserPhotoDataTable tablePhoto = new MriyaUserDataDataSet.UserPhotoDataTable();
            MriyaUserDataDataSetTableAdapters.UserPhotoTableAdapter tablePhotoAdapter = new MriyaUserDataDataSetTableAdapters.UserPhotoTableAdapter();

            rowPicture = null;
            pictureBox1.Image = null;

            if (nRecordID < 1)
                return;

            tablePhoto.Clear();
            try
            {
                tablePhotoAdapter.FillById(tablePhoto, nRecordID);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Помилка завантаження фотографії:\r\n" + ex.Message, "Загрузка фото",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pictureBox1.Refresh();
                return;
            }
            if (tablePhoto.Rows.Count > 0)
                rowPicture = tablePhoto.Rows[0] as MriyaUserDataDataSet.UserPhotoRow;

            if (rowPicture != null)
            {
                System.IO.MemoryStream streamMem = new System.IO.MemoryStream(rowPicture.PhotoBinary);
                pictureBox1.Image = System.Drawing.Image.FromStream(streamMem);
            }
            else
            {
                pictureBox1.Refresh();
            }
            bUploadPending = false;
            buttonUpload.Enabled = false;
            buttonDelete.Enabled = (rowPicture != null);
            buttonSaveImageAs.Enabled = (rowPicture != null);
        }

        private void UploadPictureToDB()
        {
            if (!bUploadPending)
                return;

            if (!System.IO.Directory.Exists(openFileDialogImage.FileName) == false)
            {
                MessageBox.Show("Файл \"" + openFileDialogImage.FileName + "\" не знайден!", "Загрузка фото",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                bUploadPending = false;
                buttonUpload.Enabled = false;
                return;
            }

            try
            {

                Image image = System.Drawing.Image.FromFile(openFileDialogImage.FileName);
                Byte[] byteImage = System.IO.File.ReadAllBytes(openFileDialogImage.FileName);
                string sFileName = System.IO.Path.GetFileNameWithoutExtension(openFileDialogImage.FileName);
                string sFileExt = System.IO.Path.GetExtension(openFileDialogImage.FileName);
                int nSize = byteImage.Length;
                int nWidth = image.Width;
                int nHeight = image.Height;

                if (rowPicture != null)
                    UpdatePicture(image, byteImage, sFileName, sFileExt, nSize, nWidth, nHeight);
                else
                    InsertPicture(image, byteImage, sFileName, sFileExt, nSize, nWidth, nHeight);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка під час завантаження файлу:\r\n" + ex.Message,
                    "Завантажити файл", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            bUploadPending = false;
            buttonUpload.Enabled = false;

            ReadPictureFromDB();
        }

        private void UpdatePicture(Image image, Byte[] byteImage, string sFileName, 
            string sFileExt, int nSize, int nWidth, int nHeight)
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionStringMriyaDataDb);
            SqlCommand cmd = new SqlCommand("UPDATE [UserPhoto] SET [PhotoBinary]=@PhotoBinary, [PhotoFileName]=@PhotoFileName, [PhotoFileExtension]=@PhotoFileExtension, [PhotoSize]=@PhotoSize, [PhotoWidth]=@PhotoWidth, [PhotoHeight]=@PhotoHeight, [TimeStamp]=@TimeStamp, [Description]=@Description WHERE [Id]=@Id;", 
                connection);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = nRecordID;
            cmd.Parameters.Add("@PhotoBinary", SqlDbType.VarBinary).Value = byteImage;
            cmd.Parameters.Add("@PhotoFileName", SqlDbType.NVarChar, 255).Value = sFileName;
            cmd.Parameters.Add("@PhotoFileExtension", SqlDbType.NVarChar, 10).Value = sFileExt;
            cmd.Parameters.Add("@PhotoSize", SqlDbType.Int).Value = nSize;
            cmd.Parameters.Add("@PhotoWidth", SqlDbType.Int).Value = nWidth;
            cmd.Parameters.Add("@PhotoHeight", SqlDbType.Int).Value = nHeight;
            cmd.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = "";

            connection.Open();
            cmd.ExecuteScalar();
            connection.Close();
        }

        private void InsertPicture(Image image, Byte[] byteImage, string sFileName, 
            string sFileExt, int nSize, int nWidth, int nHeight)
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionStringMriyaDataDb);
            SqlCommand cmd = new SqlCommand("INSERT INTO [UserPhoto] ([Id], [PhotoBinary], [PhotoFileName], [PhotoFileExtension], [PhotoSize], [PhotoWidth], [PhotoHeight], [TimeStamp], [Description]) VALUES (@Id, @PhotoBinary, @PhotoFileName, @PhotoFileExtension, @PhotoSize, @PhotoWidth, @PhotoHeight, @TimeStamp, @Description);", 
                connection);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = nRecordID;
            cmd.Parameters.Add("@PhotoBinary", SqlDbType.VarBinary).Value = byteImage;
            cmd.Parameters.Add("@PhotoFileName", SqlDbType.NVarChar, 255).Value = sFileName;
            cmd.Parameters.Add("@PhotoFileExtension", SqlDbType.NVarChar, 10).Value = sFileExt;
            cmd.Parameters.Add("@PhotoSize", SqlDbType.Int).Value = nSize;
            cmd.Parameters.Add("@PhotoWidth", SqlDbType.Int).Value = nWidth;
            cmd.Parameters.Add("@PhotoHeight", SqlDbType.Int).Value = nHeight;
            cmd.Parameters.Add("@TimeStamp", SqlDbType.DateTime).Value = DateTime.Now;
            cmd.Parameters.Add("@Description", SqlDbType.NVarChar).Value = "";

            connection.Open();
            cmd.ExecuteScalar();
            connection.Close();
        }
        
        private void DeletePicture()
        {
            SqlConnection connection = new SqlConnection(Properties.Settings.Default.connectionStringMriyaDataDb);
            SqlCommand cmd = new SqlCommand("DELETE FROM [UserPhoto] WHERE Id=@Id;",
                connection);
            cmd.Parameters.Add("@Id", SqlDbType.Int).Value = nRecordID;

            try
            {
                connection.Open();
                cmd.ExecuteScalar();
                connection.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Сталася помилка під час видалення фотографії:\r\n" + ex.Message,
                    "Видалення фотографії", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ReadPictureFromDB();
        }

        private void SavePicture()
        {
            if (rowPicture == null)
                return;

            saveFileDialogImage.FileName = rowPicture.PhotoFileName + rowPicture.PhotoFileExtension;
            if (saveFileDialogImage.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllBytes(saveFileDialogImage.FileName, rowPicture.PhotoBinary);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Сталася помилка під час спроби збереження файлу:\r\n" + ex.Message,
                        "Зберегти файл", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
