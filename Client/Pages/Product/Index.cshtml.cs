using AutoMapper;
using Client.Ultils;
using Client.WebRequests;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.Dtos.EditDtos;
using Share.Models.Dtos.SearchDtos;
using Share.Models.Dtos.ViewDtos;
using Share.Models.ResponseObject;
using Share.Ultils;
using System.ComponentModel.DataAnnotations;

namespace Client.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public IndexModel(ICustomHttpClient request, IMapper mapper, Logger logger, IWebHostEnvironment environment)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
            _webHostEnvironment = environment;
        }
        public IFormFile? UploadImage {  get; set; }

        public ProductSearchDto Search { get; set; } = new ProductSearchDto();
        public List<ProductViewDto> ViewModels { get; set; } = new();
        public ProductEditDto EditModel { get; set; } = new();
        public List<CategoryViewDto> Categories { get; set; } = new();
        public List<SupplierViewDto> Suppliers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int pageIndex, bool DataAdded)
        {
            try
            {
                ViewData["DataAdded"] = DataAdded;
                await GetBaseDataAsync(pageIndex);
                return Page();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return Redirect(GlobalVariants.PAGE_400);
            }
        }

        public async Task<IActionResult> OnPostAddAsync(ProductEditDto EditModel)
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
                        var uploadFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", "product");
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
                    await GetBaseDataAsync();
                    return Page();
                }
                #region check import or export product
                EditModel.ForSell = EditModel.SupplierId == -1;
                EditModel.SupplierId = null;
                #endregion

                var request = await _request.PostJsonAsync(RestApiName.POST_ADD_PRODUCT, EditModel);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
                if (result.Status)
                {
                    return RedirectToPage("./Index", new { DataAdded = true});
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
                    BaseUrl = "Product"
                };
                Search.IsIncludeSupplier = true;
                Search.IsIncludeCategory = true;
                var requestProduct = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_PRODUCT, Search);
                var requestCategories = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_CATEGORY, new CategorySearchDto()
                {
                    IsEnable = true
                });
                var requestSuppliers = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_SUPPLIER, new SupplierSearchDto()
                {
                    IsEnable = true
                });

                var dataProduct = await requestProduct.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
                var dataCategories = await requestCategories.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Category>>();
                var dataSuppliers = await requestSuppliers.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
                if (dataCategories.Status)
                {
                    Categories = _mapper.Map<List<CategoryViewDto>>(dataCategories.Objects);
                }
                if (dataSuppliers.Status)
                {
                    Suppliers = _mapper.Map<List<SupplierViewDto>>(dataSuppliers.Objects);
                }
                if (dataProduct.Status)
                {
                    ViewModels = _mapper.Map<List<ProductViewDto>>(dataProduct.Objects);
                    Search.Page.Total = dataProduct.Total;
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

        public async Task<IActionResult> OnPostDeleteAsync(int productId, int pageIndex)
        {
            try
            {
                var apiUrl = string.Format(RestApiName.DELETE_PRODUCT, productId);
                var request = await _request.DeleteAsync(apiUrl);
                var data = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
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

        private void ValidateImageUpload(IFormFile? imageFile)
        {
            if(imageFile == null)
            {
                ModelState.AddModelError("UploadImage", "Please upload an image");
            }
            else if (imageFile.Length > 4 * 1024 * 1024) // 4MB
            {
                ModelState.AddModelError("UploadImage", "The file size cannot exceed 4MB.");
            }
            else if (!new[] { ".jpg", ".jpeg", ".png", ".gif" }.Contains(Path.GetExtension(imageFile.FileName).ToLowerInvariant()))
            {
                ModelState.AddModelError("UploadImage", "The file type must be one of the following: .jpg, .jpeg, .png, .gif.");
            }
        }
    }
}
