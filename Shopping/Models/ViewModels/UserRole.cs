using System.ComponentModel.DataAnnotations;

namespace Shopping.Models.ViewModels
{
	public class UserRole

	{
		public string Id { get; set; }
		[Required(ErrorMessage = "Please enter username")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Please enter email"), EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Please enter password")]
		public string Password { get; set; }
		public string Role {  get; set; }

	}
}
