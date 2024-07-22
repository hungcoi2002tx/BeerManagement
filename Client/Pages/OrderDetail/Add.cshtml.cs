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
    public class AddModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public List<OrderDetailViewDto> OrderDetailViewDtos { get; set; } = new ();
        public List<ProductViewDto> ProductViewDtos { get; set; } = new();

        public AddModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
           
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int orderId)
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

                    OrderDetailViewDtos = _mapper.Map<List<OrderDetailViewDto>>(ProductViewDtos);
                    foreach (var item in OrderDetailViewDtos)
                    {
                        item.OrderId = orderId;
                    }

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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || OrderDetailViewDtos == null)
                {
                    return Page();
                }

                var orderDetailAddDtos = _mapper.Map<List<OrderDetailAddDto>>(OrderDetailViewDtos);


                var request = await _httpCustom.PostJsonAsync(RestApiName.ADD_ORDER_DETAIL, orderDetailAddDtos);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.OrderDetail>>();

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
