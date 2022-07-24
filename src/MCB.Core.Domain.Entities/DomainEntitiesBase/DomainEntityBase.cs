using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Domain.Entities.Abstractions.ValueObjects;
using MCB.Core.Infra.CrossCutting.DateTime;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Models;

namespace MCB.Core.Domain.Entities.DomainEntitiesBase;

public abstract class DomainEntityBase
    : IDomainEntity
{
    // Fields
    private ValidationInfoValueObject _validationInfoValueObject = new();

    // Properties
    public Guid Id { get; private set; }
    public Guid TenantId { get; private set; }
    public AuditableInfoValueObject AuditableInfo { get; private set; }
    public DateTimeOffset RegistryVersion { get; private set; }
    public ValidationInfoValueObject ValidationInfo => _validationInfoValueObject.Clone();

    // Constructors
    protected DomainEntityBase()
    {
        AuditableInfo = new AuditableInfoValueObject();
    }

    // Private Methods
    private TDomainEntityBase SetId<TDomainEntityBase>(Guid id)
        where TDomainEntityBase : DomainEntityBase
    {
        Id = id;
        return (TDomainEntityBase)this;
    }
    private TDomainEntityBase GenerateNewId<TDomainEntityBase>()
        where TDomainEntityBase : DomainEntityBase
    {
        return SetId<TDomainEntityBase>(Guid.NewGuid());
    }
    private TDomainEntityBase SetTenant<TDomainEntityBase>(Guid tenantId)
        where TDomainEntityBase : DomainEntityBase
    {
        TenantId = tenantId;
        return (TDomainEntityBase)this;
    }
    private TDomainEntityBase SetAuditableInfo<TDomainEntityBase>(string createdBy, DateTimeOffset createdAt, string? updatedBy, DateTimeOffset? updatedAt, string sourcePlatform)
        where TDomainEntityBase : DomainEntityBase
    {
        AuditableInfo = new AuditableInfoValueObject(
            createdBy,
            createdAt,
            updatedBy,
            updatedAt,
            sourcePlatform
        );

        return (TDomainEntityBase)this;
    }
    private TDomainEntityBase SetRegistryVersion<TDomainEntityBase>(DateTimeOffset registryVersion)
         where TDomainEntityBase : DomainEntityBase
    {
        RegistryVersion = registryVersion;
        return (TDomainEntityBase)this;
    }
    private TDomainEntityBase GenerateNewRegistryVersion<TDomainEntityBase>()
        where TDomainEntityBase : DomainEntityBase
    {
        return SetRegistryVersion<TDomainEntityBase>(DateTimeProvider.GetDate());
    }
    private TDomainEntityBase SetValidationInfo<TDomainEntityBase>(ValidationInfoValueObject validationInfoValueObject)
         where TDomainEntityBase : DomainEntityBase
    {
        _validationInfoValueObject = validationInfoValueObject;
        return (TDomainEntityBase)this;
    }

    // Protected Abstract Methods
    protected abstract DomainEntityBase CreateInstanceForCloneInternal();

    protected void AddValidationMessageInternal(ValidationMessageType validationMessageType, string code, string description)
    {
        _validationInfoValueObject.AddValidationMessage(validationMessageType, code, description);
    }
    protected void AddValidationMessageInternal(ValidationMessage validationMessage)
    {
        _validationInfoValueObject.AddValidationMessage(
            validationMessage.ValidationMessageType, 
            validationMessage.Code, 
            validationMessage.Description
        );
    }
    protected void AddInformationValidationMessageInternal(string code, string description)
    {
        AddValidationMessageInternal(ValidationMessageType.Information, code, description);
    }
    protected void AddWarningValidationMessageInternal(string code, string description)
    {
        AddValidationMessageInternal(ValidationMessageType.Warning, code, description);
    }
    protected void AddErrorValidationMessageInternal(string code, string description)
    {
        AddValidationMessageInternal(ValidationMessageType.Error, code, description);
    }
    protected void AddFromValidationResultInternal(ValidationResult validationResult)
    {
        if (!validationResult.HasValidationMessage)
            return;

        foreach (var validationMessage in validationResult.ValidationMessageCollection)
            AddValidationMessageInternal(validationMessage);
    }

    protected virtual bool Validate(Func<ValidationResult> handle)
    {
        foreach (var validationMessage in handle().ValidationMessageCollection)
            AddValidationMessageInternal(
                validationMessage.ValidationMessageType,
                validationMessage.Code,
                validationMessage.Description
            );

        return ValidationInfo.IsValid;
    }

    protected TDomainEntityBase RegisterNewInternal<TDomainEntityBase>(Guid tenantId, string executionUser, string sourcePlatform)
        where TDomainEntityBase : DomainEntityBase
    {
        return GenerateNewId<TDomainEntityBase>()
            .SetTenant<TDomainEntityBase>(tenantId)
            .SetAuditableInfo<TDomainEntityBase>(
                createdBy: executionUser,
                createdAt: DateTimeProvider.GetDate(),
                updatedBy: null,
                updatedAt: null,
                sourcePlatform
            )
            .GenerateNewRegistryVersion<TDomainEntityBase>();
    }
    protected TDomainEntityBase SetExistingInfoInternal<TDomainEntityBase>(Guid id, Guid tenantId, string createdBy, DateTimeOffset createdAt, string? updatedBy, DateTimeOffset? updatedAt, string sourcePlatform, DateTimeOffset registryVersion)
        where TDomainEntityBase : DomainEntityBase
    {
        return SetId<TDomainEntityBase>(id)
            .SetTenant<TDomainEntityBase>(tenantId)
            .SetAuditableInfo<TDomainEntityBase>(
                createdBy,
                createdAt,
                updatedBy,
                updatedAt,
                sourcePlatform
            )
            .SetRegistryVersion<TDomainEntityBase>(registryVersion);
    }

    protected TDomainEntityBase RegisterModificationInternal<TDomainEntityBase>(string executionUser, string sourcePlatform)
        where TDomainEntityBase : DomainEntityBase
    {
        return SetAuditableInfo<TDomainEntityBase>(
            createdBy: AuditableInfo.CreatedBy,
            createdAt: AuditableInfo.CreatedAt,
            updatedBy: executionUser,
            updatedAt: DateTimeProvider.GetDate(),
            sourcePlatform
        )
        .GenerateNewRegistryVersion<TDomainEntityBase>();
    }

    protected TDomainEntityBase DeepCloneInternal<TDomainEntityBase>()
        where TDomainEntityBase : DomainEntityBase
    {
        return
            ((TDomainEntityBase)CreateInstanceForCloneInternal())
            .SetExistingInfoInternal<TDomainEntityBase>(
                Id,
                TenantId,
                AuditableInfo.CreatedBy,
                AuditableInfo.CreatedAt,
                AuditableInfo.LastUpdatedBy,
                AuditableInfo.LastUpdatedAt,
                AuditableInfo.LastSourcePlatform,
                RegistryVersion
            )
            .SetValidationInfo<TDomainEntityBase>(ValidationInfo);
    }
}
