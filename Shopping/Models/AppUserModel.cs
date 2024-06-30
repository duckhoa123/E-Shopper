using Microsoft.AspNetCore.Identity;

namespace Shopping.Models
{
	public class AppUserModel:IdentityUser
	{
		public string Occupation {  get; set; }
	}
}
