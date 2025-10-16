using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using StockVision.Models;

namespace StockVision.Pages
{
    public class CadastroModel : PageModel
    {
        private readonly AuthService _authService;

        [BindProperty]
        public string Nome { get; set; } = string.Empty;

        [BindProperty]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        public string Senha { get; set; } = string.Empty;

        [BindProperty]
        public string ConfirmarSenha { get; set; } = string.Empty;

        [BindProperty]
        public string Empresa { get; set; } = string.Empty;

        [BindProperty]
        public string Telefone { get; set; } = string.Empty;

        public string MensagemErro { get; set; } = string.Empty;
        public string MensagemSucesso { get; set; } = string.Empty;

        public CadastroModel(AuthService authService)
        {
            _authService = authService;
        }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(Nome) || string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Senha))
            {
                MensagemErro = "Por favor, preencha todos os campos obrigatórios.";
                return Page();
            }

            if (Senha != ConfirmarSenha)
            {
                MensagemErro = "As senhas não coincidem.";
                return Page();
            }

            if (Senha.Length < 6)
            {
                MensagemErro = "A senha deve ter pelo menos 6 caracteres.";
                return Page();
            }

            var usuario = new Usuario
            {
                Nome = Nome,
                Email = Email,
                Senha = Senha,
                Empresa = Empresa,
                Telefone = Telefone
            };

            var sucesso = _authService.Registrar(usuario);
            if (sucesso)
            {
                MensagemSucesso = "Conta criada com sucesso! Você já pode fazer login.";
                // Limpar os campos
                Nome = Email = Senha = ConfirmarSenha = Empresa = Telefone = string.Empty;
            }
            else
            {
                MensagemErro = "Este email já está em uso. Tente outro email.";
            }

            return Page();
        }
    }
}