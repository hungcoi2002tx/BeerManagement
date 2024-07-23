using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Dashboard
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

		public int TotalEmployeeOn { get; set; }
		public int TotalMenus { get; set; }
		public Double Total { get; set; }
		public int TotalEmployeeOff { get; set; }

		public List<ProductViewDto> ProductInStocks { get; set; }
		public async Task<IActionResult> OnGetAsync()
		{
			try
			{
				await GetBaseDataAsync();
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

		private async Task GetBaseDataAsync()
		{
			await GetTotalEmployeeAsync();
			await GetProductInStockAsync();
			await GetTotalRevenueAsync();
			await GetProductInStockAsync();
		}

		private async Task GetProductInStockAsync()
		{
			var request = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_HISTORYIMPORT, new ImportHistorySearchDto()
			{
				IsEnable = true,
				IsInclueProduct = true
			});
			if (request.CheckValidRequestExtention() != null)
			{
				throw new AuthenticationException(request.CheckValidRequestExtention());
			}
			var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.ImportHistory>>();
			if (datas.Status)
			{
				var data = datas.Objects;
				var top5Products = data
									.GroupBy(t => t.ProductId)
									.Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(t => t.Quantity) })
									.OrderByDescending(p => p.TotalQuantity)
									.Take(5);
				var ids = top5Products.Select(x => x.ProductId).ToList();

			}
		}

		private async Task GetTotalEmployeeAsync()
		{
			var request = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_USER, new UserSearchDto()
			{
				Role = 1
			});
			if (request.CheckValidRequestExtention() != null)
			{
				throw new AuthenticationException(request.CheckValidRequestExtention());
			}
			var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.User>>();
			if (datas.Status)
			{
				var data = datas.Objects;
				TotalEmployeeOn = data.Where(x => x.IsEnable == true).Count();
				TotalEmployeeOff = data.Where(x => x.IsEnable == false).Count();
			}
		}

        private async Task GetTotalMenuAsync()
        {
            var request = await _request.PostJsonAsync(RestApiName.GET_LIST_ORDER_BY_CONDITION, new OrderSearchDto()
            {
                PaymentStatus = PaymentStatus.PAID
            });
            if (request.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(request.CheckValidRequestExtention());
            }
            var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();
            if (datas.Status)
            {
                var data = datas.Objects;
                TotalMenus = data.Where(x => x.IsEnable == true).Count();
            }
        }

		private async Task GetTotalRevenueAsync()
		{
			var request = await _request.PostJsonAsync(RestApiName.GET_LIST_ORDER_BY_CONDITION, new OrderSearchDto()
			{
				PaymentStatus = PaymentStatus.PAID
			});
			if (request.CheckValidRequestExtention() != null)
			{
				throw new AuthenticationException(request.CheckValidRequestExtention());
			}
			var datas = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();
			if (datas.Status)
			{
				var data = datas.Objects;
				Total = (double)data.Where(x => x.IsEnable == true).Sum(x => x.Total);
			}
		}
	}
}
