using FluentValidation;
using SFA.DAS.FAA.Domain.SearchResults;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.InputValidation.Fluent.Extensions;
using System.Text.RegularExpressions;

namespace SFA.DAS.FAA.Web.Validators;

public class GetSearchResultsRequestValidator : AbstractValidator<GetSearchResultsRequest>
{
    private static readonly int[] AllowedDistances = [2, 5, 10, 15, 20, 30, 40];
    private static readonly Regex NumericRegex = new(@"^\d+$", RegexOptions.Compiled, TimeSpan.FromSeconds(5));
    private static readonly Regex ValidFreeTextCharactersRegex = new(@"^[a-zA-Z0-9\s,'-]*$", RegexOptions.Compiled, TimeSpan.FromSeconds(5));

    public GetSearchResultsRequestValidator()
    {
        RuleFor(x => x.SearchTerm)
            .ValidFreeTextCharacters();
        RuleFor(x => x.Location)
            .ValidFreeTextCharacters();

        RuleForEach(x => x.RouteIds)
            .Must(id => NumericRegex.IsMatch(id)).WithMessage("Each RouteId must be numeric.")
            .MaximumLength(10).WithMessage("RouteId must be at most 10 digits.")
            .When(x => x.RouteIds is {Count: > 0});

        RuleForEach(x => x.LevelIds)
            .Must(id => NumericRegex.IsMatch(id)).WithMessage("Each LevelId must be numeric.")
            .MaximumLength(10).WithMessage("LevelId must be at most 10 digits.")
            .When(x => x.LevelIds is { Count: > 0 });

        RuleFor(x => x.Location)
            .MaximumLength(100)
            .Must(id => ValidFreeTextCharactersRegex.IsMatch(id)).WithMessage("Location contains invalid characters.")
            .When(x => !string.IsNullOrEmpty(x.Location));

        RuleFor(x => x.Distance)
            .Must(value => value != null && AllowedDistances.Contains(value.Value))
            .WithMessage("Distance contains invalid characters.")
            .When(x => x.Distance.HasValue);

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be at least 1.")
            .When(x => x.PageNumber.HasValue);

        RuleFor(x => x.Sort)
            .Must(BeAValidSortOption).WithMessage("Invalid sort option.")
            .When(x => !string.IsNullOrWhiteSpace(x.Sort));
    }

    private static bool BeAValidSortOption(string? sort)
    {
        if (string.IsNullOrWhiteSpace(sort))
            return false;

        var allowed = Enum.GetNames(typeof(VacancySort))
            .Select(x => x.ToLowerInvariant())
            .ToArray();

        return allowed.Contains(sort.ToLowerInvariant());
    }
}