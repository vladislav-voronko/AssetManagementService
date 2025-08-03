using AssetManagementService.Application.Dto;
using FluentValidation;

public class CreateAssetRequestValidator : AbstractValidator<CreateAssetRequest>
{
    public CreateAssetRequestValidator()
    {
        RuleFor(x => x.Symbol)
            .NotEmpty()
            .MaximumLength(10);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(100);
    }
}
