using System;
using FluentValidation;
using FluentValidation.Validators;
using IndustryPulse.Application.DTOs.Linhas;

namespace IndustryPulse.Application.Validators;

public class CriarLinhaValidator : AbstractValidator<CriarLinhaDTO>
{
    public CriarLinhaValidator()
    {
        RuleFor(x => x.Nome)
            .NotEmpty().WithMessage("O nome da linha é obrigatório.")
            .MaximumLength(100).WithMessage("O nome deve ter no máximo 100 caracteres.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição da linha é obrigatória.")
            .MaximumLength(300).WithMessage("A descrição deve ter no máximo 300 caracteres.");

        RuleFor(x => x.CapacidadeHora)
            .GreaterThan(0)
            .WithMessage("A capacidade por hora deve ser maior que zero.");
    }
}
