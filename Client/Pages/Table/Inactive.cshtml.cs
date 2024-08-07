using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Table
{
    public class InactiveModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        public List<Share.Models.Dtos.ViewDtos.TableViewDto> Tables { get; set; } = new();

        public InactiveModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            try
            {
                var response = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_TABLE_BY_CONDITION, new TableSearchDto()
                {
                    Status = TableStatus.INACTIVE,
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
    }
}
