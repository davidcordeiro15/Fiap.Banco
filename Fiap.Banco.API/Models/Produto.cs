using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models;

public abstract class Produto
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int idProduto { get; set; }

    [Required]
    public string nmProduto { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public abstract bool PodeSerContratado(Cliente cliente, out string motivo);
}

public class Emprestimo : Produto
{
    public decimal ValorSolicitado { get; set; }
    public int Parcelas { get; set; }

    public override bool PodeSerContratado(Cliente cliente, out string motivo)
    {
        motivo = string.Empty;

        if (ValorSolicitado <= 0)
        {
            motivo = "Valor do empréstimo inválido.";
            return false;
        }

        if (Parcelas < 1 || Parcelas > 60)
        {
            motivo = "Quantidade de parcelas inválida.";
            return false;
        }

        if (cliente is PessoaFisica pf)
        {
            var idade = DateTime.UtcNow.Year - pf.DataNascimento.Year;
            if (pf.DataNascimento.Date > DateTime.UtcNow.Date.AddYears(-idade))
            {
                idade--;
            }

            if (idade < 18)
            {
                motivo = "Cliente PF menor de idade.";
                return false;
            }

            if (ValorSolicitado > 50000m)
            {
                motivo = "Valor acima do limite para Pessoa Física.";
                return false;
            }

            return true;
        }

        if (cliente is PessoaJuridica)
        {
            if (ValorSolicitado > 250000m)
            {
                motivo = "Valor acima do limite para Pessoa Jurídica.";
                return false;
            }

            return true;
        }

        motivo = "Tipo de cliente não suportado para empréstimo.";
        return false;
    }
}

public class MaquinaDeCartao : Produto
{
    public decimal VolumeMensalEstimado { get; set; }
    public decimal TaxaPercentual { get; set; }

    public override bool PodeSerContratado(Cliente cliente, out string motivo)
    {
        motivo = string.Empty;
        if (cliente is not PessoaJuridica)
        {
            motivo = "Máquina de cartão disponível somente para Pessoa Jurídica.";
            return false;
        }

        return VolumeMensalEstimado > 0 && TaxaPercentual >= 0;
    }
}

public class ReceberSalario : Produto
{
    public string? EmpresaConveniada { get; set; }
    public decimal SalarioMensal { get; set; }

    public override bool PodeSerContratado(Cliente cliente, out string motivo)
    {
        motivo = string.Empty;
        if (cliente is not PessoaFisica)
        {
            motivo = "Receber salário disponível somente para Pessoa Física.";
            return false;
        }

        if (string.IsNullOrWhiteSpace(EmpresaConveniada))
        {
            motivo = "Empresa conveniada é obrigatória.";
            return false;
        }

        return SalarioMensal >= 0;
    }
}
