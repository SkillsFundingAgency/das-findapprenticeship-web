namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<TResponse> Put<TResponse>(IPutApiRequest request);
        Task<TResponse?> PostWithResponseCode<TResponse>(IPostApiRequest request);
        Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request);
        Task PostWithResponseCode(IPostApiRequest request);
    }
}