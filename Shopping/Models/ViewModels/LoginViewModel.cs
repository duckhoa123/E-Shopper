using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.ViewModels
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Please enter username")]
		public string Username { get; set; }
		
		[DataType(DataType.Password), Required(ErrorMessage = "Please enter password")]
		public string Password { get; set; }
		public string ReturnUrl {  get; set; }
	}
}
