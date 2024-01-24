namespace SFA.DAS.FAA.Infrastructure.Extensions
{
    public static class HttpRequestMessageExtensions
    {
        public static void AddVersion(this HttpRequestMessage httpRequestMessage, string version)
        {
            httpRequestMessage.Headers.Add("X-Version", version);
        }
    }
}
