using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockVision.Models;

namespace StockVision.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Senha { get; set; } = string.Empty;

        public string MensagemErro { get; set; } = string.Empty;

        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha))
            {
                MensagemErro = "Por favor, preencha todos os campos.";
                return Page();
            }

            var usuario = _authService.Login(Email, Senha);
            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioLogado", "true");
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);

                return RedirectToPage("/Dashboard");
            }
            else
            {
                MensagemErro = "Email ou senha incorretos.";
                return Page();
            }
        }
    }
}