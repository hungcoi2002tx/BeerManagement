using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.EditModels;
using Share.Models.SearchModels;
using Share.Models.ViewModels;
using Share.Ultils;

namespace Client.Pages.Supperlier
{
    public class IndexModel : PageModel
    {

        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;


        public IndexModel(ICustomHttpClient request, IMapper mapper)
        {
            _request = request;
            _mapper = mapper;
        }

        public SupplierSearchModel Search { get; set; } = new SupplierSearchModel();
        public List<SupplierViewModel> ViewModels { get; set; } = new();
        [BindProperty]
        public SupplierEditModel EditModel { get; set; }
        public async Task<IActionResult> OnGetAsync(int pageIndex)
        {
            try
            {
                await GetBaseDataAsync(pageIndex);
                return Page();
            }
            catch (Exception ex)
            {
                return Redirect(GlobalVariants.PAGE_404);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || EditModel == null)
                {
                    await GetBaseDataAsync();
                    return Page();
                }
                EditModel.IsEnable = true;
                #region check duplicate phone number
                //var dataReturn = await GetModelBySearchAsync(new SupplierSearchModel()
                //{
                //    PhoneNumber = EditModel.PhoneNumber
                //});
                //if (dataReturn.Total != 0)
                //{
                //    ModelState.AddModelError("EditModel.PhoneNumber", "PhoneNumber already exists.");
                //    await GetBaseDataAsync();
                //    return Page();
                //}
                #endregion
                var request = await _request.PostJsonAsync(RestApiName.POST_Add_SUPPLIER, EditModel);
                var result = await request.Content.ReadFromJsonAsync<ExecuteRespone<Share.Models.Domain.Supplier>>();
                ViewData["DataAdded"] = true;
                await GetBaseDataAsync();
                return Page();
            }
            catch (Exception ex)
            {
                ViewData["DataAdded"] = false;
                return Redirect(GlobalVariants.PAGE_404);
            }
        }

        private async Task GetBaseDataAsync(int pageIndex = 1)
        {
            Search.Page = new Share.Constant.Page()
            {
                PageIndex = pageIndex == 0 ? 1 : pageIndex,
                BaseUrl = "Supplier"
            };
            var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_SUPPLIER, Search);
            var datas = await request.Content.ReadFromJsonAsync<ExecuteRespone<Share.Models.Domain.Supplier>>();
            ViewModels = _mapper.Map<List<SupplierViewModel>>(datas.Objects);
            Search.Page.Total = datas.Total;
            int i = (Search.Page.PageIndex - 1) * Search.Page.PageSize + 1;
            foreach (var item in ViewModels)
            {
                item.Stt = i++;
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync(int supplierId, int pageIndex)
        {
            try
            {
                var apiUrl = string.Format(RestApiName.DELETE_SUPPLIER, supplierId);
                var request = await _request.DeleteAsync(apiUrl);
                var data = await request.Content.ReadFromJsonAsync<ExecuteRespone<Share.Models.Domain.Supplier>>();
                if (!data.Status)
                {
                    return Redirect(GlobalVariants.PAGE_404);
                }
                ViewData["DataDeleted"] = true;
                await GetBaseDataAsync(pageIndex);
                return RedirectToAction("OnGetAsync");
            }
            catch (Exception ex)
            {
                return Redirect(GlobalVariants.PAGE_404);
            }
        }
    }
}