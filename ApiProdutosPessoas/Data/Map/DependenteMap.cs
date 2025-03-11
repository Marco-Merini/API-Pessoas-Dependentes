using ApiProdutosPessoas.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Data.Map
{
    public class DependenteMap : IEntityTypeConfiguration<DependenteModel>
    {
        public void Configure(EntityTypeBuilder<DependenteModel> builder)
        {
            builder.ToTable("Dependentes");

            builder.HasKey(d => d.Id);

            builder.HasOne(d => d.Pessoa)
                .WithMany(p => p.Dependentes)
                .HasForeignKey(d => d.CodigoPessoa)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Dependente)
                .WithMany()
                .HasForeignKey(d => d.CodigoDependente)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
