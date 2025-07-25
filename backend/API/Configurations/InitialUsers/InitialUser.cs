﻿using API.Configurations.InitialUsers.Interfaces;

namespace API.Configurations.InitialUsers
{
    public class InitialUser : IInitialUser
    {
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
    }
}
