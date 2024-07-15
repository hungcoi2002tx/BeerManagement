using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Constant;
using Share.Models.EditModels;
using Share.Models.SearchModels;
using Share.Models.ViewModels;
using Share.Ultils;

namespace Client.Pages.Product
{
    public class UpdateModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _environment;

        public UpdateModel(ICustomHttpClient request, IMapper mapper, IWebHostEnvironment environment)
        {
            _request = request;
            _mapper = mapper;
            _environment = environment;
        }

        public ProductEditModel EditModel { get; set; } = new();
        public IFormFile? UploadImage { get; set; }
        public List<CategoryViewModel> Categories { get; set; } = new();
        public List<SupplierViewModel> Suppliers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id != 0)
                {
                    var requestCategories = await _request.GetAsync(RestApiName.GET_ALL_LIST_CATEGORY);
                    var requestSuppliers = await _request.GetAsync(RestApiName.GET_ALL_LIST_SUPPLIER);

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

                    var result = await GetModelBySearchAsync(new ProductSearchModel
                    {
                        Id = id,
                    });
                    var model = result.Objects.FirstOrDefault();
                    if (model != null)
                    {
                        EditModel = _mapper.Map<ProductEditModel>(model);
                        return Page();
                    }
                    else
                    {
                        throw new Exception();
                    }
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

        public async Task<IActionResult> OnPostAsync(ProductEditModel EditModel)
        {
            try
            {
                ValidateImageUpload(UploadImage);

                if (!ModelState.IsValid || EditModel == null)
                {
                    await OnGetAsync(EditModel.Id);
                    return Page();
                }

                if (UploadImage != null)
                {
                    var fileExtention = Path.GetExtension(UploadImage.FileName).ToLowerInvariant();
                    var fileName = UploadImage.FileName.GenerateGuid() + fileExtention;
                    var uploadFolder = Path.Combine(_environment.WebRootPath, "images", "product");
                    if (!Directory.Exists(uploadFolder))
                    {
                        Directory.CreateDirectory(uploadFolder);
                    }
                    var file = Path.Combine(uploadFolder, fileName);
                    using (var fileStream = new FileStream(file, FileMode.Create))
                    {
                        await UploadImage.CopyToAsync(fileStream);
                    }
                    var oldPath = Path.Combine(uploadFolder, EditModel.Image);
                    System.IO.File.Delete(oldPath);
                    EditModel.Image = fileName;
                }
                
                var request = await _request.PutAsync(RestApiName.PUT_PRODUCT, EditModel);
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
                if (!result.Status)
                {
                    return Page();
                }
                return RedirectToPage(GlobalVariants.LINK_PRODUCT_INDEX);
            }
            catch (Exception ex)
            {
                ViewData["DataAdded"] = false;
                return Redirect(GlobalVariants.PAGE_404);
            }
        }

        private async Task<ResponseCustom<Share.Models.Domain.Product>> GetModelBySearchAsync(ProductSearchModel search)
        {
            try
            {
                var requestGetData = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_PRODUCT, search);
                var dataReturn = await requestGetData.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
                return dataReturn;
            }
            catch (Exception ex)
            {
                return new ResponseCustom<Share.Models.Domain.Product>();
            }
        }

        private void ValidateImageUpload(IFormFile? imageFile)
        {
            if(imageFile == null)
            {
                return;
            }
            if (imageFile.Length > 4 * 1024 * 1024) // 4MB
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
