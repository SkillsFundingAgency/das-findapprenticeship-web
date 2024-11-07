namespace SFA.DAS.FAA.Domain.Models;

public record SavedSearch(
    Guid Id,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParameters SearchParameters
);