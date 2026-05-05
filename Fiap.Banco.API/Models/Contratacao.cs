using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fiap.Banco.API.Enums;

namespace Fiap.Banco.API.Models;

public class Contratacao
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int idContratacao { get; set; }

    [Required]
    public int idCliente { get; set; }

    public Cliente? Cliente { get; set; }

    [Required]
    public int idAgencia { get; set; }

    public Agencia? Agencia { get; set; }

    [Required]
    public int idProduto { get; set; }

    public Produto? Produto { get; set; }

    [Required]
    public StatusContratacao Status { get; set; }

    [Required]
    public string TipoProduto { get; set; } = string.Empty;

    public string? MensagemProcessamento { get; set; }

    public int Tentativas { get; set; }

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public DateTime? DataAtualizacao { get; set; }
}
