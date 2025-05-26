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

namespace MusicRadio.Web.Pages.Albums
{
    [Authorize]
    public class EditarModel(IAlbumSetService albumSetService, IMapper mapper, ILogger<EditarModel> logger, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IAlbumSetService _albumSetService = albumSetService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<EditarModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        [BindProperty]
        public AlbumViewModel AlbumVM { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {

                int albumId = TempData["AlbumId"] as int? ?? 0;

                if (albumId == 0)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Id de álbum incorrecto."), _jsonSerializerOptions);

                    ModelState.AddModelError(string.Empty, $"Álbum {albumId}no existe");
                    return RedirectToPage("Index");
                }

                var albumSet = await _albumSetService.GetByIdAsync(albumId);
                if (albumSet == null)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Álbum no encontrado."), _jsonSerializerOptions);

                    return RedirectToPage("Index");
                }

                var albumSetDto = _mapper.Map<AlbumSetDto>(albumSet);

                AlbumVM = _mapper.Map<AlbumViewModel>(albumSetDto);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el Álbum");
                ModelState.AddModelError(string.Empty, $"Error al cargar el Álbum: {ex.Message}");
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {

                var albumSetDto = _mapper.Map<AlbumSetDto>(AlbumVM);
                var albumSet = _mapper.Map<AlbumSet>(albumSetDto);
                var result = await _albumSetService.UpdateAsync(albumSet);

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
                ModelState.AddModelError(string.Empty, "Ocurrió un error al Editar el álbum");
                _logger.LogError(ex, "Ocurrió un error al Editar el álbum");
                return Page();
            }
        }
    }
}
