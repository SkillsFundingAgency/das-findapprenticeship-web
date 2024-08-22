using Microsoft.AspNetCore.Mvc.Abstractions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace SFA.DAS.FAA.Web.Extensions
{
    public static class ActionDescriptorExtensions
    {
        public static bool HasAttribute<T>(this ActionDescriptor actionDescriptor) where T: Attribute
        {
            if (actionDescriptor is not ControllerActionDescriptor controllerActionDescription) return false;

            if (controllerActionDescription.ControllerTypeInfo != null && controllerActionDescription.ControllerTypeInfo.GetCustomAttributes<T>().Any())
            {
                return true;
            }

            return controllerActionDescription.MethodInfo?.GetCustomAttributes<T>().Any() ?? false;
        }
    }
}
