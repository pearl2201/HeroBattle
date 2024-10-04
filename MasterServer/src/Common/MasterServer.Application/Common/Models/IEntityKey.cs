namespace MasterServer.Application.Common.Models
{
    public interface IEntityKey<T>
    {
        public T Id { get; set; }
    }

    public interface IEntityIntKey : IEntityKey<int>
    {

    }
}
