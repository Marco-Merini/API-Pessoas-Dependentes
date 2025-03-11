using ApiProdutosPessoas.Data;
using ApiProdutosPessoas.Models;
using ApiProdutosPessoas.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Repositories
{
    public class PessoaRepositorio : InterfacePessoa
    {
        private readonly TESTEAPIPESSOASDEPENDENTES _dbContext;
        private readonly Random _random = new Random();

        public PessoaRepositorio(TESTEAPIPESSOASDEPENDENTES dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<PessoaModel>> BuscarTodasPessoas()
        {
            return await _dbContext.Pessoas
                .Include(p => p.Cidade)
                .ToListAsync();
        }

        public async Task<List<PessoaModel>> BuscarPessoasPorNome(string nome)
        {
            return await _dbContext.Pessoas
                .Include(p => p.Cidade)
                .Where(p => p.Nome.Contains(nome))
                .ToListAsync();
        }

        public async Task<List<PessoaModel>> BuscarPessoasPorCidade(int codigoCidade)
        {
            return await _dbContext.Pessoas
                .Include(p => p.Cidade)
                .Where(p => p.CodigoIBGE == codigoCidade)
                .ToListAsync();
        }

        public async Task<PessoaModel> BuscarPessoaPorCodigo(int codigo)
        {
            var pessoa = await _dbContext.Pessoas
                .Include(p => p.Cidade)
                .FirstOrDefaultAsync(p => p.Codigo == codigo);

            return pessoa;
        }

        private async Task<int> GerarCodigoUnico()
        {
            int codigo;
            bool existe;

            do
            {
                codigo = _random.Next(100000, 1000000);
                existe = await _dbContext.Pessoas.AnyAsync(p => p.Codigo == codigo);
            } while (existe);

            return codigo;
        }

        public async Task<PessoaModel> AdicionarPessoa(PessoaModel pessoa)
        {

            // Gera um código único para a pessoa
            pessoa.Codigo = await GerarCodigoUnico();

            await _dbContext.Pessoas.AddAsync(pessoa);
            await _dbContext.SaveChangesAsync();

            return pessoa;
        }

        public async Task<PessoaModel> AtualizarPessoa(PessoaModel pessoa, int codigo)
        {
            var pessoaExistente = await _dbContext.Pessoas.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (pessoaExistente == null)
            {
                throw new Exception($"Pessoa com o código {codigo} não foi encontrada.");
            }

            // Verifica se a cidade existe
            var cidadeExiste = await _dbContext.Cidades.AnyAsync(c => c.CodigoIBGE == pessoa.CodigoIBGE);
            if (!cidadeExiste)
            {
                throw new Exception($"Cidade com o código IBGE {pessoa.CodigoIBGE} não foi encontrada.");
            }

            pessoaExistente.Nome = pessoa.Nome;
            pessoaExistente.Idade = pessoa.Idade;
            pessoaExistente.CPF = pessoa.CPF;
            pessoaExistente.Logradouro = pessoa.Logradouro;
            pessoaExistente.NumeroEstabelecimento = pessoa.NumeroEstabelecimento;
            pessoaExistente.Bairro = pessoa.Bairro;
            pessoaExistente.CEP = pessoa.CEP;
            pessoaExistente.CodigoIBGE = pessoa.CodigoIBGE;

            _dbContext.Pessoas.Update(pessoaExistente);
            await _dbContext.SaveChangesAsync();

            // Atualiza a propriedade de navegação
            pessoaExistente.Cidade = await _dbContext.Cidades.FirstOrDefaultAsync(c => c.CodigoIBGE == pessoa.CodigoIBGE);

            return pessoaExistente;
        }

        public async Task<bool> DeletarPessoa(int codigo)
        {
            var pessoa = await _dbContext.Pessoas.FirstOrDefaultAsync(p => p.Codigo == codigo);
            if (pessoa == null)
            {
                throw new Exception($"Pessoa com o código {codigo} não foi encontrada.");
            }

            _dbContext.Pessoas.Remove(pessoa);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}