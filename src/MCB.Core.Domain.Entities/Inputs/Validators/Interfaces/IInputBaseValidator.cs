using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;

namespace MCB.Core.Domain.Entities.Inputs.Validators.Interfaces
{
    public interface IInputBaseValidator<TInputBase>
        : IValidator<TInputBase>
        where TInputBase : InputBase
    {
    }
}
