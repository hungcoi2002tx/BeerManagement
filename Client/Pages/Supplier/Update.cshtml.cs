using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Supplier
{
    public class UpdateModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;


        public UpdateModel(ICustomHttpClient request, IMapper mapper, Logger logger)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
        }

        public SupplierSearchDto Search { get; set; } = new SupplierSearchDto();
        public List<SupplierViewDto> ViewModels { get; set; } = new();
        [BindProperty]
        public SupplierEditDto EditModel { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id != 0)
                {
                    #region get id
                    var result = await GetModelBySearchAsync(new SupplierSearchDto
                    {
                        Id = id,
                    });
                    var model = result.Objects.FirstOrDefault();
                    if (model != null)
                    {
                        EditModel = _mapper.Map<SupplierEditDto>(model);
                        return Page();
                    }
                    else
                    {
                        throw new Exception();
                    }
                    #endregion
                }
                else
                {
                    throw new Exception();
                }
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect("/Error404");
            }
        }

        public async Task<IActionResult> OnPostAsync(SupplierEditDto EditModel)
        {
            try
            {
                if (!ModelState.IsValid || EditModel == null)
                {
                    return Page();
                }
                EditModel.IsEnable = true;
                var request = await _request.PutAsync(RestApiName.PUT_SUPPLIER, EditModel);
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
                if (!result.Status)
                {
                    return Page();
                }
                return RedirectToPage(GlobalVariants.LINK_SUPPLIER_INDEX);
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception ex)
            {
                ViewData["DataAdded"] = false;
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_404);
            }
        }

        private async Task<ResponseCustom<Share.Models.Domain.Supplier>> GetModelBySearchAsync(SupplierSearchDto search)
        {
            var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_SUPPLIER, search);
            if (request.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(request.CheckValidRequestExtention());
            }
            var dataReturn = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
            return dataReturn;
        }
    }
}
