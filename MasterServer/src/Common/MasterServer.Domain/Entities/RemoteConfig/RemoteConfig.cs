namespace MasterServer.Domain.Entities.RemoteConfig
{
    public class RemoteConfig : BaseAuditableEntity
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
        public List<RemoteConfigOverride> OverrideConfigs { get; set; }
    }
}
