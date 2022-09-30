﻿using MCB.Core.Domain.Entities.Abstractions.Specifications.Interfaces;
using MCB.Core.Infra.CrossCutting.Abstractions.DateTime;

namespace MCB.Core.Domain.Entities.DomainEntitiesBase.Specifications;

public class DomainEntitySpecifications
    : IDomainEntitySpecifications
{
    // Protected Properties
    protected IDateTimeProvider DateTimeProvider { get; }

    // Constructors
    public DomainEntitySpecifications(IDateTimeProvider dateTimeProvider)
    {
        DateTimeProvider = dateTimeProvider;
    }

    // Constructors
    public bool IdShouldRequired(Guid id)
    {
        return id != Guid.Empty;
    }

    public bool TenantIdShouldRequired(Guid tenantId)
    {
        return tenantId != Guid.Empty;
    }

    public bool CreatedAtShouldRequired(DateTimeOffset createdAt)
    {
        return createdAt != DateTimeOffset.MinValue;
    }
    public bool CreatedAtShouldValid(DateTimeOffset createdAt)
    {
        return CreatedAtShouldRequired(createdAt)
            && createdAt <= DateTimeProvider.GetDate();
    }

    public bool CreatedByShouldRequired(string createdBy)
    {
        return !string.IsNullOrWhiteSpace(createdBy);
    }
    public bool CreatedByShouldValid(string createdBy)
    {
        return CreatedByShouldRequired(createdBy)
            && createdBy.Length <= 250;
    }

    public bool LastUpdatedAtShouldRequired(DateTimeOffset? lastUpdatedAt)
    {
        return lastUpdatedAt != null
            && lastUpdatedAt != DateTimeOffset.MinValue;
    }
    public bool LastUpdatedAtShouldValid(DateTimeOffset? lastUpdatedAt, DateTimeOffset createdAt)
    {
        return LastUpdatedAtShouldRequired(lastUpdatedAt)
            && lastUpdatedAt > createdAt
            && lastUpdatedAt <= DateTimeProvider.GetDate();
    }

    public bool LastUpdatedByShouldRequired(string lastUpdatedBy)
    {
        return !string.IsNullOrWhiteSpace(lastUpdatedBy);
    }
    public bool LastUpdatedByShouldValid(string lastUpdatedBy)
    {
        return LastUpdatedByShouldRequired(lastUpdatedBy)
            && lastUpdatedBy.Length <= 250;
    }

    public bool LastSourcePlatformShouldRequired(string lastSourcePlatform)
    {
        return !string.IsNullOrWhiteSpace(lastSourcePlatform);
    }
    public bool LastSourcePlatformShouldValid(string lastSourcePlatform)
    {
        return LastSourcePlatformShouldRequired(lastSourcePlatform)
            && lastSourcePlatform.Length <= 250;
    }

    public bool RegistryVersionShouldRequired(DateTimeOffset registryVersion)
    {
        return registryVersion != DateTimeOffset.MinValue;
    }
    public bool RegistryVersionShouldValid(DateTimeOffset registryVersion)
    {
        return RegistryVersionShouldRequired(registryVersion)
            && registryVersion <= DateTimeProvider.GetDate();
    }
}
