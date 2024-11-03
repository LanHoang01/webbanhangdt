﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebBanDT.Repository.Validation;

namespace WebBanDT.Models
{
    public class ProductModel
	{
		[Key]
		public int Id { get; set; }
		[Required (ErrorMessage = "Yêu cầu nhập tên sản phẩm")]
		public string Name { get; set; }
		public string Slug { get; set; }
		[Required, MinLength(4, ErrorMessage = "Yêu cầu nhập mô  tả sản phẩm")]
		public string Description { get; set; }
		[Required( ErrorMessage = "Yêu cầu nhập giá sản phẩm")]
		
		public decimal Price { get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage ="Chọn một thương hiệu")]
		public int BrandId { get; set; }
        [Required, Range(1, int.MaxValue, ErrorMessage = "Chọn một danh mục")]
        public int CategoryId { get; set; }
		
        public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
        public string Image { get; set; } 
        [NotMapped]
		[FileExtension]
		public IFormFile ImageUpload { get; set; } 
		
	}
}