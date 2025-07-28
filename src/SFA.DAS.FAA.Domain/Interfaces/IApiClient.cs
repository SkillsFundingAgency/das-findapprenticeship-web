namespace SFA.DAS.FAA.Domain.Interfaces;

public interface IApiClient
{
    Task<TResponse> Get<TResponse>(IGetApiRequest request);
    Task<ApiResponse<TResponse>> GetWithResponseCodeAsync<TResponse>(IGetApiRequest request, CancellationToken cancellationToken = default);
    Task<TResponse> Put<TResponse>(IPutApiRequest request);
    Task<ApiResponse<TResponse>> PostWithResponseCodeAsync<TResponse>(IPostApiRequest request, CancellationToken cancellationToken = default);
    Task<ApiResponse<TResponse>> PutWithResponseCode<TResponse>(IPutApiRequest request);
    Task Post(IPostApiRequest request);
    Task<TResponse?> Post<TResponse>(IPostApiRequest request);
}