using SFA.DAS.FAA.Domain.Interfaces;

namespace SFA.DAS.FAA.Domain.BrowseByInterests;

public class GetBrowseByInterestsApiRequest : IGetApiRequest
{
    public string GetUrl => "searchapprenticeships/browsebyinterests";
}