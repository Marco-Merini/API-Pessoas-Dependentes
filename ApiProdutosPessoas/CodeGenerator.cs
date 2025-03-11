// Utilities/CodeGenerator.cs
using ApiProdutosPessoas.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using System.Threading.Tasks;

public static class CodeGenerator
{
    private static readonly Random _random = new Random();

    // Gera um código aleatório de 5 dígitos para Pessoa
    public static int GenerateRandomPessoaCode()
    {
        return _random.Next(10000, 100000);
    }

    // Gera um código aleatório de 7 dígitos para IBGE
    public static int GenerateRandomIBGECode()
    {
        return _random.Next(1000000, 10000000);
    }

    // Gera um código aleatório de 3 dígitos para País
    public static int GenerateRandomPaisCode()
    {
        return 55;
    }

    // Gera um CPF aleatório formatado
    public static string GenerateRandomCPF()
    {
        StringBuilder cpf = new StringBuilder();

        // Gera os 9 primeiros dígitos
        for (int i = 0; i < 9; i++)
        {
            cpf.Append(_random.Next(0, 10));
        }

        // Adiciona os dígitos verificadores (simplificados para este exemplo)
        cpf.Append(_random.Next(0, 10));
        cpf.Append(_random.Next(0, 10));

        // Formata o CPF (XXX.XXX.XXX-XX)
        return $"{cpf.ToString().Substring(0, 3)}.{cpf.ToString().Substring(3, 3)}.{cpf.ToString().Substring(6, 3)}-{cpf.ToString().Substring(9, 2)}";
    }

    // Gera um CEP aleatório formatado
    public static string GenerateRandomCEP()
    {
        int cepNumber = _random.Next(10000000, 100000000);
        return $"{cepNumber.ToString().Substring(0, 5)}-{cepNumber.ToString().Substring(5, 3)}";
    }

    // Verifica se o código de pessoa já existe no banco de dados
    public static async Task<int> GenerateUniquePessoaCode(TESTEAPIPESSOASDEPENDENTES dbContext)
    {
        int code;
        bool exists;

        do
        {
            code = GenerateRandomPessoaCode();
            exists = await dbContext.Pessoas.AnyAsync(p => p.Codigo == code);
        } while (exists);

        return code;
    }

    // Verifica se o código de IBGE já existe no banco de dados
    public static async Task<int> GenerateUniqueIBGECode(TESTEAPIPESSOASDEPENDENTES dbContext)
    {
        int code;
        bool exists;

        do
        {
            code = GenerateRandomIBGECode();
            exists = await dbContext.Cidades.AnyAsync(c => c.CodigoIBGE == code);
        } while (exists);

        return code;
    }

    // Verifica se o CPF já existe no banco de dados
    public static async Task<string> GenerateUniqueCPF(TESTEAPIPESSOASDEPENDENTES dbContext)
    {
        string cpf;
        bool exists;

        do
        {
            cpf = GenerateRandomCPF();
            exists = await dbContext.Pessoas.AnyAsync(p => p.CPF == cpf);
        } while (exists);

        return cpf;
    }
}