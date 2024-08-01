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

            return controllerActionDescription.ControllerTypeInfo.GetCustomAttribute<T>() != null
                   || controllerActionDescription.MethodInfo.GetCustomAttribute<T>() != null;
        }
    }
}
