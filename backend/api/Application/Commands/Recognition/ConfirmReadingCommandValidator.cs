using FluentValidation;

namespace MeterVision.Api.Application.Commands.Recognition;

/// <summary>
/// Валидатор команды подтверждения показания счётчика.
/// </summary>
/// <remarks>
/// Проверяет бизнес-правила, которые должны выполняться до обработки команды:
/// неотрицательное значение показания и непустой серийный номер.
/// Регистрируется автоматически через <c>AddValidatorsFromAssemblyContaining</c>
/// и вызывается из <c>ValidationBehavior</c> в конвейере Mediator.
/// </remarks>
public class ConfirmReadingCommandValidator : AbstractValidator<ConfirmCommand>
{
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="ConfirmReadingCommandValidator"/>
    /// и настраивает правила валидации.
    /// </summary>
    public ConfirmReadingCommandValidator()
    {
        // Значение показания не может быть отрицательным:
        // счётчики не поддерживают обратный ход, отрицательное значение — признак ошибки распознавания.
        RuleFor(x => x.Value)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Значение показания не может быть отрицательным.");

        // Серийный номер обязателен: без него невозможно сопоставить показание со счётчиком.
        RuleFor(x => x.SerialNumber)
            .NotEmpty()
            .WithMessage("Серийный номер обязателен.");
    }
}
