using AutoMapper;
using DevIO.Api.ViewModels;
using DevIO.Business.Interfaces;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace DevIO.Api.Controllers
{
    [Route("api/produtos")]
    public class ProdutoController : MainController
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoService _produtoService;
        private readonly IMapper _mapper;
        public ProdutoController(IProdutoRepository produtoRepository, IProdutoService produtoService, IMapper mapper,
            INotificador notificador) :base(notificador)
        {
            _produtoRepository = produtoRepository;
            _produtoService = produtoService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoViewModel>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoViewModel>>(await _produtoRepository.ObterProdutosFornecedores());
              
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> ObterPorId(Guid id)
        {           
            var produtoViewModel = await ObterProduto(id);

            if (produtoViewModel == null) return NotFound();

            return produtoViewModel;
        }

        [HttpPost]
        public async Task<ActionResult<ProdutoViewModel>> Adicionar(ProdutoViewModel produtoViewModel)
        {
            if(!ModelState.IsValid) return CustomResponse(ModelState);

            await _produtoService.Adicionar(_mapper.Map<Produto>(produtoViewModel));

            return CustomResponse(HttpStatusCode.Created ,produtoViewModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ProdutoViewModel>> Atualizar(Guid id, ProdutoViewModel produtoViewModel)
        {
            if( id != produtoViewModel.Id)
            {
                NotificarErro("Os ids informados não são iguais!");
                return CustomResponse();
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var produtosAtualizacao = await ObterProduto(id);

            produtosAtualizacao.FornecedorId = produtoViewModel.FornecedorId;
            produtosAtualizacao.Nome = produtoViewModel.Nome;
            produtosAtualizacao.Descricao = produtoViewModel.Descricao;
            produtosAtualizacao.Valor = produtoViewModel.Valor;
            produtosAtualizacao.Ativo = produtoViewModel.Ativo;

            await _produtoService.Atualizar(_mapper.Map<Produto>(produtosAtualizacao));

            return CustomResponse(HttpStatusCode.NoContent);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> Excluir(Guid id)
        {
            var produto = ObterProduto(id);

            if(produto == null) return NotFound();

            await _produtoService.Remover(id);

            return CustomResponse(HttpStatusCode.NoContent);

        }

        [NonAction]
        public async Task<ProdutoViewModel> ObterProduto(Guid id) 
        {
            return _mapper.Map<ProdutoViewModel>(await _produtoRepository.ObterProdutoFornecedor(id));
        }
    }
}
