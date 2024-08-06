namespace SFA.DAS.FAA.Web.Attributes
{
    /// <summary>
    /// Exempts accounts that have not completed setup from access restrictions on the target
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AllowIncompleteAccountAccessAttribute : Attribute
    {
    }
}
