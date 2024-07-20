using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.Dtos.ViewDtos
{
    public class CategoryViewDto
    {
        public int Id { get; set; }
        [Display(Name = "No")]
        public int Stt { get; set; }
        [Display(Name = "Name")]
        public string Name { get; set; } = null!;
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Display(Name = "Image")]
        public string? Image { get; set; }
        [Display(Name = "Active Status")]
        public bool IsEnable { get; set; }

        public string GetImageUrl
        {
            get => $"./images/category/{Image}";
            set { }
        }
    }
}
