using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Services;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Songs
{
    [Authorize]
    public class CreateModel(ISongSetService songSetService, IAlbumSetService _albumSetService, ILogger<CreateModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly ISongSetService _songSetService = songSetService;
        private readonly IAlbumSetService _albumSetService = _albumSetService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        [BindProperty]
        public SongsViewModel SongVM { get; set; } = default!;

        public SelectList AlbumSet { get; set; } = null!;

        public async Task<IActionResult> OnGetAsync()
        {

            await LoadSelectListAsync();
            return Page();

        }

        private async Task LoadSelectListAsync()
        {
            try
            {
                var albumSet = await _albumSetService.GetAllAsync();

                AlbumSet = new SelectList(albumSet, "Id", "Name");
                var subjectItems = albumSet.Select(s => new
                {
                    Id = s.Id,
                    DisplayText = $"{s.Name}"
                });

                AlbumSet = new SelectList(subjectItems, "Id", "DisplayText");
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
                await LoadSelectListAsync();
            }

            try
            {

                var songSet = new SongSet
                {
                    Id = SongVM.Id,
                    Name = SongVM.Name,
                    Album_Id = SongVM.Album_Id
                };

                var result = await _songSetService.AddAsync(songSet);

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
                _logger.LogError(ex, "Error al guardar el registro de la canción");
                ModelState.AddModelError(string.Empty, $"Error al guardar el registro de la canción: {ex.Message}");
                return Page();
            }
        }


    }
}
