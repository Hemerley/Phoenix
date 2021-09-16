using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class FrmLauncher : Form
    {
        private bool authMode = false;
        public FrmClient gameWindow;

        public FrmLauncher()
        {
            InitializeComponent();

            this.client = new Classes.Network.Client();
            this.client.OnActivity += Client_OnActivity;
            this.client.IsConnected += Client_IsConnected;
            this.client.IsClosed += Client_IsClosed;
            this.dgvCharacter.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvCharacter.MultiSelect = false;

            this.gameWindow = new();

            // Setup UI Display
            this.pnlSide.Height = btnAccount.Height;
            this.pnlSide.Top = btnAccount.Top;
            this.pnlSide.Left = btnAccount.Left;
            this.btnAccount.BackColor = Color.FromArgb(57, 62, 70);
        }

        #region -- Network Controllers --

        private readonly Classes.Network.Client client;

        #endregion

        #region -- Data Controllers --

        /// <summary>
        /// Sends command to server.
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(Command command)
        {
            var message = CommandFactory.FormatCommand(command);
            this.client.Send(message);
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
                        Username = this.txtAccountName.Text,
                        Password = this.txtPassword.Text
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
                        Username = this.txtNAccount.Text,
                        Email = this.txtEmail.Text,
                        Password = this.txtNPassword.Text
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
                    {
                        var authResponseCommand = command as AuthenticateResponseCommand;
                        if (authResponseCommand.Success)
                        {
                            this.pnlAuthenicate.Invoke((Action)delegate
                            {
                                this.btnAccount.Enabled = false;
                                this.btnCreate.Enabled = false;
                                this.lblAccount.Text = this.txtAccountName.Text;
                                this.txtAccountName.Text = "";
                                this.txtPassword.Text = "";
                            });
                            this.Invoke((Action)delegate
                            {
                                this.pnlAuthenicate.Hide();
                                this.pnlAccountView.Show();

                            });
                            var getCharacterListCmd = new GetCharacterListCommand();

                            SendCommand(getCharacterListCmd);
                        }
                        else
                        {
                            this.pnlAuthenicate.Invoke((Action)delegate
                            {
                                this.client.Stop();
                                this.btnAccount.Enabled = true;
                                this.btnCreate.Enabled = true;
                                this.btnAuthenticate.Enabled = true;
                            });
                            MessageBox.Show("Account or Password incorrect!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }
                #endregion

                #region -- NewAccountResponse Command --

                case CommandType.NewAccountResponse:
                    {
                        var accountResponseCommand = command as NewAccountResponseCommand;
                        if (accountResponseCommand.Success)
                        {
                            this.Invoke((Action)delegate
                            {
                                this.pnlAccountCreate.Hide();
                                this.pnlAccountView.Show();
                                this.lblAccount.Text = this.txtNAccount.Text;
                            });
                        }
                        else
                        {
                            this.pnlAuthenicate.Invoke((Action)delegate
                            {
                                this.btnAccount.Enabled = true;
                                this.btnCreate.Enabled = true;
                                this.btnNCreate.Enabled = true;
                            });
                            MessageBox.Show("Accounts Already Exists!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.client.Stop();
                        }
                        return;
                    }
                #endregion

                #region -- NewCharacterResponse --

                case CommandType.NewChracterResponse:
                    var characterResponseCommand = command as NewCharacterResponseCommand;
                    {
                        if (characterResponseCommand.Success)
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
                    }
                #endregion

                #region -- CharacterListResponseCommand --
                
                case CommandType.CharacterListResponse:
                    {
                        var characterListResponseCmd = command as CharacterListResponseCommand;

                        if (characterListResponseCmd.Success)
                        {
                            foreach (Character character in characterListResponseCmd.Characters)
                            {
                                this.Invoke((Action)delegate
                                {
                                    int rowId = this.dgvCharacter.Rows.Add();
                                    DataGridViewRow row = this.dgvCharacter.Rows[rowId];
                                    row.Cells["dgvCharacterName"].Value = character.Name;
                                    row.Cells["dgvCharacterCaste"].Value = character.Caste;
                                    row.Cells["dgvCharacterPhilosophy"].Value = character.Philosophy;
                                });
                            }
                        }

                        return;
                    }
                #endregion

                #region -- CharacterConnectResponseCommmand --

                case CommandType.CharacterLoginResponse:
                    {
                        var charConnectResponseCommand = command as CharacterConnectResponseCommand;

                        if (charConnectResponseCommand.Success)
                        {
                            this.Invoke((Action)delegate
                            {
                                this.client.OnActivity -= Client_OnActivity;
                                this.client.IsConnected -= Client_IsConnected;
                                this.client.IsClosed -= Client_IsClosed;
                                this.gameWindow.Initialize(this.client, charConnectResponseCommand.Character, this);
                                this.gameWindow.Show();
                                this.Hide();
                            });
                        }
                        else
                        {
                            MessageBox.Show("Something Went Wrong!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return;
                    }
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

        private void TxtCharacterName_Enter(object sender, EventArgs e)
        {
            if (this.txtCharacterName.Text == "Name")
            {
                this.txtCharacterName.Text = "";
            }
        }

        private void TxtCharacterName_Leave(object sender, EventArgs e)
        {
            if (this.txtCharacterName.Text.Trim() == "")
            {
                this.txtCharacterName.Text = "Name";
                this.cboGender.Enabled = false;
                this.cboPhilosophy.Enabled = false;
                this.cboImage.Enabled = false;
                return;
            }
            else
            {
                this.cboGender.Enabled = true;
                this.txtCharacterName.Enabled = false;
            }
        }

        private void CboGender_TextChanged(object sender, EventArgs e)
        {
            if (this.cboGender.Text == "Male" || this.cboGender.Text == "Female")
            {
                this.cboPhilosophy.Enabled = true;
                this.cboGender.Enabled = false;
                DirectoryInfo directory = new("./Images/Avatar/");
                FileInfo[] Archives = directory.GetFiles("*.png");

                foreach (FileInfo fileInfo in Archives)
                {
                    if (this.cboGender.Text == "Male")
                    {
                        if (fileInfo.Name.StartsWith("m"))
                        {
                            this.cboImage.Items.Add(fileInfo.Name);
                        }
                    }
                    else if (this.cboGender.Text == "Female")
                    {
                        if (fileInfo.Name.StartsWith("f"))
                        {
                            this.cboImage.Items.Add(fileInfo.Name);
                        }
                    }
                }
            }
        }

        private void CboPhilosophy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.cboPhilosophy.Items.Contains(this.cboPhilosophy.Text))
            {
                this.cboPhilosophy.Enabled = false;
                this.cboImage.Enabled = true;
            }
        }

        private void TxtCharacterName_TextChanged(object sender, EventArgs e)
        {
            this.cboGender.Enabled = true;
        }

        private void CboImage_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pbCharacter.Image = new Bitmap("./Images/Avatar/" + cboImage.Text);
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void BtnAccount_Click(object sender, EventArgs e)
        {
            this.pnlSide.Height = btnAccount.Height;
            this.pnlSide.Top = btnAccount.Top;
            this.pnlSide.Left = btnAccount.Left;
            this.btnAccount.BackColor = Color.FromArgb(57, 62, 70);
            this.pnlAccountCreate.Visible = false;
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            this.pnlSide.Height = btnCreate.Height;
            this.pnlSide.Top = btnCreate.Top;
            this.pnlSide.Left = btnCreate.Left;
            this.btnCreate.BackColor = Color.FromArgb(57, 62, 70);
            this.pnlAccountCreate.Visible = true;
        }

        private void BtnCharacterCreate_Click(object sender, EventArgs e)
        {
            this.pnlCharacterCreation.Show();
            this.pnlAccountView.Hide();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.pnlCharacterCreation.Hide();
            this.cboGender.Enabled = false;
            this.cboPhilosophy.Enabled = false;
            this.cboImage.Enabled = false;
            this.txtCharacterName.Enabled = true;
            this.txtCharacterName.Text = "Name";
            this.cboGender.Text = "Gender";
            this.cboPhilosophy.Text = "School of Philosophy";
            this.cboImage.Text = "Select Character Image";
            this.pnlAccountView.Show();
        }

        private void BtnCharacterConnect_Click(object sender, EventArgs e)
        {
            if (this.dgvCharacter.SelectedRows.Count > 0)
            {
                string name = this.dgvCharacter.SelectedRows[0].Cells["dgvCharacterName"].Value.ToString();
                var newCharacterConnectCmd = new CharacterConnectCommand
                {
                    Name = name
                };
                SendCommand(newCharacterConnectCmd);
            }
        }

        private void BtnNCreate_Click(object sender, EventArgs e)
        {
            if (this.txtNAccount.Text == "Account Name" || this.txtNAccount.Text.Contains(" ") || Helper.hasSpecialChar(this.txtNAccount.Text))
            {
                MessageBox.Show("Account Names cannot contain spaces or \\|!#$%&/()=?»«@£§€{}.-;'<>_,", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (this.txtEmail.Text == "E-mail Address" || !Helper.IsValidEmail(this.txtEmail.Text))
            {
                MessageBox.Show("Please enter a valid e-mail address.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!Helper.hasSpecialChar(this.txtNPassword.Text) || !Helper.hasSpecialChar(this.txtNVerifyPassword.Text) || this.txtNPassword.Text.Length <= 6 || this.txtNVerifyPassword.Text.Length <= 6 || !Helper.HasUpperLowerDigit(this.txtNVerifyPassword.Text) || !Helper.HasUpperLowerDigit(this.txtNPassword.Text) || this.txtNPassword.Text.Contains(" ") || this.txtNVerifyPassword.Text.Contains(" ")) 
            {
                MessageBox.Show("Passwords must be longer than 6 characters, cannot contain spaces, and must contain a number, upper character, lower character, and a special character '\\|!#$%&/()=?»«@£§€{}.-;'<>_,'", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (this.txtNVerifyPassword.Text != this.txtNVerifyPassword.Text)
            {
                MessageBox.Show("Passwords do not match!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                this.btnNCreate.Enabled = false;
                this.btnAccount.Enabled = false;
                this.btnCreate.Enabled = false;
                this.authMode = false;
                this.client.Start(IPAddress.Loopback, Constants.LIVE_PORT);
            }
        }
        
        private void BtnAuthenticate_Click(object sender, EventArgs e)
        {
            if (this.txtAccountName.Text == "" || this.txtPassword.Text == "" || this.txtAccountName.Text == "Account Name" || this.txtPassword.Text == "Password" || Helper.hasSpecialChar(this.txtAccountName.Text))
            {
                MessageBox.Show("Please enter valid credentials and then attempt to authenticate.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                this.btnAuthenticate.Enabled = false;
                this.authMode = true;
                this.client.Start(IPAddress.Loopback, Constants.LIVE_PORT);
            }
        }

        private void BtnNCharacterCreate_Click(object sender, EventArgs e)
        {
            if (this.txtCharacterName.Text == "" || this.txtCharacterName.Text == "Chracter Name")
            {
                MessageBox.Show("Please choose a character name!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (Helper.hasSpecialChar(this.txtCharacterName.Text))
            {
                MessageBox.Show("Character names cannot contain special characters.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (this.cboGender.Text == "" || this.cboGender.Text == "Gender")
            {
                MessageBox.Show("Please choose a gender!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (this.cboPhilosophy.Text == "" || this.cboPhilosophy.Text == "Philosophy")
            {
                MessageBox.Show("Please choose a philosophy!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (!cboImage.Items.Contains(cboImage.Text))
            {
                MessageBox.Show("Chosen image does not exist. Please select an image!", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (this.cboPhilosophy.Text == "War" || this.cboPhilosophy.Text == "Faith" || this.cboPhilosophy.Text == "Arcane" || this.cboPhilosophy.Text == "Subversion")
            {
                if (this.cboGender.Text == "Male" || this.cboGender.Text == "Female")
                {
                    this.btnNCharcterCreate.Enabled = false;
                    var createCharacterCommand = new NewCharacterCommand();
                    if (this.cboPhilosophy.Text == "War")
                    {
                        createCharacterCommand.Philosophy = 0;
                    }
                    else if (this.cboPhilosophy.Text == "Arcane")
                    {
                        createCharacterCommand.Philosophy = 1;
                    }
                    else if (this.cboPhilosophy.Text == "Faith")
                    {
                        createCharacterCommand.Philosophy = 2;
                    }
                    else if (this.cboPhilosophy.Text == "Subversion")
                    {
                        createCharacterCommand.Philosophy = 3;
                    }
                    createCharacterCommand.CharacterName = txtCharacterName.Text;
                    createCharacterCommand.Gender = cboGender.Text;
                    createCharacterCommand.Image = cboImage.Text;
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

        private void BtnCreate_Leave(object sender, EventArgs e)
        {
            this.btnCreate.BackColor = Color.FromArgb(34, 40, 49);
        }

        private void BtnAccount_Leave(object sender, EventArgs e)
        {
            this.btnAccount.BackColor = Color.FromArgb(34, 40, 49);
        }

        private void TxtAccountName_Enter(object sender, EventArgs e)
        {
           if (this.txtAccountName.Text == "Account Name")
            {
                this.txtAccountName.Text = "";
            }
        }
        
        private void TxtPassword_Enter(object sender, EventArgs e)
        {
            if (this.txtPassword.Text == "Password")
            {
                this.txtPassword.Text = "";
                this.txtPassword.PasswordChar = '*';
            }
        }
        
        private void TxtPassword_Leave(object sender, EventArgs e)
        {
            if (this.txtPassword.Text.Trim() == "")
            {
                this.txtPassword.Text = "Password";
                this.txtPassword.PasswordChar = '\0';
            }
        }
        
        private void TxtAccountName_Leave(object sender, EventArgs e)
        {
            if (this.txtAccountName.Text.Trim() == "")
            {
                this.txtAccountName.Text = "Account Name";
            }
        }
        
        private void TxtNAccount_Enter(object sender, EventArgs e)
        {
            if (this.txtNAccount.Text == "Account Name")
            {
                this.txtNAccount.Text = "";
            }
        }
        
        private void TxtEmail_Enter(object sender, EventArgs e)
        {
            if (this.txtEmail.Text == "E-mail Address")
            {
                this.txtEmail.Text = "";
            }
        }

        private void TxtNPassword_Enter(object sender, EventArgs e)
        {
            if (this.txtNPassword.Text == "Password")
            {
                this.txtNPassword.Text = "";
                this.txtNPassword.PasswordChar = '*';
            }
        }
        
        private void TxtNVerifyPassword_Enter(object sender, EventArgs e)
        {
            if (this.txtNVerifyPassword.Text == "Verify Password")
            {
                this.txtNVerifyPassword.Text = "";
                this.txtNVerifyPassword.PasswordChar = '*';
            }
        }
        
        private void TxtNAccount_Leave(object sender, EventArgs e)
        {
            if (this.txtNAccount.Text.Trim() == "")
            {
                this.txtNAccount.Text = "Account Name";
            }
        }
        
        private void TxtEmail_Leave(object sender, EventArgs e)
        {
            if (this.txtEmail.Text.Trim() == "")
            {
                this.txtEmail.Text = "E-mail Address";
            }
        }
        
        private void TxtNPassword_Leave(object sender, EventArgs e)
        {
            if (this.txtNPassword.Text.Trim() == "")
            {
                this.txtNPassword.Text = "Password";
                this.txtNPassword.PasswordChar = '\0';
            }
        }
        
        private void TxtNVerifyPassword_Leave(object sender, EventArgs e)
        {
            if (this.txtNVerifyPassword.Text.Trim() == "")
            {
                this.txtNVerifyPassword.Text = "Verify Password";
                this.txtNVerifyPassword.PasswordChar = '\0';
            }
        }

        #endregion

    }
}
