using FluentAssertions;
using MCB.Core.Domain.Entities.Specifications;
using MCB.Core.Infra.CrossCutting.DateTime;
using System;
using Xunit;

namespace MCB.Core.Domain.Entities.Tests.SpecificationsTests
{
    public class DomainEntitySpecificationsBaseTest
    {
        [Fact]
        public void DomainEntitySpecifications_IdShouldRequired_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var id = Guid.NewGuid();

            // Act
            var result = domainEntitySpecifications.IdShouldRequired(id);

            // Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_IdShouldRequired_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var id = Guid.Empty;

            // Act
            var result = domainEntitySpecifications.IdShouldRequired(id);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_TenantIdShouldRequired_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var teiantId = Guid.NewGuid();

            // Act
            var result = domainEntitySpecifications.TenantIdShouldRequired(teiantId);

            // Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_TenantIdShouldRequired_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var tenantId = Guid.Empty;

            // Act
            var result = domainEntitySpecifications.TenantIdShouldRequired(tenantId);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_CreationInfoShouldRequired_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var createdBy = new string('a', 250);
            var createdAt = DateTimeProvider.GetDate().AddDays(-1);
            var lastSourcePlatform = new string('a', 250);

            // Act
            var result = domainEntitySpecifications.CreationInfoShouldRequired(createdAt, createdBy, lastSourcePlatform);

            // Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_CreationInfoShouldRequired_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var createdAtInvalid = DateTimeOffset.MinValue;
            var createdAtValid = DateTimeProvider.GetDate();

            var createdByInvalid = string.Empty;
            var createdByValid = new string('a', 250);

            var lastSourcePlatform = default(string);

            // Act
            var result1 = domainEntitySpecifications.CreationInfoShouldRequired(createdAtValid, createdByValid, lastSourcePlatform);
            var result2 = domainEntitySpecifications.CreationInfoShouldRequired(createdAtValid, createdByInvalid, lastSourcePlatform);
            var result3 = domainEntitySpecifications.CreationInfoShouldRequired(createdAtInvalid, createdByInvalid, lastSourcePlatform);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
            result3.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_CreationInfoShouldValid_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var createdBy = new string('a', 250);
            var lastSourcePlatform = new string('a', 250);

            var createdAt1 = DateTimeProvider.GetDate().AddDays(-1);
            var createdAt2 = DateTimeProvider.GetDate();

            // Act
            var result1 = domainEntitySpecifications.CreationInfoShouldValid(createdAt1, createdBy, lastSourcePlatform);
            var result2 = domainEntitySpecifications.CreationInfoShouldValid(createdAt2, createdBy, lastSourcePlatform);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_CreationInfoShouldValid_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var createdAtValid = DateTimeProvider.GetDate().AddDays(-1);
            var createdAtInvalid = DateTimeProvider.GetDate().AddDays(1);

            var createdByInvalid = new string('a', 251);
            var createdByValid = new string('a', 250);

            var lastSourcePlatformInvalid = new string('a', 251);

            // Act
            var result1 = domainEntitySpecifications.CreationInfoShouldValid(createdAtValid, createdByInvalid, lastSourcePlatformInvalid);
            var result2 = domainEntitySpecifications.CreationInfoShouldValid(createdAtInvalid, createdByValid, lastSourcePlatformInvalid);
            var result3 = domainEntitySpecifications.CreationInfoShouldValid(createdAtInvalid, createdByInvalid, lastSourcePlatformInvalid);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
            result3.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_UpdateInfoShouldRequired_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var updatedBy = new string('a', 250);
            var lastSourcePlatform = new string('a', 250);

            var lastUpdateDate1 = DateTimeProvider.GetDate().AddDays(-1);
            var lastUpdateDate2 = DateTimeProvider.GetDate();

            // Act
            var result1 = domainEntitySpecifications.UpdateInfoShouldRequired(lastUpdateDate1, updatedBy, lastSourcePlatform);
            var result2 = domainEntitySpecifications.UpdateInfoShouldRequired(lastUpdateDate2, updatedBy, lastSourcePlatform);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_UpdateInfoShouldRequired_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var lastUpdateDateInvalid = DateTimeOffset.MinValue;
            var lastUpdateDateValid = DateTimeProvider.GetDate();

            var lastUpdateByInvalid = string.Empty;
            var lastUpdateByValid = new string('a', 250);

            var lastSourcePlatformInvalid = string.Empty;

            // Act
            var result1 = domainEntitySpecifications.UpdateInfoShouldRequired(lastUpdateDateValid, lastUpdateByInvalid, lastSourcePlatformInvalid);
            var result2 = domainEntitySpecifications.UpdateInfoShouldRequired(lastUpdateDateInvalid, lastUpdateByValid, lastSourcePlatformInvalid);
            var result3 = domainEntitySpecifications.UpdateInfoShouldRequired(lastUpdateAt: null, lastUpdateByValid, lastSourcePlatformInvalid);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
            result3.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_UpdateInfoShouldValid_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var createdAt = DateTimeProvider.GetDate().AddDays(-1);
            var lastUpdatedAt = DateTimeProvider.GetDate();
            var lastUpdatedBy = new string('a', 250);
            var lastSourcePlatform = new string('a', 250);

            // Act
            var result = domainEntitySpecifications.UpdateInfoShouldValid(createdAt, lastUpdatedAt, lastUpdatedBy, lastSourcePlatform);

            // Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_UpdateInfoShouldValid_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var validCreatedAtValid = DateTimeProvider.GetDate().AddDays(-1);
            var futureCreatedAtValid = DateTimeProvider.GetDate().AddDays(+1);

            var validUpdatedAtValid = DateTimeProvider.GetDate();
            var futureUpdatedAtValid = DateTimeProvider.GetDate().AddDays(+1);

            var lastUpdatedByInvalid = new string('a', 251);
            var lastUpdatedByValid = new string('a', 250);

            var lastSourcePlatform = new string('a', 251);

            // Act
            var result1 = domainEntitySpecifications.UpdateInfoShouldValid(futureCreatedAtValid, validUpdatedAtValid, lastUpdatedByInvalid, lastSourcePlatform);
            var result2 = domainEntitySpecifications.UpdateInfoShouldValid(validCreatedAtValid, futureUpdatedAtValid, lastUpdatedByInvalid, lastSourcePlatform);
            var result3 = domainEntitySpecifications.UpdateInfoShouldValid(validCreatedAtValid, validUpdatedAtValid, lastUpdatedByInvalid, lastSourcePlatform);
            var result4 = domainEntitySpecifications.UpdateInfoShouldValid(validCreatedAtValid, lastUpdatedAt: null, lastUpdatedByInvalid, lastSourcePlatform);
            var result5 = domainEntitySpecifications.UpdateInfoShouldValid(validCreatedAtValid, lastUpdatedAt: null, lastUpdatedByValid, lastSourcePlatform);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
            result3.Should().Be(expectedResult);
            result4.Should().Be(expectedResult);
            result5.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_RegistryVersionShouldRequired_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var registryVersion = DateTimeProvider.GetDate();

            // Act
            var result = domainEntitySpecifications.RegistryVersionShouldRequired(registryVersion);

            // Assert
            result.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_RegistryVersionShouldRequired_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var registryVersion = DateTimeOffset.MinValue;

            // Act
            var result = domainEntitySpecifications.RegistryVersionShouldRequired(registryVersion);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void DomainEntitySpecifications_RegistryVersionShouldValid_Success()
        {
            // Arrange
            var expectedResult = true;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();

            var registryVersion1 = DateTimeProvider.GetDate();
            var registryVersion2 = DateTimeProvider.GetDate().AddDays(-1);

            // Act
            var result1 = domainEntitySpecifications.RegistryVersionShouldValid(registryVersion1);
            var result2 = domainEntitySpecifications.RegistryVersionShouldValid(registryVersion2);

            // Assert
            result1.Should().Be(expectedResult);
            result2.Should().Be(expectedResult);
        }
        [Fact]
        public void DomainEntitySpecifications_RegistryVersionShouldValid_Fail()
        {
            // Arrange
            var expectedResult = false;
            var domainEntitySpecifications = new DummyDomainEntitySpecifications();
            var registryVersion = DateTimeProvider.GetDate().AddDays(1);

            // Act
            var result = domainEntitySpecifications.RegistryVersionShouldValid(registryVersion);

            // Assert
            result.Should().Be(expectedResult);
        }

        public class DummyDomainEntitySpecifications
            : DomainEntitySpecifications
        {

        }
    }
}
