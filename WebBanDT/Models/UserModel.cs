using System.ComponentModel.DataAnnotations;

namespace WebBanDT.Models
{
	public class UserModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage ="Nhập username")]
		public string UserName { get; set; }
		[Required(ErrorMessage = "Nhập email"),EmailAddress]
		public string Email { get; set; }
		[DataType(DataType.Password),Required(ErrorMessage ="Nhập mật khẩu")]
		public string Password { get; set; }


	}
}
