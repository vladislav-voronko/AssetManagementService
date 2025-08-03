using FluentValidation;
using AssetManagementService.Application.Dto;

namespace AssetManagementService.Application.Validators
{
    public class CreateTradeRequestValidator : AbstractValidator<CreateTradeRequest>
    {
        public CreateTradeRequestValidator()
        {
            RuleFor(x => x.Amount)
                .GreaterThan(0);

            RuleFor(x => x.PriceAmount)
                .GreaterThan(0);

            RuleFor(x => x.PriceCurrency)
                .NotEmpty()
                .Length(3);

            RuleFor(x => x.Date)
                .LessThanOrEqualTo(DateTimeOffset.UtcNow);
        }
    }
}
