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
        public Share.Models.Dtos.EditDtos.ImportHistoryEditDto EditModel { get; set; }

        public List<Share.Models.Domain.Product> Products { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                await GetBaseDataAsync();
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

        public async Task<IActionResult> OnPostAsync(Share.Models.Dtos.EditDtos.ImportHistoryEditDto EditModel)
        {
            try
            {
                if (!ModelState.IsValid || EditModel == null)
                {
                    return Page();
                }
                var request = await _request.PostJsonAsync(RestApiName.POST_ADD_HISTORYIMPORT, EditModel);
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.ImportHistory>>();
                if (result.Status)
                {
                    return RedirectToPage(GlobalVariants.LINK_CATEGORY_INDEX);
                }
                else
                {
                    return Redirect(GlobalVariants.PAGE_500);
                }
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

        private async Task GetBaseDataAsync()
        {
            var requestProducts = await _request.PostJsonAsync(RestApiName.GET_ALL_LIST_PRODUCT, new ProductSearchDto()
            {
                IsEnable = true,
                IsForSell = false
            });
            if (requestProducts.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(requestProducts.CheckValidRequestExtention());
            }
            var dataProduct = await requestProducts.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();

            if (dataProduct.Status)
            {
                Products = dataProduct.Objects;
            }
        }
    }
}
