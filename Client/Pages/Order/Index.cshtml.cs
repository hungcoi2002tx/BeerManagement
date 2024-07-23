using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Order
{
    public class IndexModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        public OrderSearchDto Search { get; set; } = new();
        public List<OrderViewDto> Orders { get; set; } = new();

        public IndexModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGet(int pageIndex)
        {
            try
            {
                await GetlDataAsync(pageIndex);

                return Page();
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }

        private async Task GetlDataAsync(int pageIndex)
        {
            Search.Page = new Share.Models.PagingObject.Page()
            {
                PageIndex = pageIndex == 0 ? 1 : pageIndex,
                BaseUrl = "Order"
            };

            var response = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_ORDER_BY_CONDITION, Search);
            if (response.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(response.CheckValidRequestExtention());
            }
            var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();

            if (data.Status)
            {
                Orders = _mapper.Map<List<OrderViewDto>>(data.Objects);
                Search.Page.Total = data.Total;

                int i = (Search.Page.PageIndex - 1) * Search.Page.PageSize + 1;
            }
            else
            {
                throw new Exception("Error while get data");
            }
        }
    }
}
