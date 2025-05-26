using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Infrastructure.Data;
using MusicRadio.Infrastructure.Services;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Security.Claims;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Purchases
{
    public class CreateModel(MusicRadioDbContext _context, IPurchaseDetailService purchaseDetailService, IAlbumSetService albumSetService, ISongSetService songSetService, ILogger<CreateModel> logger, IMapper mapper, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IPurchaseDetailService _purchaseDetailService = purchaseDetailService;
        private readonly IAlbumSetService _albumSetService = albumSetService;
        private readonly ISongSetService _songSetService = songSetService;
        private readonly ILogger<CreateModel> _logger = logger;
        private readonly IMapper _mapper = mapper;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;
        public IList<AlbumViewModel> AlbumSetVM { get; set; } = default!;
        public string? UserId { get; private set; } = string.Empty;

        [BindProperty]
        public IList<PurchaseDetailViewModel> PurchaseVM { get; set; } = default!;

        public async Task<IList<PurchaseDetailViewModel>> GetCatalogAsync(string? UserId)
        {
            var albumSet = await _albumSetService.GetAllAsync();
            var albumSetdto = _mapper.Map<List<AlbumSetDto>>(albumSet);

            var songSet = await _songSetService.GetAllAsync();
            var songSetDto = _mapper.Map<List<SongSetDto>>(songSet);

            // Crear un diccionario agrupando las canciones por Album_Id
            var songsByAlbum = songSetDto.GroupBy(song => song.Album_Id)
                                         .ToDictionary(g => g.Key, g => g.ToList());


            var purchaseVM = new List<PurchaseDetailViewModel>();

            var albums = await _context.AlbumSet.ToListAsync();
            foreach (var album in albums)
            {
                songsByAlbum.TryGetValue(album.Id, out var songList);

                var purchaseDetail = new PurchaseDetailViewModel
                {
                    Album_Id = album.Id,
                    Total = album.Precio,
                    Client_Id = UserId!,
                    AlbumSet = new AlbumSet
                    {
                        Id = album.Id,
                        Name = album.Name,
                        Precio = album.Precio
                    },
                    SongSet = songList ?? [] // Asigna la lista de canciones o una vacía
                };

                purchaseVM.Add(purchaseDetail);
            }

            return purchaseVM;

        }
        public async Task<ActionResult> OnGetAsync()
        {
            try
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                PurchaseVM = await GetCatalogAsync(UserId);

                return Page();
            }
            catch (Exception ex)
            {

                TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Error al cargar la lista de álbums para la compra"), _jsonSerializerOptions);
                _logger.LogError(ex, "Error al cargar la lista de compras realizadas.");
                ModelState.AddModelError(string.Empty, $"Error al cargar la lista de álbums: {ex.Message}");
                return Page();
            }
        }


        public async Task<IActionResult> OnPostAsync(int Album_Id, decimal Precio)
        {

            if (Album_Id == 0 || Precio <= 0)
            {
                TempData["OperationResult"] = JsonSerializer.Serialize(OperationResult.Fail("Datos inválidos para la compra."), _jsonSerializerOptions);
                return Page();
            }

            try
            {
                UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                var purchaseDetail = new PurchaseDetail
                {
                    Client_Id = UserId!,
                    Album_Id = Album_Id,
                    Total = Precio
                };

                var result = await _purchaseDetailService.AddAsync(purchaseDetail);

                if (!result.Success)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);
                    ModelState.AddModelError(string.Empty, result.Message ?? "Ha ocurrido un error.");
                    return Page();
                }

                PurchaseVM = await GetCatalogAsync(UserId);

                TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);

                return RedirectToPage("./Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al guardar la compra");
                ModelState.AddModelError(string.Empty, $"Error al guardar la compra: {ex.Message}");
                return Page();
            }
        }

        }
    }
