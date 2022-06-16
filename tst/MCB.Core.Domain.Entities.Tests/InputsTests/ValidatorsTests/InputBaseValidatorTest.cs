using FluentAssertions;
using MCB.Core.Domain.Entities.Inputs;
using MCB.Core.Domain.Entities.Inputs.Validators;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MCB.Core.Domain.Entities.Tests.InputsTests.ValidatorsTests
{
    public class InputBaseValidatorTest
    {
        [Fact]
        public void InputBase_Should_Be_Valid()
        {
            // Arrange
            var dummyInput = new DummyInput(
                tenantId: Guid.NewGuid(),
                executionUser: "marcelo.castelo",
                sourcePlatform: "dummySourcePlatform"
            );
            var dummyInputValidator = new DummyInputValidator();

            // Act
            var validationResult = dummyInputValidator.Validate(dummyInput);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeTrue();
            validationResult.HasError.Should().BeFalse();
            validationResult.HasValidationMessage.Should().BeFalse();
            validationResult.ValidationMessageCollection.Should().BeEmpty();
        }
    }

    public record DummyInput
        : InputBase
    {
        public DummyInput(
            Guid tenantId, 
            string executionUser, 
            string sourcePlatform
        ) : base(tenantId, executionUser, sourcePlatform)
        {
        }
    }

    public class DummyInputValidator
        : InputBaseValidator<DummyInput>
    {
        protected override void ConfigureFluentValidationConcreteValidatorInternal(ValidatorBase<DummyInput>.FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
        {

        }
    }
}
