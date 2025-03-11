using ApiProdutosPessoas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Repositories.Interfaces
{
    public interface InterfaceCidade
    {
        Task<List<CidadeModel>> BuscarTodasCidades();
        Task<CidadeModel> BuscarCidadePorCodigo(int codigoIBGE);
        Task<CidadeModel> AdicionarCidade(CidadeModel cidade);
        Task<CidadeModel> AtualizarCidade(CidadeModel cidade, int codigoIBGE);
        Task<bool> DeletarCidade(int codigoIBGE);
    }
}
