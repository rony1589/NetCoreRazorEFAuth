using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Interfaces;
using MusicRadio.Shared.Common;
using System;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Albums
{
    [Authorize]
    public class IndexModel(IAlbumSetService albumSetService, ILogger<IndexModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IAlbumSetService _albumSetService = albumSetService;
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
        public IList<AlbumSetDto> AlbumSet { get; set; } = default!;
        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                var albumSet = await _albumSetService.GetAllAsync();
                AlbumSet = _mapper.Map<List<AlbumSetDto>>(albumSet);
                return Page();
            }
            catch (Exception ex)
            {
               
                TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Error al cargar la lista de Álbums"), _jsonSerializerOptions);
                _logger.LogError(ex, "Error al cargar la lista de Álbums.");
                ModelState.AddModelError(string.Empty, $"Error al cargar la lista de Álbums: {ex.Message}");
                return Page();
            }

        }
        public IActionResult OnPostAsync(int id, string action)
        {
            if (action == "Editar")
            {
                TempData["AlbumId"] = id;
                return RedirectToPage("Editar");
            }
            else if (action == "Eliminar")
            {
                TempData["AlbumId"] = id;
                return RedirectToPage("Delete");
            }

            return Page(); 
        }

    }
}
