using Phoenix.Client.Classes.Extensions;
using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Commands.Failure;
using Phoenix.Common.Commands.Request;
using Phoenix.Common.Commands.Response;
using Phoenix.Common.Commands.Server;
using Phoenix.Common.Commands.Staff;
using Phoenix.Common.Commands.Updates;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using System;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class FrmClient : Form
    {
        private Classes.Network.Client client;
        private Character character;
        private FrmLauncher launcherForm;
        private readonly FrmDebug debugForm;
        public event EventHandler<string> NewPacket;
        private readonly int chatLength = 100;
        private string lastCommand = "";

        public FrmClient(FrmLauncher frmLauncher)
        {
            InitializeComponent();
            InitializeControl();
            this.debugForm = new(this);
            this.launcherForm = frmLauncher;
            if (Constants.DEBUG_SHOW)
                debugForm.Show();
        }

        #region -- Client Events --
        private void Client_OnActivity(object sender, string e)

        {
            if (Constants.DEBUG_SHOW)
            {
                Invoke((Action)delegate
                {
                    this.NewPacket?.Invoke(this, e);
                });
            }
            string[] commands = e.Split("%", StringSplitOptions.RemoveEmptyEntries);
            foreach (string c in commands)
            {
                var command = CommandFactory.ParseCommand(c);
                switch (command.CommandType)
                {

                    #region -- Client Connect Response --
                    case CommandType.ClientConnectResponse:
                        {
                            var clientConnectResponseCommand = command as ClientConnectResponse;

                            if (clientConnectResponseCommand.Success)
                            {
                                this.Invoke((Action)delegate
                                {
                                    UpdateChat(clientConnectResponseCommand.Message);
                                });
                                var clientRoomCommand = new ClientRoomRequest
                                {
                                    RoomID = this.character.RoomID
                                };
                                SendCommand(clientRoomCommand);
                                continue;
                            }
                            else
                            {
                                MessageBox.Show("Server Response Failed on Connection Handshake. Please restart your client and try again.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                this.client.Stop();
                            }

                            continue;
                        }
                    #endregion

                    #region -- Client Room Response --
                    case CommandType.ClientRoomResponse:
                        {
                            var clientRooomResponseCommand = command as ClientRoomResponse;

                            if (clientRooomResponseCommand.Success)
                            {
                                this.Invoke((Action)delegate
                                {
                                    this.lstvRoom.Items.Clear();
                                    this.lstvDrops.Items.Clear();

                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\bsouth\b", "~cSouth~w", RegexOptions.IgnoreCase);
                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\bnorth\b", "~cNorth~w", RegexOptions.IgnoreCase);
                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\bwest\b", "~cWest~w", RegexOptions.IgnoreCase);
                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\beast\b", "~cEast~w", RegexOptions.IgnoreCase);
                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\bup\b", "~cUp~w", RegexOptions.IgnoreCase);
                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\bdown\b", "~cDown~w", RegexOptions.IgnoreCase);
                                    clientRooomResponseCommand.Room.Exits = Regex.Replace(clientRooomResponseCommand.Room.Exits, @"\ball\b", "~cAll~w", RegexOptions.IgnoreCase);

                                    var message = $"~g<~w{clientRooomResponseCommand.Room.Name}~g>~w {clientRooomResponseCommand.Room.Description} {clientRooomResponseCommand.Room.Exits}\n";
                                    UpdateChat(message);
                                    foreach (Character character in clientRooomResponseCommand.Room.RoomCharacters)
                                    {
                                        UpdateRoom(1, character.Name, character.Image, character.Type);
                                    }
                                    foreach (NPC NPC in clientRooomResponseCommand.Room.RoomNPC)
                                    {
                                        UpdateRoom(1, NPC.Name, NPC.Image, NPC.Type);
                                    }
                                    foreach (Item item in clientRooomResponseCommand.Room.RoomItems)
                                    {
                                        UpdateDrop(1, item.Name, item.Image, item.Rarity);
                                    }
                                });
                            }

                            continue;
                        }
                    #endregion

                    #region -- Room Player Update --
                    case CommandType.RoomCharacterUpdate:
                        {
                            var RoomCharacterUpdateCommand = command as RoomCharacterUpdate;

                            this.Invoke((Action)delegate
                            {
                                UpdateRoom(RoomCharacterUpdateCommand.Mode, RoomCharacterUpdateCommand.Character.Name, RoomCharacterUpdateCommand.Character.Image, RoomCharacterUpdateCommand.Character.Type);
                            });

                            continue;
                        }
                    #endregion

                    #region -- Room NPC Update --
                    case CommandType.RoomNPCUpdate:
                        {
                            var roomNPCUpdateCommand = command as RoomNPCUpdate;

                            this.Invoke((Action)delegate
                            {
                                UpdateRoom(roomNPCUpdateCommand.Mode, roomNPCUpdateCommand.NPC.Name, roomNPCUpdateCommand.NPC.Image, roomNPCUpdateCommand.NPC.Type);
                            });

                            continue;
                        }
                    #endregion

                    #region -- Character Stat Update --
                    case CommandType.CharacterStatUpdate:
                        {
                            var parsedCommand = command as CharacterStatUpdate;

                            this.Invoke((Action)delegate
                            {
                                this.character = parsedCommand.Character;
                                this.CharacterLoad();
                            });

                            continue;
                        }
                    #endregion

                    #region -- Room Item Update --
                    case CommandType.RoomItemUpdate:
                        {
                            var parsedCommand = command as RoomItemUpdate;

                            this.Invoke((Action)delegate
                            {
                                this.UpdateDrop(parsedCommand.Mode, parsedCommand.Item.Name, parsedCommand.Item.Image, parsedCommand.Item.Rarity);
                            });

                            continue;
                        }
                    #endregion

                    #region -- Message Room --
                    case CommandType.MessageRoom:
                        {
                            this.Invoke((Action)delegate
                            {
                                var messageRoomCommand = command as MessageRoomServer;
                                if (messageRoomCommand.Character == null)
                                {
                                    UpdateChat($"{messageRoomCommand.Message}\n");
                                }
                                else
                                {
                                    UpdateChat($"~yFrom ~w{messageRoomCommand.Character.Name.FirstCharToUpper()}~y: {messageRoomCommand.Message}\n");
                                }
                            });
                            continue;
                        }

                    #endregion

                    #region -- Message Player --
                    case CommandType.MessageDirect:
                        {
                            this.Invoke((Action)delegate
                            {
                                var messagePlayerCommand = command as MessageDirectServer;
                                if (messagePlayerCommand.SendingName == this.character.Name)
                                {
                                    UpdateChat($"~mTell To ~w{messagePlayerCommand.ReceivingName.FirstCharToUpper()}~m: {messagePlayerCommand.Message}\n");
                                }
                                else if (messagePlayerCommand.ReceivingName == this.character.Name)
                                {
                                    UpdateChat($"~mTell From ~w{messagePlayerCommand.SendingName.FirstCharToUpper()}~m: {messagePlayerCommand.Message}\n");
                                }
                                else
                                {
                                    UpdateChat($"{messagePlayerCommand.Message}\n");
                                }
                            });
                            continue;
                        }

                    #endregion

                    #region -- Message World --
                    case CommandType.MessageWorld:
                        {
                            this.Invoke((Action)delegate
                            {
                                var messageWorldCommand = command as MessageWorldServer;
                                UpdateChat($"~w*~qThe ancients rejoice chanting: ~w{messageWorldCommand.Message}\n");
                            });
                            continue;
                        }

                    #endregion

                    #region -- Map Response --
                    case CommandType.MapResponse:
                        {
                            var roomMapResponse = command as RoomMapResponse;
                            if (roomMapResponse.Success)
                            {
                                this.Invoke((Action)delegate
                                {
                                    this.pbMap.DrawMap(roomMapResponse.To2DArray());
                                });
                            }
                            continue;
                        }
                    #endregion

                    #region -- No Command --
                    case CommandType.NoCommand:
                        var noCommand = command as NoCommandFailure;
                        this.Invoke((Action)delegate
                        {
                            UpdateChat($"{noCommand.Message}\n");
                        });
                        continue;
                    #endregion

                    #region -- No Player --
                    case CommandType.NoPlayer:
                        var noPlayer = command as NoPlayerFailure;
                        this.Invoke((Action)delegate
                        {
                            UpdateChat($"{noPlayer.Message}\n");
                        });
                        continue;
                        #endregion

                }
            }
        }
        private void Client_IsConnected(object sender, bool isReconnected)
        {

        }
        private void Client_IsClosed(object sender, bool remote)
        {
            switch (remote)
            {
                case true:
                    MessageBox.Show("The connection was closed by the server.", Constants.GAME_NAME_DISPLAY, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.launcherForm.Invoke((Action)delegate
                    {

                    });
                    return;
                default:
                    return;
            }
        }
        #endregion

        #region -- Send Command --
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

        #region -- Inits & Text Bar --
        private void InitializeControl()
        {
            this.Text = Constants.GAME_NAME_DISPLAY;

            pnlCharacter.ForeColor = Color.FromArgb(238, 238, 238);
            pnlCharacter.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlMap.ForeColor = Color.FromArgb(238, 238, 238);
            pnlMap.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlHotBar.ForeColor = Color.FromArgb(238, 238, 238);
            pnlHotBar.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlGameChat.ForeColor = Color.FromArgb(238, 238, 238);
            pnlGameChat.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlSecondaryWindow.ForeColor = Color.FromArgb(238, 238, 238);
            pnlSecondaryWindow.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlBuff.ForeColor = Color.FromArgb(238, 238, 238);
            pnlBuff.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlRoom.ForeColor = Color.FromArgb(238, 238, 238);
            pnlRoom.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlDrops.ForeColor = Color.FromArgb(238, 238, 238);
            pnlDrops.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlEquipped.ForeColor = Color.FromArgb(238, 238, 238);
            pnlEquipped.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            pnlInventory.ForeColor = Color.FromArgb(238, 238, 238);
            pnlInventory.Font = new Font("Nirmala UI", 9, FontStyle.Bold);

            MoveControls.Init(this.pnlCharacter);
            MoveControls.Init(this.pnlMap);
            MoveControls.Init(this.pnlHotBar);
            MoveControls.Init(this.pnlSecondaryWindow);
            MoveControls.Init(this.pnlGameChat);
            MoveControls.Init(this.pnlBuff);
            MoveControls.Init(this.pnlRoom);
            MoveControls.Init(this.pnlDrops);
            MoveControls.Init(this.pnlEquipped);
            MoveControls.Init(this.pnlInventory);
            MoveControls.Init(this.pnlInput);

            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Helmet");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Shoulder");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Cloak");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Neck");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Wrist");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Chest");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Waist");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Legs");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Gloves");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Boots");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Main Hand");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Off Hand");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Left Ring");
            UpdateEquipped(1, "None", "", "(Junk)", "Slot: Right Ring");
        }
        private void FrmClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        public void Initialize(Classes.Network.Client client, Character character, FrmLauncher parent)
        {
            this.client = client;
            this.character = character;
            this.launcherForm = parent;
            this.client.OnActivity += Client_OnActivity;
            this.client.IsConnected += Client_IsConnected;
            this.client.IsClosed += Client_IsClosed;
            var clientConnectCommand = new ClientConnectRequest
            {
                Id = this.character.Id
            };

            SendCommand(clientConnectCommand);
            this.CharacterLoad();
            this.rtbChat.SelectionColor = Color.LawnGreen;
            this.rtbChat.AppendText("Connecting to server...\n");
        }
        private void TxtInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.NumPad8)
            {
                e.SuppressKeyPress = true;
                HandleCommands("n");
                return;
            }

            if (e.KeyCode == Keys.NumPad6)
            {
                e.SuppressKeyPress = true;
                HandleCommands("e");
                return;
            }

            if (e.KeyCode == Keys.NumPad2)
            {
                e.SuppressKeyPress = true;
                HandleCommands("s");
                return;
            }

            if (e.KeyCode == Keys.NumPad4)
            {
                e.SuppressKeyPress = true;
                HandleCommands("w");
                return;
            }

            if (e.KeyCode == Keys.Up)
            {
                this.txtInput.Text = "";
                txtInput.Text = this.lastCommand;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                if (txtInput.Text == "")
                    return;
                HandleCommands(txtInput.Text.Trim());
                this.lastCommand = txtInput.Text;
                txtInput.Text = "";
                return;
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

        private void PnlTaskBar_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                _ = SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        protected override void OnMaximizedBoundsChanged(EventArgs e)
        {
            base.OnMaximizedBoundsChanged(e);
            foreach (Screen screen in Screen.AllScreens)
            {
                if (screen.Primary)
                {
                    this.Bounds = screen.Bounds;
                }
            }
        }
        #endregion

        #region -- Client Updaters --

        #region -- Client Start Updates --
        private void CharacterLoad()
        {
            this.lblName.Text = "Name: " + this.character.Name.ToString().FirstCharToUpper();
            if (!this.ilAvatar.Images.Keys.Contains(this.character.Image))
            {
                this.ilAvatar.Images.Add(this.character.Image, Image.FromFile("./Images/Avatar/" + this.character.Image));
            }
            this.pbPlayer.Image = Image.FromFile("./Images/Avatar/" + this.character.Image);
            this.lblCaste.Text = "Caste: " + this.character.Caste;
            this.lblRank.Text = "Rank: " + this.character.Rank;
            this.lblPhilsophy.Text = "Philosophy: " + this.character.Philosophy;
            this.lblBaseStr.Text = this.character.Strength.ToString();
            this.lblBaseAgi.Text = this.character.Agility.ToString();
            this.lblBaseInt.Text = this.character.Intellect.ToString();
            this.lblBaseStam.Text = this.character.Stamina.ToString();
            this.lblBaseDamage.Text = this.character.Damage.ToString();
            this.lblBaseCrit.Text = this.character.Crit.ToString() + "%";
            this.lblBaseMastery.Text = this.character.Mastery.ToString() + "%";
            this.lblBaseHaste.Text = this.character.Haste.ToString() + "%";
            this.lblBaseVers.Text = this.character.Versatility.ToString() + "%";
            this.lblWeight.Text = "Weight: 0 / " + (this.character.Strength * 2);
            this.vbExp.Maximum = this.character.MaxExperience;
            if (this.character.Experience > this.vbExp.Maximum)
                this.vbExp.Value = this.vbExp.Maximum;
            else
                this.vbExp.Value = this.character.Experience;
            this.vbExp.Refresh();
            this.vbHealth.Maximum = this.character.Health;
            this.vbHealth.Value = this.character.CurrentHealth;
            this.vbHealth.Refresh();
            this.vbMana.Maximum = this.character.Mana;
            this.vbMana.Value = this.character.CurrentMana;
            this.vbMana.Refresh();
            this.lblGold.Text = "Gold: " + this.character.Gold.ToString();
            foreach (Item item in this.character.Items)
            {
                if (item.IsEquipped)
                    UpdateEquipped(3, item.Name, item.Image, item.Rarity, "", item.SlotIndex);
                else if (item.SlotIndex != -1)
                    UpdateInventory(3, item.Name, item.Image, item.Rarity, item.Type, item.SlotIndex);
                else
                    UpdateInventory(1, item.Name, item.Image, item.Rarity, item.Type);
            }
        }
        #endregion

        #region -- Client UI Updates --
        private void UpdateRoom(int mode, string NPCName = "", string NPCImage = "", string NPCType = "")
        {
            // Mode 1 = Add, Mode 2 = Remove
            if (mode == 1)
            {
                if (!this.ilAvatar.Images.Keys.Contains(NPCImage))
                {
                    this.ilAvatar.Images.Add(NPCImage, Image.FromFile("./Images/Avatar/" + NPCImage));
                }
                lstvRoom.Items.Add(NPCName.FirstCharToUpper(), NPCImage).SubItems.Add(NPCType);
                int NPCIndex = lstvRoom.Items.Count - 1;
                UpdateNPCColor(NPCIndex, NPCType);
            }
            else if (mode == 2)
            {
                foreach (ListViewItem item in lstvRoom.Items)
                {
                    if (item.Text == NPCName)
                    {
                        lstvRoom.Items.Remove(item);
                    }
                }
            }
        }

        private void UpdateDrop(int mode, string itemName = "", string itemImage = "", string itemType = "")
        {
            // Mode 1 = Add, Mode 2 = Remove
            if (mode == 1)
            {
                if (!this.ilItems.Images.Keys.Contains(itemImage))
                {
                    this.ilItems.Images.Add(itemImage, Image.FromFile("./Images/Items/" + itemImage));
                }
                lstvDrops.Items.Add(itemName, itemImage).SubItems.Add(itemType);
                int NPCIndex = lstvDrops.Items.Count - 1;
                UpdateDropColor(NPCIndex, itemType);
            }
            else if (mode == 2)
            {
                foreach (ListViewItem item in lstvDrops.Items)
                {
                    if (item.Selected)
                    {
                        lstvDrops.Items.Remove(item);
                    }
                }
            }
        }

        private void UpdateEquipped(int mode, string itemName = "", string itemImage = "", string itemType = "", string slotType = "", int updateSlot = -1)
        {
            // Mode 1 = Add, Mode 2 = Remove, Mode 3 = Update
            if (mode == 1)
            {
                lstvEquipped.Items.Add(itemName).SubItems.Add(slotType);
                int NPCIndex = lstvEquipped.Items.Count - 1;
                lstvEquipped.Items[NPCIndex].SubItems.Add(itemType);
                UpdateEquipColor(NPCIndex, itemType);
            }
            else if (mode == 2)
            {
                foreach (ListViewItem item in lstvRoom.Items)
                {
                    if (item.Selected)
                    {
                        item.Text = "None";
                        item.ForeColor = Color.Gray;
                    }
                }
            }
            else if (mode == 3)
            {
                if (!this.ilItems.Images.Keys.Contains(itemImage))
                {
                    this.ilItems.Images.Add(itemImage, Image.FromFile("./Images/Items/" + itemImage));
                }
                switch (updateSlot)
                {
                    case 0: // Helmet
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 1: // Shoulder
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 2: // Cloak
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 3: // Neck
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 4: // Wrist
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 5: // Chest
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 6: // Waist
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 7: // Legs
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 8: // Gloves
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 9: // Boots
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 10: // Main Hand
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 11: // Off Hand
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 12: // Left Ring
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    case 13: // Right Ring
                        lstvEquipped.Items[updateSlot].Text = itemName;
                        lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                        lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                        UpdateEquipColor(updateSlot, itemType);
                        return;
                    default:
                        return;
                }
            }
        }

        private void UpdateInventory(int mode, string itemName = "", string itemImage = "", string itemType = "", string slotType = "", int updateSlot = -1)
        {
            // Mode 1 = Add, Mode 2 = Remove
            if (mode == 1)
            {
                if (!this.ilItems.Images.Keys.Contains(itemImage))
                {
                    this.ilItems.Images.Add(itemImage, Image.FromFile("./Images/Items/" + itemImage));
                }
                lstvInventory.Items.Add(itemName, itemImage).SubItems.Add(slotType);
                int NPCIndex = lstvInventory.Items.Count - 1;
                lstvInventory.Items[NPCIndex].SubItems.Add(itemType);
                UpdateInventoryColor(NPCIndex, itemType);
            }
            else if (mode == 2)
            {
                foreach (ListViewItem item in lstvInventory.Items)
                {
                    if (item.Selected)
                    {
                        lstvInventory.Items.Remove(item);
                    }
                }
            }
            else if (mode == 3)
            {
                if (lstvInventory.Items.Count < updateSlot && updateSlot != -1)
                {
                    if (!this.ilItems.Images.Keys.Contains(itemImage))
                    {
                        this.ilItems.Images.Add(itemImage, Image.FromFile("./Images/Items/" + itemImage));
                    }
                    lstvInventory.Items.Add(itemName, itemImage).SubItems.Add(slotType);
                    int NPCIndex = lstvInventory.Items.Count - 1;
                    lstvInventory.Items[NPCIndex].SubItems.Add(itemType);
                    UpdateInventoryColor(NPCIndex, itemType);
                    return;
                }
                else
                {
                    lstvInventory.Items[updateSlot - 1].Text = itemName;
                    lstvInventory.Items[updateSlot - 1].ImageKey = itemImage;
                    lstvInventory.Items[updateSlot - 1].SubItems[2].Text = itemType;
                    UpdateInventoryColor(updateSlot - 1, itemType);
                    return;
                }
            }
        }
        #endregion

        #region -- Client Color Formatting --
        private void UpdateEquipColor(int NPCIndex, string itemType)
        {
            switch (itemType)
            {
                case "(Junk)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
                case "(Common)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Uncommon)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Rare)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Epic)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Legendary)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Ancient)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                case "(Ethryeal)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.PaleTurquoise;
                    return;
                case "(Godly)":
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.OrangeRed;
                    return;
                default:
                    lstvEquipped.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }

        private void UpdateDropColor(int NPCIndex, string itemType)
        {
            switch (itemType)
            {
                case "(Junk)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
                case "(Common)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Uncommon)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Rare)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Epic)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Legendary)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Ancient)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                case "(Ethyreal)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.PaleTurquoise;
                    return;
                case "(Godly)":
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.OrangeRed;
                    return;
                default:
                    lstvDrops.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }

        private void UpdateNPCColor(int NPCIndex, string NPCType)
        {
            switch (NPCType)
            {
                case "(God)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Demi-God)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Yellow;
                    return;
                case "(Immortal)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Hero)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Player)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Friendly)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Spawned)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.LightSteelBlue;
                    return;
                case "(Neutral)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Bisque;
                    return;
                case "(Hostile)":
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                default:
                    lstvRoom.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }

        private void UpdateInventoryColor(int NPCIndex, string itemType)
        {
            switch (itemType)
            {
                case "(Junk)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
                case "(Common)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Uncommon)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Rare)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Epic)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Legendary)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Ancient)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                case "(Ethyreal)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.PaleTurquoise;
                    return;
                case "(Godly)":
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.OrangeRed;
                    return;
                default:
                    lstvInventory.Items[NPCIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }
        #endregion

        #endregion

        #region -- Server Incoming Messages --
        private void UpdateEquipItem(string itemName, string itemImage, string itemType, string slotType, int updateSlot)
        {
            UpdateEquipped(3, itemName, itemImage, itemType, slotType, 12);
            UpdateInventory(2);
        }

        private void UpdateChat(string message)
        {
            if (this.rtbChat.Lines.Length > chatLength)
            {
                this.rtbChat.ReadOnly = false;
                this.rtbChat.SelectionStart = 0;
                this.rtbChat.Select(0, this.rtbChat.GetFirstCharIndexFromLine(chatLength - 10));
                this.rtbChat.Cut();
                Clipboard.Clear();
                this.rtbChat.ReadOnly = true;
            }
            message = Helper.ReturnPipe(message);
            message = Helper.ReturnTilda(message);
            message = Helper.ReturnCaret(message);
            message = Helper.ReturnPercent(message);
            string[] displayMessage = message.Split("~");
            if (displayMessage[0] != "")
            {
                this.rtbChat.SelectionColor = Helper.ReturnColor(displayMessage[0].ToCharArray()[0]);
                this.rtbChat.AppendText(displayMessage[0]);
            }
            for (int i = 1; i < displayMessage.Length; i++)
            {
                if (displayMessage[i] == "")
                    continue;
                this.rtbChat.SelectionColor = Helper.ReturnColor(displayMessage[i].ToCharArray()[0]);
                this.rtbChat.AppendText(displayMessage[i].Remove(0, 1));
            }
            this.rtbChat.SelectionStart = this.rtbChat.Text.Length;
            this.rtbChat.ScrollToCaret();
        }
        #endregion

        #region -- Server Outgoing Messages --
        private void HandleCommands(string message)
        {
            message = Helper.RemoveCaret(message);
            message = Helper.RemovePipe(message);
            message = Helper.RemoveTilda(message);
            message = Helper.RemovePercent(message);

            string[] command = message.Split(" ", StringSplitOptions.RemoveEmptyEntries);

            if (command[0].StartsWith("/"))
            {
                switch (command[0].ToLower()[1..])
                {
                    case "wmsg":
                    case "wm":
                        {
                            if (command.Length < 2)
                            {
                                UpdateChat("~rYou need to enter a message to send to the world!\n");
                                return;
                            }
                            message = message.Replace("/wmsg ", "");
                            message = message.Replace("/wm ", "");
                            SendCommand(new MessageWorldServer
                            {
                                ID = this.character.Id,
                                Message = message
                            });
                            return;
                        }
                    case "t":
                    case "tell":
                        {
                            if (command.Length < 3)
                            {
                                UpdateChat("~rPlease enter a message when sending a tell!\n");
                                return;
                            }
                            message = message.Replace("/tell ", "");
                            message = message.Replace("/t ", "");
                            message = message.Replace(command[1], "");
                            SendCommand(new MessageDirectServer
                            {
                                SendingName = this.character.Name,
                                ReceivingName = command[1],
                                Message = message
                            });
                            return;
                        }
                    case "su":
                    case "summon":
                        {
                            if (command.Length < 2)
                            {
                                UpdateChat("~rPlease enter a player name!\n");
                                return;
                            }
                            message = message.Replace("/summon ", "");
                            message = message.Replace("/su ", "");
                            SendCommand(new SummonPlayerStaff
                            {
                                Type = 1,
                                Name = command[1],
                            });
                            return;
                        }
                    default:
                        SendCommand(new SlashCommandRequest
                        {
                            Message = message
                        });
                        return;
                }
            }
            else
            {
                switch (command[0].ToLower())
                {
                    case "north":
                    case "n":
                        {
                            SendCommand(new CharacterMoveRequest
                            {
                                Direction = "north"
                            });
                            return;
                        }
                    case "south":
                    case "s":
                        {
                            SendCommand(new CharacterMoveRequest
                            {
                                Direction = "south"
                            });
                            return;
                        }
                    case "west":
                    case "w":
                        {
                            SendCommand(new CharacterMoveRequest
                            {
                                Direction = "west"
                            });
                            return;
                        }
                    case "east":
                    case "e":
                        {
                            SendCommand(new CharacterMoveRequest
                            {
                                Direction = "east"
                            });
                            return;
                        }
                    default:
                        {
                            SendCommand(new MessageRoomServer
                            {
                                Character = this.character,
                                Message = message
                            });
                            return;
                        }
                }
            }

        }
        #endregion

        #region -- Client Menu Code --
        private void DropToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstvInventory.Items)
            {
                if (item.Selected)
                {
                    _ = item.Text;
                    _ = item.ImageKey;
                    _ = item.SubItems[2].Text;
                    _ = item.SubItems[1].Text;
                    // TODO: Send Update Request Pack
                }
            }
        }
        private void EquipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lstvInventory.Items)
            {
                if (item.Selected)
                {
                    _ = item.Text;
                    _ = item.ImageKey;
                    _ = item.SubItems[2].Text;
                    _ = item.SubItems[1].Text;
                    // TODO: Send Update Request Pack
                }
            }
        }
        private void CMDropsGet_Click(object sender, EventArgs e)
        {
            SendCommand(new ItemLootRequest
            {
                DropIndex = lstvDrops.FocusedItem.Index
            });
        }
        #endregion


    }
}
