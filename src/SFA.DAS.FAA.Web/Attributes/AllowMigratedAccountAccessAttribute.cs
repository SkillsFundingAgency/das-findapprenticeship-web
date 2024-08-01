namespace SFA.DAS.FAA.Web.Attributes;

/// <summary>
/// Exempts accounts that have already been migrated from access restrictions on the target
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AllowMigratedAccountAccessAttribute : Attribute
{
}