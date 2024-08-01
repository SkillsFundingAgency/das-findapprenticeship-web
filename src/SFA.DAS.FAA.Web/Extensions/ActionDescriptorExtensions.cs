using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class ActionDescriptorExtensions
    {
        public static bool HasAttribute<T>(this ActionDescriptor actionDescriptor) where T: Attribute
        {
            return actionDescriptor is ControllerActionDescriptor controllerActionDescription && controllerActionDescription.MethodInfo.GetCustomAttributes<T>().Any();
        }
    }
}
