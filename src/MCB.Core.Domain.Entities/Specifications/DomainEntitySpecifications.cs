using MCB.Core.Domain.Entities.Abstractions.Specifications.Interfaces;
using MCB.Core.Infra.CrossCutting.DateTime;

namespace MCB.Core.Domain.Entities.Specifications;

public class DomainEntitySpecifications
    : IDomainEntitySpecifications
{
    public bool IdShouldRequired(Guid id)
    {
        return id != Guid.Empty;
    }

    public bool TenantIdShouldRequired(Guid tenantId)
    {
        return tenantId != Guid.Empty;
    }

    public bool CreationInfoShouldRequired(DateTimeOffset createdAt, string createdBy, string lastSourcePlatform)
    {
        return createdAt > DateTimeOffset.MinValue 
            && !string.IsNullOrWhiteSpace(createdBy)
            && !string.IsNullOrWhiteSpace(lastSourcePlatform);
    }
    public bool CreationInfoShouldValid(DateTimeOffset createdAt, string createdBy, string lastSourcePlatform)
    {
        return CreationInfoShouldRequired(createdAt, createdBy, lastSourcePlatform)
            && createdAt <= DateTimeProvider.GetDate()
            && createdBy.Length <= 250
            && lastSourcePlatform.Length <= 250;
    }

    public bool UpdateInfoShouldRequired(DateTimeOffset? lastUpdatedAt, string lastUpdatedBy, string lastSourcePlatform)
    {
        return lastUpdatedAt is not null
            && lastUpdatedAt > DateTimeOffset.MinValue
            && !string.IsNullOrWhiteSpace(lastUpdatedBy)
            && !string.IsNullOrWhiteSpace(lastSourcePlatform);
    }
    public bool UpdateInfoShouldValid(DateTimeOffset createdAt, DateTimeOffset? lastUpdatedAt, string lastUpdatedBy, string lastSourcePlatform)
    {
        return UpdateInfoShouldRequired(lastUpdatedAt, lastUpdatedBy, lastSourcePlatform)
            && lastUpdatedAt > createdAt
            && lastUpdatedAt <= DateTimeProvider.GetDate()
            && lastUpdatedBy.Length <= 250
            && lastSourcePlatform.Length <= 250;
    }

    public bool RegistryVersionShouldRequired(DateTimeOffset registryVersion)
    {
        return registryVersion > DateTimeOffset.MinValue;
    }
    public bool RegistryVersionShouldValid(DateTimeOffset registryVersion)
    {
        return RegistryVersionShouldRequired(registryVersion)
            && registryVersion <= DateTimeProvider.GetDate();
    }
}
