using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace CodePulse.API.Models.Dto
{
    public class LoginResponseDto
    {
        public string  Email { get; set; }

        public string Token { get; set; }

        public  List<string> Roles { get; set;  }
    }
}
