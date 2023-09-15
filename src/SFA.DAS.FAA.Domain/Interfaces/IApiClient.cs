namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
        Task<int> Ping();
        Task<TResponse> Post<TResponse, TPostData>(IPostApiRequest<TPostData> request);
        Task Delete(IDeleteApiRequest request);
    }
}