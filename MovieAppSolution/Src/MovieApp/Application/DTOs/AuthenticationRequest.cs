using System;
using System.Collections.Generic;
using System.Text;

namespace MovieApp.Application.DTOs
{
    public class AuthenticationRequest
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
