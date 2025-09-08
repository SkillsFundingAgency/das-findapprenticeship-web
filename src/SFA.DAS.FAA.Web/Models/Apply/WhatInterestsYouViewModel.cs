using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class WhatInterestsYouViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }

    public string? StandardName { get; set; }
    public string? EmployerName { get; set; }
    [DataType(DataType.MultilineText)]
    public string? AnswerText { get; set; }
    public bool? IsSectionCompleted { get; set; }
    public bool AutoSave { get; set; }
}