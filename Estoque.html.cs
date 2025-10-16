using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockVision.Models;
using System.ComponentModel.DataAnnotations;

namespace StockVision.Pages
{
    public class EstoqueModel : PageModel
    {
        private readonly EstoqueService _estoqueService;

        public List<Produto> Produtos { get; set; } = new List<Produto>();
        public List<Produto> ProdutosComAlerta { get; set; } = new List<Produto>();

        // Propriedades para os formulários
        [BindProperty]
        public string ProdutoNome { get; set; } = string.Empty;

        [BindProperty]
        public int Quantidade { get; set; }

        [BindProperty]
        public string Observacao { get; set; } = string.Empty;

        [BindProperty]
        public string Pessoa { get; set; } = string.Empty;

        [BindProperty]
        public DateTime DataDevolucao { get; set; }

        public EstoqueModel(EstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        public void OnGet()
        {
            CarregarDados();
        }

        // Handler para Entrada
        public IActionResult OnPostEntrada()
        {
            if (!string.IsNullOrEmpty(ProdutoNome) && Quantidade > 0)
            {
                _estoqueService.RegistrarEntrada(ProdutoNome, Quantidade, Observacao);
                TempData["Sucesso"] = $"Entrada de {Quantidade} unidades de '{ProdutoNome}' registrada com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Por favor, preencha todos os campos obrigatórios.";
            }

            return RedirectToPage();
        }

        // Handler para Saída
        public IActionResult OnPostSaida()
        {
            if (!string.IsNullOrEmpty(ProdutoNome) && Quantidade > 0)
            {
                var sucesso = _estoqueService.RegistrarSaida(ProdutoNome, Quantidade, Observacao);
                if (sucesso)
                {
                    TempData["Sucesso"] = $"Saída de {Quantidade} unidades de '{ProdutoNome}' registrada com sucesso!";
                }
                else
                {
                    TempData["Erro"] = $"Erro: Estoque insuficiente de '{ProdutoNome}' ou produto não encontrado.";
                }
            }
            else
            {
                TempData["Erro"] = "Por favor, preencha todos os campos obrigatórios.";
            }

            return RedirectToPage();
        }

        // Handler para Empréstimo
        public IActionResult OnPostEmprestimo()
        {
            if (!string.IsNullOrEmpty(ProdutoNome) && Quantidade > 0 && !string.IsNullOrEmpty(Pessoa) && DataDevolucao > DateTime.Now)
            {
                var sucesso = _estoqueService.RegistrarEmprestimo(ProdutoNome, Quantidade, Pessoa, DataDevolucao, Observacao);
                if (sucesso)
                {
                    TempData["Sucesso"] = $"Empréstimo de {Quantidade} unidades de '{ProdutoNome}' para {Pessoa} registrado com sucesso!";
                }
                else
                {
                    TempData["Erro"] = $"Erro: Estoque insuficiente de '{ProdutoNome}' ou produto não encontrado.";
                }
            }
            else
            {
                TempData["Erro"] = "Por favor, preencha todos os campos obrigatórios com dados válidos.";
            }

            return RedirectToPage();
        }

        private void CarregarDados()
        {
            Produtos = _estoqueService.ObterTodos();
            ProdutosComAlerta = _estoqueService.ObterComAlerta();
        }
    }
}
