using $safeprojectname$.RequestDto.Example;
using FluentValidation;

namespace $safeprojectname$.Validators
{
    public class ExampleValidator : AbstractValidator<ExampleDto>
    {
        public ExampleValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("El campo Name es requerido.");
            RuleFor(x => x.Id).NotEmpty().WithMessage("El campo Id es requerido.");
        }
    }
}
