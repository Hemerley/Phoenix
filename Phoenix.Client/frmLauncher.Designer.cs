
using System.Drawing;

namespace Phoenix.Client
{
    partial class FrmLauncher
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLauncher));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pnlSide = new System.Windows.Forms.Panel();
            this.btnStore = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnAccount = new System.Windows.Forms.Button();
            this.pnlAccount = new System.Windows.Forms.Panel();
            this.lblAccount = new System.Windows.Forms.Label();
            this.ptbAccountImage = new System.Windows.Forms.PictureBox();
            this.pnlAuthenicate = new System.Windows.Forms.Panel();
            this.lblHeader1 = new System.Windows.Forms.Label();
            this.btnAuthenticate = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtAccountName = new System.Windows.Forms.TextBox();
            this.ptbDiscord = new System.Windows.Forms.PictureBox();
            this.ptbPhoenix1 = new System.Windows.Forms.PictureBox();
            this.pnlAccountView = new System.Windows.Forms.Panel();
            this.dgvCharacter = new System.Windows.Forms.DataGridView();
            this.dgvCharacterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCharacterCaste = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvCharacterPhilosophy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDeleteCharacter = new System.Windows.Forms.Button();
            this.btnTransferCharacter = new System.Windows.Forms.Button();
            this.btnCharacterCreate = new System.Windows.Forms.Button();
            this.lblHeader3 = new System.Windows.Forms.Label();
            this.btnCharacterConnect = new System.Windows.Forms.Button();
            this.ptbPhoenix3 = new System.Windows.Forms.PictureBox();
            this.pnlAccountCreate = new System.Windows.Forms.Panel();
            this.txtNVerifyPassword = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblHeader2 = new System.Windows.Forms.Label();
            this.btnNCreate = new System.Windows.Forms.Button();
            this.txtNPassword = new System.Windows.Forms.TextBox();
            this.txtNAccount = new System.Windows.Forms.TextBox();
            this.ptbPhoenix2 = new System.Windows.Forms.PictureBox();
            this.pnlCharacterCreation = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cboImage = new System.Windows.Forms.ComboBox();
            this.pbCharacter = new System.Windows.Forms.PictureBox();
            this.txtClassInformation = new System.Windows.Forms.TextBox();
            this.cboPhilosophy = new System.Windows.Forms.ComboBox();
            this.cboGender = new System.Windows.Forms.ComboBox();
            this.lblHeader4 = new System.Windows.Forms.Label();
            this.btnNCharcterCreate = new System.Windows.Forms.Button();
            this.txtCharacterName = new System.Windows.Forms.TextBox();
            this.ptbPhoenix4 = new System.Windows.Forms.PictureBox();
            this.pnlSide.SuspendLayout();
            this.pnlAccount.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbAccountImage)).BeginInit();
            this.pnlAuthenicate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDiscord)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix1)).BeginInit();
            this.pnlAccountView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCharacter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix3)).BeginInit();
            this.pnlAccountCreate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix2)).BeginInit();
            this.pnlCharacterCreation.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCharacter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix4)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlSide
            // 
            this.pnlSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.pnlSide.Controls.Add(this.btnStore);
            this.pnlSide.Controls.Add(this.btnExit);
            this.pnlSide.Controls.Add(this.btnCreate);
            this.pnlSide.Controls.Add(this.btnAccount);
            this.pnlSide.Controls.Add(this.pnlAccount);
            this.pnlSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlSide.Location = new System.Drawing.Point(0, 0);
            this.pnlSide.Name = "pnlSide";
            this.pnlSide.Size = new System.Drawing.Size(200, 572);
            this.pnlSide.TabIndex = 0;
            // 
            // btnStore
            // 
            this.btnStore.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnStore.Enabled = false;
            this.btnStore.FlatAppearance.BorderSize = 0;
            this.btnStore.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStore.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStore.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnStore.Location = new System.Drawing.Point(0, 226);
            this.btnStore.Name = "btnStore";
            this.btnStore.Size = new System.Drawing.Size(200, 41);
            this.btnStore.TabIndex = 4;
            this.btnStore.Text = "Store";
            this.btnStore.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnStore.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExit.FlatAppearance.BorderSize = 0;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnExit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnExit.Location = new System.Drawing.Point(0, 531);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(200, 41);
            this.btnExit.TabIndex = 3;
            this.btnExit.Text = "Exit";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // btnCreate
            // 
            this.btnCreate.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnCreate.FlatAppearance.BorderSize = 0;
            this.btnCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreate.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnCreate.Location = new System.Drawing.Point(0, 185);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(200, 41);
            this.btnCreate.TabIndex = 2;
            this.btnCreate.Text = "Create Account";
            this.btnCreate.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.BtnCreate_Click);
            this.btnCreate.Leave += new System.EventHandler(this.BtnCreate_Leave);
            // 
            // btnAccount
            // 
            this.btnAccount.Dock = System.Windows.Forms.DockStyle.Top;
            this.btnAccount.FlatAppearance.BorderSize = 0;
            this.btnAccount.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAccount.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnAccount.Location = new System.Drawing.Point(0, 144);
            this.btnAccount.Name = "btnAccount";
            this.btnAccount.Size = new System.Drawing.Size(200, 41);
            this.btnAccount.TabIndex = 1;
            this.btnAccount.Text = "Account";
            this.btnAccount.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAccount.UseVisualStyleBackColor = true;
            this.btnAccount.Click += new System.EventHandler(this.BtnAccount_Click);
            this.btnAccount.Leave += new System.EventHandler(this.BtnAccount_Leave);
            // 
            // pnlAccount
            // 
            this.pnlAccount.Controls.Add(this.lblAccount);
            this.pnlAccount.Controls.Add(this.ptbAccountImage);
            this.pnlAccount.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlAccount.Location = new System.Drawing.Point(0, 0);
            this.pnlAccount.Name = "pnlAccount";
            this.pnlAccount.Size = new System.Drawing.Size(200, 144);
            this.pnlAccount.TabIndex = 0;
            // 
            // lblAccount
            // 
            this.lblAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.lblAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.lblAccount.Location = new System.Drawing.Point(3, 96);
            this.lblAccount.Name = "lblAccount";
            this.lblAccount.Size = new System.Drawing.Size(194, 25);
            this.lblAccount.TabIndex = 1;
            this.lblAccount.Text = "Guest Account";
            this.lblAccount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ptbAccountImage
            // 
            this.ptbAccountImage.Image = ((System.Drawing.Image)(resources.GetObject("ptbAccountImage.Image")));
            this.ptbAccountImage.Location = new System.Drawing.Point(68, 22);
            this.ptbAccountImage.Name = "ptbAccountImage";
            this.ptbAccountImage.Size = new System.Drawing.Size(63, 63);
            this.ptbAccountImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbAccountImage.TabIndex = 0;
            this.ptbAccountImage.TabStop = false;
            // 
            // pnlAuthenicate
            // 
            this.pnlAuthenicate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            this.pnlAuthenicate.Controls.Add(this.lblHeader1);
            this.pnlAuthenicate.Controls.Add(this.btnAuthenticate);
            this.pnlAuthenicate.Controls.Add(this.txtPassword);
            this.pnlAuthenicate.Controls.Add(this.txtAccountName);
            this.pnlAuthenicate.Controls.Add(this.ptbDiscord);
            this.pnlAuthenicate.Controls.Add(this.ptbPhoenix1);
            this.pnlAuthenicate.Location = new System.Drawing.Point(222, 22);
            this.pnlAuthenicate.Name = "pnlAuthenicate";
            this.pnlAuthenicate.Size = new System.Drawing.Size(727, 529);
            this.pnlAuthenicate.TabIndex = 8;
            // 
            // lblHeader1
            // 
            this.lblHeader1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.lblHeader1.Font = new System.Drawing.Font("Viner Hand ITC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.lblHeader1.Location = new System.Drawing.Point(0, 0);
            this.lblHeader1.Name = "lblHeader1";
            this.lblHeader1.Size = new System.Drawing.Size(724, 34);
            this.lblHeader1.TabIndex = 31;
            this.lblHeader1.Text = "Project: Phoenix";
            this.lblHeader1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnAuthenticate
            // 
            this.btnAuthenticate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnAuthenticate.FlatAppearance.BorderSize = 0;
            this.btnAuthenticate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAuthenticate.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAuthenticate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnAuthenticate.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnAuthenticate.Location = new System.Drawing.Point(254, 289);
            this.btnAuthenticate.Name = "btnAuthenticate";
            this.btnAuthenticate.Size = new System.Drawing.Size(237, 35);
            this.btnAuthenticate.TabIndex = 2;
            this.btnAuthenticate.Text = "Authenticate";
            this.btnAuthenticate.UseVisualStyleBackColor = false;
            this.btnAuthenticate.Click += new System.EventHandler(this.BtnAuthenticate_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtPassword.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtPassword.Location = new System.Drawing.Point(254, 254);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.Size = new System.Drawing.Size(237, 29);
            this.txtPassword.TabIndex = 1;
            this.txtPassword.Text = "Password";
            this.txtPassword.Enter += new System.EventHandler(this.TxtPassword_Enter);
            this.txtPassword.Leave += new System.EventHandler(this.TxtPassword_Leave);
            // 
            // txtAccountName
            // 
            this.txtAccountName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtAccountName.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtAccountName.Location = new System.Drawing.Point(254, 210);
            this.txtAccountName.Name = "txtAccountName";
            this.txtAccountName.Size = new System.Drawing.Size(237, 29);
            this.txtAccountName.TabIndex = 0;
            this.txtAccountName.Text = "Account Name";
            this.txtAccountName.Enter += new System.EventHandler(this.TxtAccountName_Enter);
            this.txtAccountName.Leave += new System.EventHandler(this.TxtAccountName_Leave);
            // 
            // ptbDiscord
            // 
            this.ptbDiscord.Image = ((System.Drawing.Image)(resources.GetObject("ptbDiscord.Image")));
            this.ptbDiscord.Location = new System.Drawing.Point(674, 476);
            this.ptbDiscord.Name = "ptbDiscord";
            this.ptbDiscord.Size = new System.Drawing.Size(50, 50);
            this.ptbDiscord.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbDiscord.TabIndex = 32;
            this.ptbDiscord.TabStop = false;
            // 
            // ptbPhoenix1
            // 
            this.ptbPhoenix1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.ptbPhoenix1.Location = new System.Drawing.Point(0, 37);
            this.ptbPhoenix1.Name = "ptbPhoenix1";
            this.ptbPhoenix1.Image = Image.FromFile("./Images/UI/Logo.png");
            this.ptbPhoenix1.Size = new System.Drawing.Size(724, 489);
            this.ptbPhoenix1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbPhoenix1.TabIndex = 30;
            this.ptbPhoenix1.TabStop = false;
            // 
            // pnlAccountView
            // 
            this.pnlAccountView.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            this.pnlAccountView.Controls.Add(this.dgvCharacter);
            this.pnlAccountView.Controls.Add(this.btnDeleteCharacter);
            this.pnlAccountView.Controls.Add(this.btnTransferCharacter);
            this.pnlAccountView.Controls.Add(this.btnCharacterCreate);
            this.pnlAccountView.Controls.Add(this.lblHeader3);
            this.pnlAccountView.Controls.Add(this.btnCharacterConnect);
            this.pnlAccountView.Controls.Add(this.ptbPhoenix3);
            this.pnlAccountView.Location = new System.Drawing.Point(222, 22);
            this.pnlAccountView.Name = "pnlAccountView";
            this.pnlAccountView.Size = new System.Drawing.Size(727, 529);
            this.pnlAccountView.TabIndex = 41;
            this.pnlAccountView.Visible = false;
            // 
            // dgvCharacter
            // 
            this.dgvCharacter.AllowUserToAddRows = false;
            this.dgvCharacter.AllowUserToDeleteRows = false;
            this.dgvCharacter.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.dgvCharacter.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCharacter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCharacter.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvCharacterName,
            this.dgvCharacterCaste,
            this.dgvCharacterPhilosophy});
            this.dgvCharacter.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.dgvCharacter.Location = new System.Drawing.Point(117, 37);
            this.dgvCharacter.Name = "dgvCharacter";
            this.dgvCharacter.ReadOnly = true;
            this.dgvCharacter.RowHeadersVisible = false;
            this.dgvCharacter.Size = new System.Drawing.Size(592, 489);
            this.dgvCharacter.TabIndex = 39;
            // 
            // dgvCharacterName
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.dgvCharacterName.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvCharacterName.HeaderText = "Name";
            this.dgvCharacterName.Name = "dgvCharacterName";
            this.dgvCharacterName.ReadOnly = true;
            this.dgvCharacterName.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCharacterName.Width = 300;
            // 
            // dgvCharacterCaste
            // 
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.dgvCharacterCaste.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvCharacterCaste.HeaderText = "Caste";
            this.dgvCharacterCaste.Name = "dgvCharacterCaste";
            this.dgvCharacterCaste.ReadOnly = true;
            this.dgvCharacterCaste.Width = 140;
            // 
            // dgvCharacterPhilosophy
            // 
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(250)))), ((int)(((byte)(225)))));
            this.dgvCharacterPhilosophy.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCharacterPhilosophy.HeaderText = "Philosophy";
            this.dgvCharacterPhilosophy.Name = "dgvCharacterPhilosophy";
            this.dgvCharacterPhilosophy.ReadOnly = true;
            this.dgvCharacterPhilosophy.Width = 150;
            // 
            // btnDeleteCharacter
            // 
            this.btnDeleteCharacter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnDeleteCharacter.FlatAppearance.BorderSize = 0;
            this.btnDeleteCharacter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCharacter.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnDeleteCharacter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnDeleteCharacter.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnDeleteCharacter.Location = new System.Drawing.Point(3, 160);
            this.btnDeleteCharacter.Name = "btnDeleteCharacter";
            this.btnDeleteCharacter.Size = new System.Drawing.Size(111, 35);
            this.btnDeleteCharacter.TabIndex = 11;
            this.btnDeleteCharacter.Text = "Delete";
            this.btnDeleteCharacter.UseVisualStyleBackColor = false;
            // 
            // btnTransferCharacter
            // 
            this.btnTransferCharacter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnTransferCharacter.FlatAppearance.BorderSize = 0;
            this.btnTransferCharacter.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTransferCharacter.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnTransferCharacter.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnTransferCharacter.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnTransferCharacter.Location = new System.Drawing.Point(3, 119);
            this.btnTransferCharacter.Name = "btnTransferCharacter";
            this.btnTransferCharacter.Size = new System.Drawing.Size(111, 35);
            this.btnTransferCharacter.TabIndex = 10;
            this.btnTransferCharacter.Text = "Transfer";
            this.btnTransferCharacter.UseVisualStyleBackColor = false;
            // 
            // btnCharacterCreate
            // 
            this.btnCharacterCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnCharacterCreate.FlatAppearance.BorderSize = 0;
            this.btnCharacterCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharacterCreate.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCharacterCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnCharacterCreate.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnCharacterCreate.Location = new System.Drawing.Point(3, 78);
            this.btnCharacterCreate.Name = "btnCharacterCreate";
            this.btnCharacterCreate.Size = new System.Drawing.Size(111, 35);
            this.btnCharacterCreate.TabIndex = 9;
            this.btnCharacterCreate.Text = "Create";
            this.btnCharacterCreate.UseVisualStyleBackColor = false;
            this.btnCharacterCreate.Click += new System.EventHandler(this.BtnCharacterCreate_Click);
            // 
            // lblHeader3
            // 
            this.lblHeader3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.lblHeader3.Font = new System.Drawing.Font("Viner Hand ITC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.lblHeader3.Location = new System.Drawing.Point(0, 0);
            this.lblHeader3.Name = "lblHeader3";
            this.lblHeader3.Size = new System.Drawing.Size(727, 34);
            this.lblHeader3.TabIndex = 31;
            this.lblHeader3.Text = "Project: Phoenix";
            this.lblHeader3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnCharacterConnect
            // 
            this.btnCharacterConnect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnCharacterConnect.FlatAppearance.BorderSize = 0;
            this.btnCharacterConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCharacterConnect.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCharacterConnect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnCharacterConnect.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnCharacterConnect.Location = new System.Drawing.Point(3, 37);
            this.btnCharacterConnect.Name = "btnCharacterConnect";
            this.btnCharacterConnect.Size = new System.Drawing.Size(111, 35);
            this.btnCharacterConnect.TabIndex = 8;
            this.btnCharacterConnect.Text = "Connect";
            this.btnCharacterConnect.UseVisualStyleBackColor = false;
            this.btnCharacterConnect.Click += new System.EventHandler(this.BtnCharacterConnect_Click);
            // 
            // ptbPhoenix3
            // 
            this.ptbPhoenix3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.ptbPhoenix3.Location = new System.Drawing.Point(0, 37);
            this.ptbPhoenix3.Name = "ptbPhoenix3";
            this.ptbPhoenix3.Image = Image.FromFile("./Images/UI/Logo.png");
            this.ptbPhoenix3.Size = new System.Drawing.Size(724, 489);
            this.ptbPhoenix3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbPhoenix3.TabIndex = 30;
            this.ptbPhoenix3.TabStop = false;
            // 
            // pnlAccountCreate
            // 
            this.pnlAccountCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            this.pnlAccountCreate.Controls.Add(this.txtNVerifyPassword);
            this.pnlAccountCreate.Controls.Add(this.txtEmail);
            this.pnlAccountCreate.Controls.Add(this.lblHeader2);
            this.pnlAccountCreate.Controls.Add(this.btnNCreate);
            this.pnlAccountCreate.Controls.Add(this.txtNPassword);
            this.pnlAccountCreate.Controls.Add(this.txtNAccount);
            this.pnlAccountCreate.Controls.Add(this.ptbPhoenix2);
            this.pnlAccountCreate.Location = new System.Drawing.Point(222, 22);
            this.pnlAccountCreate.Name = "pnlAccountCreate";
            this.pnlAccountCreate.Size = new System.Drawing.Size(727, 529);
            this.pnlAccountCreate.TabIndex = 40;
            this.pnlAccountCreate.Visible = false;
            // 
            // txtNVerifyPassword
            // 
            this.txtNVerifyPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtNVerifyPassword.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNVerifyPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtNVerifyPassword.Location = new System.Drawing.Point(254, 315);
            this.txtNVerifyPassword.Name = "txtNVerifyPassword";
            this.txtNVerifyPassword.Size = new System.Drawing.Size(237, 29);
            this.txtNVerifyPassword.TabIndex = 6;
            this.txtNVerifyPassword.Text = "Verify Password";
            this.txtNVerifyPassword.Enter += new System.EventHandler(this.TxtNVerifyPassword_Enter);
            this.txtNVerifyPassword.Leave += new System.EventHandler(this.TxtNVerifyPassword_Leave);
            // 
            // txtEmail
            // 
            this.txtEmail.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtEmail.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtEmail.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtEmail.Location = new System.Drawing.Point(254, 245);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(237, 29);
            this.txtEmail.TabIndex = 4;
            this.txtEmail.Text = "E-mail Address";
            this.txtEmail.Enter += new System.EventHandler(this.TxtEmail_Enter);
            this.txtEmail.Leave += new System.EventHandler(this.TxtEmail_Leave);
            // 
            // lblHeader2
            // 
            this.lblHeader2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.lblHeader2.Font = new System.Drawing.Font("Viner Hand ITC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.lblHeader2.Location = new System.Drawing.Point(0, 0);
            this.lblHeader2.Name = "lblHeader2";
            this.lblHeader2.Size = new System.Drawing.Size(727, 34);
            this.lblHeader2.TabIndex = 31;
            this.lblHeader2.Text = "Project: Phoenix";
            this.lblHeader2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNCreate
            // 
            this.btnNCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnNCreate.FlatAppearance.BorderSize = 0;
            this.btnNCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNCreate.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnNCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnNCreate.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnNCreate.Location = new System.Drawing.Point(254, 350);
            this.btnNCreate.Name = "btnNCreate";
            this.btnNCreate.Size = new System.Drawing.Size(237, 35);
            this.btnNCreate.TabIndex = 7;
            this.btnNCreate.Text = "Create Account";
            this.btnNCreate.UseVisualStyleBackColor = false;
            this.btnNCreate.Click += new System.EventHandler(this.BtnNCreate_Click);
            // 
            // txtNPassword
            // 
            this.txtNPassword.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtNPassword.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNPassword.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtNPassword.Location = new System.Drawing.Point(254, 280);
            this.txtNPassword.Name = "txtNPassword";
            this.txtNPassword.Size = new System.Drawing.Size(237, 29);
            this.txtNPassword.TabIndex = 5;
            this.txtNPassword.Text = "Password";
            this.txtNPassword.Enter += new System.EventHandler(this.TxtNPassword_Enter);
            this.txtNPassword.Leave += new System.EventHandler(this.TxtNPassword_Leave);
            // 
            // txtNAccount
            // 
            this.txtNAccount.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtNAccount.Font = new System.Drawing.Font("Nirmala UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtNAccount.Location = new System.Drawing.Point(254, 210);
            this.txtNAccount.Name = "txtNAccount";
            this.txtNAccount.Size = new System.Drawing.Size(237, 29);
            this.txtNAccount.TabIndex = 3;
            this.txtNAccount.Text = "Account Name";
            this.txtNAccount.Enter += new System.EventHandler(this.TxtNAccount_Enter);
            this.txtNAccount.Leave += new System.EventHandler(this.TxtNAccount_Leave);
            // 
            // ptbPhoenix2
            // 
            this.ptbPhoenix2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.ptbPhoenix2.Location = new System.Drawing.Point(0, 37);
            this.ptbPhoenix2.Name = "ptbPhoenix2";
            this.ptbPhoenix2.Image = Image.FromFile("./Images/UI/Logo.png");
            this.ptbPhoenix2.Size = new System.Drawing.Size(724, 489);
            this.ptbPhoenix2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbPhoenix2.TabIndex = 30;
            this.ptbPhoenix2.TabStop = false;
            // 
            // pnlCharacterCreation
            // 
            this.pnlCharacterCreation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(48)))), ((int)(((byte)(37)))), ((int)(((byte)(64)))));
            this.pnlCharacterCreation.Controls.Add(this.btnCancel);
            this.pnlCharacterCreation.Controls.Add(this.cboImage);
            this.pnlCharacterCreation.Controls.Add(this.pbCharacter);
            this.pnlCharacterCreation.Controls.Add(this.txtClassInformation);
            this.pnlCharacterCreation.Controls.Add(this.cboPhilosophy);
            this.pnlCharacterCreation.Controls.Add(this.cboGender);
            this.pnlCharacterCreation.Controls.Add(this.lblHeader4);
            this.pnlCharacterCreation.Controls.Add(this.btnNCharcterCreate);
            this.pnlCharacterCreation.Controls.Add(this.txtCharacterName);
            this.pnlCharacterCreation.Controls.Add(this.ptbPhoenix4);
            this.pnlCharacterCreation.Location = new System.Drawing.Point(222, 22);
            this.pnlCharacterCreation.Name = "pnlCharacterCreation";
            this.pnlCharacterCreation.Size = new System.Drawing.Size(727, 529);
            this.pnlCharacterCreation.TabIndex = 41;
            this.pnlCharacterCreation.Visible = false;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnCancel.FlatAppearance.BorderSize = 0;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnCancel.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnCancel.Location = new System.Drawing.Point(267, 330);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 35);
            this.btnCancel.TabIndex = 42;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // cboImage
            // 
            this.cboImage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.cboImage.Enabled = false;
            this.cboImage.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboImage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.cboImage.FormattingEnabled = true;
            this.cboImage.Location = new System.Drawing.Point(180, 295);
            this.cboImage.Name = "cboImage";
            this.cboImage.Size = new System.Drawing.Size(162, 25);
            this.cboImage.TabIndex = 41;
            this.cboImage.Text = "Select Character Image";
            this.cboImage.SelectedIndexChanged += new System.EventHandler(this.CboImage_SelectedIndexChanged);
            // 
            // pbCharacter
            // 
            this.pbCharacter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.pbCharacter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pbCharacter.Location = new System.Drawing.Point(74, 190);
            this.pbCharacter.Name = "pbCharacter";
            this.pbCharacter.Size = new System.Drawing.Size(100, 100);
            this.pbCharacter.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbCharacter.TabIndex = 40;
            this.pbCharacter.TabStop = false;
            // 
            // txtClassInformation
            // 
            this.txtClassInformation.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtClassInformation.Enabled = false;
            this.txtClassInformation.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtClassInformation.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtClassInformation.Location = new System.Drawing.Point(348, 190);
            this.txtClassInformation.Multiline = true;
            this.txtClassInformation.Name = "txtClassInformation";
            this.txtClassInformation.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtClassInformation.Size = new System.Drawing.Size(306, 175);
            this.txtClassInformation.TabIndex = 39;
            this.txtClassInformation.Text = "Philosophy Information Here";
            // 
            // cboPhilosophy
            // 
            this.cboPhilosophy.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.cboPhilosophy.Enabled = false;
            this.cboPhilosophy.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboPhilosophy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.cboPhilosophy.FormattingEnabled = true;
            this.cboPhilosophy.Items.AddRange(new object[] {
            "War",
            "Arcane",
            "Faith",
            "Subversion"});
            this.cboPhilosophy.Location = new System.Drawing.Point(180, 260);
            this.cboPhilosophy.Name = "cboPhilosophy";
            this.cboPhilosophy.Size = new System.Drawing.Size(162, 25);
            this.cboPhilosophy.TabIndex = 14;
            this.cboPhilosophy.Text = "School of Philosophy";
            this.cboPhilosophy.SelectedIndexChanged += new System.EventHandler(this.CboPhilosophy_SelectedIndexChanged);
            // 
            // cboGender
            // 
            this.cboGender.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.cboGender.Enabled = false;
            this.cboGender.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cboGender.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.cboGender.FormattingEnabled = true;
            this.cboGender.Items.AddRange(new object[] {
            "Male",
            "Female"});
            this.cboGender.Location = new System.Drawing.Point(180, 225);
            this.cboGender.Name = "cboGender";
            this.cboGender.Size = new System.Drawing.Size(162, 25);
            this.cboGender.TabIndex = 13;
            this.cboGender.Text = "Gender";
            this.cboGender.TextChanged += new System.EventHandler(this.CboGender_TextChanged);
            // 
            // lblHeader4
            // 
            this.lblHeader4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.lblHeader4.Font = new System.Drawing.Font("Viner Hand ITC", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblHeader4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.lblHeader4.Location = new System.Drawing.Point(0, 0);
            this.lblHeader4.Name = "lblHeader4";
            this.lblHeader4.Size = new System.Drawing.Size(724, 34);
            this.lblHeader4.TabIndex = 31;
            this.lblHeader4.Text = "Project: Phoenix";
            this.lblHeader4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnNCharcterCreate
            // 
            this.btnNCharcterCreate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.btnNCharcterCreate.FlatAppearance.BorderSize = 0;
            this.btnNCharcterCreate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNCharcterCreate.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnNCharcterCreate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.btnNCharcterCreate.ImageAlign = System.Drawing.ContentAlignment.TopRight;
            this.btnNCharcterCreate.Location = new System.Drawing.Point(180, 330);
            this.btnNCharcterCreate.Name = "btnNCharcterCreate";
            this.btnNCharcterCreate.Size = new System.Drawing.Size(75, 35);
            this.btnNCharcterCreate.TabIndex = 15;
            this.btnNCharcterCreate.Text = "Create";
            this.btnNCharcterCreate.UseVisualStyleBackColor = false;
            this.btnNCharcterCreate.Click += new System.EventHandler(this.BtnNCharacterCreate_Click);
            // 
            // txtCharacterName
            // 
            this.txtCharacterName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.txtCharacterName.Font = new System.Drawing.Font("Nirmala UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtCharacterName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.txtCharacterName.Location = new System.Drawing.Point(180, 190);
            this.txtCharacterName.Name = "txtCharacterName";
            this.txtCharacterName.Size = new System.Drawing.Size(162, 25);
            this.txtCharacterName.TabIndex = 12;
            this.txtCharacterName.Text = "Name";
            this.txtCharacterName.TextChanged += new System.EventHandler(this.TxtCharacterName_TextChanged);
            this.txtCharacterName.Enter += new System.EventHandler(this.TxtCharacterName_Enter);
            this.txtCharacterName.Leave += new System.EventHandler(this.TxtCharacterName_Leave);
            // 
            // ptbPhoenix4
            // 
            this.ptbPhoenix4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(40)))), ((int)(((byte)(49)))));
            this.ptbPhoenix4.Location = new System.Drawing.Point(0, 37);
            this.ptbPhoenix4.Name = "ptbPhoenix4";
            this.ptbPhoenix4.Image = Image.FromFile("./Images/UI/Logo.png");
            this.ptbPhoenix4.Size = new System.Drawing.Size(724, 489);
            this.ptbPhoenix4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.ptbPhoenix4.TabIndex = 30;
            this.ptbPhoenix4.TabStop = false;
            // 
            // FrmLauncher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(62)))), ((int)(((byte)(70)))));
            this.ClientSize = new System.Drawing.Size(969, 572);
            this.Controls.Add(this.pnlAccountCreate);
            this.Controls.Add(this.pnlAccountView);
            this.Controls.Add(this.pnlCharacterCreation);
            this.Controls.Add(this.pnlAuthenicate);
            this.Controls.Add(this.pnlSide);
            this.Font = new System.Drawing.Font("Nirmala UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmLauncher";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FrmLauncher_MouseDown);
            this.pnlSide.ResumeLayout(false);
            this.pnlAccount.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ptbAccountImage)).EndInit();
            this.pnlAuthenicate.ResumeLayout(false);
            this.pnlAuthenicate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbDiscord)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix1)).EndInit();
            this.pnlAccountView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCharacter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix3)).EndInit();
            this.pnlAccountCreate.ResumeLayout(false);
            this.pnlAccountCreate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix2)).EndInit();
            this.pnlCharacterCreation.ResumeLayout(false);
            this.pnlCharacterCreation.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbCharacter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ptbPhoenix4)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlSide;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnAccount;
        private System.Windows.Forms.Panel pnlAccount;
        private System.Windows.Forms.Label lblAccount;
        private System.Windows.Forms.PictureBox ptbAccountImage;
        private System.Windows.Forms.Panel pnlAuthenicate;
        private System.Windows.Forms.Button btnStore;
        private System.Windows.Forms.Label lblHeader1;
        private System.Windows.Forms.Button btnAuthenticate;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.TextBox txtAccountName;
        private System.Windows.Forms.PictureBox ptbDiscord;
        private System.Windows.Forms.PictureBox ptbPhoenix1;
        private System.Windows.Forms.Panel pnlCharacterCreation;
        private System.Windows.Forms.TextBox txtClassInformation;
        private System.Windows.Forms.ComboBox cboPhilosophy;
        private System.Windows.Forms.ComboBox cboGender;
        private System.Windows.Forms.Label lblHeader4;
        private System.Windows.Forms.Button btnNCharcterCreate;
        private System.Windows.Forms.TextBox txtCharacterName;
        private System.Windows.Forms.PictureBox ptbPhoenix4;
        private System.Windows.Forms.Panel pnlAccountView;
        private System.Windows.Forms.DataGridView dgvCharacter;
        private System.Windows.Forms.Button btnDeleteCharacter;
        private System.Windows.Forms.Button btnTransferCharacter;
        private System.Windows.Forms.Button btnCharacterCreate;
        private System.Windows.Forms.Label lblHeader3;
        private System.Windows.Forms.Button btnCharacterConnect;
        private System.Windows.Forms.PictureBox ptbPhoenix3;
        private System.Windows.Forms.Panel pnlAccountCreate;
        private System.Windows.Forms.TextBox txtNVerifyPassword;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblHeader2;
        private System.Windows.Forms.Button btnNCreate;
        private System.Windows.Forms.TextBox txtNPassword;
        private System.Windows.Forms.TextBox txtNAccount;
        private System.Windows.Forms.PictureBox ptbPhoenix2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCharacterName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCharacterCaste;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvCharacterPhilosophy;
        private System.Windows.Forms.PictureBox pbCharacter;
        private System.Windows.Forms.ComboBox cboImage;
        private System.Windows.Forms.Button btnCancel;
    }
}