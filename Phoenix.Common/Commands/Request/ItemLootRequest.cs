using Phoenix.Common.Commands.Factory;
using System.Collections.Generic;


namespace Phoenix.Common.Commands.Request
{
    public class ItemLootRequest : Command
    {
        public int DropIndex { get; set; }

        public ItemLootRequest()
        {
            this.CommandType = CommandType.ItemLootRequest;
        }
        public override IEnumerable<IEnumerable<string>> GetCommandParts()
        {
            return new List<List<string>>
            {
                new List<string>
                {
                    this.DropIndex.ToString()
                }
            };
        }
    }
}
