namespace MasterServer.Application.Common.Exceptions
{
    public class UnauthorizeException : Exception
    {
        public UnauthorizeException() : base("Player was not found!")
        {

        }
    }
}
