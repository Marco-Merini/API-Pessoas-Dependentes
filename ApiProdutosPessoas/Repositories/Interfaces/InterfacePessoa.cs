using ApiProdutosPessoas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Repositories.Interfaces
{
    public interface InterfacePessoa
    {
        Task<List<PessoaModel>> BuscarTodasPessoas();
        Task<List<PessoaModel>> BuscarPessoasPorNome(string nome);
        Task<List<PessoaModel>> BuscarPessoasPorCidade(int codigoCidade);
        Task<PessoaModel> BuscarPessoaPorCodigo(int codigo);
        Task<PessoaModel> AdicionarPessoa(PessoaModel pessoa);
        Task<PessoaModel> AtualizarPessoa(PessoaModel pessoa, int codigo);
        Task<bool> DeletarPessoa(int codigo);
        Task<DependenteModel> VincularDependente(int codigoPessoa, int codigoDependente);
        Task<bool> DesvincularDependente(int codigoPessoa, int codigoDependente);
    }
}
