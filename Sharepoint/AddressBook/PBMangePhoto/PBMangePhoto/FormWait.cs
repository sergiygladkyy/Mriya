using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PBMangePhoto
{
    public partial class FormWait : Form
    {
        private string message;

        public string Message
        {
            get { return message; }
            set { 
                message = value;
                labelMessage.Text = message;
            }
        }
	
        public FormWait(string message)
        {
            InitializeComponent();
            Show(message);
        }

        public FormWait()
        {
            InitializeComponent();
        }

        public void Show(string message)
        {
            this.message = message;
            Show();
            Update();
        }

        private void WaitForm_Load(object sender, EventArgs e)
        {
            labelMessage.Text = message;
        }

        private void labelMessage_Click(object sender, EventArgs e)
        {

        }
    }
}