namespace SmartPortaria.Application.DTOs
{
    public enum TipoUsuario { Visitante = 0, Morador = 1 }

    public class UsuarioDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public TipoUsuario Tipo { get; set; }
        public string? EnderecoResidencial { get; set; }
        public string? Observacao { get; set; }
        public string? FotoBase64 { get; set; }
    }
}
