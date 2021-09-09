using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class frmLauncher : Form
    {

        public frmLauncher()
        {
            InitializeComponent();

            this.client = new Client();
            this.client.OnActivity += Client_OnActivity;

            // Setup UI Display
            pnlSide.Height = btnAccount.Height;
            pnlSide.Top = btnAccount.Top;
            pnlSide.Left = btnAccount.Left;
            btnAccount.BackColor = Color.FromArgb(57, 62, 70);
        }

        #region ---Network Controllers---
        
        private Client client;

        #endregion

        #region ---Form Controllers---

        private void Client_OnActivity(object sender, string e)
        {
            this.pnlAccountView.Invoke((Action)delegate
            {
                this.pnlAccountView.Visible = true;
           });
        }

        #endregion

        #region ---Move Window---

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        
        private void frmLauncher_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        
        #endregion

        #region ---Button Controllers---
       
        private void btnAccount_Click(object sender, EventArgs e)
        {
            pnlSide.Height = btnAccount.Height;
            pnlSide.Top = btnAccount.Top;
            pnlSide.Left = btnAccount.Left;
            btnAccount.BackColor = Color.FromArgb(57, 62, 70);
            pnlAccountCreate.Visible = false;
        }
        
        private void btnCreate_Click(object sender, EventArgs e)
        {
            pnlSide.Height = btnCreate.Height;
            pnlSide.Top = btnCreate.Top;
            pnlSide.Left = btnCreate.Left;
            btnCreate.BackColor = Color.FromArgb(57, 62, 70);
            pnlAccountCreate.Visible = true;
        }
        
        private void btnNCreate_Click(object sender, EventArgs e)
        {
            //TODO: Creation Stuff
            btnNCreate.Enabled = false;
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        
        private void btnCreate_Leave(object sender, EventArgs e)
        {
            btnCreate.BackColor = Color.FromArgb(34, 40, 49);
        }
        
        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            btnAuthenticate.Enabled = false;
            // TODO: Authentication Stuff
            this.client.Start(IPAddress.Loopback, 4444);
        }
        
        private void btnCharcterCreate_Click(object sender, EventArgs e)
        {
            btnCharcterCreate.Enabled = false;
        }
        
        private void btnAccount_Leave(object sender, EventArgs e)
        {
            btnAccount.BackColor = Color.FromArgb(34, 40, 49);
        }
        
        #endregion

        #region ---Form Design---

        private void txtAccountName_Enter(object sender, EventArgs e)
        {
           if (txtAccountName.Text == "Account Name")
            {
                txtAccountName.Text = "";
            }
        }
        
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.PasswordChar = '*';
            }
        }
        
        private void txtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == "")
            {
                txtPassword.Text = "Password";
                txtPassword.PasswordChar = '\0';
            }
        }
        
        private void txtAccountName_Leave(object sender, EventArgs e)
        {
            if (txtAccountName.Text.Trim() == "")
            {
                txtAccountName.Text = "Account Name";
            }
        }
        
        private void txtNAccount_Enter(object sender, EventArgs e)
        {
            if (txtNAccount.Text == "Account Name")
            {
                txtNAccount.Text = "";
            }
        }
        
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "E-mail Address")
            {
                txtEmail.Text = "";
            }
        }
        
        private void txtNPassword_Enter(object sender, EventArgs e)
        {
            if (txtNPassword.Text == "Password")
            {
                txtNPassword.Text = "";
                txtNPassword.PasswordChar = '*';
            }
        }
        
        private void txtNVerifyPassword_Enter(object sender, EventArgs e)
        {
            if (txtNVerifyPassword.Text == "Verify Password")
            {
                txtNVerifyPassword.Text = "";
                txtNVerifyPassword.PasswordChar = '*';
            }
        }
        
        private void txtNAccount_Leave(object sender, EventArgs e)
        {
            if (txtNAccount.Text.Trim() == "")
            {
                txtNAccount.Text = "Account Name";
            }
        }
        
        private void txtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                txtEmail.Text = "E-mail Address";
            }
        }
        
        private void txtNPassword_Leave(object sender, EventArgs e)
        {
            if (txtNPassword.Text.Trim() == "")
            {
                txtNPassword.Text = "Password";
                txtNPassword.PasswordChar = '\0';
            }
        }
        
        private void txtNVerifyPassword_Leave(object sender, EventArgs e)
        {
            if (txtNVerifyPassword.Text.Trim() == "")
            {
                txtNVerifyPassword.Text = "Verify Password";
                txtNVerifyPassword.PasswordChar = '\0';
            }
        }
        
        #endregion

    }
}
