    using FluentValidation;
    using MobileStore.Application.DTOs.Brand;


namespace MobileStore.Application.Validators.BrandValidators
{
    public class UpdateBrandValidator : AbstractValidator<UpdateBrandDto>
    {
        public UpdateBrandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Brand name is required")
                .Length(2, 100).WithMessage("Brand name must be between 2 and 100 characters");

            RuleFor(x => x.Logo)
                .MaximumLength(255).WithMessage("Logo URL cannot exceed 255 characters");

            RuleFor(x => x.IsActive)
                .NotNull().WithMessage("IsActive status is required");
        }
    }
}