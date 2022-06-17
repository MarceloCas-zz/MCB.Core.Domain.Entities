﻿using FluentAssertions;
using MCB.Core.Infra.CrossCutting.DateTime;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace MCB.Core.Domain.Entities.Tests;

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
        customer.AuditableInfo.LastUpdatedBy.Should().BeNull();
        customer.AuditableInfo.LastUpdatedAt.Should().BeNull();
        customer.AuditableInfo.LastSourcePlatform.Should().Be(sourcePlatform);
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
        var createdAt = DateTimeProvider.GetDate();
        var updatedBy = "marcelo.castelo@github.com";
        var updatedAt = DateTimeProvider.GetDate();
        var sourcePlatform = "AppDemo";
        var registryVersion = DateTimeProvider.GetDate();

        // Act
        customer.SetExistingInfoExposed(id, tenantId, createdBy, createdAt, updatedBy, updatedAt, sourcePlatform, registryVersion);

        // Assert
        customer.Id.Should().Be(id);
        customer.TenantId.Should().Be(tenantId);
        customer.AuditableInfo.CreatedBy.Should().Be(createdBy);
        customer.AuditableInfo.CreatedAt.Should().Be(createdAt);
        customer.AuditableInfo.LastUpdatedBy.Should().Be(updatedBy);
        customer.AuditableInfo.LastUpdatedAt.Should().Be(updatedAt);
        customer.AuditableInfo.LastSourcePlatform.Should().Be(sourcePlatform);
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
        var initialUpdatedAt = customer.AuditableInfo.LastUpdatedAt;
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
        customer.AuditableInfo.LastUpdatedBy.Should().Be(modificationExecutionUser);
        customer.AuditableInfo.LastUpdatedAt.Should().BeAfter(initialUpdatedAt ?? default);
        customer.AuditableInfo.LastSourcePlatform.Should().Be(modificationSourcePlatform);
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

    [Fact]
    public void DomainEntityBase_Should_DeepCloneInternal()
    {
        // Arrange
        var customer = new Customer();
        var tenantId = Guid.NewGuid();
        var executionUser = "marcelo.castelo@outlook.com";
        var sourcePlatform = "AppDemo";
        var initialCreatedAt = customer.AuditableInfo.CreatedAt;
        var initialRegistryVersion = customer.RegistryVersion;

        customer.RegisterNewExposed(tenantId, executionUser, sourcePlatform);

        customer.AddInformationValidationMessageExposed("INFO_1", "INFORMATION");
        customer.AddWarningValidationMessageExposed("WARNING_1", "WARNING");
        customer.AddErrorValidationMessageExposed("ERROR_1", "ERROR");
        customer.ValidationInfo.AddErrorValidationMessage("ERROR_2", "ERROR");

        // Act
        var newCustomer = customer.DeepCloneInternalExposed();
        customer.AddErrorValidationMessageExposed("ERROR_2", "ERROR");
        customer.SetExistingInfoExposed(
            id: Guid.NewGuid(),
            tenantId: Guid.NewGuid(),
            createdBy: Guid.NewGuid().ToString(),
            createdAt: DateTimeProvider.GetDate(),
            updatedBy: Guid.NewGuid().ToString(),
            updatedAt: DateTimeProvider.GetDate(),
            sourcePlatform: Guid.NewGuid().ToString(),
            registryVersion: DateTimeProvider.GetDate()
        );

        // Assert
        newCustomer.Id.Should().NotBe(default(Guid));
        newCustomer.TenantId.Should().Be(tenantId);
        newCustomer.AuditableInfo.CreatedBy.Should().Be(executionUser);
        newCustomer.AuditableInfo.CreatedAt.Should().BeAfter(initialCreatedAt);
        newCustomer.AuditableInfo.LastUpdatedBy.Should().BeNull();
        newCustomer.AuditableInfo.LastUpdatedAt.Should().BeNull();
        newCustomer.AuditableInfo.LastSourcePlatform.Should().Be(sourcePlatform);
        newCustomer.RegistryVersion.Should().BeAfter(initialRegistryVersion);

        newCustomer.ValidationInfo.Should().NotBeNull();
        newCustomer.ValidationInfo.IsValid.Should().BeFalse();
        newCustomer.ValidationInfo.HasValidationMessage.Should().BeTrue();
        newCustomer.ValidationInfo.HasError.Should().BeTrue();
        newCustomer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
        newCustomer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(3);

        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[0].ValidationMessageType.Should().Be(ValidationMessageType.Information);
        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[0].Code.Should().Be("INFO_1");
        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[0].Description.Should().Be("INFORMATION");

        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[1].ValidationMessageType.Should().Be(ValidationMessageType.Warning);
        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[1].Code.Should().Be("WARNING_1");
        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[1].Description.Should().Be("WARNING");

        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[2].ValidationMessageType.Should().Be(ValidationMessageType.Error);
        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[2].Code.Should().Be("ERROR_1");
        newCustomer.ValidationInfo.ValidationMessageCollection.ToList()[2].Description.Should().Be("ERROR");
    }

    [Fact]
    public void DomainEntityBase_Should_Validate()
    {
        // Arrange
        var customer = new Customer();

        // Act
        var isValid = customer.Validate();

        // Assert
        customer.ValidationInfo.Should().NotBeNull();
        customer.ValidationInfo.HasValidationMessage.Should().BeTrue();
        customer.ValidationInfo.HasError.Should().BeTrue();
        isValid.Should().BeFalse();
        customer.ValidationInfo.IsValid.Should().Be(isValid);
        customer.ValidationInfo.ValidationMessageCollection.Should().NotBeNull();
        customer.ValidationInfo.ValidationMessageCollection.Should().HaveCount(3);

        var validationMessageCollection = customer.ValidationInfo.ValidationMessageCollection.ToList();

        validationMessageCollection[0].ValidationMessageType.Should().Be(ValidationMessageType.Error);
        validationMessageCollection[0].Code.Should().Be(Customer.ERROR_CODE);
        validationMessageCollection[0].Description.Should().Be(Customer.ERROR_DESCRIPTION);

        validationMessageCollection[1].ValidationMessageType.Should().Be(ValidationMessageType.Warning);
        validationMessageCollection[1].Code.Should().Be(Customer.WARNING_CODE);
        validationMessageCollection[1].Description.Should().Be(Customer.WARNING_DESCRIPTION);

        validationMessageCollection[2].ValidationMessageType.Should().Be(ValidationMessageType.Information);
        validationMessageCollection[2].Code.Should().Be(Customer.INFO_CODE);
        validationMessageCollection[2].Description.Should().Be(Customer.INFO_DESCRIPTION);
    }

    #region Models
    public class Customer
        : DomainEntityBase
    {
        // Constants
        public static readonly string ERROR_CODE = "code_1";
        public static readonly string ERROR_DESCRIPTION = "description_1";

        public static readonly string WARNING_CODE = "code_2";
        public static readonly string WARNING_DESCRIPTION = "description_2";

        public static readonly string INFO_CODE = "code_3";
        public static readonly string INFO_DESCRIPTION = "description_3";

        // Protected Abstract Methods
        protected override DomainEntityBase CreateInstanceForCloneInternal()
        {
            return new Customer();
        }

        // Protected Methods
        public void AddValidationMessageExposed(ValidationMessageType validationMessageType, string code, string description)
            => AddValidationMessageExposed(validationMessageType, code, description);

        public void AddInformationValidationMessageExposed(string code, string description)
            => AddInformationValidationMessageInternal<Customer>(code, description);

        public void AddWarningValidationMessageExposed(string code, string description)
            => AddWarningValidationMessageInternal<Customer>(code, description);

        public void AddErrorValidationMessageExposed(string code, string description)
            => AddErrorValidationMessageInternal<Customer>(code, description);

        public bool Validate()
        {
            return Validate<Customer>(() =>
                new ValidationResult(
                    new List<ValidationMessage> {
                            new ValidationMessage(
                                validationMessageType: ValidationMessageType.Error,
                                code: ERROR_CODE,
                                description: ERROR_DESCRIPTION
                            ),
                            new ValidationMessage(
                                validationMessageType: ValidationMessageType.Warning,
                                code: WARNING_CODE,
                                description: WARNING_DESCRIPTION
                            ),
                            new ValidationMessage(
                                validationMessageType: ValidationMessageType.Information,
                                code: INFO_CODE,
                                description: INFO_DESCRIPTION
                            )
                    }
                )
            );
        }

        public DomainEntityBase RegisterNewExposed(
            Guid tenantId,
            string executionUser,
            string sourcePlatform
        ) => RegisterNewInternal<Customer>(tenantId, executionUser, sourcePlatform);

        public DomainEntityBase SetExistingInfoExposed(
            Guid id,
            Guid tenantId,
            string createdBy,
            DateTimeOffset createdAt,
            string updatedBy,
            DateTimeOffset? updatedAt,
            string sourcePlatform,
            DateTimeOffset registryVersion
        ) => SetExistingInfoInternal<Customer>(id, tenantId, createdBy, createdAt, updatedBy, updatedAt, sourcePlatform, registryVersion);

        public DomainEntityBase RegisterModificationExposed(
            string executionUser,
            string sourcePlatform
        ) => RegisterModificationInternal<Customer>(executionUser, sourcePlatform);

        public Customer DeepCloneInternalExposed()
            => DeepCloneInternal<Customer>();
    } 
    #endregion
}

