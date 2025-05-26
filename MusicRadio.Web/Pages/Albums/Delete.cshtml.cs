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
    public class DeleteModel(IAlbumSetService albumSetService, IMapper mapper, ILogger<DeleteModel> logger,JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IAlbumSetService _albumSetService = albumSetService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<DeleteModel> _logger = logger;
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
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("�lbum no existe."), _jsonSerializerOptions);

                    ModelState.AddModelError(string.Empty, $"�lbum {albumId}no existe");
                    return RedirectToPage("Index");
                }

                var albumSet = await _albumSetService.GetByIdAsync(albumId);
                if (albumSet == null)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("�lbum no encontrado."), _jsonSerializerOptions);

                    return RedirectToPage("Index");
                }

                var albumSetDto = _mapper.Map<AlbumSetDto>(albumSet);
                AlbumVM = _mapper.Map<AlbumViewModel>(albumSetDto);

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cargar el �lbum");
                ModelState.AddModelError(string.Empty, $"Error al cargar el �lbum: {ex.Message}");
                return Page();
            }

        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (AlbumVM.Id <=0)
            {
                return Page();
            }

            try
            {

                var albumSetDto = _mapper.Map<AlbumSetDto>(AlbumVM);
                var albumSet = _mapper.Map<AlbumSet>(albumSetDto);
                var result = await _albumSetService.DeleteAsync(albumSet.Id);

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
                ModelState.AddModelError(string.Empty, "Ocurri� un error al Eliminar el �lbum");
                _logger.LogError(ex, "Ocurri� un error al Eliminar el �lbum");
                return Page();
            }
        }
    }
}
