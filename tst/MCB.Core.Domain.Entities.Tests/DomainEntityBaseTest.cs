using FluentAssertions;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;
using System;
using System.Linq;
using Xunit;

namespace MCB.Core.Domain.Entities.Tests
{
    public class DomainEntityBaseTest
    {
        [Fact]
        public void DomainEntityBase_Should_RegisterNew()
        {
            // Arrange
            var customer = new Customer();
            var tenantId = Guid.NewGuid();
            var executionUser = "marcelo.castelo@outlook.com";
            var sourcePlatform = "AppDemo";
            var initialCreatedAt = customer.AuditableInfo.CreatedAt;
            var initialRegistryVersion = customer.RegistryVersion;

            // Act
            customer.RegisterNewExposed(tenantId, executionUser, sourcePlatform);

            // Assert
            customer.Id.Should().NotBe(default(Guid));
            customer.TenantId.Should().Be(tenantId);
            customer.AuditableInfo.CreatedBy.Should().Be(executionUser);
            customer.AuditableInfo.CreatedAt.Should().BeAfter(initialCreatedAt);
            customer.AuditableInfo.UpdatedBy.Should().BeNull();
            customer.AuditableInfo.UpdatedAt.Should().BeNull();
            customer.AuditableInfo.SourcePlatform.Should().Be(sourcePlatform);
            customer.RegistryVersion.Should().BeAfter(initialRegistryVersion);
            customer.ValidationInfo.Should().NotBeNull();
            customer.ValidationInfo.IsValid.Should().BeTrue();
            customer.ValidationInfo.HasValidationMessage.Should().BeFalse();
            customer.ValidationInfo.HasError.Should().BeFalse();
            customer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
            customer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(0);
        }

        [Fact]
        public void DomainEntityBase_Should_SetExistingInfo()
        {
            // Arrange
            var customer = new Customer();
            var id = Guid.NewGuid();
            var tenantId = Guid.NewGuid();
            var createdBy = "marcelo.castelo@outlook.com";
            var createdAt = DateTimeOffset.UtcNow;
            var updatedBy = "marcelo.castelo@github.com";
            var updatedAt = DateTimeOffset.UtcNow;
            var sourcePlatform = "AppDemo";
            var registryVersion = DateTimeOffset.UtcNow;

            // Act
            customer.SetExistingInfoExposed(id, tenantId, createdBy, createdAt, updatedBy, updatedAt, sourcePlatform, registryVersion);

            // Assert
            customer.Id.Should().Be(id);
            customer.TenantId.Should().Be(tenantId);
            customer.AuditableInfo.CreatedBy.Should().Be(createdBy);
            customer.AuditableInfo.CreatedAt.Should().Be(createdAt);
            customer.AuditableInfo.UpdatedBy.Should().Be(updatedBy);
            customer.AuditableInfo.UpdatedAt.Should().Be(updatedAt);
            customer.AuditableInfo.SourcePlatform.Should().Be(sourcePlatform);
            customer.RegistryVersion.Should().Be(registryVersion);
            customer.ValidationInfo.Should().NotBeNull();
            customer.ValidationInfo.IsValid.Should().BeTrue();
            customer.ValidationInfo.HasValidationMessage.Should().BeFalse();
            customer.ValidationInfo.HasError.Should().BeFalse();
            customer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
            customer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(0);
        }

        [Fact]
        public void DomainEntityBase_Should_RegisterModification()
        {
            // Arrange
            var customer = new Customer();
            var tenantId = Guid.NewGuid();
            var executionUser = "marcelo.castelo@outlook.com";
            var sourcePlatform = "AppDemo";
            customer.RegisterNewExposed(tenantId, executionUser, sourcePlatform);

            var initialId = customer.Id;
            var initialCreatedBy = customer.AuditableInfo.CreatedBy;
            var initialCreatedAt = customer.AuditableInfo.CreatedAt;
            var initialUpdatedAt = customer.AuditableInfo.UpdatedAt;
            var initialRegistryVersion = customer.RegistryVersion;

            var modificationExecutionUser = "marcelo.castelo@github.com";
            var modificationSourcePlatform = "AppDemo2";

            // Act
            customer.RegisterModificationExposed(modificationExecutionUser, modificationSourcePlatform);

            // Assert
            customer.Id.Should().Be(initialId);
            customer.TenantId.Should().Be(tenantId);
            customer.AuditableInfo.CreatedBy.Should().Be(initialCreatedBy);
            customer.AuditableInfo.CreatedAt.Should().Be(initialCreatedAt);
            customer.AuditableInfo.UpdatedBy.Should().Be(modificationExecutionUser);
            customer.AuditableInfo.UpdatedAt.Should().BeAfter(initialUpdatedAt ?? default);
            customer.AuditableInfo.SourcePlatform.Should().Be(modificationSourcePlatform);
            customer.RegistryVersion.Should().BeAfter(initialRegistryVersion);
            customer.ValidationInfo.Should().NotBeNull();
            customer.ValidationInfo.IsValid.Should().BeTrue();
            customer.ValidationInfo.HasValidationMessage.Should().BeFalse();
            customer.ValidationInfo.HasError.Should().BeFalse();
            customer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
            customer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(0);
        }

        [Fact]
        public void DomainEntityBase_Should_Add_Validation_Message()
        {
            // Arrange
            var customer = new Customer();

            // Act
            customer.AddInformationValidationMessageExposed("INFO_1", "INFORMATION");
            customer.AddWarningValidationMessageExposed("WARNING_1", "WARNING");
            customer.AddErrorValidationMessageExposed("ERROR_1", "ERROR");

            // Assert
            customer.ValidationInfo.Should().NotBeNull();
            customer.ValidationInfo.IsValid.Should().BeFalse();
            customer.ValidationInfo.HasValidationMessage.Should().BeTrue();
            customer.ValidationInfo.HasError.Should().BeTrue();
            customer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
            customer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(3);

            customer.ValidationInfo.ValidationMessageCollection.ToList()[0].ValidationMessageType.Should().Be(ValidationMessageType.Information);
            customer.ValidationInfo.ValidationMessageCollection.ToList()[0].Code.Should().Be("INFO_1");
            customer.ValidationInfo.ValidationMessageCollection.ToList()[0].Description.Should().Be("INFORMATION");

            customer.ValidationInfo.ValidationMessageCollection.ToList()[1].ValidationMessageType.Should().Be(ValidationMessageType.Warning);
            customer.ValidationInfo.ValidationMessageCollection.ToList()[1].Code.Should().Be("WARNING_1");
            customer.ValidationInfo.ValidationMessageCollection.ToList()[1].Description.Should().Be("WARNING");

            customer.ValidationInfo.ValidationMessageCollection.ToList()[2].ValidationMessageType.Should().Be(ValidationMessageType.Error);
            customer.ValidationInfo.ValidationMessageCollection.ToList()[2].Code.Should().Be("ERROR_1");
            customer.ValidationInfo.ValidationMessageCollection.ToList()[2].Description.Should().Be("ERROR");
        }

        [Fact]
        public void DomainEntityBase_Should_ValidationInfo_Modified_Only_For_DomainEntity()
        {
            // Arrange
            var customer = new Customer();

            // Act
            customer.AddInformationValidationMessageExposed("INFO_1", "INFORMATION");
            customer.AddWarningValidationMessageExposed("WARNING_1", "WARNING");
            customer.AddErrorValidationMessageExposed("ERROR_1", "ERROR");
            customer.ValidationInfo.AddErrorValidationMessage("ERROR_2", "ERROR");

            // Assert
            customer.ValidationInfo.Should().NotBeNull();
            customer.ValidationInfo.IsValid.Should().BeFalse();
            customer.ValidationInfo.HasValidationMessage.Should().BeTrue();
            customer.ValidationInfo.HasError.Should().BeTrue();
            customer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
            customer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(3);

            customer.ValidationInfo.ValidationMessageCollection.ToList()[0].ValidationMessageType.Should().Be(ValidationMessageType.Information);
            customer.ValidationInfo.ValidationMessageCollection.ToList()[0].Code.Should().Be("INFO_1");
            customer.ValidationInfo.ValidationMessageCollection.ToList()[0].Description.Should().Be("INFORMATION");

            customer.ValidationInfo.ValidationMessageCollection.ToList()[1].ValidationMessageType.Should().Be(ValidationMessageType.Warning);
            customer.ValidationInfo.ValidationMessageCollection.ToList()[1].Code.Should().Be("WARNING_1");
            customer.ValidationInfo.ValidationMessageCollection.ToList()[1].Description.Should().Be("WARNING");

            customer.ValidationInfo.ValidationMessageCollection.ToList()[2].ValidationMessageType.Should().Be(ValidationMessageType.Error);
            customer.ValidationInfo.ValidationMessageCollection.ToList()[2].Code.Should().Be("ERROR_1");
            customer.ValidationInfo.ValidationMessageCollection.ToList()[2].Description.Should().Be("ERROR");
        }
    }

    public class Customer
        : DomainEntityBase
    {
        // Protected Methods
        public void AddValidationMessageExposed(ValidationMessageType validationMessageType, string code, string description)
            => AddValidationMessageExposed(validationMessageType, code, description);

        public void AddInformationValidationMessageExposed(string code, string description)
            => AddInformationValidationMessage(code, description);

        public void AddWarningValidationMessageExposed(string code, string description)
            => AddWarningValidationMessage(code, description);

        public void AddErrorValidationMessageExposed(string code, string description)
            => AddErrorValidationMessage(code, description);

        public DomainEntityBase RegisterNewExposed(
            Guid tenantId,
            string executionUser,
            string sourcePlatform
        ) => RegisterNew(tenantId, executionUser, sourcePlatform);
        
        public DomainEntityBase SetExistingInfoExposed(
            Guid id,
            Guid tenantId,
            string createdBy,
            DateTimeOffset createdAt,
            string updatedBy,
            DateTimeOffset? updatedAt,
            string sourcePlatform,
            DateTimeOffset registryVersion
        ) => SetExistingInfo(id, tenantId, createdBy, createdAt, updatedBy, updatedAt, sourcePlatform, registryVersion);

        public DomainEntityBase RegisterModificationExposed(
            string executionUser,
            string sourcePlatform
        ) => RegisterModification(executionUser, sourcePlatform);
    }
}
