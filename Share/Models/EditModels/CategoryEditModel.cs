﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Share.Models.EditModels
{
    public class CategoryEditModel
    {
		public int Id { get; set; }
		public string Name { get; set; } = null!;
		public string? Description { get; set; }
		public string? Image { get; set; }
		public bool IsEnable { get; set; } = true;
	}
}
