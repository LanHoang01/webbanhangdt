using System.ComponentModel.DataAnnotations;

namespace WebBanDT.Models.ViewModel
{
	public class LoginViewModel
	{
		public int Id { get; set; }
		[Required(ErrorMessage = "Nhập username")]
		public string UserName { get; set; }
		[DataType(DataType.Password), Required(ErrorMessage = "Nhập mật khẩu")]
		public string Password { get; set; }
		public string ReturnUrl { get; set; }
	}
}
