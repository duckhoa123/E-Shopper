using Shopping.Repository.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Shopping.Models
{
    public class ProductModel
    {
		[Key]
		public int Id { get; set; }
		[Required, MinLength(4, ErrorMessage = "Please provide name")]
		public string Name { get; set; }
		[Required, MinLength(4, ErrorMessage = "Please provide description")]
		public string Description { get; set; }
		public string Slug { get; set; }
		[Required(ErrorMessage = "Please provide price")]
		[Range(0.01,double.MaxValue)]
		[Column(TypeName ="decimal(8,2)")]
		public decimal Price { get; set; }
		[Required, Range(1,int.MaxValue,ErrorMessage ="Please choose brand")]
		public int BrandId {  get; set; }
		[Required, Range(1, int.MaxValue, ErrorMessage = "Please choose category")]
		public int  CategoryId  { get; set; }
		public CategoryModel Category { get; set; }
		public BrandModel Brand { get; set; }
		public string Image { get; set; }
		[NotMapped]
		[FileExtension]
		public IFormFile? ImageUpload {  get; set; }
	}
}
