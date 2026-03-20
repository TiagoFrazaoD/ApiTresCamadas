using DevIO.Business.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DevIO.Data.Mappings
{
    public class FornecedorMapping : IEntityTypeConfiguration<Fornecedor>
    {
        public void Configure(EntityTypeBuilder<Fornecedor> builder)
        {
            builder.HasKey(f => f.Id);

            builder.Property(f => f.Nome)
                .IsRequired()
                .HasColumnType("varchar(200)");

            builder.Property(f => f.Documento)
                .IsRequired()
                .HasColumnType("varchar(14)");

            builder.HasOne(e => e.Endereco)
                .WithOne(f => f.Fornecedor);    

            builder.HasMany(p => p.Produtos)
                .WithOne(f => f.Fornecedor)
                .HasForeignKey(f => f.FornecedorId);

            builder.ToTable("Fornecedores");
        }
    }
}
