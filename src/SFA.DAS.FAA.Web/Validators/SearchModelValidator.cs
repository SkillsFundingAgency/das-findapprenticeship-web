using FluentValidation;
using SFA.DAS.FAA.Web.Models;
using SFA.DAS.InputValidation.Fluent.Extensions;

namespace SFA.DAS.FAA.Web.Validators;

public class SearchModelValidator : AbstractValidator<SearchModel>
{
    public SearchModelValidator()
    {
        RuleFor(x => x.WhereSearchTerm)
            .ValidFreeTextCharacters();
        RuleFor(x => x.WhatSearchTerm)
            .ValidFreeTextCharacters();
    }
}