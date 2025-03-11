using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Models
{
    public class DependenteModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodigoPessoa { get; set; }

        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodigoDependente { get; set; }

        [ForeignKey("CodigoPessoa")]
        public PessoaModel Pessoa { get; set; }

        [ForeignKey("CodigoDependente")]
        public PessoaModel Dependente { get; set; }
    }
}
