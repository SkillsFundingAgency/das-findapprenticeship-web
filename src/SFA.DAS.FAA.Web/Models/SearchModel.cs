using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models;

public class SearchModel
{
    [FromQuery] 
    public string? WhereSearchTerm { get; set; } 
    [FromQuery] 
    public string? WhatSearchTerm { get; set; }
}