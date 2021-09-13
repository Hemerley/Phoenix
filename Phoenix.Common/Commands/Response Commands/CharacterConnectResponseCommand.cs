using Phoenix.Common.Data.Types;
using System.Collections.Generic;


namespace Phoenix.Common
{
    public class CharacterConnectResponseCommand : Command
    {
        public int Success { get; set; }
        public Character Character { get; set; }

        public CharacterConnectResponseCommand()
        {
            this.CommandType = CommandType.CharacterLoginResponse;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            var response = new List<string>();
            var character = new List<List<string>>();
            response.Add(this.Success.ToString());
            if (this.Character == null)
            {
                return character;
            }

            character.Add(new List<string>{
            this.Character.Id.ToString(),
            this.Character.AccountId.ToString(),
            this.Character.Name.ToString(),
            this.Character.Type.ToString(),
            this.Character.Image.ToString(),
            this.Character.Gender.ToString(),
            this.Character.HisHer.ToString(),
            this.Character.HeShe.ToString(),
            this.Character.Experience.ToString(),
            this.Character.Title.ToString(),
            this.Character.Caste.ToString(),
            this.Character.Rank.ToString(),
            this.Character.Philosophy.ToString(),
            this.Character.Alignment.ToString(),
            this.Character.Creation.ToString(),
            this.Character.Strength.ToString(),
            this.Character.Agility.ToString(),
            this.Character.Intellect.ToString(),
            this.Character.Stamina.ToString(),
            this.Character.Damage.ToString(),
            this.Character.Health.ToString(),
            this.Character.Mana.ToString(),
            this.Character.RoomID.ToString(),
            this.Character.Crit.ToString(),
            this.Character.Mastery.ToString(),
            this.Character.Haste.ToString(),
            this.Character.Versatility.ToString()
            });

            return character;
        }
    }
}
