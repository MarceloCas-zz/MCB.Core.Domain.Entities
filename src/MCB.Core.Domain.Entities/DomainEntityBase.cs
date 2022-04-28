using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Domain.Entities.Abstractions.ValueObjects;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;

namespace MCB.Core.Domain.Entities
{
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
        public ValidationInfoValueObject ValidationInfo => (ValidationInfoValueObject)_validationInfoValueObject.Clone();

        // Constructors
        protected DomainEntityBase()
        {
            AuditableInfo = new AuditableInfoValueObject();
        }

        // Private Methods
        private DomainEntityBase SetId(Guid id)
        {
            Id = id;
            return this;
        }
        private DomainEntityBase GenerateNewId() => SetId(Guid.NewGuid());
        private DomainEntityBase SetTenant(Guid tenantId)
        {
            TenantId = tenantId;
            return this;
        }
        private DomainEntityBase SetAuditableInfo(
            string createdBy,
            DateTimeOffset createdAt,
            string? updatedBy,
            DateTimeOffset? updatedAt,
            string sourcePlatform
        )
        {
            AuditableInfo = new AuditableInfoValueObject(
                createdBy,
                createdAt,
                updatedBy,
                updatedAt,
                sourcePlatform
            );

            return this;
        }
        private DomainEntityBase SetRegistryVersion(DateTimeOffset registryVersion)
        {
            RegistryVersion = registryVersion;
            return this;
        }
        private DomainEntityBase GenerateNewRegistryVersion() => SetRegistryVersion(DateTimeOffset.UtcNow);
        private DomainEntityBase SetValidationInfo(ValidationInfoValueObject validationInfoValueObject)
        {
            _validationInfoValueObject = validationInfoValueObject;
            return this;
        }

        // Protected Abstract Methods
        protected abstract DomainEntityBase CreateInstanceForCloneInternal();

        // Protected Methods
        protected void AddValidationMessageInternal(ValidationMessageType validationMessageType, string code, string description)
        {
            _validationInfoValueObject.AddValidationMessage(validationMessageType, code, description);
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

        protected DomainEntityBase RegisterNewInternal(
            Guid tenantId,
            string executionUser,
            string sourcePlatform
        )
        {
            return GenerateNewId()
                .SetTenant(tenantId)
                .SetAuditableInfo(
                    createdBy: executionUser,
                    createdAt: DateTimeOffset.UtcNow,
                    updatedBy: null,
                    updatedAt: null,
                    sourcePlatform
                )
                .GenerateNewRegistryVersion();
        }
        protected DomainEntityBase SetExistingInfoInternal(
            Guid id,
            Guid tenantId,
            string createdBy,
            DateTimeOffset createdAt,
            string? updatedBy,
            DateTimeOffset? updatedAt,
            string sourcePlatform,
            DateTimeOffset registryVersion
        )
        {
            return SetId(id)
                .SetTenant(tenantId)
                .SetAuditableInfo(
                    createdBy,
                    createdAt,
                    updatedBy,
                    updatedAt,
                    sourcePlatform
                )
                .SetRegistryVersion(registryVersion);
        }
        
        protected DomainEntityBase RegisterModificationInternal(
            string executionUser,
            string sourcePlatform
        )
        {
            return SetAuditableInfo(
                createdBy: AuditableInfo.CreatedBy,
                createdAt: AuditableInfo.CreatedAt,
                updatedBy: executionUser,
                updatedAt: DateTimeOffset.UtcNow,
                sourcePlatform
            )
            .GenerateNewRegistryVersion();
        }

        protected TDomainEntityBase DeepCloneInternal<TDomainEntityBase>()
            where TDomainEntityBase : DomainEntityBase
        {
            return (TDomainEntityBase)
                CreateInstanceForCloneInternal()
                .SetExistingInfoInternal(
                    Id,
                    TenantId,
                    AuditableInfo.CreatedBy,
                    AuditableInfo.CreatedAt,
                    AuditableInfo.UpdatedBy,
                    AuditableInfo.UpdatedAt,
                    AuditableInfo.SourcePlatform,
                    RegistryVersion
                )
                .SetValidationInfo(ValidationInfo);
        }
    }
}
