using System;
using FluentValidation;
using IndustryPulse.Application.DTOs.Ordens;

namespace IndustryPulse.Application.Validators;

public class RegistrarParadaValidator : AbstractValidator<RegistrarParadaDTO>
{
    public RegistrarParadaValidator()
    {
        RuleFor(x => x.Motivo)
            .IsInEnum()
            .WithMessage("Motivo de parada inválido.");

        RuleFor(x => x.Descricao)
            .MaximumLength(500)
            .WithMessage("A descrição deve ter no máximo 500 caracteres.")
            .When(x => x.Descricao != null);
    }
}
