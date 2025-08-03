using AssetManagementService.Application.Dto;
using FluentValidation;

namespace AssetManagementService.Application.Validators
{
    public class CreateReplenishmentRequestValidator : AbstractValidator<CreateReplenishmentRequest>
    {
        public CreateReplenishmentRequestValidator()
        {
            RuleFor(x => x.Amount)
            .GreaterThan(0);

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow);

            RuleFor(x => x.Note)
                .MaximumLength(500)
                .When(x => !string.IsNullOrEmpty(x.Note));
        }
    }
}
