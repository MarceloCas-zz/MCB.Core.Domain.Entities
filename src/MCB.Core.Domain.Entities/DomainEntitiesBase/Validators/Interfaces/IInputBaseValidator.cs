using MCB.Core.Domain.Entities.DomainEntitiesBase.Inputs;
using MCB.Core.Infra.CrossCutting.DesignPatterns.Validator.Abstractions;

namespace MCB.Core.Domain.Entities.DomainEntitiesBase.Validators.Interfaces;

public interface IInputBaseValidator<TInputBase>
    : IValidator<TInputBase>
    where TInputBase : InputBase
{
}
