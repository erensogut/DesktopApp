using System;
namespace DesktopApp.Model
{
	
        public class LoginResponse
        {
            public string? token { get; set; }

            public DateTime expiration { get; set; }
        }
    
}

