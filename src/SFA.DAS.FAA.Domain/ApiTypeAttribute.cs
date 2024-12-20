namespace SFA.DAS.FAA.Domain;

public class ApiTypeAttribute(string apiResponseName) : Attribute
{
    public readonly string ApiResponseName = apiResponseName;
}