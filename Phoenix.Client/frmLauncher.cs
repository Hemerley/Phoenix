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
                this.pnlAuthenicate.Invoke((Action)delegate
                {
                    var authCommand = new AuthenticateCommand();
                    authCommand.Username = txtAccountName.Text.Trim();
                    authCommand.Password = txtPassword.Text.Trim();
                    SendCommand(authCommand);
                });
            }
            else
            {
                this.pnlAccountCreate.Invoke((Action)delegate
                {
                    var command = new NewAccountCommand();
                    command.CommandType = CommandType.NewAccount;
                    command.Username = txtNAccount.Text;
                    command.Email = txtEmail.Text;
                    command.Password = txtNPassword.Text;
                    SendCommand(command);
                });
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
                    var authResponseCmd = command as AuthenticateResponseCommand;
                    if (authResponseCmd.Success == 1)
                    {
                        this.pnlAuthenicate.Invoke((Action)delegate
                        {
                            btnAccount.Enabled = false;
                            btnCreate.Enabled = false;
                            lblAccount.Text = this.txtAccountName.Text;
                            this.txtAccountName.Text = "";
                            this.txtPassword.Text = "";
                        });
                        this.Invoke((Action)delegate
                        {
                            pnlAuthenicate.Hide();
                            pnlAccountView.Show();
                        });
                    }
                    else
                    {
                        this.pnlAuthenicate.Invoke((Action)delegate
                        {
                            btnAccount.Enabled = true;
                            btnCreate.Enabled = true;
                            btnAuthenticate.Enabled = true;
                        });
                        MessageBox.Show("Account or Password incorrect!", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;

                case CommandType.NewAccountResponse:
                   var accountResponseCmd = command as NewAccountResponseCommand;
                    if (accountResponseCmd.Success == 1)
                    {
                        this.Invoke((Action)delegate
                        {
                            pnlAccountCreate.Hide();
                            pnlAccountView.Show();
                            this.lblAccount.Text = this.txtNAccount.Text;
                        });
                    }
                    else
                    {
                        this.pnlAuthenicate.Invoke((Action)delegate
                        {
                            btnAccount.Enabled = true;
                            btnCreate.Enabled = true;
                            btnNCreate.Enabled = true;
                        });
                        MessageBox.Show("Accounts Already Exists!", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                case CommandType.NewChracterResponse:
                    var characterResponseCmd = command as NewCharacterResponseCommand;
                    if(characterResponseCmd.Success == 1)
                    {
                        this.Invoke((Action)delegate
                        {
                            // Update Character List
                            this.pnlCharacterCreation.Hide();
                            this.pnlAccountView.Show();
                        });
                    }
                    else
                    {
                        MessageBox.Show("Character Already Exists!", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            if(txtNPassword.Text == txtNVerifyPassword.Text)
            {
                btnNCreate.Enabled = false;
                btnAccount.Enabled = false;
                btnCreate.Enabled = false;
                authMode = false;
                this.client.Start(IPAddress.Loopback, Constants.LIVE_PORT);
            }
            else
            {
                MessageBox.Show("Passwords don't match!",Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
            this.client.Start(IPAddress.Loopback, Constants.LIVE_PORT);
        }

        private void btnNCharacterCreate_Click(object sender, EventArgs e)
        {
            if (txtCharacterName.Text == "" || txtCharacterName.Text == "Chracter Name")
            {
                MessageBox.Show("Please choose a character name!", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboGender.Text == "" || cboGender.Text == "Gender")
            {
                MessageBox.Show("Please choose a gender!", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cboPhilosophy.Text == "" || cboPhilosophy.Text == "Philosophy")
            {
                MessageBox.Show("Please choose a philosophy!", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cboPhilosophy.Text == "War" || cboPhilosophy.Text == "Faith" || cboPhilosophy.Text == "Chaos" || cboPhilosophy.Text == "Subversion")
            {
                if (cboGender.Text == "Male" || cboGender.Text == "Female")
                {
                    btnNCharcterCreate.Enabled = false;
                    var createCharacterCommand = new NewCharacterCommand();
                    createCharacterCommand.CharacterName = txtCharacterName.Text;
                    createCharacterCommand.Gender = cboGender.Text;
                    createCharacterCommand.Philosophy = 0;
                    SendCommand(createCharacterCommand);
                }
            }
            else
            {
                MessageBox.Show("Please check your entries. They must match availble entries in the drop down.", Constants.GAME_NAME + " V" + Constants.GAME_VERSION, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
