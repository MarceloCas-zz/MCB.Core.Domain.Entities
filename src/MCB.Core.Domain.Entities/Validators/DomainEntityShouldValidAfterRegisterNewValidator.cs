using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications.Interfaces;
using MCB.Core.Domain.Entities.Abstractions.Validators.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;

namespace MCB.Core.Domain.Entities.Validators
{
    public class DomainEntityShouldValidAfterRegisterNewValidator<TDomainEntity>
        : ValidatorBase<TDomainEntity>,
        IDomainEntityShouldValidAfterRegisterNewValidator<TDomainEntity>
        where TDomainEntity : DomainEntityBase
    {
        // Fields
        private readonly IDomainEntitySpecifications _domainEntitySpecifications;

        // Constructors
        public DomainEntityShouldValidAfterRegisterNewValidator(
            IDomainEntitySpecifications domainEntitySpecifications
        )
        {
            _domainEntitySpecifications = domainEntitySpecifications;
        }

        // Protected Methods
        protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
        {
            // Id
            fluentValidationValidatorWrapper.RuleFor(customer => customer.Id)
                .Must(id => _domainEntitySpecifications.IdShouldRequired(id))
                .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.IdShouldRequired)))
                .WithSeverity(Severity.Error);

            // Tenant Id
            fluentValidationValidatorWrapper.RuleFor(customer => customer.TenantId)
                .Must(tenantId => _domainEntitySpecifications.TenantIdShouldRequired(tenantId))
                .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.TenantIdShouldRequired)))
                .WithSeverity(Severity.Error);

            // Creation Info
            fluentValidationValidatorWrapper.RuleFor(customer => customer.AuditableInfo)
                .NotNull()
                .Must(auditableInfo => _domainEntitySpecifications.CreationInfoShouldRequired(auditableInfo.CreatedAt, auditableInfo.CreatedBy, auditableInfo.LastSourcePlatform))
                .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.CreationInfoShouldRequired)))
                .WithSeverity(Severity.Error)
                .Must(auditableInfo => _domainEntitySpecifications.CreationInfoShouldValid(auditableInfo.CreatedAt, auditableInfo.CreatedBy, auditableInfo.LastSourcePlatform))
                .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.CreationInfoShouldValid)))
                .WithSeverity(Severity.Error);

            // Registry Version
            fluentValidationValidatorWrapper.RuleFor(customer => customer.RegistryVersion)
                .NotNull()
                .Must(registryVersion => _domainEntitySpecifications.RegistryVersionShouldRequired(registryVersion))
                .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.CreationInfoShouldRequired)))
                .WithSeverity(Severity.Error)
                .Must(registryVersion => _domainEntitySpecifications.RegistryVersionShouldValid(registryVersion))
                .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.CreationInfoShouldValid)))
                .WithSeverity(Severity.Error);
        }
    }
}
