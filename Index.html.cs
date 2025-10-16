using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockVision.Models;

namespace StockVision.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AuthService _authService;
        private readonly EstoqueService _estoqueService;

        [BindProperty] public string EmailLogin { get; set; } = string.Empty;
        [BindProperty] public string SenhaLogin { get; set; } = string.Empty;
        [BindProperty] public string NomeCadastro { get; set; } = string.Empty;
        [BindProperty] public string EmailCadastro { get; set; } = string.Empty;
        [BindProperty] public string SenhaCadastro { get; set; } = string.Empty;
        [BindProperty] public string ConfirmarSenhaCadastro { get; set; } = string.Empty;
        [BindProperty] public string EmpresaCadastro { get; set; } = string.Empty;

        public string MensagemErroLogin { get; set; } = string.Empty;
        public string MensagemErroCadastro { get; set; } = string.Empty;
        public string MensagemSucessoCadastro { get; set; } = string.Empty;

        public IndexModel(AuthService authService, EstoqueService estoqueService)
        {
            _authService = authService;
            _estoqueService = estoqueService;
        }

        public IActionResult OnGet()
        {
            // Se usuário já está logado, redireciona direto para o Dashboard
            if (HttpContext.Session.GetString("UsuarioLogado") == "true")
            {
                return RedirectToPage("/Dashboard");
            }

            return Page();
        }

        public IActionResult OnPostLogin()
        {
            if (string.IsNullOrEmpty(EmailLogin) || string.IsNullOrEmpty(SenhaLogin))
            {
                MensagemErroLogin = "Por favor, preencha todos os campos.";
                return Page();
            }

            var usuario = _authService.Login(EmailLogin, SenhaLogin);
            if (usuario != null)
            {
                HttpContext.Session.SetString("UsuarioLogado", "true");
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
                HttpContext.Session.SetString("UsuarioEmail", usuario.Email);

                // Redireciona DIRETO para o Dashboard
                return RedirectToPage("/Dashboard");
            }

            MensagemErroLogin = "Email ou senha incorretos.";
            return Page();
        }

        public IActionResult OnPostCadastro()
        {
            if (string.IsNullOrEmpty(NomeCadastro) || string.IsNullOrEmpty(EmailCadastro) || string.IsNullOrEmpty(SenhaCadastro))
            {
                MensagemErroCadastro = "Por favor, preencha todos os campos.";
                return Page();
            }

            if (SenhaCadastro != ConfirmarSenhaCadastro)
            {
                MensagemErroCadastro = "As senhas não coincidem.";
                return Page();
            }

            if (SenhaCadastro.Length < 6)
            {
                MensagemErroCadastro = "A senha deve ter pelo menos 6 caracteres.";
                return Page();
            }

            var usuario = new Usuario
            {
                Nome = NomeCadastro,
                Email = EmailCadastro,
                Senha = SenhaCadastro,
                Empresa = EmpresaCadastro
            };

            var sucesso = _authService.Registrar(usuario);

            if (sucesso)
            {
                // Após cadastro bem-sucedido, faz login AUTOMÁTICO e vai para o Dashboard
                HttpContext.Session.SetString("UsuarioLogado", "true");
                HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
                HttpContext.Session.SetString("UsuarioNome", usuario.Nome);
                HttpContext.Session.SetString("UsuarioEmail", usuario.Email);

                return RedirectToPage("/Dashboard");
            }
            else
            {
                MensagemErroCadastro = "Este email já está em uso.";
                return Page();
            }
        }
    }
}
