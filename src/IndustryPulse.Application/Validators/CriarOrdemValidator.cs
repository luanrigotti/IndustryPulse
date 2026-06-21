using System;
using FluentValidation;
using IndustryPulse.Application.DTOs.Ordens;

namespace IndustryPulse.Application.Validators;

public class CriarOrdemValidator : AbstractValidator<CriarOrdemDTO>
{
    public CriarOrdemValidator()
    {
        RuleFor(x => x.ProdutoId)
            .GreaterThan(0)
            .WithMessage("ProdutoId deve ser um valor válido.");

        RuleFor(x => x.LinhaProducaoId)
            .GreaterThan(0)
            .WithMessage("LinhaProducaoId deve ser um valor válido.");

        RuleFor(x => x.QuantidadePlanejada)
            .GreaterThan(0)
            .WithMessage("A quantidade planejada deve ser maior que zero");

        RuleFor(x => x.DataPrevisao)
            .GreaterThan(DateTime.UtcNow)
            .WithMessage("A data de previsão deve ser uma data futura");
    }
}
