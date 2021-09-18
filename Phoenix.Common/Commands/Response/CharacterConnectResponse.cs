using Phoenix.Common.Commands.Factory;
using Phoenix.Common.Data.Types;
using System.Collections.Generic;


namespace Phoenix.Common.Commands.Response
{
    public class CharacterConnectResponse : Command
    {
        public bool Success { get; set; }

        public Character Character { get; set; }

        public string Message { get; set; }

        public CharacterConnectResponse()
        {
            this.CommandType = CommandType.CharacterLoginResponse;
        }

        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            var response = new List<string>();
            var character = new List<List<string>>();
            response.Add(this.Success.ToString());
            response.Add(this.Message.ToString());
            character.Add(response);
            if (this.Character == null)
            {
                return character;
            }
            character.Add(new List<string>{
                this.Character.Id.ToString(),
                this.Character.AccountId.ToString(),
                this.Character.Name,
                this.Character.Type,
                this.Character.TypeID.ToString(),
                this.Character.Image,
                this.Character.Gender,
                this.Character.HisHer,
                this.Character.HeShe,
                this.Character.Experience.ToString(),
                this.Character.Title,
                this.Character.Caste,
                this.Character.CasteID.ToString(),
                this.Character.Rank,
                this.Character.RankID.ToString(),
                this.Character.Philosophy,
                this.Character.PhilosophyID.ToString(),
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
