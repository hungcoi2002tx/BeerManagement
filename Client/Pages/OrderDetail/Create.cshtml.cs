using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.AddDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;
using System.Data;

namespace Client.Pages.OrderDetail
{
    public class CreateModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public List<OrderDetailViewDto> OrderDetailViewDtos { get; set; } = new();
        public List<ProductViewDto> ProductViewDtos { get; set; } = new();

        public CreateModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpCustom.PostJsonAsync(RestApiName.POST_PAGE_LIST_PRODUCT, new ProductSearchDto()
                {
                    IsForSell = true,
                    IsEnable = true
                });

                var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();

                if (data.Status)
                {
                    ProductViewDtos = _mapper.Map<List<ProductViewDto>>(data.Objects);

                    int i = 1;
                    foreach (var item in ProductViewDtos)
                    {
                        item.Stt = i++;
                    }

                    this.OrderDetailViewDtos = _mapper.Map<List<OrderDetailViewDto>>(ProductViewDtos);

                }
                if (data.StatusCode == 500)
                {
                    return Redirect(GlobalVariants.PAGE_500);
                }

                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
