using FluentValidation;
using MCB.Core.Domain.Entities.Abstractions.Specifications.Interfaces;
using MCB.Core.Domain.Entities.Abstractions.Validators.Interfaces;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions.Enums;

namespace MCB.Core.Domain.Entities.Validators;

public class DomainEntityShouldValidAfterRegisterModificationValidator<TDomainEntity>
    : DomainEntityShouldValidAfterRegisterNewValidator<TDomainEntity>,
    IDomainEntityShouldValidAfterRegisterModificationValidator<TDomainEntity>
    where TDomainEntity : DomainEntityBase
{
    // Fields
    private readonly IDomainEntitySpecifications _domainEntitySpecifications;

    // Constructors
    public DomainEntityShouldValidAfterRegisterModificationValidator(
        IDomainEntitySpecifications domainEntitySpecifications
    ): base(domainEntitySpecifications)
    {
        _domainEntitySpecifications = domainEntitySpecifications;
    }

    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        base.ConfigureFluentValidationConcreteValidator(fluentValidationValidatorWrapper);

        // Update Info
        fluentValidationValidatorWrapper.RuleFor(customer => customer.AuditableInfo)
            .NotNull()
            .Must(auditableInfo => _domainEntitySpecifications.UpdateInfoShouldRequired(auditableInfo.LastUpdatedAt, auditableInfo.LastUpdatedBy, auditableInfo.LastSourcePlatform))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.UpdateInfoShouldRequired)))
            .WithSeverity(Severity.Error)
            .Must(auditableInfo => _domainEntitySpecifications.UpdateInfoShouldValid(auditableInfo.CreatedAt, auditableInfo.LastUpdatedAt, auditableInfo.CreatedBy, auditableInfo.LastSourcePlatform))
            .WithErrorCode(CreateMessageCodeInternal(ValidationMessageType.Error, nameof(IDomainEntitySpecifications.UpdateInfoShouldValid)))
            .WithSeverity(Severity.Error);
    }
}
