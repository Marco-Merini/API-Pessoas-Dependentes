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
    public class CidadeController : ControllerBase
    {
        private readonly InterfaceCidade _cidadeRepositorio;

        public CidadeController(InterfaceCidade cidadeRepositorio)
        {
            _cidadeRepositorio = cidadeRepositorio;
        }

        [HttpGet]
        public async Task<ActionResult<List<CidadeModel>>> BuscarTodasCidades()
        {
            List<CidadeModel> cidades = await _cidadeRepositorio.BuscarTodasCidades();
            return Ok(cidades);
        }

        [HttpGet("{codigoIBGE}")]
        public async Task<ActionResult<CidadeModel>> BuscarCidadePorCodigo(int codigoIBGE)
        {
            CidadeModel cidade = await _cidadeRepositorio.BuscarCidadePorCodigo(codigoIBGE);

            if (cidade == null)
            {
                return NotFound($"Cidade com código IBGE {codigoIBGE} não encontrada.");
            }

            return Ok(cidade);
        }

        [HttpPost]
        public async Task<ActionResult<CidadeModel>> AdicionarCidade([FromBody] CidadeModel cidadeModel)
        {
            try
            {
                // O CodigoIBGE e CodigoPais serão gerados automaticamente pelo repositório se não forem fornecidos
                CidadeModel cidade = await _cidadeRepositorio.AdicionarCidade(cidadeModel);
                return CreatedAtAction(nameof(BuscarCidadePorCodigo), new { codigoIBGE = cidade.CodigoIBGE }, cidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{codigoIBGE}")]
        public async Task<ActionResult<CidadeModel>> AtualizarCidade([FromBody] CidadeModel cidadeModel, int codigoIBGE)
        {
            try
            {
                cidadeModel.CodigoIBGE = codigoIBGE;
                CidadeModel cidade = await _cidadeRepositorio.AtualizarCidade(cidadeModel, codigoIBGE);
                return Ok(cidade);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{codigoIBGE}")]
        public async Task<ActionResult<bool>> DeletarCidade(int codigoIBGE)
        {
            try
            {
                bool deletado = await _cidadeRepositorio.DeletarCidade(codigoIBGE);
                if (!deletado)
                {
                    return NotFound($"Cidade com código IBGE {codigoIBGE} não encontrada.");
                }
                return Ok(deletado);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}