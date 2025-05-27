using System.Text.Json.Serialization;

namespace SFA.DAS.FAA.Domain.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApprenticeshipTypes
{
    Standard,
    Foundation,
}