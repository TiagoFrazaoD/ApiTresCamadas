using DevIO.Business.Models;

namespace DevIO.Business.Interfaces
{
    public interface IProdutoRepository : IRepository<Produto>
    {
        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);
        Task<IEnumerable<Fornecedor>> ObterFornecedoresPorProduto(Guid produtoId);
        Task<Produto> ObterProdutoFornecedor(Guid id);
    }
}
