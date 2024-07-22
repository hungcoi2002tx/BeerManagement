using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.UserInfo
{
    public class EditModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;


        public EditModel(ICustomHttpClient request, IMapper mapper, Logger logger)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
        }

        [BindProperty]
        public UserEditDto EditDto { get; set; }

        public bool isAdd { get; set; } = true;
        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    isAdd = false;
                    var request = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_USER
                        , new CategorySearchDto
                        {
                            Id = id
                        });
                    if (request.CheckValidRequestExtention() != null)
                    {
                        throw new AuthenticationException(request.CheckValidRequestExtention());
                    }
                    var entity = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.User>>();
                    if (!entity.Status || entity.StatusCode == 500)
                    {
                        return Redirect(GlobalVariants.PAGE_500);
                    }
                    if (entity.Objects?.Any() != true)
                    {
                        return Redirect(GlobalVariants.PAGE_400);
                    }
                    EditDto = _mapper.Map<UserEditDto>(entity.Objects.First());
                }
                return Page();
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception ex)
            {
                ViewData["DataAdded"] = false;
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || EditDto == null)
                {
                    return Page();
                }
                HttpResponseMessage request;
                ResponseCustom<Share.Models.Domain.User> result;
                if (EditDto.Id != 0 && EditDto.Id != null)
                {
                    request = await _request.PutAsync(RestApiName.PUT_USER, EditDto);
                }
                else
                {
                    request = await _request.PostJsonAsync(RestApiName.POST_ADD_USER, EditDto);
                }
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.User>>();
                if(result.StatusCode == 400)
                {
                    ModelState.AddModelError("", result.Message);
                    EditDto.Id = 0;
                    return Page();
                }
                if (result.Status)
                {
                    return RedirectToPage("./Index", new { DataAdded = true });
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
                ViewData["DataAdded"] = false;
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
