using ApiProdutosPessoas.Models;
using ApiProdutosPessoas.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly InterfacePessoa _pessoaRepositorio;

        public PessoaController(InterfacePessoa pessoaRepositorio)
        {
            _pessoaRepositorio = pessoaRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<PessoaModel>>> BuscarTodasPessoas()
        {
            List<PessoaModel> pessoas = await _pessoaRepositorio.BuscarTodasPessoas();
            return Ok(pessoas);
        }

        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<List<PessoaModel>>> BuscarPessoasPorNome(string nome)
        {
            List<PessoaModel> pessoas = await _pessoaRepositorio.BuscarPessoasPorNome(nome);
            return Ok(pessoas);
        }

        [HttpGet("cidade/{codigoCidade}")]
        public async Task<ActionResult<List<PessoaModel>>> BuscarPessoasPorCidade(int codigoCidade)
        {
            List<PessoaModel> pessoas = await _pessoaRepositorio.BuscarPessoasPorCidade(codigoCidade);
            return Ok(pessoas);
        }

        [HttpGet("{codigo}")]
        public async Task<ActionResult<PessoaModel>> BuscarPessoaPorCodigo(int codigo)
        {
            PessoaModel pessoa = await _pessoaRepositorio.BuscarPessoaPorCodigo(codigo);

            if (pessoa == null)
            {
                return NotFound($"Pessoa com código {codigo} não encontrada.");
            }

            return Ok(pessoa);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaModel>> AdicionarPessoa([FromBody] PessoaModel pessoaModel)
        {
            try
            {
                // O Codigo, CPF, CEP, CodigoIBGE e CodigoPais serão gerados automaticamente pelo repositório
                // quando necessário - até mesmo quando são passados valores como "string" ou 0
                PessoaModel pessoa = await _pessoaRepositorio.AdicionarPessoa(pessoaModel);
                return CreatedAtAction(nameof(BuscarPessoaPorCodigo), new { codigo = pessoa.Codigo }, pessoa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{codigo}")]
        public async Task<ActionResult<PessoaModel>> AtualizarPessoa([FromBody] PessoaModel pessoaModel, int codigo)
        {
            try
            {
                pessoaModel.Codigo = codigo;
                PessoaModel pessoa = await _pessoaRepositorio.AtualizarPessoa(pessoaModel, codigo);
                return Ok(pessoa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{codigo}")]
        public async Task<ActionResult<bool>> DeletarPessoa(int codigo)
        {
            try
            {
                bool deletado = await _pessoaRepositorio.DeletarPessoa(codigo);
                return Ok(deletado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}