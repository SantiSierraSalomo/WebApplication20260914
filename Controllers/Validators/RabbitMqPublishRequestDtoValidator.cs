using FluentValidation;

namespace WebApplication20260914.Controllers.Validators;

public sealed class RabbitMqPublishRequestDtoValidator : AbstractValidator<RabbitMqPublishRequestDto>
{
    public RabbitMqPublishRequestDtoValidator()
    {
        RuleFor(x => x.Header1)
            .NotEmpty();

        RuleFor(x => x.Header2)
            .NotEmpty();

        RuleFor(x => x.UserId)
            .NotEmpty();

        RuleFor(x => x.AppId)
            .NotEmpty();

        RuleFor(x => x.Body)
            .NotEmpty();
    }
}
