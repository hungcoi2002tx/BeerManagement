using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.EditModels;
using Share.Models.SearchModels;
using Share.Models.ViewModels;
using Share.Ultils;
using System.ComponentModel.DataAnnotations;

namespace Client.Pages.Product
{
    public class IndexModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly Logger _logger;
        private readonly IWebHostEnvironment _environment;

        public IFormFile? UploadImage {  get; set; }

        public IndexModel(ICustomHttpClient request, IMapper mapper, Logger logger, IWebHostEnvironment environment)
        {
            _request = request;
            _mapper = mapper;
            _logger = logger;
            _environment = environment;
        }

        public ProductSearchModel Search { get; set; } = new ProductSearchModel();
        public List<ProductViewModel> ViewModels { get; set; } = new();
        public ProductEditModel EditModel { get; set; } = new();
        public List<CategoryViewModel> Categories { get; set; } = new();
        public List<SupplierViewModel> Suppliers { get; set; } = new();

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

        public async Task<IActionResult> OnPostAddAsync(ProductEditModel EditModel)
        {
            try
            {
                if (!ModelState.IsValid || EditModel == null)
                {
                    await GetBaseDataAsync();
                    return Page();
                }
                if(UploadImage != null)
                {
                    var file = Path.Combine(_environment.WebRootPath, "images", "product", UploadImage.FileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await UploadImage.CopyToAsync(fileStream);
                    }
                    EditModel.Image = $"/images/product/{UploadImage.FileName}";
                }

                var request = await _request.PostJsonAsync(RestApiName.POST_Add_PRODUCT, EditModel);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
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
                Search.Page = new Share.Ultils.Page()
                {
                    PageIndex = pageIndex == 0 ? 1 : pageIndex,
                    BaseUrl = "Product"
                };
                var requestProduct = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_PRODUCT, Search);
                var requestCategories = await _request.GetAsync(RestApiName.GET_ALL_LIST_CATEGORY);
                var requestSuppliers = await _request.GetAsync(RestApiName.GET_ALL_LIST_SUPPLIER);

                var dataProduct = await requestProduct.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
                var dataCategories = await requestCategories.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Category>>();
                var dataSuppliers = await requestSuppliers.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Supplier>>();
                if (dataCategories.Status)
                {
                    Categories = _mapper.Map<List<CategoryViewModel>>(dataCategories.Objects);
                }
                if (dataSuppliers.Status)
                {
                    Suppliers = _mapper.Map<List<SupplierViewModel>>(dataSuppliers.Objects);
                }
                if (dataProduct.Status)
                {
                    ViewModels = _mapper.Map<List<ProductViewModel>>(dataProduct.Objects);
                    foreach(var product in ViewModels)
                    {
                        product.SupplierName = Suppliers.FirstOrDefault(c => c.Id == product.SupplierId).SupplierName;
                        product.CategoryName = Categories.FirstOrDefault(c => c.Id == product.CategoryId).Name;
                    }
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
    }
}
