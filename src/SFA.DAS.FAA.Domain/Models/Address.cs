namespace SFA.DAS.FAA.Domain.Models;

public record Address(
    string? AddressLine1,
    string? AddressLine2,
    string? AddressLine3,
    string? AddressLine4,
    string? Postcode,
    double? Latitude = null,
    double? Longitude = null)
{
    public static readonly Address Empty = new (null, null, null, null, null);
};