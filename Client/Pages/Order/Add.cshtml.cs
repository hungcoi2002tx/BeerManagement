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

namespace Client.Pages.Order
{
    public class AddModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public OrderAddDto OrderAddDto { get; set; } = new();

        [BindProperty]
        public int TableId { get; set; }

        public AddModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public void OnGet(int tableId)
        {
            this.TableId = tableId;
            OrderAddDto.Date = DateTime.Now;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || OrderAddDto == null)
                {
                    return Page();
                }

                OrderAddDto.TableId = TableId;
                var request = await _httpCustom.PostJsonAsync(RestApiName.ADD_ORDER, OrderAddDto);
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Order>>();

                if (result.Status)
                {
                    await ChangeTableStatus(TableId);
                    return RedirectToPage("/OrderDetail/Add", new { orderId = result.Object.Id });
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

        private async Task ChangeTableStatus(int id)
        {
            try
            {
                var response = await GetTableById(id);
               
                response.Status = TableStatus.ACTIVE;
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
    }
}
