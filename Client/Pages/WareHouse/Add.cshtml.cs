using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.WareHouse
{
    public class AddModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddModel(ICustomHttpClient request, IMapper mapper, Logger logger, IWebHostEnvironment webHostEnvironment)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }
        [BindProperty]
        public Share.Models.Domain.Product ProductSelected { get; set; }

        public List<Share.Models.Domain.Product> Products { get; set; } = new();

        public Share.Models.Dtos.EditDtos.ImportHistoryEditDto EditModel { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await GetBaseDataAsync();
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
                await GetBaseDataAsync();
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }

        private async Task GetBaseDataAsync()
        {
            try
            {
                var requestProducts = await _request.PostJsonAsync(RestApiName.GET_ALL_LIST_PRODUCT, new ProductSearchDto()
                {
                    IsEnable = true,
                    IsForSell = false
                });

                var dataProduct = await requestProducts.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();

                if (dataProduct.Status)
                {
                    Products = dataProduct.Objects;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
