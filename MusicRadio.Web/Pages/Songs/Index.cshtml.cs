using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Songs
{
    [Authorize]
    public class IndexModel(ISongSetService songSetService, ILogger<IndexModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly ISongSetService _songSetService = songSetService;
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
        public IList<SongsViewModel> SongSetVM { get; set; } = default!;

        public async Task<ActionResult> OnGetAsync()
        {
            try
            {

                var songSet = await _songSetService.GetAllAsync();
                var SongSetdto = _mapper.Map<List<SongSetDto>>(songSet);
                SongSetVM = _mapper.Map<List<SongsViewModel>>(SongSetdto);

                return Page();
            }
            catch (Exception ex)
            {

                TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Error al cargar la lista de Canciones"), _jsonSerializerOptions);
                _logger.LogError(ex, "Error al cargar la lista de Canciones.");
                ModelState.AddModelError(string.Empty, $"Error al cargar la lista de Canciones: {ex.Message}");
                return Page();
            }

        }

        public IActionResult OnPostAsync(int id, string action)
        {
            if (action == "Editar")
            {
                TempData["SongId"] = id;
                return RedirectToPage("Editar");
            }
            else if (action == "Eliminar")
            {
                TempData["SongId"] = id;
                return RedirectToPage("Delete");
            }

            return Page();
        }
    }
}
