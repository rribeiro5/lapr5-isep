namespace DDDSample1.Domain.Users
{
    public class LoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public LoginDto(string email, string password)
        {
            this.Email = email;
            this.Password = password;
        }
    }
}