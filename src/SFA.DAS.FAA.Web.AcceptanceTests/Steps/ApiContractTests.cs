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
        
        foreach (var (key, expectedSchema) in expectedTypes)
        {
            // Match type from the assembly
            var actualType = apiAssembly.GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(ApiTypeAttribute), inherit: false).Any())
                .FirstOrDefault(c => c.CustomAttributes.FirstOrDefault(x =>
                    x.AttributeType == typeof(ApiTypeAttribute) &&
                    x.ConstructorArguments.FirstOrDefault().Value?.ToString() == key) != null);

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
                var expectedType = GetOpenApiFormat(propertySchema.Format) ?? propertySchema.Type.GetDisplayName().ToLower();
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
    
    private static string GetOpenApiTypeName(Type type)
    {
        return type switch
        {
            not null when type == typeof(string) => "string",
            not null when type == typeof(int) => "int",
            not null when type == typeof(long) => "long",
            not null when type == typeof(int?) => "int-nullable",
            not null when type == typeof(long?) => "long-nullable",
            not null when type == typeof(float) || type == typeof(double) || type == typeof(decimal) => "number",
            not null when type == typeof(float?) || type == typeof(double?) || type == typeof(decimal?) =>
                "number-nullable",
            not null when type == typeof(bool) => "boolean",
            not null when type == typeof(bool?) => "boolean-nullable",
            not null when type == typeof(DateTime) => "datetime",
            not null when type == typeof(DateTime?) => "datetime-nullable",
            not null when type == typeof(List<>) => "array",
            not null when type.GetGenericArguments().Any() && type.GenericTypeArguments[0].IsEnum => "enum",
            not null when type.IsEnum => "enum",
            not null when type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>) ||
                                                 type.GetGenericTypeDefinition() == typeof(IEnumerable<>)) => "array",
            _ => "object"
        };
    }

    private static string? GetOpenApiFormat(string? propertySchemaFormat)
    {
        if (propertySchemaFormat == null)
        {
            return null;
        }

        return propertySchemaFormat.ToLower() switch
        {
            "date-time" => "datetime",
            "int32" => "int",
            "int64" => "long",
            _ => null
        };
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