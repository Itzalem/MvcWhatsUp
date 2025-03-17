namespace MvcWhatsUp.Models
{
    public class LoginModel
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginModel() 
        {
            
        }

        public LoginModel (string username, string password)
        {
            UserName = username;
            Password = password;
        }

    }
}
