using CreateAccount.GetAddressesByPostcode;
using SFA.DAS.FAA.Web.Models.User;

namespace SFA.DAS.FAA.Web.UnitTests.Models;

public class WhenCreatingSelectAddressViewModel
{
    [Test, MoqAutoData]
    public async Task Then_Addresses_Found_Are_Mapped(GetAddressesByPostcodeQueryResult queryResult)
    {
        var model = (SelectAddressViewModel)queryResult;

        model.Addresses.Count().Should().Be(queryResult.Addresses.Count());
    }
}