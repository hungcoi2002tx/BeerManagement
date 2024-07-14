using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.SearchModels;
using Share.Models.ViewModels;
using Share.Ultils;

namespace Client.Pages.Category
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

        public CategorySearchModel Search { get; set; } = new CategorySearchModel();
        public List<CategoryViewModel> ViewModels { get; set; } = new();
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
                Search.Page = new Share.Ultils.Page()
                {
                    PageIndex = pageIndex == 0 ? 1 : pageIndex,
                    BaseUrl = "Category"
                };
                var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_CATEGORY, Search);
                var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Category>>();
                if (datas.Status)
                {
                    ViewModels = _mapper.Map<List<CategoryViewModel>>(datas.Objects);
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
    }
}
