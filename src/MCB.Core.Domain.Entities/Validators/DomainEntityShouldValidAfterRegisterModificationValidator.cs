using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications.Interfaces;
using MCB.Core.Domain.Entities.Abstractions.Validators.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCB.Core.Domain.Entities.Validators;

public class DomainEntityShouldValidAfterRegisterModificationValidator<TDomainEntity>
    : ValidatorBase<TDomainEntity>,
    IDomainEntityShouldValidAfterRegisterModificationValidator<TDomainEntity>
    where TDomainEntity : DomainEntityBase
{
    // Fields
    private readonly IDomainEntitySpecifications _domainEntitySpecifications;

    // Constructors
    public DomainEntityShouldValidAfterRegisterModificationValidator(
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
            .Must(auditableInfo => _domainEntitySpecifications.CreationInfoShouldRequired(auditableInfo.CreatedAt, auditableInfo.CreatedBy))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.CreationInfoShouldRequired)))
            .WithSeverity(Severity.Error)
            .Must(auditableInfo => _domainEntitySpecifications.CreationInfoShouldValid(auditableInfo.CreatedAt, auditableInfo.CreatedBy))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.CreationInfoShouldValid)))
            .WithSeverity(Severity.Error);

        // Update Info
        fluentValidationValidatorWrapper.RuleFor(customer => customer.AuditableInfo)
            .NotNull()
            .Must(auditableInfo => _domainEntitySpecifications.UpdateInfoShouldRequired(auditableInfo.LastUpdatedAt, auditableInfo.LastUpdatedBy))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.UpdateInfoShouldRequired)))
            .WithSeverity(Severity.Error)
            .Must(auditableInfo => _domainEntitySpecifications.UpdateInfoShouldValid(auditableInfo.CreatedAt, auditableInfo.LastUpdatedAt, auditableInfo.CreatedBy))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.UpdateInfoShouldValid)))
            .WithSeverity(Severity.Error);


        // Last Source Platform
        fluentValidationValidatorWrapper.RuleFor(customer => customer.AuditableInfo.LastSourcePlatform)
            .Must(lastSourcePlatform => _domainEntitySpecifications.SourcePlatform(lastSourcePlatform))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.TenantIdShouldRequired)))
            .WithSeverity(Severity.Error);
    }
}
