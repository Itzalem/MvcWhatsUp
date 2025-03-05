namespace MvcWhatsUp.Models
{
	public class User
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public string MobileNumber { get; set; }
		public string EmailAddress { get; set; }
		public string Password { get; set; }

		public User()
		{
		}

		public User (int id, string name, string mobileNumber, string emailAddress, string password)
		{
			UserId = id;
			UserName = name;
			MobileNumber = mobileNumber;
			EmailAddress = emailAddress;
			Password = password;
		}
	}
}
