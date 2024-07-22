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

namespace Client.Pages.Order
{
    public class PayModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public List<OrderDetailViewDto> OrderDetailViewDtos { get; set; } = new();

        [BindProperty]
        public int orderId { get; set; }
        [BindProperty]
        public double Total { get; set; } = 0;

        public PayModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {

            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task OnGetAsync(int orderId)
        {
            this.orderId = orderId;
            var responseOrderDetail = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_ORDER_DETAIL_BY_CONDITION, new OrderDetailSearchDto()
            {
                OrderId = orderId,
                GetProduct = true
            });
            var dataOrderDetail = await responseOrderDetail.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.OrderDetail>>();

            OrderDetailViewDtos = _mapper.Map<List<OrderDetailViewDto>>(dataOrderDetail.Objects);
            int i = 1;
            foreach (var item in OrderDetailViewDtos)
            {
                item.Stt = i++;
                Total = Total + (item.Quantity * item.UnitPrice);
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (orderId != 0)
                {
                    var response = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_ORDER_BY_CONDITION
                        , new OrderSearchDto
                        {
                            Id = orderId
                        });

                    var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();

                    if (!data.Status || data.StatusCode == 500)
                    {
                        return Redirect(GlobalVariants.PAGE_500);
                    }

                    if (data.Objects?.Any() != true)
                    {
                        return Redirect(GlobalVariants.PAGE_400);
                    }

                    var order = _mapper.Map<OrderEditDto>(data.Objects.First());
                    order.Total = Total;
                    order.PaymentDate = DateTime.Now;
                    order.PaymentStatus = Share.Constant.PaymentStatus.PAID;

                   await UpdateOrder(order);
                    await ChangeTableStatus(order.TableId);

                }
                return RedirectToPage("/Table/Active");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }

        private async Task ChangeTableStatus(int id)
        {
            try
            {
                var response = await GetTableById(id);
                response.Status = TableStatus.INACTIVE;
                await ChangeTableStatus(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task<TableEditDto> GetTableById(int id)
        {
            var response = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_TABLE_BY_CONDITION
                     , new TableSearchDto
                     {
                         Id = id
                     });

            var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

            if (!data.Status || data.StatusCode == 500)
            {
                throw new Exception("Error GetTableById");
            }

            if (data.Objects?.Any() != true)
            {
                throw new Exception("Error GetTableById");
            }

            var tableEditDto = _mapper.Map<TableEditDto>(data.Objects.First());
            return tableEditDto;
        }

        private async Task ChangeTableStatus(TableEditDto obj)
        {
            if (!ModelState.IsValid || obj == null)
            {
                throw new Exception("Error ChangeTableStatus");
            }

            var request = await _httpCustom.PutAsync(RestApiName.UPDATE_TABLE, obj);
            var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Table>>();

            if (!result.Status)
            {
                throw new Exception("Error ChangeTableStatus");
            }
        }

        private async Task UpdateOrder(OrderEditDto orderEditDto)
        {
            try
            {
                if (!ModelState.IsValid || orderEditDto == null)
                {
                   throw new Exception(nameof(orderEditDto));
                }

                var request = await _httpCustom.PutAsync(RestApiName.UPDATE_ORDER, orderEditDto);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();

                if (!result.Status)
                {
                    throw new Exception(nameof(orderEditDto));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw new Exception(nameof(orderEditDto));
            }
        }
    }
}
