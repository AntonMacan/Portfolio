using FluentValidation;
using IzvidaciWebApp.Domain.Models;
using IzvidaciWebApp.ViewModels;

namespace IzvidaciWebApp.ModelsValidation
{
  public class SkolaViewModelValidator : AbstractValidator<SkolaViewModel>
  {
    public SkolaViewModelValidator()
    {
            RuleFor(d => d.NazivSkole)
              .NotEmpty().WithMessage("Naziv škole je obvezno polje");        

      
    }
  }
}
