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
using System.Collections.Generic;

namespace Client.Pages.Product
{
    public class UpdateModel : PageModel
    {
        private readonly ICustomHttpClient _request;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly Logger _logger;

        public UpdateModel(ICustomHttpClient request, IMapper mapper, IWebHostEnvironment environment, Logger logger)
        {
            _request = request;
            _mapper = mapper;
            _webHostEnvironment = environment;
            _logger = logger;
        }

        public ProductEditDto EditModel { get; set; } = new();
        public IFormFile? UploadImage { get; set; }
        public List<CategoryViewDto> Categories { get; set; } = new();
        public List<SupplierViewDto> Suppliers { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            try
            {
                if (id != 0)
                {
                    await GetBaseDataAsync();

                    var result = await GetModelBySearchAsync(new ProductSearchDto
                    {
                        Id = id,
                    });

                    var model = result.Objects.FirstOrDefault();
                    if (model != null)
                    {
                        EditModel = _mapper.Map<ProductEditDto>(model);
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
            var requestCategories = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_CATEGORY, new CategorySearchDto()
            {
                IsEnable = true
            });
            var requestSuppliers = await _request.PostJsonAsync(RestApiName.POST_ALL_LIST_SUPPLIER, new SupplierSearchDto()
            {
                IsEnable = true
            });
            if (requestCategories.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(requestCategories.CheckValidRequestExtention());
            }
            if (requestSuppliers.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(requestSuppliers.CheckValidRequestExtention());
            }
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
        }

        public async Task<IActionResult> OnPostAsync(ProductEditDto EditModel)
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
                    await OnGetAsync(EditModel.Id);
                    return Page();
                }

                var request = await _request.PutAsync(RestApiName.PUT_PRODUCT, EditModel);
                if (request.CheckValidRequestExtention() != null)
                {
                    throw new AuthenticationException(request.CheckValidRequestExtention());
                }
                var result = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
                if (!result.Status)
                {
                    return Page();
                }
                return RedirectToPage(GlobalVariants.LINK_PRODUCT_INDEX);
            }
            catch (AuthenticationException ex)
            {
                return Redirect(ex.Message);
            }
            catch (Exception ex)
            {
                ViewData["DataAdded"] = false;
                return Redirect(GlobalVariants.PAGE_404);
            }
        }

        private async Task<ResponseCustom<Share.Models.Domain.Product>> GetModelBySearchAsync(ProductSearchDto search)
        {

            var request = await _request.PostJsonAsync(RestApiName.POST_PAGE_LIST_PRODUCT, search);
            if (request.CheckValidRequestExtention() != null)
            {
                throw new AuthenticationException(request.CheckValidRequestExtention());
            }
            var dataReturn = await request.Content.ReadFromJsonAsync<ResponseCustom<Share.Models.Domain.Product>>();
            return dataReturn;


        }
    }
}
