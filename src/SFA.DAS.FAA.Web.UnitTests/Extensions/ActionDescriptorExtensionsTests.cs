using System.Reflection;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using NUnit.Framework;
using SFA.DAS.FAA.Web.Extensions;

namespace SFA.DAS.FAA.Web.UnitTests.Extensions
{
    [TestFixture]
    public class ActionDescriptorExtensionsTests
    {
        [Test]
        public void HasAttribute_ReturnsTrue_WhenControllerHasAttribute()
        {
            // Arrange
            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(DummyAttributedClass).GetTypeInfo(),
                MethodInfo = typeof(DummyAttributedClass).GetMethod(nameof(DummyAttributedClass.DummyMethod))!
            };

            // Act
            var result = controllerActionDescriptor.HasAttribute<SampleAttribute>();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void HasAttribute_ReturnsTrue_WhenMethodHasAttribute()
        {
            // Arrange
            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(DummyUnAttributedClass).GetTypeInfo(),
                MethodInfo = typeof(DummyUnAttributedClass).GetMethod(nameof(DummyUnAttributedClass.DummyMethod))!
            };

            // Act
            var result = controllerActionDescriptor.HasAttribute<SampleAttribute>();

            // Assert
            result.Should().BeTrue();
        }

        [Test]
        public void HasAttribute_ReturnsFalse_WhenNoAttributePresent()
        {
            // Arrange
            var controllerActionDescriptor = new ControllerActionDescriptor
            {
                ControllerTypeInfo = typeof(DummyUnAttributedClass).GetTypeInfo(),
                MethodInfo = typeof(DummyUnAttributedClass).GetMethod(nameof(DummyUnAttributedClass.DummyUnattributedMethod))!
            };

            // Act
            var result = controllerActionDescriptor.HasAttribute<SampleAttribute>();

            // Assert
            result.Should().BeFalse();
        }

        [Test]
        public void HasAttribute_ReturnsFalse_WhenActionDescriptorIsNotControllerActionDescriptor()
        {
            // Arrange
            var actionDescriptor = new ActionDescriptor();

            // Act
            var result = actionDescriptor.HasAttribute<SampleAttribute>();

            // Assert
            result.Should().BeFalse();
        }

        internal class SampleAttribute : Attribute { }


        [Sample]
        internal class DummyAttributedClass
        {
            public void DummyMethod()
            {
            }
        }

        internal class DummyUnAttributedClass
        {
            [Sample]
            public void DummyMethod()
            {
            }

            public void DummyUnattributedMethod()
            {
            }
        }
    }
}
