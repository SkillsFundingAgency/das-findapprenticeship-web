namespace SFA.DAS.FAA.Domain.Interfaces
{
    public interface IApiClient
    {
        Task<TResponse> Get<TResponse>(IGetApiRequest request);
    }
}