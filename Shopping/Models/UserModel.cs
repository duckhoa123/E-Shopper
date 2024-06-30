using System.ComponentModel.DataAnnotations;

namespace Shopping.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage="Please enter username")]
		public string Username { get; set; }
		[Required(ErrorMessage = "Please enter email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Please enter password")]
		public string Password { get; set; }
		
	}
}
