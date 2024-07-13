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
		public AddModel(ICustomHttpClient request, IMapper mapper, Logger logger)
		{
			_request = request;
			_mapper = mapper;
			_logger = logger;
		}

        [BindProperty]
        public CategoryEditModel EditModel { get; set; } = new CategoryEditModel();
		public string PreviewImageUrl { get; set; }

		public async Task OnGetAsync()
		{
			try
			{
				PreviewImageUrl = EditModel.Image ?? "abc";
			}
			catch (Exception ex)
			{

			}
		}

        public async Task OnPostAsync()
        {
            try
            {
                PreviewImageUrl = EditModel.Image ?? "abc";
            }
            catch (Exception ex)
            {

            }
        }
    }
}
