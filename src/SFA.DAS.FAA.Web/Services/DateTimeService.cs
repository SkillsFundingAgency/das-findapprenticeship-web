using System;
using SFA.DAS.FAT.Domain.Interfaces;

namespace SFA.DAS.FAT.Web.Services;

public class DateTimeService : IDateTimeService
{
    public DateTime GetDateTime() => DateTime.UtcNow;
}
