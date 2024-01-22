namespace SFA.DAS.FAA.Web.Models.SearchResults
{
    public class SelectedFilter
    {
        public string FieldName { get; set; } = null!;
        public int FieldOrder { get; set; }
        public List<SearchApprenticeFilterItem> Filters { get; set; } = new List<SearchApprenticeFilterItem>();
    }
}
