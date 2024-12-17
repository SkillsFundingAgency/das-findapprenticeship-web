using System.Reflection;
using System.Runtime.CompilerServices;
using Microsoft.OpenApi.Extensions;
using NSwag;
using NUnit.Framework;
using SFA.DAS.FAA.Domain;
using SFA.DAS.FAA.Domain.Applications.GetApplications;
using SFA.DAS.FAA.Web.AcceptanceTests.Infrastructure;
using TechTalk.SpecFlow;

namespace SFA.DAS.FAA.Web.AcceptanceTests.Steps;

[Binding]
public class ApiContractTests(ScenarioContext context)
{
    [Then("all the api response types match")]
    public async Task Then_The_Contract_Matches()
    {
        var url = context.Get<string>(ContextKeys.ApiSpecUrl);

        if (string.IsNullOrEmpty(url))
        {
            Assert.Pass("No outer api url found");
            return;
        }
        
        var document = await OpenApiDocument.FromUrlAsync(url);
        var apiAssembly = Assembly.Load(typeof(GetApplicationsApiResponse).Assembly.FullName!);
        var expectedTypes = document.Definitions;

        var validationErrors = new List<string>();
        
        foreach (var definition in expectedTypes)
        {
            var expectedSchema = definition.Value;

            // Match type from the assembly
            var actualType = apiAssembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(ApiTypeAttribute), inherit: false).Any())
                .FirstOrDefault(c => c.CustomAttributes.FirstOrDefault(x =>
                    x.AttributeType == typeof(ApiTypeAttribute) &&
                    x.ConstructorArguments.FirstOrDefault().Value?.ToString() == definition.Key) != null);

            if (actualType == null)
            {
                continue;
            }
            Console.WriteLine($"Validating {actualType.Name}");

            // Validate properties
            foreach (var expectedProperty in expectedSchema.Properties)
            {
                var propertyName = expectedProperty.Key;
                var propertySchema = expectedProperty.Value;
                
                var actualProperty = actualType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);
                
                if (actualProperty == null)
                {
                    //ignore properties that we don't have in our contract
                    continue;
                }
                
                // Compare property types
                var actualPropertyTypeAsApiTypeName = GetOpenApiTypeName(actualProperty.PropertyType);
                var expectedType =GetOpenApiFormat(propertySchema.Format) ?? propertySchema.Type.GetDisplayName().ToLower();
                var isApiPropertyRequired = expectedSchema.RequiredProperties.Contains(propertyName);
                var isNullable = expectedProperty.Value.IsNullableRaw;
                var isEnum = propertySchema is { HasReference: true, ActualSchema.IsObject: false };
                if (expectedType == "none")
                {
                    expectedType = isEnum ? "enum" : "object";

                    if (!isApiPropertyRequired)
                    {
                        expectedType += "-nullable";
                    }
                }
                else if(isNullable.HasValue && isNullable.Value)
                {
                    if (!isApiPropertyRequired)
                    {
                        expectedType += "-nullable";    
                    }
                }
                
                var isActualRequired = IsRequired(actualProperty);
                if (!isActualRequired && (actualPropertyTypeAsApiTypeName.Equals("string", StringComparison.CurrentCultureIgnoreCase) 
                                          || actualPropertyTypeAsApiTypeName.Equals("enum", StringComparison.CurrentCultureIgnoreCase)
                                          || actualPropertyTypeAsApiTypeName.Equals("array", StringComparison.CurrentCultureIgnoreCase)
                                          || actualPropertyTypeAsApiTypeName.Equals("object", StringComparison.CurrentCultureIgnoreCase)))
                {
                    actualPropertyTypeAsApiTypeName += "-nullable";
                }

                if (isApiPropertyRequired != isActualRequired)
                {
                    validationErrors.Add($"{propertyName} is required - on class {actualType.Name}");
                }

                if (expectedType != actualPropertyTypeAsApiTypeName)
                {
                    validationErrors.Add($"{propertyName}: with {actualPropertyTypeAsApiTypeName} does not match expected type {expectedType} - on class {actualType.Name}");
                }

                if (isEnum)
                {
                    var type = Nullable.GetUnderlyingType(actualProperty.PropertyType) ?? actualProperty.PropertyType;

                    var areEnumValuesNumeric = false;
                    
                    foreach (var expectedValue in expectedProperty.Value.ActualSchema.Enumeration.Select(x => x!.ToString()).ToList())
                    {
                        if (long.TryParse(expectedValue, out _))
                        {
                            areEnumValuesNumeric = true;
                        }
                        
                        if (!Enum.TryParse(type!, expectedValue, true, out _))
                        {
                            validationErrors.Add($"{expectedValue} is not defined in enum type {type.Name} - on class {actualType.Name}");           
                        }
                    }

                    if (areEnumValuesNumeric)
                    {
                        Console.WriteLine($"The enum {propertyName} has only numeric values - check that options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); is included in the API");
                    }
                    
                }
            }
        }
        Assert.That(validationErrors, Is.Empty, string.Join("\n",validationErrors));
    }
    
    private string GetOpenApiTypeName(Type type)
    {
        switch (type)
        {
            case not null when type == typeof(string):
                return "string";
            case not null when type == typeof(int):
                return "int";
            case not null when type == typeof(long):
                return "long";
            case not null when type == typeof(int?):
                return "int-nullable";
            case not null when type == typeof(long?):
                return "long-nullable";
            case not null when type == typeof(float) || type == typeof(double) || type == typeof(decimal):
                return "number";
            case not null when type == typeof(float?) || type == typeof(double?) || type == typeof(decimal?):
                return "number-nullable";
            case not null when type == typeof(bool):
                return "boolean";
            case not null when type == typeof(bool?):
                return "boolean-nullable";
            case not null when type == typeof(DateTime):
                return "datetime";
            case not null when type == typeof(DateTime?):
                return "datetime-nullable";
            case not null when type == typeof(List<>):
                return "array";
            case not null when type.GetGenericArguments().Any() && type.GenericTypeArguments[0].IsEnum:
                return "enum";
            case not null when type.IsEnum:
                return "enum";
            case not null when type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IEnumerable<>)):
                return "array";
            default:
                return "object";
        }
    }

    private string? GetOpenApiFormat(string? propertySchemaFormat)
    {
        if (propertySchemaFormat == null)
        {
            return null;
        }
        // Map .NET types to OpenAPI types (simplified for demo purposes)
        switch (propertySchemaFormat.ToLower())
        {
            case "date-time":
                return "datetime";
            case "int32":
                return "int";
            case "int64":
                return "long";
            default:
                return null;
        }
    }
    
    private static bool IsRequired(PropertyInfo property)
    {
        var nullabilityInfoContext = new NullabilityInfoContext();
        var info = nullabilityInfoContext.Create(property);
        if (info.WriteState == NullabilityState.Nullable || info.ReadState == NullabilityState.Nullable)
        {
            return false;
        }

        return property.CustomAttributes.FirstOrDefault(c => c.AttributeType == typeof(RequiredMemberAttribute)) != null;
    }
}