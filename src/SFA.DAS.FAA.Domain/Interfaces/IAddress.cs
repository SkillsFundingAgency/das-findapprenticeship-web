namespace SFA.DAS.FAA.Domain.Interfaces;

public interface IAddress
{
    string AddressLine1 { get; set; }
    string AddressLine2 { get; set; }
    string AddressLine3 { get; set; }
    string AddressLine4 { get; set; }
    string Postcode { get; set; }
}