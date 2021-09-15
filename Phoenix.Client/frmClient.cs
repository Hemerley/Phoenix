using Phoenix.Client.Classes.Extensions;
using Phoenix.Common.Data;
using Phoenix.Common.Data.Types;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Phoenix.Client
{
    public partial class FrmClient : Form
    {
        private Classes.Network.Client client;
        private Character character;
        private FrmLauncher launcherForm;

        public FrmClient()
        {
            InitializeComponent();
            InitializeControl();
        }

        public void Initialize(Classes.Network.Client client, Character character, FrmLauncher parent)
        {
            this.client = client;
            this.character = character;
            this.launcherForm = parent;
            this.client.OnActivity += Client_OnActivity;
            this.client.IsConnected += Client_IsConnected;
            this.client.IsClosed += Client_IsClosed;
            this.CharacterLoad();
            this.UpdateRoom(1, this.character.Name, this.character.Image, this.character.Type, true);
        }

        private void Client_OnActivity(object sender, string e)
        {

        }

        private void Client_IsConnected(object sender, bool isReconnected)
        {

        }

        private void Client_IsClosed(object sender, bool remote)
        {

        }



        #region ---Initialize Controls---

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

            MoveControls.Init(pnlCharacter);
            MoveControls.Init(pnlMap);
            MoveControls.Init(pnlHotBar);
            MoveControls.Init(pnlSecondaryWindow);
            MoveControls.Init(pnlGameChat);
            MoveControls.Init(pnlBuff);
            MoveControls.Init(pnlRoom);
            MoveControls.Init(pnlDrops);
            MoveControls.Init(pnlEquipped);
            MoveControls.Init(pnlInventory);

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
        
        #endregion

        #region ---Move Window---

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

        #endregion

        #region ---Client Updaters---

        private void CharacterLoad()
        {

            this.lblName.Text = "Name: " + this.character.Name.ToString();
            this.ilAvatar.Images.Add(this.character.Image, Image.FromFile("./Images/Avatar/" + this.character.Image));
            this.pictureBox1.Image = Image.FromFile("./Images/Avatar/" + this.character.Image);
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

        }

        // Window Updaters
        private void UpdateRoom(int mode, string entityName = "", string entityImage = "", string entityType = "", bool imageShow = true)
        {
            // Mode 1 = Add, Mode 2 = Remove
            if (mode == 1)
            {
                if (imageShow)
                {
                    lstvRoom.Items.Add(entityName, entityImage).SubItems.Add(entityType);
                    int entityIndex = lstvRoom.Items.Count - 1;
                    UpdateEntityColor(entityIndex, entityType);
                }
                else
                {
                    lstvRoom.Items.Add(entityName).SubItems.Add(entityType);
                    int entityIndex = lstvRoom.Items.Count - 1;
                    UpdateEntityColor(entityIndex, entityType);
                }
            }
            else if (mode == 2)
            {
                foreach (ListViewItem item in lstvRoom.Items)
                {
                    if (item.Selected)
                    {
                        lstvRoom.Items.Remove(item);
                    }
                }
            }
        }

        private void UpdateDrop(int mode, string itemName = "", string itemImage = "", string itemType = "", bool imageShow = true)
        {
            // Mode 1 = Add, Mode 2 = Remove
            if (mode == 1)
            {
                if (imageShow)
                {
                    lstvDrops.Items.Add(itemName, itemImage).SubItems.Add(itemType);
                    int entityIndex = lstvDrops.Items.Count - 1;
                    UpdateDropColor(entityIndex, itemType);
                }
                else
                {
                    lstvDrops.Items.Add(itemName).SubItems.Add(itemType);
                    int entityIndex = lstvDrops.Items.Count - 1;
                    UpdateDropColor(entityIndex, itemType);
                }
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
        
        private void UpdateEquipped(int mode, string itemName = "", string itemImage = "", string itemType = "", string slotType = "", int updateSlot = -1, bool imageShow = true)
        {
            // Mode 1 = Add, Mode 2 = Remove, Mode 3 = Update
            if (mode == 1)
            {
                if (imageShow)
                {
                    lstvEquipped.Items.Add(itemName, itemImage).SubItems.Add(slotType);
                    int entityIndex = lstvEquipped.Items.Count - 1;
                    lstvEquipped.Items[entityIndex].SubItems.Add(itemType);
                    UpdateEquipColor(entityIndex, itemType);
                }
                else
                {
                    lstvEquipped.Items.Add(itemName).SubItems.Add(slotType);
                    int entityIndex = lstvEquipped.Items.Count - 1;
                    lstvEquipped.Items[entityIndex].SubItems.Add(itemType);
                    UpdateEquipColor(entityIndex, itemType);
                }
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
                switch (updateSlot)
                {
                    case 0: // Helmet
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 1: // Shoulder
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 2: // Cloak
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 3: // Neck
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 4: // Wrist
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 5: // Chest
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 6: // Waist
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 7: // Legs
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 8: // Gloves
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 9: // Boots
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 10: // Main Hand
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return; 
                    case 11: // Off Hand
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 12: // Left Ring
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    case 13: // Right Ring
                        if (imageShow)
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].ImageKey = itemImage;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        else
                        {
                            lstvEquipped.Items[updateSlot].Text = itemName;
                            lstvEquipped.Items[updateSlot].SubItems[2].Text = itemType;
                            UpdateEquipColor(updateSlot, itemType);
                        }
                        return;
                    default:
                        return;
                }
            }
        }
        
        private void UpdateInventory(int mode, string itemName = "", string itemImage = "", string itemType = "", string slotType = "", bool imageShow = true)
        {
            // Mode 1 = Add, Mode 2 = Remove
            if (mode == 1)
            {
                if (imageShow)
                {
                    lstvInventory.Items.Add(itemName, itemImage).SubItems.Add(slotType);
                    int entityIndex = lstvInventory.Items.Count - 1;
                    lstvInventory.Items[entityIndex].SubItems.Add(itemType);
                    UpdateInventoryColor(entityIndex, itemType);
                }
                else
                {
                    lstvInventory.Items.Add(itemName).SubItems.Add(slotType);
                    int entityIndex = lstvInventory.Items.Count - 1;
                    lstvInventory.Items[entityIndex].SubItems.Add(itemType);
                    UpdateInventoryColor(entityIndex, itemType);
                }
            }
            else if (mode == 2)
            {
                foreach(ListViewItem item in lstvInventory.Items)
                {
                    if (item.Selected)
                    {
                        lstvInventory.Items.Remove(item);
                    }
                }
            }
        }

        // Client Options
        

        // Color Formatting
        private void UpdateEquipColor(int entityIndex, string itemType)
        {
            switch (itemType)
            {
                case "(Junk)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
                case "(Common)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Uncommon)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Rare)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Epic)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Legendary)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Ancient)":
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                default:
                    lstvEquipped.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }

        private void UpdateDropColor(int entityIndex, string itemType)
        {
            switch (itemType)
            {
                case "(Junk)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
                case "(Common)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Uncommon)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Rare)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Epic)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Legendary)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Ancient)":
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                default:
                    lstvDrops.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }
        
        private void UpdateEntityColor(int entityIndex, string entityType)
        {
            switch (entityType)
            {
                case "(God)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Demi-God)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Yellow;
                    return;
                case "(Immortal)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Hero)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Player)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Friendly)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Spawned)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.LightSteelBlue;
                    return;
                case "(Neutral)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Bisque;
                    return;
                case "(Hostile)":
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                default:
                    lstvRoom.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }
        
        private void UpdateInventoryColor(int entityIndex, string itemType)
        {
            switch (itemType)
            {
                case "(Junk)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
                case "(Common)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.OldLace;
                    return;
                case "(Uncommon)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.Chartreuse;
                    return;
                case "(Rare)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.CornflowerBlue;
                    return;
                case "(Epic)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.Violet;
                    return;
                case "(Legendary)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.Orange;
                    return;
                case "(Ancient)":
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.Red;
                    return;
                default:
                    lstvInventory.Items[entityIndex].SubItems[0].ForeColor = Color.Gray;
                    return;
            }
        }

        // Server Requests
        private void UpdateEquipItem(string itemName, string itemImage, string itemType, string slotType, int updateSlot)
        {
            UpdateEquipped(3, itemName, itemImage, itemType, slotType, 12);
            UpdateInventory(2);
        }

        #endregion

        #region ---Client Menu Code---

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

        #endregion

        private void FrmClient_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            this.client.Stop();
        }
    }
}
