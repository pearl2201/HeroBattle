using System.ComponentModel.DataAnnotations.Schema;

namespace MasterServer.Domain.Entities.RemoteConfig
{
    public class RemoteConfigOverride : BaseAuditableEntity
    {
        public int ConfigId { get; set; }
        [ForeignKey(nameof(ConfigId))]
        public RemoteConfig Config { get; set; }

        public OverrideConditionKey ConditionKey { get; set; }

        public GeneralValue ConditionValue { get; set; }

        public OverrideConditionOp Op { get; set; }
    }

    public enum OverrideConditionKey
    {

    }

    public enum OverrideConditionOp
    {

    }

    public struct GeneralValue
    {
        public string StringValue { get; set; }

        public int IntValue { get; set; }

        public float FloatValue { get; set; }
    }
}
