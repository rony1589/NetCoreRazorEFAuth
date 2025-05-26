using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Purchases
{
    [Authorize]
    public class IndexModel(IPurchaseDetailService purchaseDetailService, IAlbumSetService albumSetService,  ISongSetService songSetService, ILogger<IndexModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IPurchaseDetailService _purchaseDetailService = purchaseDetailService;
        private readonly IAlbumSetService _albumSetService = albumSetService;
        private readonly ISongSetService _songSetService = songSetService;
        private readonly ILogger<IndexModel> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
        public IList<PurchaseDetailViewModel> PurchaseVM { get; set; } = default!;
        public string? UserId { get; private set; } = string.Empty;
        public string? UserRole { get; private set; } = string.Empty;
        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                UserRole = User.FindFirst(ClaimTypes.Role)?.Value;

                var purchaseDetail = await _purchaseDetailService.GetAllAsync(UserId);

                var purchaseDetaildto = _mapper.Map<List<PurchaseDetailDto>>(purchaseDetail);

                PurchaseVM = _mapper.Map<List<PurchaseDetailViewModel>>(purchaseDetaildto);

                return Page();
            }
            catch (Exception ex)
            {

                TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Error al cargar la lista de compras realizadas"), _jsonSerializerOptions);
                _logger.LogError(ex, "Error al cargar la lista de compras realizadas.");
                ModelState.AddModelError(string.Empty, $"Error al cargar la lista de Canciones: {ex.Message}");
                return Page();
            }

        }

    }
}
