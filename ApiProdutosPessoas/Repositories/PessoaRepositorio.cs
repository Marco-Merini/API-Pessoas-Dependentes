﻿using ApiProdutosPessoas.Data;
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

        public async Task<PessoaModel> AdicionarPessoa(PessoaModel pessoa)
        {
            // Gera um código único para a pessoa se for 0
            if (pessoa.Codigo == 0)
            {
                pessoa.Codigo = await CodeGenerator.GenerateUniquePessoaCode(_dbContext);
            }

            // Gera um CPF aleatório único se for nulo, vazio ou "string"
            if (string.IsNullOrEmpty(pessoa.CPF) || pessoa.CPF == "string")
            {
                pessoa.CPF = await CodeGenerator.GenerateUniqueCPF(_dbContext);
            }

            // Gera um CEP aleatório se for nulo, vazio ou "string"
            if (string.IsNullOrEmpty(pessoa.CEP) || pessoa.CEP == "string")
            {
                pessoa.CEP = CodeGenerator.GenerateRandomCEP();
            }

            // Verifica se a cidade existe e cria se necessário
            if (pessoa.CodigoIBGE == 0 && pessoa.Cidade != null)
            {
                // Criar a cidade primeiro
                var cidadeModel = pessoa.Cidade;
                if (cidadeModel.CodigoIBGE == 0)
                {
                    cidadeModel.CodigoIBGE = await CodeGenerator.GenerateUniqueIBGECode(_dbContext);
                }

                if (cidadeModel.CodigoPais == 0)
                {
                    cidadeModel.CodigoPais = CodeGenerator.GenerateRandomPaisCode();
                }

                // Verifica se a cidade já existe com esse nome e UF
                var cidadeExistente = await _dbContext.Cidades
                    .FirstOrDefaultAsync(c => c.Nome == cidadeModel.Nome && c.UF == cidadeModel.UF);

                if (cidadeExistente != null)
                {
                    pessoa.CodigoIBGE = cidadeExistente.CodigoIBGE;
                }
                else
                {
                    await _dbContext.Cidades.AddAsync(cidadeModel);
                    await _dbContext.SaveChangesAsync();
                    pessoa.CodigoIBGE = cidadeModel.CodigoIBGE;
                }
            }
            else if (pessoa.CodigoIBGE == 0)
            {
                throw new Exception("É necessário informar um código IBGE válido ou fornecer os dados da cidade.");
            }
            else
            {
                // Verifica se a cidade existe
                var cidadeExiste = await _dbContext.Cidades.AnyAsync(c => c.CodigoIBGE == pessoa.CodigoIBGE);
                if (!cidadeExiste)
                {
                    throw new Exception($"Cidade com o código IBGE {pessoa.CodigoIBGE} não foi encontrada.");
                }
            }

            // Limpa a propriedade de navegação antes de salvar
            var cidadeTemp = pessoa.Cidade;
            pessoa.Cidade = null;

            await _dbContext.Pessoas.AddAsync(pessoa);
            await _dbContext.SaveChangesAsync();

            // Carrega a propriedade de navegação
            pessoa.Cidade = await _dbContext.Cidades.FirstOrDefaultAsync(c => c.CodigoIBGE == pessoa.CodigoIBGE);

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
            if (pessoa.CodigoIBGE > 0)
            {
                var cidadeExiste = await _dbContext.Cidades.AnyAsync(c => c.CodigoIBGE == pessoa.CodigoIBGE);
                if (!cidadeExiste)
                {
                    throw new Exception($"Cidade com o código IBGE {pessoa.CodigoIBGE} não foi encontrada.");
                }
            }
            else if (pessoa.Cidade != null)
            {
                // Criar ou atualizar a cidade
                var cidadeModel = pessoa.Cidade;
                if (cidadeModel.CodigoIBGE == 0)
                {
                    cidadeModel.CodigoIBGE = await CodeGenerator.GenerateUniqueIBGECode(_dbContext);
                }

                if (cidadeModel.CodigoPais == 0)
                {
                    cidadeModel.CodigoPais = CodeGenerator.GenerateRandomPaisCode();
                }

                var cidadeExistente = await _dbContext.Cidades
                    .FirstOrDefaultAsync(c => c.Nome == cidadeModel.Nome && c.UF == cidadeModel.UF);

                if (cidadeExistente != null)
                {
                    pessoa.CodigoIBGE = cidadeExistente.CodigoIBGE;
                }
                else
                {
                    await _dbContext.Cidades.AddAsync(cidadeModel);
                    await _dbContext.SaveChangesAsync();
                    pessoa.CodigoIBGE = cidadeModel.CodigoIBGE;
                }
            }

            pessoaExistente.Nome = pessoa.Nome;
            pessoaExistente.Idade = pessoa.Idade;

            // Atualiza o CPF apenas se não for nulo, vazio ou "string"
            if (!string.IsNullOrEmpty(pessoa.CPF) && pessoa.CPF != "string")
            {
                pessoaExistente.CPF = pessoa.CPF;
            }

            pessoaExistente.Logradouro = pessoa.Logradouro;
            pessoaExistente.NumeroEstabelecimento = pessoa.NumeroEstabelecimento;
            pessoaExistente.Bairro = pessoa.Bairro;

            // Atualiza o CEP apenas se não for nulo, vazio ou "string"
            if (!string.IsNullOrEmpty(pessoa.CEP) && pessoa.CEP != "string")
            {
                pessoaExistente.CEP = pessoa.CEP;
            }

            if (pessoa.CodigoIBGE > 0)
            {
                pessoaExistente.CodigoIBGE = pessoa.CodigoIBGE;
            }

            _dbContext.Pessoas.Update(pessoaExistente);
            await _dbContext.SaveChangesAsync();

            // Atualiza a propriedade de navegação
            pessoaExistente.Cidade = await _dbContext.Cidades.FirstOrDefaultAsync(c => c.CodigoIBGE == pessoaExistente.CodigoIBGE);

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