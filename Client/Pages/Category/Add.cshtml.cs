using AutoMapper;
using Client.Ultils;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Domain;
using Share.Models.EditModels;
using Share.Models.SearchModels;
using Share.Ultils;
using System.IO;

namespace Client.Pages.Category
{
    public class AddModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AddModel(ICustomHttpClient request, IMapper mapper, Logger logger, IWebHostEnvironment webHostEnvironment)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = webHostEnvironment;
        }

        [BindProperty]
        public CategoryEditModel EditModel { get; set; } = new CategoryEditModel();
        public IFormFile? UploadImage { get; set; }
        public async Task<IActionResult> OnGetAsync(int id = 0)
        {
            try
            {
                if (id != 0)
                {
                    var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_CATEGORY
                        , new CategorySearchModel
                        {
                            Id = id
                        });
                    var entity = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Category>>();
                    if (!entity.Status || entity.StatusCode == 500)
                    {
                        return Redirect(GlobalVariants.PAGE_500);
                    }
                    if (entity.Objects?.Any() != true)
                    {
                        return Redirect(GlobalVariants.PAGE_400);
                    }
                    EditModel = _mapper.Map<CategoryEditModel>(entity.Objects.First());
                }
                return Page();
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
                if (UploadImage != null)
                {
                    var errorImage = UploadImage.ValidateImageUpload();
                    if (errorImage.key != null)
                    {
                        ModelState.AddModelError(errorImage.key, errorImage.value);
                    }
                    else
                    {
                        string? fileName = null;
                        var fileExtention = Path.GetExtension(UploadImage.FileName).ToLowerInvariant();
                        fileName = UploadImage.FileName.GenerateGuid() + fileExtention;
                        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "category");
                        if (!Directory.Exists(uploadFolder))
                        {
                            Directory.CreateDirectory(uploadFolder);
                        }
                        var file = Path.Combine(uploadFolder, fileName);
                        using (var fileStream = new FileStream(file, FileMode.Create))
                        {
                            await UploadImage.CopyToAsync(fileStream);
                        }
                        EditModel.Image = fileName;
                    }
                }
                if (!ModelState.IsValid || EditModel == null)
                {
                    return Page();
                }
                HttpResponseMessage request;
                ResponseCustom<Share.Models.Domain.Category> result;
                if (EditModel.Id != 0 && EditModel.Id != null)
                {
                    request = await _request.PutAsync(RestApiName.PUT_CATEGORY, EditModel);
                }
                else
                {
                    request = await _request.PostJsonAsync(RestApiName.POST_Add_CATEGORY, EditModel);
                }
                result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Category>>();
                if (result.Status)
                {
                    return RedirectToPage(GlobalVariants.LINK_CATEGORY_INDEX);
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
