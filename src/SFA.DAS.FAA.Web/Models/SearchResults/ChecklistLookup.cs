namespace SFA.DAS.FAA.Web.Models.SearchResults
{
    public class ChecklistLookup
    {
        public string Name { get; }
        public string Value { get; }
        public string? Hint { get; }
        public string Checked { get; set; }

        public ChecklistLookup(string name, string value, string? hint = null, bool isChecked = false)
        {
            Name = name;
            Value = value;
            Hint = hint;
            Checked = isChecked ? "checked" : string.Empty;
        }
    }
}
