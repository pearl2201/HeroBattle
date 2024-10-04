namespace MasterServer.Domain.Enums
{
    [Flags]
    public enum PlayerFeatureStatus : Int64
    {
        None = 0,
        Attack = 1 << 0,
        Defense = 1 << 1
    }
}
