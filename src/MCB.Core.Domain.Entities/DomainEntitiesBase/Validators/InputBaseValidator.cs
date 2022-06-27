using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using FluentValidation;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Core.Domain.Entities.Abstractions.Specifications.Interfaces;

namespace MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;

public abstract class InputBaseValidator<TInputBase>
    : ValidatorBase<TInputBase>,
    Infra.CrossCutting.DesignPatterns.Validator.Abstractions.IValidator<TInputBase>
    where TInputBase : InputBase
{
    // Fields
    private readonly IDomainEntitySpecifications _domainEntitySpecifications;

    // Constructors
    protected InputBaseValidator(
        IDomainEntitySpecifications domainEntitySpecifications
    )
    {
        _domainEntitySpecifications = domainEntitySpecifications;
    }

    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        fluentValidationValidatorWrapper.RuleFor(input => input.TenantId)
            .Must(tenantId => _domainEntitySpecifications.TenantIdShouldRequired(tenantId))
            .WithErrorCode(nameof(IDomainEntitySpecifications.TenantIdShouldRequired))
            .WithSeverity(Severity.Error);

        ConfigureFluentValidationConcreteValidatorInternal(fluentValidationValidatorWrapper);
    }
    protected abstract void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper);
}
