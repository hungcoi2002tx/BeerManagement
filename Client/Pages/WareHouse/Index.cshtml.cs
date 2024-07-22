using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.WareHouse
{
    public class IndexModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;

        public IndexModel(ICustomHttpClient request, IMapper mapper, Logger logger)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
        }

        public ImportHistorySearchDto Search { get; set; } = new ImportHistorySearchDto();
        public List<ImportHistoryViewDto> ViewModels { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int pageIndex, bool DataAdded)
        {
            try
            {
                ViewData["DataAdded"] = DataAdded;
                await GetBaseDataAsync(pageIndex);
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

        private async Task GetBaseDataAsync(int pageIndex = 1)
        {

            Search.Page = new Share.Models.PagingObject.Page()
            {
                PageIndex = pageIndex == 0 ? 1 : pageIndex,
                BaseUrl = "WareHouse"
            };
            Search.IsInclueProduct = true;
            var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_HISTORYIMPORT, Search);
            if (request.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(request.CheckValidRequestExtention());
            }
            var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.ImportHistory>>();
            if (datas.Status)
            {
                ViewModels = _mapper.Map<List<ImportHistoryViewDto>>(datas.Objects);
                Search.Page.Total = datas.Total;
                int i = (Search.Page.PageIndex - 1) * Search.Page.PageSize + 1;
                foreach (var item in ViewModels)
                {
                    item.Stt = i++;
                }
            }
            else
            {
                throw new Exception("Error");
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int warehouseId, int pageIndex)
        {
            try
            {
                var apiUrl = string.Format(RestApiName.DELETE_HISTORYIMPORT, warehouseId);
                var request = await _request.DeleteAsync(apiUrl);
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                var data = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.ImportHistory>>();
                if (!data.Status)
                {
                    if (data.StatusCode == 404)
                    {
                        return Redirect(GlobalVariants.PAGE_404);
                    }
                    else
                    {
                        return Redirect(GlobalVariants.PAGE_500);
                    }
                }
                ViewData["DataDeleted"] = true;
                await GetBaseDataAsync(pageIndex);
                return Page();
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception ex)
            {
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
