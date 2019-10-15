namespace AutenticacaoAPI.Entities
{
    public class Usuario
    {
        public int Id { get; set; }
        public string UsuarioNome { get; set; }
        public string Senha { get; set; }
        public string Token { get; set; }
    }
}