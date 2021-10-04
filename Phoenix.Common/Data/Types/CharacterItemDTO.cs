using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Phoenix.Common.Data.Types
{
    public class CharacterItemDTO
    {
        public int CharId { get; set; }
        public int CharAccountId { get; set; }
        public string CharName { get; set; }
        public int CharTypeID { get; set; }
        public string CharImage { get; set; }
        public string CharGender { get; set; }
        public string CharHisHer { get; set; }
        public string CharHeShe { get; set; }
        public int CharExperience { get; set; }
        public string CharTitle { get; set; }
        public int CharCasteID { get; set; }
        public int CharRankID { get; set; }
        public int CharPhilosophyID { get; set; }
        public int CharAlignment { get; set; }
        public int CharCreation { get; set; }
        public int CharStrength { get; set; }
        public int CharAgility { get; set; }
        public int CharIntellect { get; set; }
        public int CharStamina { get; set; }
        public int CharDamage { get; set; }
        public int CharHealth { get; set; }
        public int CharMana { get; set; }
        public int CharRoomID { get; set; }
        public int CharCurrentHealth { get; set; }
        public int CharCurrentMana { get; set; }
        public bool CharAutoLoot { get; set; }
        public bool CharAutoAttack { get; set; }
        public int CharRecall { get; set; }
        public bool CharHealthRegen { get; set; }
        public bool CharIsDead { get; set; }
        public bool CharIsGhosted { get; set; }
        public int? CIID { get; set; }
        public int? CICharacterID { get; set; }
        public int? CIItemID { get; set; }
        public int? CIItemAmount { get; set; }
        public int? CISlotIndex { get; set; }
        public bool? CIIsEquipped { get; set; }
        public int? ItemId { get; set; }
        public string ItemName { get; set; }
        public string ItemImage { get; set; }
        public int? ItemTypeID { get; set; }
        public int? ItemSlotID { get; set; }
        public int? ItemValue { get; set; }
        public int? ItemRarityID { get; set; }
        public int? ItemWeight { get; set; }
        public int? ItemDamage { get; set; }
        public int? ItemStrength { get; set; }
        public int? ItemAgility { get; set; }
        public int? ItemIntellect { get; set; }
        public int? ItemStamina { get; set; }
        public double? ItemCrit { get; set; }
        public double? ItemHaste { get; set; }
        public double? ItemMastery { get; set; }
        public double? ItemVersatility { get; set; }
        public int? ItemPhilosophyReq { get; set; }
        public int? ItemStrengthReq { get; set; }
        public int? ItemAgilityReq { get; set; }
        public int? ItemIntellectReq { get; set; }
        public int? ItemStaminaReq { get; set; }
        public int? ItemAlignmentReq { get; set; }
        public int? ItemRankReq { get; set; }
        public string ItemScript { get; set; }
    }
}
