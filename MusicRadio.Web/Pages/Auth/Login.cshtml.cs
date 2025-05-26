using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Interfaces;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MusicRadio.Web.Pages.Auth
{
    public class LoginModel(IAuthService authService, IMapper mapper, ILogger<LoginModel> logger, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IAuthService _authService = authService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<LoginModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;


        [BindProperty]
        public LoginViewModel LoginViewModel { get; set; } = default!;
        public string? ReturnUrl { get; set; } = null;


        public async Task<IActionResult> OnPostAsync()
        {
            
            if (ModelState.IsValid)
            {
                try
                {
                    var loginDto = _mapper.Map<LoginDto>(LoginViewModel);

                    var result = await _authService.LoginAsync(loginDto.Email, loginDto.Password);

                    if (result.Success)
                    {
                        _logger.LogInformation("Redirigiendo a /Index - Login exitoso");
                        var redirectUrl = Url.Page("/Index") ?? "/Error";
                        Response.Redirect(redirectUrl);
                    }

                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Usuario o Credenciales inválidas"), _jsonSerializerOptions);
                    ModelState.AddModelError(string.Empty, "Usuario o Credenciales inválidas");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error al iniciar sesión.");
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
               
            }

            return Page();
        }

    }
}
