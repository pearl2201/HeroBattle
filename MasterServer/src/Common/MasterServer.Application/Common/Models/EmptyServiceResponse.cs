namespace MasterServer.Application.Common.Models
{
    public class EmptyServiceResponse
    {
        public static ServiceResult<EmptyServiceResponse> Create()
        {
            return ServiceResult.Success(new EmptyServiceResponse());
        }
    }

    public class GenericIntIdResponse
    {
        public int Id { get; set; }

        public GenericIntIdResponse(int id)
        { this.Id = id; }

        public static ServiceResult<GenericIntIdResponse> Create(int id)
        {
            return ServiceResult.Success(new GenericIntIdResponse(id));
        }
    }
}
