namespace SFA.DAS.FAA.Application.Interfaces
{
    public interface ISessionService
    {
        void Set(string key, string value);
        void Set<T>(T model);
        string? Get(string key);
        T Get<T>();
        void Delete(string key);
        void Delete<T>(T model);
        void Clear();
        bool Contains<T>();

        Guid GetUserId();
    }
}