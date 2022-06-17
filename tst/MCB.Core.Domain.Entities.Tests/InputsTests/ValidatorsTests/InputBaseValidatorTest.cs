using FluentAssertions;
using MCB.Core.Domain.Entities.Inputs;
using MCB.Core.Domain.Entities.Inputs.Validators;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using System;
using System.Linq;
using Xunit;

namespace MCB.Core.Domain.Entities.Tests.InputsTests.ValidatorsTests
{
    public class InputBaseValidatorTest
    {
        [Fact]
        public void InputBaseValidator_Should_Be_Valid()
        {
            // Arrange
            var dummyInput = new DummyInput(
                tenantId: Guid.NewGuid(),
                executionUser: new string('a', 150),
                sourcePlatform: new string('a', 150)
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

        [Fact]
        public void InputBaseValidator_Should_Validate_Required_Fields()
        {
            // Arrange
            var dummyInput = new DummyInput(
                tenantId: Guid.Empty,
                executionUser: null,
                sourcePlatform: null
            );
            var dummyInputValidator = new DummyInputValidator();

            // Act
            var validationResult = dummyInputValidator.Validate(dummyInput);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.HasError.Should().BeTrue();
            validationResult.HasValidationMessage.Should().BeTrue();
            validationResult.ValidationMessageCollection.Should().NotBeEmpty();
            validationResult.ValidationMessageCollection.Should().HaveCount(3);

            var validationMessageCollection = validationResult.ValidationMessageCollection.ToList();

            validationMessageCollection[0].Code.Should().Be(InputBaseValidator<DummyInput>.TenantIdIsRequiredErrorCode);
            validationMessageCollection[0].Description.Should().Be(InputBaseValidator<DummyInput>.TenantIdIsRequiredMessage);
            validationMessageCollection[0].ValidationMessageType.Should().Be(Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums.ValidationMessageType.Error);

            validationMessageCollection[1].Code.Should().Be(InputBaseValidator<DummyInput>.ExecutionUserIsRequiredErrorCode);
            validationMessageCollection[1].Description.Should().Be(InputBaseValidator<DummyInput>.ExecutionUserIsRequiredMessage);
            validationMessageCollection[1].ValidationMessageType.Should().Be(Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums.ValidationMessageType.Error);

            validationMessageCollection[2].Code.Should().Be(InputBaseValidator<DummyInput>.SourcePlatformIsRequiredErrorCode);
            validationMessageCollection[2].Description.Should().Be(InputBaseValidator<DummyInput>.SourcePlatformIsRequiredMessage);
            validationMessageCollection[2].ValidationMessageType.Should().Be(Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums.ValidationMessageType.Error);
        }

        [Fact]
        public void InputBaseValidator_Should_Validate_Maximum_Length()
        {
            // Arrange
            var dummyInput = new DummyInput(
                tenantId: Guid.NewGuid(),
                executionUser: new string('a', 151),
                sourcePlatform: new string('a', 151)
            );
            var dummyInputValidator = new DummyInputValidator();

            // Act
            var validationResult = dummyInputValidator.Validate(dummyInput);

            // Assert
            validationResult.Should().NotBeNull();
            validationResult.IsValid.Should().BeFalse();
            validationResult.HasError.Should().BeTrue();
            validationResult.HasValidationMessage.Should().BeTrue();
            validationResult.ValidationMessageCollection.Should().NotBeEmpty();
            validationResult.ValidationMessageCollection.Should().HaveCount(2);

            var validationMessageCollection = validationResult.ValidationMessageCollection.ToList();

            validationMessageCollection[0].Code.Should().Be(InputBaseValidator<DummyInput>.ExecutionUserShouldLessThanOrEqualToMaximumLengthErrorCode);
            validationMessageCollection[0].Description.Should().Be(InputBaseValidator<DummyInput>.ExecutionUserShouldLessThanOrEqualToMaximumLengthMessage);
            validationMessageCollection[0].ValidationMessageType.Should().Be(Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums.ValidationMessageType.Error);

            validationMessageCollection[1].Code.Should().Be(InputBaseValidator<DummyInput>.SourcePlatformShouldLessThanOrEqualToMaximumLengthErrorCode);
            validationMessageCollection[1].Description.Should().Be(InputBaseValidator<DummyInput>.SourcePlatformShouldLessThanOrEqualToMaximumLengthMessage);
            validationMessageCollection[1].ValidationMessageType.Should().Be(Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums.ValidationMessageType.Error);
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
