using FluentValidation;
using SFA.DAS.FAA.Web.Models.SearchResults;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.FAA.Web.Validators;

public class GetSearchResultsRequestValidator : AbstractValidator<GetSearchResultsRequest>
{
    public GetSearchResultsRequestValidator()
    {
        RuleFor(x => x.SearchTerm)
            .ValidFreeTextCharacters();
        RuleFor(x => x.Location)
            .ValidFreeTextCharacters();
    }    
}