using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Core.Interfaces;

namespace MusicRadio.Web.Pages.Auth
{
    public class LogoutModel(IAuthService authService) : PageModel
    {
        private readonly IAuthService _authService = authService;

        public async Task<IActionResult> OnPostAsync()
        {
            if (TempData.ContainsKey("OperationResult"))
            {
                TempData.Remove("OperationResult"); // Solo se elimina si existe
            }

            await _authService.LogoutAsync();
            return RedirectToPage("/Auth/Login");
        }
    }
}
