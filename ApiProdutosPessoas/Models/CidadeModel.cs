using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Models
{
    public class CidadeModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodigoIBGE { get; set; }

        [Required(ErrorMessage = "O nome da cidade é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A UF é obrigatória")]
        public string UF { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CodigoPais { get; set; }
    }
}
