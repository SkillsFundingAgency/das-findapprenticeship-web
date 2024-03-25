using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }
    public QualificationDisplayTypeViewModel QualificationDisplayTypeViewModel { get; set; }
    [FromRoute]
    public Guid QualificationReferenceId { get; set; }
    public List<SubjectViewModel> Subjects { get; set; }
    public List<CourseDataListItem> Courses { get; set; }
}

public class SubjectViewModel
{
    public static implicit operator SubjectViewModel(GetQualificationsApiResponse.Qualification source)
    {
        return new SubjectViewModel
        {
            Grade =  source.Grade,
            Name = source.Subject,
            AdditionalInformation = source.AdditionalInformation,
            IsPredicted = source.IsPredicted,
            Id = source.Id,
            Level = source.AdditionalInformation
        };
    }
    public Guid? Id { get; set; }
    public string? Grade { get; set; }
    public string Name { get; set; }
    public string? Level { get; set; }
    public bool? IsPredicted { get; set; }
    public string? AdditionalInformation { get; set; }
    public bool? IsDeleted { get; set; }
}

public class CourseDataListItem
{
    public static implicit operator CourseDataListItem(CourseApiResponse source)
    {
        return new CourseDataListItem
        {
            Id = source.Id,
            Title = $"{source.Title}{(source.IsStandard?" (Standard)":"")}"
        };
    }
    public string Id { get; set; }
    public string Title { get; set; }
}