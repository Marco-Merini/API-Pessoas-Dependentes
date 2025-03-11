using ApiProdutosPessoas.Data.Map;
using ApiProdutosPessoas.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiProdutosPessoas.Data
{
    public class TESTEAPIPESSOASDEPENDENTES : DbContext
    {
        public TESTEAPIPESSOASDEPENDENTES(DbContextOptions<TESTEAPIPESSOASDEPENDENTES> options)
            : base(options)
        {
        }

        public DbSet<DependenteModel> PessoasDependentes { get; set; }
        public DbSet<PessoaModel> Pessoas { get; set; }
        public DbSet<CidadeModel> Cidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new CidadeMap());
            modelBuilder.ApplyConfiguration(new DependenteMap());
            base.OnModelCreating(modelBuilder);
        }
    }
}
