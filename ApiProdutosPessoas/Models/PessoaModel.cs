using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Models
{
    public class PessoaModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Codigo { get; set; }

        [Required(ErrorMessage = "O nome da pessoa é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A idade da pessoa é obrigatória")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O CPF da pessoa é obrigatório")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O logradouro é obrigatório")]
        public string Logradouro { get; set; }

        [Required(ErrorMessage = "O número do estabelecimento é obrigatório")]
        public string NumeroEstabelecimento { get; set; }

        [Required(ErrorMessage = "O bairro é obrigatório")]
        public string Bairro { get; set; }

        [Required(ErrorMessage = "O CEP é obrigatório")]
        public string CEP { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodigoIBGE { get; set; }

        public CidadeModel Cidade { get; set; }

        public List<DependenteModel> Dependentes { get; set; }
    }
}
