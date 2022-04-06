using FluentAssertions;
using System;
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
            customer.RegisterNew(tenantId, executionUser, sourcePlatform);

            // Assert
            customer.Id.Should().NotBe(default(Guid));
            customer.TenantId.Should().Be(tenantId);
            customer.AuditableInfo.CreatedBy.Should().Be(executionUser);
            customer.AuditableInfo.CreatedAt.Should().BeAfter(initialCreatedAt);
            customer.AuditableInfo.UpdatedBy.Should().BeNull();
            customer.AuditableInfo.UpdatedAt.Should().BeNull();
            customer.AuditableInfo.SourcePlatform.Should().Be(sourcePlatform);
            customer.RegistryVersion.Should().BeAfter(initialRegistryVersion);
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
            customer.SetExistingInfo(id, tenantId, createdBy, createdAt, updatedBy, updatedAt, sourcePlatform, registryVersion);

            // Assert
            customer.Id.Should().Be(id);
            customer.TenantId.Should().Be(tenantId);
            customer.AuditableInfo.CreatedBy.Should().Be(createdBy);
            customer.AuditableInfo.CreatedAt.Should().Be(createdAt);
            customer.AuditableInfo.UpdatedBy.Should().Be(updatedBy);
            customer.AuditableInfo.UpdatedAt.Should().Be(updatedAt);
            customer.AuditableInfo.SourcePlatform.Should().Be(sourcePlatform);
            customer.RegistryVersion.Should().Be(registryVersion);
        }

        [Fact]
        public void DomainEntityBase_Should_RegisterModification()
        {
            // Arrange
            var customer = new Customer();
            var tenantId = Guid.NewGuid();
            var executionUser = "marcelo.castelo@outlook.com";
            var sourcePlatform = "AppDemo";
            customer.RegisterNew(tenantId, executionUser, sourcePlatform);

            var initialId = customer.Id;
            var initialCreatedBy = customer.AuditableInfo.CreatedBy;
            var initialCreatedAt = customer.AuditableInfo.CreatedAt;
            var initialUpdatedAt = customer.AuditableInfo.UpdatedAt;
            var initialRegistryVersion = customer.RegistryVersion;

            var modificationExecutionUser = "marcelo.castelo@github.com";
            var modificationSourcePlatform = "AppDemo2";

            // Act
            customer.RegisterModification(modificationExecutionUser, modificationSourcePlatform);

            // Assert
            customer.Id.Should().Be(initialId);
            customer.TenantId.Should().Be(tenantId);
            customer.AuditableInfo.CreatedBy.Should().Be(initialCreatedBy);
            customer.AuditableInfo.CreatedAt.Should().Be(initialCreatedAt);
            customer.AuditableInfo.UpdatedBy.Should().Be(modificationExecutionUser);
            customer.AuditableInfo.UpdatedAt.Should().BeAfter(initialUpdatedAt ?? default);
            customer.AuditableInfo.SourcePlatform.Should().Be(modificationSourcePlatform);
            customer.RegistryVersion.Should().BeAfter(initialRegistryVersion);
        }
    }

    public class Customer
        : DomainEntityBase
    {

    }
}
