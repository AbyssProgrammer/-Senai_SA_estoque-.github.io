using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockVision.Models;

namespace StockVision.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly EstoqueService _estoqueService;
        private readonly AuthService _authService;

        public string NomeUsuario { get; set; } = "Usuário";
        public int TotalProdutos { get; set; }
        public int TotalEntradas { get; set; }
        public int TotalSaidas { get; set; }
        public int AlertasAtivos { get; set; }
        public List<Movimentacao> UltimasMovimentacoes { get; set; } = new List<Movimentacao>();
        public List<Produto> ProdutosComAlerta { get; set; } = new List<Produto>();

        public DashboardModel(EstoqueService estoqueService, AuthService authService)
        {
            _estoqueService = estoqueService;
            _authService = authService;
        }

        public IActionResult OnGet()
        {
            // Verificar se usuário está logado
            if (HttpContext.Session.GetString("UsuarioLogado") != "true")
            {
                return RedirectToPage("/Index");
            }

            // Carregar dados do usuário
            var usuarioId = HttpContext.Session.GetInt32("UsuarioId");
            var usuario = _authService.ObterPorId(usuarioId ?? 0);

            if (usuario != null)
            {
                NomeUsuario = usuario.Nome;
            }

            // Carregar dados do estoque
            var produtos = _estoqueService.ObterTodos();
            TotalProdutos = produtos.Count;
            ProdutosComAlerta = _estoqueService.ObterComAlerta();
            AlertasAtivos = ProdutosComAlerta.Count;

            // Carregar movimentações
            UltimasMovimentacoes = _estoqueService.ObterTodasMovimentacoes()
                .OrderByDescending(m => m.DataMovimentacao)
                .Take(5)
                .ToList();

            TotalEntradas = _estoqueService.ObterEntradas().Count;
            TotalSaidas = _estoqueService.ObterSaidas().Count;

            return Page();
        }
    }
}