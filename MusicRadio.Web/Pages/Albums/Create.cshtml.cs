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
    public class CreateModel(IAlbumSetService albumSetService , IMapper mapper, ILogger<CreateModel> logger, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IAlbumSetService _albumSetService = albumSetService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<CreateModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        [BindProperty]
        public AlbumViewModel AlbumVM { get; set; } = default!;
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
                var result = await _albumSetService.AddAsync(albumSet);

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
                ModelState.AddModelError(string.Empty, "Ocurrió un error al crear el álbum");
                _logger.LogError(ex, "Ocurrió un error al crear el álbum");
                return Page();
            }
        }
    }
}
