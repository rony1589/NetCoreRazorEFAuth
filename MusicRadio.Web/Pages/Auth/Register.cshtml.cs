using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MusicRadio.Application.DTOs;
using MusicRadio.Core.Entities;
using MusicRadio.Core.Interfaces;
using MusicRadio.Shared.Common;
using MusicRadio.Web.ViewModels;
using System.Text.Json;

namespace MusicRadio.Web.Pages.Auth
{
    public class RegisterModel(IClientService clientService, IMapper mapper, ILogger<RegisterModel> logger, JsonSerializerOptions jsonSerializerOptions) : PageModel
    {
        private readonly IClientService _clientService = clientService;
        private readonly IMapper _mapper = mapper;
        private readonly ILogger<RegisterModel> _logger = logger;
        private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

        [BindProperty]
        public RegisterViewModel RegisterVM { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            try
            {
                var clientDto = _mapper.Map<ClientDto>(RegisterVM);
                var client = _mapper.Map<Client>(clientDto);
                var result = await _clientService.AddAsync(client);

                if (result.Success)
                {
                    TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);
                    return Page();
                    
                }

                TempData["OperationResult"] = JsonSerializer.Serialize((OperationResult)result, _jsonSerializerOptions);
              
                
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error al momento del registro.");
                _logger.LogError(ex, "Exception al Registrarse como usuario.");
            }

            return Page();
        }
    }
}
