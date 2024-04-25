using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FAA.Domain.Apply.Qualifications;

namespace SFA.DAS.FAA.Web.Models.Apply;

public class AddQualificationViewModel
{
    [FromRoute]
    public required Guid ApplicationId { get; set; }
    public QualificationDisplayTypeViewModel? QualificationDisplayTypeViewModel { get; set; }
    [FromRoute]
    public Guid QualificationReferenceId { get; set; }
    public List<SubjectViewModel> Subjects { get; set; }
    public List<CourseDataListItem>? Courses { get; set; }
    public bool IsApprenticeship { get; set; }
    public Guid? SingleQualificationId { get; set; }
    public string QualificationType { get; set; }
    public static Dictionary<string, int> BuildPropertyOrderDictionary()
    {   
        var itemCount = 0;

        var properties = new List<string>();
        properties.AddRange(typeof(SubjectViewModel).GetProperties().Select(c => c.Name).ToList());
        
        var propertyOrderDictionary = properties.Select(c => new
        {
            Order = itemCount++,
            Name = c
        }).ToDictionary(key => key.Name, value => value.Order);
        
        return propertyOrderDictionary;
    }
}

public class SubjectViewModel
{
    public static implicit operator SubjectViewModel(GetQualificationsApiResponse.Qualification source)
    {
        return new SubjectViewModel
        {
            QualificationReferenceId = source.QualificationReferenceId,
            Grade =  source.Grade,
            Name = source.Subject,
            AdditionalInformation = source.AdditionalInformation,
            IsPredicted = source.IsPredicted.HasValue && source.IsPredicted.Value,
            Id = source.Id,
            Level = source.AdditionalInformation
        };
    }

    public Guid QualificationReferenceId { get; set; }

    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Level { get; set; }
    public string? AdditionalInformation { get; set; }
    public string? Grade { get; set; }
    public bool? IsPredicted { get; set; }
    
    public bool? IsDeleted { get; set; }
}

public class CourseDataListItem
{
    public static implicit operator CourseDataListItem(CourseApiResponse source)
    {
        return new CourseDataListItem
        {
            Id = source.Id,
            Title = source.Title
        };
    }
    public string Id { get; set; }
    public string Title { get; set; }
}