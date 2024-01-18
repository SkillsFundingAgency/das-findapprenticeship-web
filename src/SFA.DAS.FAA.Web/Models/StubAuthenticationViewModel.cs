using SFA.DAS.GovUK.Auth.Models;

namespace SFA.DAS.FAA.Web.Models;

public class StubAuthenticationViewModel : StubAuthUserDetails
{
    public long MobilePhone { get; set; }
    public string ReturnUrl { get; set; }
}