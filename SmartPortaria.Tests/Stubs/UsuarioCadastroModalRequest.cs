using System;

namespace SmartPortaria.Application.DTOs
{
    public class UsuarioCadastroModalRequest
    {
        public string? Nome { get; set; }
        public string? Documento { get; set; }
        public float[]? VetorFacial { get; set; }
        public Guid CadastradoPorId { get; set; }
    }
}
