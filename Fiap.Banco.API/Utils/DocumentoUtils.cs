using System.Text.RegularExpressions;

namespace Fiap.Banco.API.Utils;

public static class DocumentoUtils
{

    public static string SomenteNumeros(string value)
        => Regex.Replace(value ?? string.Empty, "\\D", string.Empty);

    public static bool CpfValido(string cpf)
    {
        cpf = SomenteNumeros(cpf);
        return cpf.Length == 11 && cpf.Distinct().Count() > 1;
    }

    public static bool CnpjValido(string cnpj)
    {
        cnpj = SomenteNumeros(cnpj);
        return cnpj.Length == 14 && cnpj.Distinct().Count() > 1;
    }
}
