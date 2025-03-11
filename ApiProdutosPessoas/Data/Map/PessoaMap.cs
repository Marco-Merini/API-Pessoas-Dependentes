using ApiProdutosPessoas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Data.Map
{
    public class PessoaMap : IEntityTypeConfiguration<PessoaModel>
    {
        public void Configure(EntityTypeBuilder<PessoaModel> builder)
        {
            builder.HasKey(x => x.Codigo);
            builder.Property(x => x.Nome).IsRequired().HasMaxLength(255);
            builder.Property(x => x.Idade).IsRequired();
            builder.Property(x => x.CPF).IsRequired().HasMaxLength(14);
            builder.Property(x => x.Logradouro).IsRequired().HasMaxLength(255);
            builder.Property(x => x.NumeroEstabelecimento).IsRequired().HasMaxLength(10);
            builder.Property(x => x.Bairro).IsRequired().HasMaxLength(100);
            builder.Property(x => x.CEP).IsRequired().HasMaxLength(10);

            builder.HasOne(x => x.Cidade)
                   .WithMany()
                   .HasForeignKey(x => x.CodigoIBGE);
        }
    }
}
