using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator;
using FluentValidation;

namespace MCB.Core.Domain.Entities.Inputs.Validators
{
    public abstract class InputBaseValidator<TInputBase>
        : ValidatorBase<TInputBase>,
        Infra.CrossCutting.DesignPatterns.Validator.Abstractions.IValidator<TInputBase>
        where TInputBase : InputBase
    {
        // TenantId
        public static readonly string TenantIdIsRequiredErrorCode = nameof(TenantIdIsRequiredErrorCode);
        public static readonly string TenantIdIsRequiredMessage = nameof(TenantIdIsRequiredMessage);
        public static readonly Severity TenantIdIsRequiredSeverity = Severity.Error;

        // Execution User
        public static readonly string ExecutionUserIsRequiredErrorCode = nameof(ExecutionUserIsRequiredErrorCode);
        public static readonly string ExecutionUserIsRequiredMessage = nameof(ExecutionUserIsRequiredMessage);
        public static readonly Severity ExecutionUserIsRequiredSeverity = Severity.Error;

        public static readonly string ExecutionUserShouldLessThanOrEqualToMaximumLengthErrorCode = nameof(ExecutionUserShouldLessThanOrEqualToMaximumLengthErrorCode);
        public static readonly string ExecutionUserShouldLessThanOrEqualToMaximumLengthMessage = nameof(ExecutionUserShouldLessThanOrEqualToMaximumLengthMessage);
        public static readonly Severity ExecutionUserShouldLessThanOrEqualToMaximumLengthSeverity = Severity.Error;

        // Source Platform
        public static readonly string SourcePlatformIsRequiredErrorCode = nameof(SourcePlatformIsRequiredErrorCode);
        public static readonly string SourcePlatformIsRequiredMessage = nameof(SourcePlatformIsRequiredMessage);
        public static readonly Severity SourcePlatformIsRequiredSeverity = Severity.Error;

        public static readonly string SourcePlatformShouldLessThanOrEqualToMaximumLengthErrorCode = nameof(SourcePlatformShouldLessThanOrEqualToMaximumLengthErrorCode);
        public static readonly string SourcePlatformShouldLessThanOrEqualToMaximumLengthMessage = nameof(SourcePlatformShouldLessThanOrEqualToMaximumLengthMessage);
        public static readonly Severity SourcePlatformShouldLessThanOrEqualToMaximumLengthSeverity = Severity.Error;

        // Protected Methods
        protected override void ConfigureFluentValidationConcreteValidator(FluentValidationValidatorWrapper fluentValidationValidatorWrapper)
        {
            fluentValidationValidatorWrapper.RuleFor(input => input.TenantId)
                .Must(tenantId => tenantId != Guid.Empty)
                .WithErrorCode(TenantIdIsRequiredErrorCode)
                .WithMessage(TenantIdIsRequiredMessage)
                .WithSeverity(TenantIdIsRequiredSeverity);

            fluentValidationValidatorWrapper.RuleFor(input => input.ExecutionUser)
                .NotNull()
                .WithErrorCode(ExecutionUserIsRequiredErrorCode)
                .WithMessage(ExecutionUserIsRequiredMessage)
                .WithSeverity(ExecutionUserIsRequiredSeverity)
                .MaximumLength(150)
                .WithErrorCode(ExecutionUserShouldLessThanOrEqualToMaximumLengthErrorCode)
                .WithMessage(ExecutionUserShouldLessThanOrEqualToMaximumLengthMessage)
                .WithSeverity(ExecutionUserShouldLessThanOrEqualToMaximumLengthSeverity);

            fluentValidationValidatorWrapper.RuleFor(input => input.SourcePlatform)
                .NotNull()
                .WithErrorCode(SourcePlatformIsRequiredErrorCode)
                .WithMessage(SourcePlatformIsRequiredMessage)
                .WithSeverity(SourcePlatformIsRequiredSeverity)
                .MaximumLength(150)
                .WithErrorCode(SourcePlatformShouldLessThanOrEqualToMaximumLengthErrorCode)
                .WithMessage(SourcePlatformShouldLessThanOrEqualToMaximumLengthMessage)
                .WithSeverity(SourcePlatformShouldLessThanOrEqualToMaximumLengthSeverity);

            ConfigureFluentValidationConcreteValidatorInternal(fluentValidationValidatorWrapper);
        }
        protected abstract void ConfigureFluentValidationConcreteValidatorInternal(FluentValidationValidatorWrapper fluentValidationValidatorWrapper);
    }
}
