using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.AddDtos;
using Share.Models.Dtos.EditDtos;
using Share.Models.ResponseObject;
using Share.Ultils;

namespace Client.Pages.Table
{
    public class AddModel : PageModel
    {
        private readonly ICustomHttpClient _httpCustom;
        private readonly Logger _logger;
        private readonly IMapper _mapper;

        [BindProperty]
        public TableAddDto TableAddDto { get; set; }

        public AddModel(ICustomHttpClient httpCustom, Logger logger, IMapper mapper)
        {
            _httpCustom = httpCustom;
            _logger = logger;
            _mapper = mapper;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (!ModelState.IsValid || TableAddDto == null)
                {
                    return Page();
                }

                var request = await _httpCustom.PostJsonAsync(RestApiName.ADD_TABLE, TableAddDto);
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
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }
    }
}
