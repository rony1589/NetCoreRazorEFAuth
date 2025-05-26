using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Services;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Songs
{
    [Authorize]
    public class DeleteModel(ISongSetService songSetService, IAlbumSetService _albumSetService, ILogger<EditarModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly ISongSetService _songSetService = songSetService;
        private readonly IAlbumSetService _albumSetService = _albumSetService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EditarModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        [BindProperty]
        public SongsViewModel SongVM { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {

                int songId = TempData["SongId"] as int? ?? 0;

                if (songId == 0)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail($"Canción con Id: {songId} no existe"), _jsonSerializerOptions);

                    ModelState.AddModelError(string.Empty, $"Canción con Id: {songId} no existe");
                    return RedirectToPage("Index");
                }

                var songSet = await _songSetService.GetByIdAsync(songId);
                if (songSet == null)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Canción no encontrada."), _jsonSerializerOptions);

                    return RedirectToPage("Index");
                }

                var songSetDto = _mapper.Map<SongSetDto>(songSet);
                SongVM = _mapper.Map<SongsViewModel>(songSetDto);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al cargar la canción.");
                ModelState.AddModelError(string.Empty, $"Error al cargar la canción: {ex.Message}");
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (SongVM.Id <= 0)
            {
                return Page();
            }

            try
            {

                var songSetDto = _mapper.Map<SongSetDto>(SongVM);
                var songSet = _mapper.Map<SongSet>(songSetDto);
                var result = await _songSetService.DeleteAsync(songSet.Id);

                if (!result.Success)
                {
                    ModelState.AddModelError(string.Empty, result.Message ?? "Ha ocurrido un error.");
                    return Page();
                }

                TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);
                return RedirectToPage("./Index");

            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al Eliminar la canción");
                _logger.LogError(ex, "Ocurrió un error al Eliminar la canción");
                return Page();
            }
        }
    }
}
