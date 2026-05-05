using Fiap.Banco.API.Enums;

namespace Fiap.Banco.API.DTOs;

public record ContratacaoCreateRequest(
    int idCliente,
    TipoProduto TipoProduto,
    decimal ValorSolicitado,
    int Parcelas,
    string? EmpresaConveniada,
    decimal? SalarioMensal,
    decimal? VolumeMensalEstimado,
    decimal? TaxaPercentual
);
