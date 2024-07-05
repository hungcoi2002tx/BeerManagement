using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.EditModels;
using Share.Models.SearchModels;
using Share.Models.ViewModels;
using Share.Ultils;

namespace Client.Pages.Supplier
{
    public class UpdateModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;


        public UpdateModel(ICustomHttpClient request, IMapper mapper)
        {
            _request = request;
            _mapper = mapper;
        }

        public SupplierSearchModel Search { get; set; } = new SupplierSearchModel();
        public List<SupplierViewModel> ViewModels { get; set; } = new();
        [BindProperty]
        public SupplierEditModel EditModel { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id != 0)
                {
					#region get id
					var result = await GetModelBySearchAsync(new SupplierSearchModel
                    {
                        Id = id,
                    });
                    var model = result.Objects.FirstOrDefault();
                    if (model != null)
                    {
                        EditModel = _mapper.Map<SupplierEditModel>(model);
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
            catch (Exception ex)
            {
                return Redirect("/Error404");
			}
        }

        public async Task<IActionResult> OnPostAsync(SupplierEditModel EditModel)
        {
            try
            {
                if (!ModelState.IsValid || EditModel == null)
                {
                    return Page();
                }
                EditModel.IsEnable = true;
                var request = await _request.PutAsync(RestApiName.PUT_SUPPLIER, EditModel);
                var result = await request.Content.ReadFromJsonAsync<ExecuteRespone<Share.Models.Domain.Supplier>>();
                if (!result.Status)
                {
                    return Page();
                }
                return RedirectToPage(GlobalVariants.LINK_SUPPLIER_INDEX);
            }
            catch (Exception ex)
            {
                ViewData["DataAdded"] = false;
                return Redirect(GlobalVariants.PAGE_404);
            }
        }

        private async Task<ExecuteRespone<Share.Models.Domain.Supplier>> GetModelBySearchAsync(SupplierSearchModel search)
		{
			try
			{
				var requestGetData = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_SUPPLIER, search);
				var dataReturn = await requestGetData.Content.ReadFromJsonAsync<ExecuteRespone<Share.Models.Domain.Supplier>>();
				return dataReturn;
			}
			catch (Exception ex)
			{
				return new ExecuteRespone<Share.Models.Domain.Supplier>();
			}
		}
	}
}
