using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using FluentValidation;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Core.Domain.Entities.DomainEntitiesBase.Validators.Interfaces;

namespace MCB.Core.Domain.Entities.DomainEntitiesBase.Validators;

public abstract class InputBaseValidator<TInputBase>
    : ValidatorBase<TInputBase>,
    IInputBaseValidator<TInputBase>
    where TInputBase : InputBase
{
    // Protected Methods
    protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
    {
        fluentValidationValidatorWrapper.RuleFor(input => input.TenantId)
            .Must(tenantId => tenantId != Guid.Empty)
            .WithErrorCode(IInputBaseValidator<TInputBase>.InputBaseShouldHaveTenantIdErrorCode)
            .WithMessage(IInputBaseValidator<TInputBase>.InputBaseShouldHaveTenantIdMessage)
            .WithSeverity(IInputBaseValidator<TInputBase>.InputBaseShouldHaveTenantIdSeverity);

        fluentValidationValidatorWrapper.RuleFor(input => input.ExecutionUser)
            .Must(executionUser => !string.IsNullOrWhiteSpace(executionUser))
            .WithErrorCode(IInputBaseValidator<TInputBase>.InputBaseShouldHaveExecutionUserErrorCode)
            .WithMessage(IInputBaseValidator<TInputBase>.InputBaseShouldHaveExecutionUserMessage)
            .WithSeverity(IInputBaseValidator<TInputBase>.InputBaseShouldHaveExecutionUserSeverity);

        fluentValidationValidatorWrapper.RuleFor(input => input.ExecutionUser)
            .Must(executionUser => executionUser?.Length <= 150)
            .When(inputBase => !string.IsNullOrWhiteSpace(inputBase.ExecutionUser))
            .WithErrorCode(IInputBaseValidator<TInputBase>.InputBaseShouldHaveExecutionUserWithValidLengthErrorCode)
            .WithMessage(IInputBaseValidator<TInputBase>.InputBaseShouldHaveExecutionUserWithValidLengthMessage)
            .WithSeverity(IInputBaseValidator<TInputBase>.InputBaseShouldHaveExecutionUserWithValidLengthSeverity);

        fluentValidationValidatorWrapper.RuleFor(input => input.SourcePlatform)
            .Must(sourcePlatform => !string.IsNullOrWhiteSpace(sourcePlatform))
            .WithErrorCode(IInputBaseValidator<TInputBase>.InputBaseShouldHaveSourcePlatformErrorCode)
            .WithMessage(IInputBaseValidator<TInputBase>.InputBaseShouldHaveSourcePlatformMessage)
            .WithSeverity(IInputBaseValidator<TInputBase>.InputBaseShouldHaveSourcePlatformSeverity);

        fluentValidationValidatorWrapper.RuleFor(input => input.SourcePlatform)
            .Must(sourcePlatform => sourcePlatform?.Length <= 150)
            .When(inputBase => !string.IsNullOrWhiteSpace(inputBase.SourcePlatform))
            .WithErrorCode(IInputBaseValidator<TInputBase>.InputBaseShouldHaveSourcePlatformWithValidLengthErrorCode)
            .WithMessage(IInputBaseValidator<TInputBase>.InputBaseShouldHaveSourcePlatformWithValidLengthMessage)
            .WithSeverity(IInputBaseValidator<TInputBase>.InputBaseShouldHaveSourcePlatformWithValidLengthSeverity);

        ConfigureFluentValidationConcreteValidatorInternal(fluentValidationValidatorWrapper);
    }
    protected abstract void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper);
}
