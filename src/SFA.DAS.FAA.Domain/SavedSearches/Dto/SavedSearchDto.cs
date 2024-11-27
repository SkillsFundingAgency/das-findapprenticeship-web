namespace SFA.DAS.FAA.Domain.SavedSearches.Dto;

public record SavedSearchDto(
    Guid Id,
    DateTime DateCreated,
    DateTime? LastRunDate,
    DateTime? EmailLastSendDate,
    SearchParametersDto SearchParameters
);