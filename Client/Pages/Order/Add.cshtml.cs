using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.AddDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Order
{
    public class AddModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public OrderAddDto OrderAddDto { get; set; } = new();
        public int TableId { get; set; }

        public AddModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public void OnGet(int tableId)
        {
            this.TableId = tableId;
            OrderAddDto.Date = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || OrderAddDto == null)
                {
                    return Page();
                }

                var request = await _httpCustom.PostJsonAsync(RestApiName.ADD_ORDER, OrderAddDto);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();

                if (result.Status)
                {
                    return RedirectToPage("/Table/Active");
                }
                else
                {
                    return Redirect(GlobalVariants.PAGE_500);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
