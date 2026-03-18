using DevIO.Business.Models.Validation.Documentos;
using FluentValidation;

namespace DevIO.Business.Models.Validation
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {
        public FornecedorValidation()
        {
            RuleFor(c => c.Nome).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido.")
                .Length(2, 100).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres.");

            When(c => c.TipoFornecedor == TipoFornecedor.PessoaFisica, () =>
            {
                RuleFor(c => c.Documento.Length).Equal(CpfValidacao.TamanhoCpf)
                .WithMessage("O campo {PropertyName} precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                RuleFor(c => CpfValidacao.Validar(c.Documento)).Equal(true)
                    .WithMessage("O campo {PropertyName} é inválido.");

            });
            When(c => c.TipoFornecedor == TipoFornecedor.PessoaJuridica, () =>
            {
                RuleFor(c => c.Documento.Length).Equal(CnpjValidacao.TamanhoCnpj)
                    .WithMessage("O campo {PropertyName} precisa ter {ComparisonValue} caracteres e foi fornecido {PropertyValue}.");

                RuleFor(c => CnpjValidacao.Validar(c.Documento)).Equal(true)
                    .WithMessage("O campo {PropertyName} é inválido.");
            });
        }
    }
}
