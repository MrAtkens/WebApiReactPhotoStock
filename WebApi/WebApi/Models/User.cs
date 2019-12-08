using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Login { get; set; }
        public string Password { get; set; }
    }
}