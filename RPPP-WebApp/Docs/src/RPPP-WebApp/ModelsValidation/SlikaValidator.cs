using System.Data;
using FluentValidation;
using RPPP_WebApp.Models;
using RPPP_WebApp.ViewModels;

namespace RPPP_WebApp.ModelsValidation;

public class SlikaValidator : AbstractValidator<Slike>
{
    public SlikaValidator()
    {
        RuleFor(s => s.Datum).NotEmpty().WithMessage("Potrebno unijeti datum");
        RuleFor(s => s.Smjer).NotEmpty().WithMessage("Potrebno unijeti smjer");
        RuleFor(s => s.Url).NotEmpty().WithMessage("Potrebno unijeti URL");
    }
}