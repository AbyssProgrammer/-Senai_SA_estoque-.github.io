using Microsoft.AspNetCore.Mvc.RazorPages;
using StockVision.Models;

namespace StockVision.Pages
{
    public class AlertasModel : PageModel
    {
        private readonly EstoqueService _estoqueService;

        public List<Produto> ProdutosComAlerta { get; set; } = new List<Produto>();

        public AlertasModel(EstoqueService estoqueService)
        {
            _estoqueService = estoqueService;
        }

        public void OnGet()
        {
            ProdutosComAlerta = _estoqueService.ObterComAlerta();
        }
    }
}
