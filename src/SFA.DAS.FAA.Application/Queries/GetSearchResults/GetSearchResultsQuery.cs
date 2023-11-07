using MediatR;

namespace SFA.DAS.FAA.Application.Queries.GetSearchResults;

public class GetSearchResultsQuery : IRequest<GetSearchResultsResult>
{
    public bool NationalSearch { get; set; }
    public string? Location { get; set; }
    public List<string>? SelectedRouteIds { get; set; }

    public int? Distance { get; set; }
    //public string? WhatSearchTerm { get; set; }
}