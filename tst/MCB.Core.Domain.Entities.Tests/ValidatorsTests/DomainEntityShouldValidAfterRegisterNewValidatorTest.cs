using FluentAssertions;
using MCB.Core.Domain.Entities.Specifications;
using MCB.Core.Domain.Entities.Validators;
using System;
using Xunit;

namespace MCB.Core.Domain.Entities.Tests.ValidatorsTests
{
    public class DomainEntityShouldValidAfterRegisterNewValidatorTest
    {
        [Fact]
        public void DomainEntityShouldValidAfterRegisterModificationValidator_Should_Valid()
        {
            // arrange
            var validator = new DomainEntityShouldValidAfterRegisterNewValidator<DummyDomainEntity>(
                new DomainEntitySpecifications()
            );

            var tenantId = Guid.NewGuid();
            var executionUser = "marcelo.castelo";
            var sourcePlatform = "dummySourcePlatform";

            var dummyDomainEntity = new DummyDomainEntity();
            dummyDomainEntity.RegisterNew(
                tenantId,
                executionUser,
                sourcePlatform
            );

            // Act
            var validationResult = validator.Validate(dummyDomainEntity);

            // Assert
            validationResult.IsValid.Should().BeTrue();
            validationResult.HasError.Should().BeFalse();
            validationResult.HasValidationMessage.Should().BeFalse();
        }

        public class DummyDomainEntity
            : DomainEntityBase
        {
            protected override DomainEntityBase CreateInstanceForCloneInternal()
            {
                return new DummyDomainEntity();
            }

            public DummyDomainEntity RegisterNew(Guid tenantId, string executionUser, string sourcePlatform)
            {
                return RegisterNewInternal<DummyDomainEntity>(tenantId, executionUser, sourcePlatform);
            }
        }
    }
}
