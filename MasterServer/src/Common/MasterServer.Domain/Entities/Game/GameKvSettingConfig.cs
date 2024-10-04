using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.Game
{
    public class GameKvSettingConfig
    {
        [Key(), DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Key { get; set; }

        public string StringValue { get; set; }

        public float FloatValue { get; set; }

        public int IntValue => (int)FloatValue;

        public bool Secret { get; set; }
    }
}
