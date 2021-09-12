using Phoenix.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class FrmLauncher : Form
    {
        public bool authMode = false;

        public FrmLauncher()
        {
            InitializeComponent();

            this.client = new Client();
            this.client.OnActivity += Client_OnActivity;
            this.client.IsConnected += Client_IsConnected;
            this.client.IsClosed += Client_IsClosed;
            this.dgvCharacter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCharacter.MultiSelect = false;

            frmClient gameWindow = new(this);

            foreach (string keys in iAvatar.Images.Keys)
            {
                cboImage.Items.Add(keys);
            }

            // Setup UI Display
            pnlSide.Height = btnAccount.Height;
            pnlSide.Top = btnAccount.Top;
            pnlSide.Left = btnAccount.Left;
            btnAccount.BackColor = Color.FromArgb(57, 62, 70);
        }

        #region -- Network Controllers --

        private readonly Client client;

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

        #region -- Client Events --

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
                    MessageBox.Show("The connection was closed by the server.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                default:
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
                    var authCommand = new AuthenticateCommand
                    {
                        Username = txtAccountName.Text,
                        Password = Helper.RemoveBar(txtPassword.Text)
                    };
                    SendCommand(authCommand);
                });
            }
            else
            {
                this.pnlAccountCreate.Invoke((Action)delegate
                {
                    var command = new NewAccountCommand
                    {
                        CommandType = CommandType.NewAccount,
                        Username = txtNAccount.Text,
                        Email = Helper.RemoveBar(txtEmail.Text),
                        Password = Helper.RemoveBar(txtNPassword.Text)
                    };
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
                #region -- AuthResponse Command --

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
                        var getCharacterListCmd = new GetCharacterListCommand();

                        SendCommand(getCharacterListCmd);
                    }
                    else
                    {
                        this.pnlAuthenicate.Invoke((Action)delegate
                        {
                            btnAccount.Enabled = true;
                            btnCreate.Enabled = true;
                            btnAuthenticate.Enabled = true;
                        });
                        MessageBox.Show("Account or Password incorrect!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return;
                #endregion

                #region -- NewAccountResponse Command --

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
                        MessageBox.Show("Accounts Already Exists!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.client.Stop();
                    }
                    return;

                #endregion

                #region -- NewCharacterResponse --

                case CommandType.NewChracterResponse:
                    var characterResponseCmd = command as NewCharacterResponseCommand;
                    if (characterResponseCmd.Success == 1)
                    {
                        this.Invoke((Action)delegate
                        {
                            // Update Character List
                            this.pnlCharacterCreation.Hide();
                            this.pnlAccountView.Show();
                            this.dgvCharacter.Rows.Clear();
                        });
                        var getCharacterListCmd = new GetCharacterListCommand();

                        SendCommand(getCharacterListCmd);
                    }
                    else
                    {
                        MessageBox.Show("Character Already Exists!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.client.Stop();
                    }
                    return;

                #endregion

                #region -- CharacterListResponseCommand --

                case CommandType.CharacterListResponse:
                    var characterListResponseCmd = command as CharacterListResponseCommand;

                    string[] s = characterListResponseCmd.characters.Split("~");



                    foreach(string sCharacter in s)
                    {
                        string[] character = sCharacter.Split("`");
                        this.Invoke((Action)delegate
                        {
                            int rowId = this.dgvCharacter.Rows.Add();
                            DataGridViewRow row = this.dgvCharacter.Rows[rowId];
                            row.Cells["dgvCharacterName"].Value = character[0];
                            row.Cells["dgvCharacterCaste"].Value = Helper.ReturnCasteText(Int32.Parse(character[1]));
                            row.Cells["dgvCharacterPhilosophy"].Value = Helper.ReturnPhilosophyText(Int32.Parse(character[2]));
                        });
                    }

                    return;

                #endregion
            }
        }

        #endregion

        #region -- Move Window --

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool ReleaseCapture();

        private void FrmLauncher_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        #endregion

        #region -- Button Controllers --

        private void BtnNCreate_Click(object sender, EventArgs e)
        {
            if (txtNAccount.Text == "Account Name" || txtNAccount.Text.Contains(" ") || Helper.hasSpecialChar(txtNAccount.Text))
            {
                MessageBox.Show("Account Names cannot contain spaces or \\|!#$%&/()=?»«@£§€{}.-;'<>_,", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtEmail.Text == "E-mail Address" || !Helper.IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid e-mail address.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Helper.hasSpecialChar(txtNPassword.Text) || !Helper.hasSpecialChar(txtNVerifyPassword.Text) || txtNPassword.Text.Length <= 6 || txtNVerifyPassword.Text.Length <= 6 || !Helper.HasUpperLowerDigit(txtNVerifyPassword.Text) || !Helper.HasUpperLowerDigit(txtNPassword.Text) || txtNPassword.Text.Contains(" ") || txtNVerifyPassword.Text.Contains(" ")) 
            {
                MessageBox.Show("Passwords must be longer than 6 characters, cannot contain spaces, and must contain a number, upper character, lower character, and a special character '\\|!#$%&/()=?»«@£§€{}.-;'<>_,'", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (txtNVerifyPassword.Text != txtNVerifyPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                btnNCreate.Enabled = false;
                btnAccount.Enabled = false;
                btnCreate.Enabled = false;
                authMode = false;
                this.client.Start(IPAddress.Loopback, Constants.LIVE_PORT);
            }
        }
        
        private void BtnAuthenticate_Click(object sender, EventArgs e)
        {
            if (txtAccountName.Text == "" || txtPassword.Text == "" || txtAccountName.Text == "Account Name" || txtPassword.Text == "Password" || Helper.hasSpecialChar(txtAccountName.Text))
            {
                MessageBox.Show("Please enter valid credentials and then attempt to authenticate.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                btnAuthenticate.Enabled = false;
                authMode = true;
                this.client.Start(IPAddress.Loopback, Constants.LIVE_PORT);
            }
        }

        private void BtnNCharacterCreate_Click(object sender, EventArgs e)
        {
            if (txtCharacterName.Text == "" || txtCharacterName.Text == "Chracter Name")
            {
                MessageBox.Show("Please choose a character name!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Helper.hasSpecialChar(txtCharacterName.Text))
            {
                MessageBox.Show("Character names cannot contain special characters.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cboGender.Text == "" || cboGender.Text == "Gender")
            {
                MessageBox.Show("Please choose a gender!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cboPhilosophy.Text == "" || cboPhilosophy.Text == "Philosophy")
            {
                MessageBox.Show("Please choose a philosophy!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!cboImage.Items.Contains(cboImage.Text))
            {
                MessageBox.Show("Chosen image does not exist. Please select an image!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cboPhilosophy.Text == "War" || cboPhilosophy.Text == "Faith" || cboPhilosophy.Text == "Chaos" || cboPhilosophy.Text == "Subversion")
            {
                if (cboGender.Text == "Male" || cboGender.Text == "Female")
                {
                    btnNCharcterCreate.Enabled = false;
                    var createCharacterCommand = new NewCharacterCommand();
                    if (cboPhilosophy.Text == "War")
                    {
                        createCharacterCommand.Philosophy = 0;
                    }
                    else if (cboPhilosophy.Text == "Faith")
                    {
                        createCharacterCommand.Philosophy = 1;
                    }
                    else if (cboPhilosophy.Text == "Chaos")
                    {
                        createCharacterCommand.Philosophy = 2;
                    }
                    else if (cboPhilosophy.Text == "Subversion")
                    {
                        createCharacterCommand.Philosophy = 3;
                    }
                    createCharacterCommand.CharacterName = txtCharacterName.Text;
                    createCharacterCommand.Gender = cboGender.Text;
                    createCharacterCommand.Image = cboImage.SelectedIndex;
                    SendCommand(createCharacterCommand);
                }
            }
            else
            {
                MessageBox.Show("Please check your entries. They must match availble entries in the drop down.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        #endregion

        #region -- Form Design --

        private void CboImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pbCharacter.Image = iAvatar.Images[cboImage.SelectedIndex];
        }
        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnAccount_Click(object sender, EventArgs e)
        {
            pnlSide.Height = btnAccount.Height;
            pnlSide.Top = btnAccount.Top;
            pnlSide.Left = btnAccount.Left;
            btnAccount.BackColor = Color.FromArgb(57, 62, 70);
            pnlAccountCreate.Visible = false;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            pnlSide.Height = btnCreate.Height;
            pnlSide.Top = btnCreate.Top;
            pnlSide.Left = btnCreate.Left;
            btnCreate.BackColor = Color.FromArgb(57, 62, 70);
            pnlAccountCreate.Visible = true;
        }

        private void BtnCharacterCreate_Click(object sender, EventArgs e)
        {
            pnlCharacterCreation.Show();
            pnlAccountView.Hide();
        }

        private void BtnCreate_Leave(object sender, EventArgs e)
        {
            btnCreate.BackColor = Color.FromArgb(34, 40, 49);
        }

        private void BtnAccount_Leave(object sender, EventArgs e)
        {
            btnAccount.BackColor = Color.FromArgb(34, 40, 49);
        }

        private void TxtAccountName_Enter(object sender, EventArgs e)
        {
           if (txtAccountName.Text == "Account Name")
            {
                txtAccountName.Text = "";
            }
        }
        
        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            if (txtPassword.Text == "Password")
            {
                txtPassword.Text = "";
                txtPassword.PasswordChar = '*';
            }
        }
        
        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            if (txtPassword.Text.Trim() == "")
            {
                txtPassword.Text = "Password";
                txtPassword.PasswordChar = '\0';
            }
        }
        
        private void TxtAccountName_Leave(object sender, EventArgs e)
        {
            if (txtAccountName.Text.Trim() == "")
            {
                txtAccountName.Text = "Account Name";
            }
        }
        
        private void TxtNAccount_Enter(object sender, EventArgs e)
        {
            if (txtNAccount.Text == "Account Name")
            {
                txtNAccount.Text = "";
            }
        }
        
        private void TxtEmail_Enter(object sender, EventArgs e)
        {
            if (txtEmail.Text == "E-mail Address")
            {
                txtEmail.Text = "";
            }
        }

        private void TxtNPassword_Enter(object sender, EventArgs e)
        {
            if (txtNPassword.Text == "Password")
            {
                txtNPassword.Text = "";
                txtNPassword.PasswordChar = '*';
            }
        }
        
        private void TxtNVerifyPassword_Enter(object sender, EventArgs e)
        {
            if (txtNVerifyPassword.Text == "Verify Password")
            {
                txtNVerifyPassword.Text = "";
                txtNVerifyPassword.PasswordChar = '*';
            }
        }
        
        private void TxtNAccount_Leave(object sender, EventArgs e)
        {
            if (txtNAccount.Text.Trim() == "")
            {
                txtNAccount.Text = "Account Name";
            }
        }
        
        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (txtEmail.Text.Trim() == "")
            {
                txtEmail.Text = "E-mail Address";
            }
        }
        
        private void TxtNPassword_Leave(object sender, EventArgs e)
        {
            if (txtNPassword.Text.Trim() == "")
            {
                txtNPassword.Text = "Password";
                txtNPassword.PasswordChar = '\0';
            }
        }
        
        private void TxtNVerifyPassword_Leave(object sender, EventArgs e)
        {
            if (txtNVerifyPassword.Text.Trim() == "")
            {
                txtNVerifyPassword.Text = "Verify Password";
                txtNVerifyPassword.PasswordChar = '\0';
            }
        }




        #endregion

        private void BtnCharacterConnect_Click(object sender, EventArgs e)
        {
            
        }
    }
}
