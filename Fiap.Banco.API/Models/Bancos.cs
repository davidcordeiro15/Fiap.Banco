using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fiap.Banco.API.Models;

public class Bancos
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int idBanco { get; set; }

    [Required]
    public string nomeBanco { get; set; } = string.Empty;

    public DateTime dtCriacao { get; set; }

    [Required]
    public string CEP { get; set; } = string.Empty;
}
