using System.ComponentModel.DataAnnotations;

namespace SFA.DAS.FAA.Web.Models.SavedSearches;

public class UnsubscribeSavedSearchesModel
{
    public Guid Id { get; set; }
    [MaxLength(250)]
    public string Title { get; set; }
}