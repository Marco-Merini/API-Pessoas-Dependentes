using ApiProdutosPessoas.Data;
using ApiProdutosPessoas.Models;
using ApiProdutosPessoas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Repositories
{
    public class CidadeRepositorio : InterfaceCidade
    {
        private readonly TESTEAPIPESSOASDEPENDENTES _dbContext;

        public CidadeRepositorio(TESTEAPIPESSOASDEPENDENTES dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<CidadeModel>> BuscarTodasCidades()
        {
            return await _dbContext.Cidades.ToListAsync();
        }

        public async Task<CidadeModel> BuscarCidadePorCodigo(int codigoIBGE)
        {
            return await _dbContext.Cidades.FirstOrDefaultAsync(c => c.CodigoIBGE == codigoIBGE);
        }

        public async Task<CidadeModel> AdicionarCidade(CidadeModel cidade)
        {
            // Gera um código IBGE único para a cidade se não for fornecido
            if (cidade.CodigoIBGE == 0)
            {
                cidade.CodigoIBGE = await CodeGenerator.GenerateUniqueIBGECode(_dbContext);
            }
            else
            {
                var cidadeExiste = await _dbContext.Cidades.AnyAsync(c => c.CodigoIBGE == cidade.CodigoIBGE);
                if (cidadeExiste)
                {
                    throw new Exception($"Já existe uma cidade com o código IBGE {cidade.CodigoIBGE}.");
                }
            }

            // Gera um código de país se não for fornecido
            if (cidade.CodigoPais == 0)
            {
                cidade.CodigoPais = CodeGenerator.GenerateRandomPaisCode();
            }

            await _dbContext.Cidades.AddAsync(cidade);
            await _dbContext.SaveChangesAsync();

            return cidade;
        }

        public async Task<CidadeModel> AtualizarCidade(CidadeModel cidade, int codigoIBGE)
        {
            var cidadeExistente = await _dbContext.Cidades.FirstOrDefaultAsync(c => c.CodigoIBGE == codigoIBGE);
            if (cidadeExistente == null)
            {
                throw new Exception($"Cidade com o código IBGE {codigoIBGE} não foi encontrada.");
            }

            cidadeExistente.Nome = cidade.Nome;
            cidadeExistente.UF = cidade.UF;

            // Atualiza o código do país se for fornecido
            if (cidade.CodigoPais > 0)
            {
                cidadeExistente.CodigoPais = cidade.CodigoPais;
            }

            _dbContext.Cidades.Update(cidadeExistente);
            await _dbContext.SaveChangesAsync();

            return cidadeExistente;
        }

        public async Task<bool> DeletarCidade(int codigoIBGE)
        {
            var cidade = await _dbContext.Cidades.FirstOrDefaultAsync(c => c.CodigoIBGE == codigoIBGE);
            if (cidade == null)
            {
                throw new Exception($"Cidade com o código IBGE {codigoIBGE} não foi encontrada.");
            }

            // Verifica se existem pessoas vinculadas a esta cidade
            var pessoasVinculadas = await _dbContext.Pessoas.AnyAsync(p => p.CodigoIBGE == codigoIBGE);
            if (pessoasVinculadas)
            {
                throw new Exception($"Não é possível excluir a cidade pois existem pessoas vinculadas a ela.");
            }

            _dbContext.Cidades.Remove(cidade);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}