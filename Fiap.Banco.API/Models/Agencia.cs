using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models;

public class Agencia
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int idAgencia { get; set; }

    [Required]
    public string nmEndereco { get; set; } = string.Empty;

    [Required]
    public string cep { get; set; } = string.Empty;

    public ICollection<Cliente> Clientes { get; set; } = new List<Cliente>();
    public ICollection<Contratacao> Contratacoes { get; set; } = new List<Contratacao>();
}
