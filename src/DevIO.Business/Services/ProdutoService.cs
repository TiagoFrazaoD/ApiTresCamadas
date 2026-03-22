using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using DevIO.Business.Models.Validation;


namespace DevIO.Business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        public ProdutoService(IProdutoRepository produtoRepository,
                             INotificador notificador) : base(notificador) 
        {
            _produtoRepository = produtoRepository;
        }
        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;

            var produtoExistente = await _produtoRepository.ObterPorId(produto.Id);

            if (produtoExistente != null)
            {
                Notificar("Já existe um produto com este Id.");
                return;
            }

            await _produtoRepository.Adicionar(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto)) return;
            await _produtoRepository.Atualizar(produto);
        }        

        public async Task Remover(Guid id)
        {
            var produto = await _produtoRepository.ObterPorId(id);

            if (produto == null)
            {
                Notificar("Produto não Existe!");
                return;
            }

            await _produtoRepository.Remover(id);
        }

        public void Dispose()
        {
            _produtoRepository?.Dispose();
        }
    }
}
