using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Table
{
    public class IndexModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        public TableSearchDto Search { get; set; } = new();
        public List<TableViewDto> Tables { get; set; } = new();

        public IndexModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int pageIndex)
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
                BaseUrl = "Table"
            };

            var response = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_TABLE_BY_CONDITION, Search);
            if (response.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(response.CheckValidRequestExtention());
            }
            var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

            if (data.Status)
            {
                Tables = _mapper.Map<List<TableViewDto>>(data.Objects);
                Search.Page.Total = data.Total;

                int i = (Search.Page.PageIndex - 1) * Search.Page.PageSize + 1;
                foreach (var item in Tables)
                {
                    item.Stt = i++;
                }
            }
            else
            {
                throw new Exception("Error while get data");
            }
        }

        public async Task<IActionResult> OnPostUpdateAsync()
        {
            try
            {
                var response = await _httpCustom.PostJsonAsync(RestApiName.UPDATE_TABLE, new TableSearchDto()
                {
                    Status = TableStatus.ACTIVE,
                    IsEnable = true
                });

                if (response.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(response.CheckValidRequestExtention());
                }

                var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

                if (data.Status)
                {
                    Tables = _mapper.Map<List<TableViewDto>>(data.Objects);
                }
                if (data.StatusCode == 500)
                {
                    return Redirect(GlobalVariants.PAGE_500);
                }

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

        public async Task<IActionResult> OnPostDeleteAsync(int tableId, int pageIndex)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    await GetlDataAsync(pageIndex);
                    return Page();
                }

                var apiUrl = string.Format(RestApiName.DELETE_TABLE, tableId);
                var response = await _httpCustom.DeleteAsync(apiUrl);
                if (response.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(response.CheckValidRequestExtention());
                }
                var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

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
                await GetlDataAsync(pageIndex);

                return Page();
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception)
            {
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
