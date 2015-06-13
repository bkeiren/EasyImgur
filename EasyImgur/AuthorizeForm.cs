using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EasyImgur
{
    public partial class AuthorizeForm : Form
    {
        public AuthorizeForm()
        {
            InitializeComponent();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Store credentials.
            // CredentialsHelper.StoreCredentials(textBoxPIN.Text, maskedTextBoxAccountPassword.Text);
            
            // Do stuff with the PIN number (requesting access tokens and such).
            ImgurAPI.RequestTokens(textBoxPIN.Text);
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AccountCredentialsForm_Load(object sender, EventArgs e)
        {
            //CredentialsHelper.Credentials credentials = CredentialsHelper.RetrieveCredentials();
            //textBoxPIN.Text = credentials.name;
            //maskedTextBoxAccountPassword.Text = credentials.p;

            ImgurAPI.OpenAuthorizationPage();
        }
    }
}
