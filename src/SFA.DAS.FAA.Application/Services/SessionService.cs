using Microsoft.AspNetCore.Http;
using SFA.DAS.FAA.Application.Constants;
using SFA.DAS.FAA.Application.Interfaces;
using System.Text.Json;

namespace SFA.DAS.FAA.Application.Services
{
    public class SessionService(IHttpContextAccessor httpContextAccessor) : ISessionService
    {
        public void Set(string key, string value) => httpContextAccessor.HttpContext?.Session.SetString(
            key, value);

        public void Set<T>(T model) => Set(typeof(T).Name, JsonSerializer.Serialize(model));

        public string? Get(string key) => httpContextAccessor.HttpContext?.Session.GetString(key);

        public T Get<T>()
        {
            var json = Get(typeof(T).Name);
            return (string.IsNullOrEmpty(json) ? default : JsonSerializer.Deserialize<T>(json))!;
        }

        public void Delete(string key)
        {
            if (httpContextAccessor.HttpContext != null &&
                httpContextAccessor.HttpContext.Session.Keys.Any(k => k == key))
                httpContextAccessor.HttpContext.Session.Remove(key);
        }

        public void Delete<T>(T model) => Delete(typeof(T).Name);

        public void Clear() => httpContextAccessor.HttpContext?.Session.Clear();

        public bool Contains<T>()
        {
            var result = httpContextAccessor.HttpContext?.Session.Keys.Any(k => k == typeof(T).Name);
            return result.GetValueOrDefault();
        }

        public Guid GetUserId()
        {
            var id = Guid.Empty;

            var candidateId = Get(SessionKeys.CandidateId);

            if (Guid.TryParse(candidateId, out var newGuid))
            {
                id = newGuid;
            }

            return id;
        }
    }
}