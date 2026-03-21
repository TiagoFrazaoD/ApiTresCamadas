using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace DevIO.Data.Repository
{
    public class FornecedorRepository : Repository<Fornecedor>, IFornecedorRepository
    {
        public FornecedorRepository(MeuDbContext DbContext) : base(DbContext) { }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            var fornecedor = await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == id);

            if (fornecedor == null) throw new KeyNotFoundException($"Fornecedor com ID {id} não encontrado.");

            return fornecedor;
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid fornecedorId)
        {
            var fornecedor =  await Db.Fornecedores.AsNoTracking()
                .Include(f => f.Produtos)
                .Include(f => f.Endereco)
                .FirstOrDefaultAsync(f => f.Id == fornecedorId);

            if (fornecedor == null) throw new KeyNotFoundException($"Fornecedor com ID {fornecedorId} não encontrado.");

            return fornecedor;
        }

        public async Task<Endereco> ObterEnderecoPorFornecedor(Guid fornecedorId)
        {
            var fornecedor =  await Db.Enderecos.AsNoTracking()
                .FirstOrDefaultAsync(e => e.FornecedorId == fornecedorId);

            if (fornecedor == null) throw new KeyNotFoundException($"Endereço com ID {fornecedorId} não encontrado.");

            return fornecedor;
        }

        public async Task RemoverEnderecoFornecedor(Endereco endereco)
        {
            Db.Enderecos.Remove(endereco);
            await SaveChanges();
        }
    }
}
