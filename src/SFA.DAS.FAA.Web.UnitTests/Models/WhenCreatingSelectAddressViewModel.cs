using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FAA.Application.Queries.User.GetAddressesByPostcode;
using SFA.DAS.FAA.Web.Models.User;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FAA.Web.UnitTests.Models;
public class WhenCreatingSelectAddressViewModel
{
    [Test, MoqAutoData]
    public async Task Then_Addresses_Found_Are_Mapped(GetAddressesByPostcodeQueryResult queryResult)
    {
        var model = (SelectAddressViewModel)queryResult.Addresses.ToList();

        model.Addresses.Count().Should().Be(queryResult.Addresses.Count());
    }
}
