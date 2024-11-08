using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.IdentityModel.Tokens;
using SFA.DAS.FAA.Application.Queries.SavedSearches;
using SFA.DAS.FAA.Application.Queries.SavedSearches.GetConfirmUnsubscribe;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.Models.SavedSearches
{
    public class UnsubscribeSavedSearchesViewModel
    {
        public SavedSearchViewModel SavedSearch { get; set; } = null!;
    }
}
