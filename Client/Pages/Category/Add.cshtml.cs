using AutoMapper;
using Client.WebRequests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Share.Models.EditModels;
using Share.Ultils;

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

        public async Task OnGetAsync()
        {
            try
            {

            }
            catch (Exception ex)
            {

            }
        }

        public async Task<IActionResult> OnPostAsync(IFormFile ImageFile)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return Page();
                }

                if (ImageFile != null)
                {
                    var fileName = Path.GetFileName(ImageFile.FileName);
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", "category", fileName);
                    if (!Directory.Exists(filePath))
                    {
                        Directory.CreateDirectory(filePath);
                    }

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImageFile.CopyToAsync(stream);
                    }
                    EditModel.Image = fileName;
                }
                return RedirectToPage("Success");
            }
            catch (Exception ex)
            {
                return Page();
            }
        }
    }
}
