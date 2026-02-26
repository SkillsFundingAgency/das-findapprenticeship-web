using System.Reflection;
using AutoFixture.Kernel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SFA.DAS.FAA.Web.UnitTests.Customisations;

[AttributeUsage(AttributeTargets.Parameter)]
public class ArrangeActionContextAttribute<T> : CustomizeAttribute where T : Controller
{
    public override ICustomization GetCustomization(ParameterInfo parameter)
    {
        if (parameter == null)
        {
            throw new ArgumentNullException(nameof(parameter));
        }

        if (parameter.ParameterType != typeof(ActionExecutingContext))
        {
            throw new ArgumentException(nameof(parameter));
        }

        return new ArrangeActionContextCustomisation<T>();
    }
}

public class ArrangeActionContextCustomisation<T> : ICustomization where T : Controller
{
    public void Customize(IFixture fixture)
    {
        fixture.Customizations.Add(new ActionExecutingContextBuilder<T>());

        fixture.Customize<BindingInfo>(c => c.OmitAutoProperties());
        fixture.Customize<ActionExecutingContext>(composer => composer
            .Without(context => context.Result));

        fixture.Behaviors.Add(new TracingBehavior());
    }
}
public class ActionExecutingContextBuilder<T> : ISpecimenBuilder where T : Controller
{
    public object Create(object request, ISpecimenContext context)
    {
        if (request is ParameterInfo paramInfo
            && paramInfo.ParameterType == typeof(object)
            && paramInfo.Name == "controller")
        {
            return context.Create<T>();
        }

        return new NoSpecimen();
    }
}
