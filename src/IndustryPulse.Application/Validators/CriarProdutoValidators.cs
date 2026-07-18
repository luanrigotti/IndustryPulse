using System;
using FluentValidation;
using IndustryPulse.Application.DTOs.Produtos;

namespace IndustryPulse.Application.Validators;

public class CriarProdutoValidators : AbstractValidator<CriarProdutoDTO>
{
    public CriarProdutoValidators()
    {
        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição do produto é obrigatória.")
            .MaximumLength(200).WithMessage("A descrição deve ter no máximo 200 caracteres.");

        RuleFor(x => x.UnidadeMedida)
            .NotEmpty().WithMessage("A unidade de medida é obrigatória.")
            .MaximumLength(10).WithMessage("A unidade de medida deve ter no máximo 10 caracteres.");

        RuleFor(x => x.TempoProducaoMinutos)
            .GreaterThan(0)
            .WithMessage("O tempo de produção deve ser maior que zero.");
    }

}
