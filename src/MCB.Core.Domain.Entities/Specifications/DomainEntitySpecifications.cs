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

    public bool CreationInfoShouldRequired(DateTimeOffset createdAt, string createdBy)
    {
        return createdAt > DateTimeOffset.MinValue 
            && !string.IsNullOrWhiteSpace(createdBy);
    }
    public bool CreationInfoShouldValid(DateTimeOffset createdAt, string createdBy)
    {
        return CreationInfoShouldRequired(createdAt, createdBy)
            && createdAt <= DateTimeProvider.GetDate()
            && createdBy.Length <= 250;
    }

    public bool UpdateInfoShouldRequired(DateTimeOffset? lastUpdateAt, string lastUpdatedBy)
    {
        return lastUpdateAt is not null
            && lastUpdateAt > DateTimeOffset.MinValue
            && !string.IsNullOrWhiteSpace(lastUpdatedBy);
    }
    public bool UpdateInfoShouldValid(DateTimeOffset createdAt, DateTimeOffset? lastUpdatedAt, string lastUpdatedBy)
    {
        return UpdateInfoShouldRequired(lastUpdatedAt, lastUpdatedBy)
            && lastUpdatedAt > createdAt
            && lastUpdatedAt <= DateTimeProvider.GetDate()
            && lastUpdatedBy.Length <= 250;
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
