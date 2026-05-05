using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models;

public abstract class Cliente
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int idCliente { get; set; }

    [Required]
    public string nmCliente { get; set; } = string.Empty;

    [Required]
    public int idAgencia { get; set; }

    public Agencia? Agencia { get; set; }

    public ICollection<Contratacao> Contratacoes { get; set; } = new List<Contratacao>();
}

public class PessoaFisica : Cliente
{
    [Required]
    public string CPF { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; }
}

public class PessoaJuridica : Cliente
{
    [Required]
    public string CNPJ { get; set; } = string.Empty;

    [Required]
    public string RazaoSocial { get; set; } = string.Empty;
}
