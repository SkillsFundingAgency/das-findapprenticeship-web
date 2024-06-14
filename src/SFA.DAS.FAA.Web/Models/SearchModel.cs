using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models;

public class SearchModel : AbstractValidator<SearchModel>
{
    [FromQuery] 
    public string? WhereSearchTerm { get; set; } 
    [FromQuery] 
    public string? WhatSearchTerm { get; set; }
}