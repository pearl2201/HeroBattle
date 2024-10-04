namespace MasterServer.Application.Common.Exceptions
{
    public class UnavailableFeatureException : Exception
    {
        public UnavailableFeatureException()
        {
        }

        public UnavailableFeatureException(string message)
            : base($"Feature \"{message}\" was not active.")
        {
        }

        public UnavailableFeatureException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

}
