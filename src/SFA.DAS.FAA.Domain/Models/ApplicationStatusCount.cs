using SFA.DAS.FAA.Domain.Enums;
namespace SFA.DAS.FAA.Domain.Models;

public sealed record ApplicationStatusCount(
    List<Guid> ApplicationIds,
    int Count,
    ApplicationStatus Status
);