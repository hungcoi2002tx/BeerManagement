using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.UserInfo
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

        public UserSearchDto Search { get; set; } = new UserSearchDto();
        public List<UserViewDto> ViewModels { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int pageIndex)
        {
            try
            {
                await GetBaseDataAsync(pageIndex);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }

        private async Task GetBaseDataAsync(int pageIndex)
        {
            try
            {
                Search.Page = new Share.Models.PagingObject.Page()
                {
                    PageIndex = pageIndex == 0 ? 1 : pageIndex,
                    BaseUrl = "UserInfo"
                };
                var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_USER, Search);
                var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.User>>();
                if (datas.Status)
                {
                    ViewModels = _mapper.Map<List<UserViewDto>>(datas.Objects);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int userId, int pageIndex)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await GetBaseDataAsync(pageIndex);
                    return Page();
                }
                var apiUrl = string.Format(RestApiName.DELETE_USER, userId);
                var request = await _request.DeleteAsync(apiUrl);
                var data = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.User>>();
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
            catch (Exception ex)
            {
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
