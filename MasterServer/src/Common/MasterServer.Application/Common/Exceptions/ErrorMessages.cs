namespace MasterServer.Application.Common.Exceptions
{
    public class ErrorMessages
    {
        public static string ErrorNotFound(string kind, object id) => $"{kind} {id} is not found";
    }
}
