using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Supperlier
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

		public SupplierSearchDto Search { get; set; } = new SupplierSearchDto();
		public List<SupplierViewDto> ViewModels { get; set; } = new();
		public SupplierEditDto EditModel { get; set; } = new();

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

		public async Task<IActionResult> OnPostAddAsync(SupplierEditDto EditModel)
		{
			try
			{
				if (!ModelState.IsValid || EditModel == null)
				{
					await GetBaseDataAsync();
					return Page();
				}
				var request = await _request.PostJsonAsync(RestApiName.POST_Add_SUPPLIER, EditModel);
				var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
				if (result.Status)
				{
					ViewData["DataAdded"] = true;
					await GetBaseDataAsync();
					return Page();
				}
				else
				{
					return Redirect(GlobalVariants.PAGE_500);
				}
			}
			catch (Exception ex)
			{
				ViewData["DataAdded"] = false;
				_logger.LogError(ex.Message);
				return Redirect(GlobalVariants.PAGE_400);
			}
		}

		private async Task GetBaseDataAsync(int pageIndex = 1)
		{
			try
			{
				Search.Page = new Share.Models.PagingObject.Page()
				{
					PageIndex = pageIndex == 0 ? 1 : pageIndex,
					BaseUrl = "Supplier"
				};
				var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_SUPPLIER, Search);
				var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
				if (datas.Status)
				{
					ViewModels = _mapper.Map<List<SupplierViewDto>>(datas.Objects);
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

		public async Task<IActionResult> OnPostDeleteAsync(int supplierId, int pageIndex)
		{
			try
			{
				if (!ModelState.IsValid)
				{
					await GetBaseDataAsync();
					return Page();
				}
				var apiUrl = string.Format(RestApiName.DELETE_SUPPLIER, supplierId);
				var request = await _request.DeleteAsync(apiUrl);
				var data = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
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