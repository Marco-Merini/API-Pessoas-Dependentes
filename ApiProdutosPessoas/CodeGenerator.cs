// Utilities/CodeGenerator.cs
using ApiProdutosPessoas.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

public static class CodeGenerator
{
    private static readonly Random _random = new Random();

    // Gera um código aleatório de 6 dígitos
    public static int GenerateRandomCode()
    {
        return _random.Next(100000, 1000000);
    }

    // Verifica se o código já existe no banco de dados
    public static async Task<int> GenerateUniquePessoaCode(TESTEAPIPESSOASDEPENDENTES dbContext)
    {
        int code;
        bool exists;

        do
        {
            code = GenerateRandomCode();
            exists = await dbContext.Pessoas.AnyAsync(p => p.Codigo == code);
        } while (exists);

        return code;
    }

    // Verifica se o código de marca já existe no banco de dados
    public static async Task<int> GenerateUniqueCidadeCode(TESTEAPIPESSOASDEPENDENTES dbContext)
    {
        int code;
        bool exists;

        do
        {
            code = GenerateRandomCode();
            exists = await dbContext.Cidades.AnyAsync(m => m.CodigoIBGE == code);
        } while (exists);

        return code;
    }

    public static async Task<int> GenerateUniqueDependenteCode(TESTEAPIPESSOASDEPENDENTES dbContext)
    {
        int code;
        bool exists;

        do
        {
            code = GenerateRandomCode();
            exists = await dbContext.PessoasDependentes.AnyAsync(m => m.Id == code);
        } while (exists);

        return code;
    }
}