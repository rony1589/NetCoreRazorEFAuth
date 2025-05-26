using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Songs
{
    [Authorize]
    public class EditarModel(ISongSetService songSetService, IAlbumSetService _albumSetService, ILogger<EditarModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly ISongSetService _songSetService = songSetService;
        private readonly IAlbumSetService _albumSetService = _albumSetService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EditarModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        [BindProperty]
        public SongsViewModel SongVM { get; set; } = default!;

        public SelectList AlbumSet { get; set; } = null!;
        public async Task<IActionResult> OnGetAsync()
        {

            try
            {

                int SongId = TempData["SongId"] as int? ?? 0;

                if (SongId == 0)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Id de canción incorrecta."), _jsonSerializerOptions);

                    ModelState.AddModelError(string.Empty, $"Canción {SongId}no existe");
                    return RedirectToPage("Index");
                }

                var songSet = await _songSetService.GetByIdAsync(SongId);
                if (songSet == null)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Canción no encontrada."), _jsonSerializerOptions);

                    return RedirectToPage("Index");
                }

                var songSetDto = _mapper.Map<SongSetDto>(songSet);

                SongVM = _mapper.Map<SongsViewModel>(songSetDto);

                await LoadSelectListAsync(SongVM.Album_Id);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar la Canción");
                ModelState.AddModelError(string.Empty, $"Error al cargar la Canción: {ex.Message}");
                return Page();
            }

        }

        private async Task LoadSelectListAsync(int album_id)
        {
            try
            {
                var albumSet = await _albumSetService.GetAllAsync();

                AlbumSet = new SelectList(albumSet, "Id", "Name");
                var subjectItems = albumSet.Select(s => new
                {
                    s.Id,
                    DisplayText = $"{s.Name}"
                });

                AlbumSet = new SelectList(subjectItems, "Id", "DisplayText", album_id);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, "Error al carga la lista de Álbums");
                ModelState.AddModelError(string.Empty, $"Error al carga la lista de Álbums: {ex.Message}");
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                await LoadSelectListAsync(SongVM.Album_Id);
            }

            try
            {

                var songSet = new SongSet
                {
                    Id = SongVM.Id,
                    Name = SongVM.Name,
                    Album_Id = SongVM.Album_Id
                };

                var result = await _songSetService.UpdateAsync(songSet);

                if (!result.Success)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);
                    ModelState.AddModelError(string.Empty, result.Message ?? "Ha ocurrido un error.");
                    return Page();
                }

                TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar la canción");
                ModelState.AddModelError(string.Empty, $"Error al actualizar la canción: {ex.Message}");
                return Page();
            }
        }


    }
}
