using Catalogo.Domain.Arguments;
using Catalogo.Domain.Arguments.Base;
using Catalogo.Application.Controllers;
using Catalogo.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Catalogo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProdutoHandler : Controller
    {
        private ProdutoController _controller;

        public ProdutoHandler(ICatalogoGateway gateway, IImagemProdutoGateway imagemGateway)
        {
            _controller = new ProdutoController(gateway, imagemGateway);
        }

        [HttpPost("cadastrar")]
        public async Task<ActionResult> CadastrarProduto([FromForm] ProdutoRequest request)
        {
            var response = new ResponseBase<ProdutoResponse>();
            try
            {
                response = await _controller.CadastrarProduto(request);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("listar")]
        public async Task<ActionResult> ListarProdutos()
        {
            var response = new ResponseBase<ProdutoResponse>();
            try
            {
                response = await _controller.ListarProdutos();
                return new JsonResult(new { data = response.Resultado });
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpPatch("atualizar")]
        public async Task<ActionResult> AtualizarProduto(ProdutoRequest request)
        {
            var response = new ResponseBase<ProdutoResponse>();
            try
            {
                response = await _controller.AtualizarProduto(request);
                if (!response.Sucesso)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("remover")]
        public async Task<ActionResult> RemoverProduto(int id)
        {
            var response = new ResponseBase<bool>();
            try
            {
                response = await _controller.RemoverProduto(id);
                if (!response.Sucesso)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("obterCardapio")]
        public async Task<ActionResult> ObterCardapio()
        {
            var response = new ResponseBase<CardapioResponse>();
            try
            {
                response = await _controller.ObterCardapio();
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("visualizarImagem")]
        public async Task<ActionResult> VisualizarImagem(int id)
        {
            var response = new ResponseBase<string>();
            try
            {
                response = await _controller.VisualizarImagem(id);
                if (!response.Sucesso)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }

        [HttpGet("obterProdutosPorCategoria")]
        public async Task<ActionResult> ObterProdutosPorCategoria(int categoriaId)
        {
            var response = new ResponseBase<ProdutoResponse>();
            try
            {
                response = await _controller.ObterProdutosPorCategoria(categoriaId);
                if (!response.Sucesso)
                    return BadRequest(response);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Mensagem = ex.Message;
                return BadRequest(response);
            }
        }
    }
}
