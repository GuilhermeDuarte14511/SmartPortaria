using System;

namespace SmartPortaria.Application.DTOs
{
    public class AdminDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
