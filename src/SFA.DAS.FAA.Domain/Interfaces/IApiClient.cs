namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<TResponse?> PostWithResponseCode<TResponse>(IPostApiRequest request);
    }
}