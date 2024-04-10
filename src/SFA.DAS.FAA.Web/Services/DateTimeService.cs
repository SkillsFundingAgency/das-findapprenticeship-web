using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAA.Web.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime GetDateTime() => DateTime.UtcNow;
}
