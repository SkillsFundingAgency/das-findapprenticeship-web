using Microsoft.AspNetCore.Mvc;

namespace SFA.DAS.FAA.Web.Models.Apply
{
    public class GetIndexRequest
    {
        [FromRoute] public required Guid ApplicationId { get; init; }
    }
}
