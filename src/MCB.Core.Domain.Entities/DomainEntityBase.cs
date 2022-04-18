using MCB.Core.Domain.Entities.Abstractions;
using MCB.Core.Domain.Entities.Abstractions.ValueObjects;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;

namespace MCB.Core.Domain.Entities
{
    public abstract class DomainEntityBase
        : IDomainEntity
    {
        // Fields
        private ValidationInfoValueObject _validationInfoValueObject;

        // Properties
        public Guid Id { get; private set; }
        public Guid TenantId { get; private set; }
        public AuditableInfoValueObject AuditableInfo { get; private set; }
        public DateTimeOffset RegistryVersion { get; private set; }
        public ValidationInfoValueObject ValidationInfo => (ValidationInfoValueObject)_validationInfoValueObject.Clone();

        // Constructors
        protected DomainEntityBase()
        {
            _validationInfoValueObject = new ValidationInfoValueObject();
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

        // Protected Methods
        protected void AddValidationMessage(ValidationMessageType validationMessageType, string code, string description)
        {
            _validationInfoValueObject.AddValidationMessage(validationMessageType, code, description);
        }
        protected void AddInformationValidationMessage(string code, string description)
        {
            AddValidationMessage(ValidationMessageType.Information, code, description);
        }
        protected void AddWarningValidationMessage(string code, string description)
        {
            AddValidationMessage(ValidationMessageType.Warning, code, description);
        }
        protected void AddErrorValidationMessage(string code, string description)
        {
            AddValidationMessage(ValidationMessageType.Error, code, description);
        }

        protected DomainEntityBase RegisterNew(
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
        protected DomainEntityBase SetExistingInfo(
            Guid id,
            Guid tenantId,
            string createdBy,
            DateTimeOffset createdAt,
            string updatedBy,
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
        protected DomainEntityBase RegisterModification(
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
    }
}
