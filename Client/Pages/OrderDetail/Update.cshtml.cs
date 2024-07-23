using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.AddDtos;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.OrderDetail
{
    public class UpdateModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public List<OrderDetailViewDto> OrderDetailViewDtos { get; set; } = new();
        public List<ProductViewDto> ProductViewDtos { get; set; } = new();
        [BindProperty]
        public int orderId { get; set; }

        public UpdateModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {

            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> OnGetAsync(int orderId)
        {
            try
            {
                this.orderId = orderId;

                var response = await _httpCustom.PostJsonAsync(RestApiName.POST_PAGE_LIST_PRODUCT, new ProductSearchDto()
                {
                    IsForSell = true,
                    IsEnable = true
                });
                if (response.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(response.CheckValidRequestExtention());
                }
                var data = await response.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();

                var responseOrderDetail = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_ORDER_DETAIL_BY_CONDITION, new OrderDetailSearchDto()
                {
                    OrderId = orderId
                });
                var dataOrderDetail = await responseOrderDetail.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.OrderDetail>>();


                if (data.Status)
                {
                    ProductViewDtos = _mapper.Map<List<ProductViewDto>>(data.Objects);

                    int i = 1;
                    foreach (var item in ProductViewDtos)
                    {
                        item.Stt = i++;
                    }

                    OrderDetailViewDtos = _mapper.Map<List<OrderDetailViewDto>>(ProductViewDtos);
                    foreach (var item in OrderDetailViewDtos)
                    {
                        item.OrderId = orderId;
                        foreach (var item2 in dataOrderDetail.Objects)
                        {
                            if (item2.ProductId == item.ProductId)
                            {
                                item.Quantity = item2.Quantity;
                            }
                        }
                    }
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

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                List<OrderDetailAddDto> listAdd = new List<OrderDetailAddDto>();
                if (!ModelState.IsValid || OrderDetailViewDtos == null)
                {
                    return Page();
                }

                var orderDetailAddDtos = _mapper.Map<List<OrderDetailAddDto>>(OrderDetailViewDtos);

                var responseOrderDetail = await _httpCustom.PostJsonAsync(RestApiName.GET_LIST_ORDER_DETAIL_BY_CONDITION, new OrderDetailSearchDto()
                {
                    OrderId = orderId
                });
                if (responseOrderDetail.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(responseOrderDetail.CheckValidRequestExtention());
                }
                var dataOrderDetail = await responseOrderDetail.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.OrderDetail>>();


                for (int i = 0; i < orderDetailAddDtos.Count; i++)
                {
                    var flag = false;
                    for (int j = 0; j < dataOrderDetail.Objects.Count; j++)
                    {
                        if (orderDetailAddDtos[i].ProductId == dataOrderDetail.Objects[j].ProductId)
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        listAdd.Add(orderDetailAddDtos[i]);
                        orderDetailAddDtos.Remove(orderDetailAddDtos[i]);
                    }
                }

                var request1 = await _httpCustom.PostJsonAsync(RestApiName.ADD_ORDER_DETAIL, listAdd);
                var result1 = await request1.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.OrderDetail>>();

                var request = await _httpCustom.PutAsync(RestApiName.UPDATE_ORDER_DETAIL, orderDetailAddDtos);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.OrderDetail>>();

                if (result.Status)
                {
                    return RedirectToPage("/OrderDetail/Update", new { orderId = this.orderId });
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
