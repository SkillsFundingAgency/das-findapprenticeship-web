using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class WhatInterestsYouViewModel
    {
        [FromRoute]
        public required Guid ApplicationId { get; set; }

        public string? StandardName { get; set; } //todo: source
        public string? EmployerName { get; set; } //todo: source

        public string? YourInterest { get; set; }
        public bool? IsSectionCompleted { get; set; }
    }
}
