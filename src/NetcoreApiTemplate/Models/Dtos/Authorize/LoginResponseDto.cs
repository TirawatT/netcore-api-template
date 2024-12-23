namespace NetcoreApiTemplate.Models.Dtos.Authorize
{
    public class LoginResponseDto
    {

        public string? Username { get; set; }
        public string? Email { get; set; }
        public string[] Roles { get; set; }
        public string? Token { get; set; }
        public LoginResponseDto()
        {
            this.Roles = Array.Empty<string>();
        }
    }
}
