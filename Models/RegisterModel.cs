﻿namespace TodoApp.Models
{
    public class RegisterModel
    {
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}