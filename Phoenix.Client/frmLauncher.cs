using Phoenix.Common;
using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class frmLauncher : Form
    {
        public bool authMode = false;
        
        public frmLauncher()
        {
            InitializeComponent();

            this.client = new Client();
            this.client.OnActivity += Client_OnActivity;
            this.client.IsConnected += Client_IsConnected;
            this.client.IsClosed += Client_IsClosed;

            // Setup UI Display
            pnlSide.Height = btnAccount.Height;
            pnlSide.Top = btnAccount.Top;
            pnlSide.Left = btnAccount.Left;
            btnAccount.BackColor = Color.FromArgb(57, 62, 70);
        }

        #region -- Network Controllers --
        
        private Client client;

        #endregion

        #region -- Data Controllers --

        /// <summary>
        /// Sends command to server.
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(Command command)
        {
            var message = CommandFactory.FormatCommand(command);
            client.Send(message);
        }

        #endregion

        #region -- Client Controllers --

        /// <summary>
        /// Handles closed client connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="remote"></param>
        private void Client_IsClosed(object sender, bool remote)
        {
            switch (remote)
            {
                case true:
                    MessageBox.Show("The connection was closed by the server. The client will now close.", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                default:
                    MessageBox.Show("The connection was interrupted for unknown reasons. The client will now close.", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Application.Exit();
                    return;
            }
        }

        /// <summary>
        /// Handles connected client connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isReconnected"></param>
        private void Client_IsConnected(object sender, bool isReconnected)
        {
            if (authMode)
            {
                var authCommand = new AuthenticateCommand();
                authCommand.Username = txtAccountName.Text.Trim();
                authCommand.Password = txtPassword.Text.Trim();
                SendCommand(authCommand);
            }
            else
            {
                //TODO: Account Create
            }
 
        }

        /// <summary>
        /// Handles incoming message from the client connection.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Client_OnActivity(object sender, string e)
        {
            var command = CommandFactory.ParseCommand(e);
            switch (command.CommandType)
            {
                case CommandType.AuthenticateResponse:
                    this.pnlAuthenicate.Invoke((Action)delegate
                    {
                        btnAccount.Enabled = false;
                        btnCreate.Enabled = false;
                        lblAccount.Text = this.txtAccountName.Text;
                        this.txtAccountName.Text = "";
                        this.txtPassword.Text = "";
                        pnlAuthenicate.Hide();
                        pnlAccountView.Show();
                    });
                    return;
            }
        }

        #endregion

        #region -- Move Window --

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

        #region -- Button Controllers --
       
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
            btnNCreate.Enabled = false;
            btnAccount.Enabled = false;
            btnCreate.Enabled = false;
            authMode = false;
            this.client.Start(IPAddress.Loopback, 4444);
        }
        
        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnCharacterCreate_Click(object sender, EventArgs e)
        {
            pnlCharacterCreation.Show();
            pnlAccountView.Hide();
        }

        private void btnCreate_Leave(object sender, EventArgs e)
        {
            btnCreate.BackColor = Color.FromArgb(34, 40, 49);
        }
        
        private void btnAuthenticate_Click(object sender, EventArgs e)
        {
            btnAuthenticate.Enabled = false;
            authMode = true;
            this.client.Start(IPAddress.Loopback, 4444);
        }
        
        private void btnCharcterCreate_Click(object sender, EventArgs e)
        {
            btnNCharcterCreate.Enabled = false;
        }
        
        private void btnAccount_Leave(object sender, EventArgs e)
        {
            btnAccount.BackColor = Color.FromArgb(34, 40, 49);
        }
        
        #endregion

        #region -- Form Design --

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
