using CaseLocaliza.Models.Abstractions;
using CaseLocaliza.Notifications;
using CaseLocaliza.Notifications.Abstractions;
using FluentValidation;
using FluentValidation.Results;

namespace CaseLocaliza.Services.Abstractions;

public interface IBaseService;

public abstract class BaseService : IBaseService
{
    private readonly INotifier _notificador;

    protected BaseService(INotifier notificador)
    {
        _notificador = notificador;
    }

    protected void Notify(ValidationResult validationResult)
    {
        if (validationResult.Errors.Any())
        {
            foreach (var error in validationResult.Errors)
            {
                Notify(error.ErrorMessage);
            }
        }
    }

    protected void Notify(string message)
    {
        _notificador.Add(new Notification(message));
        new ValidationResult().Errors.Add(new ValidationFailure("notify", message));
    }

    protected bool ExecuteValidation<TV, TE>(TV validacao, TE entidade)
            where TV : AbstractValidator<TE>
            where TE : Entity
    {
        var validator = validacao.Validate(entidade);

        if (!validator.IsValid)
        {
            var erros = validator.Errors.Select(x => x.ErrorMessage);
            foreach (var erro in erros)
            {
                Notify(erro);
            }
            return false;
        }

        return true;
    }
}
