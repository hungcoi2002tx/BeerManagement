using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.AddDtos;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Table
{
    public class UpdateModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public TableEditDto TableEditDto { get; set; }

        public UpdateModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id != 0)
                {
                    var response = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_TABLE_BY_CONDITION
                        , new TableSearchDto
                        {
                            Id = id
                        });
                    if (response.CheckValidRequestExtention() != null)
                    {
                        throw new AuthenticationException(response.CheckValidRequestExtention());
                    }
                    var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

                    if (!data.Status || data.StatusCode == 500)
                    {
                        return Redirect(GlobalVariants.PAGE_500);
                    }

                    if (data.Objects?.Any() != true)
                    {
                        return Redirect(GlobalVariants.PAGE_400);
                    }

                    TableEditDto = _mapper.Map<TableEditDto>(data.Objects.First());
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || TableEditDto == null)
                {
                    return Page();
                }

                var request = await _httpCustom.PutAsync(RestApiName.UPDATE_TABLE, TableEditDto);
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

                if (result.Status)
                {
                    return RedirectToPage("/Table/Index");
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
    }
}
