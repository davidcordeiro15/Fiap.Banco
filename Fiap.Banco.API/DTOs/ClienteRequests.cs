namespace Fiap.Banco.API.DTOs;

public record ClientePFRequest(string nmCliente, string CPF, DateTime DataNascimento, int idAgencia);
public record ClientePJRequest(string nmCliente, string CNPJ, string RazaoSocial, int idAgencia);
