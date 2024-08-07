namespace AppEmpresa.Classes.API.Principal
{
    public class UsuarioClass
    {
        public int Codusuario { get; set; }

        public string? Usuario { get; set; }

        public int? Codloja { get; set; }

        public char? Ativo { get; set; }

        public string? Login { get; set; }

        public string? Cpf { get; set; }

        public char? Admin { get; set; }

        public int Codfuncao { get; set; }

        public char? Bloqueado { get; set; }

        public int? Ramal { get; set; }

        public string? Email { get; set; }

        public string? EMail1 { get; set; }

        public int Coddep { get; set; }

        public DateTime? Nascimentouser { get; set; }
    }

    public class Login
    {
        public string usuario { get; set; }
        public string senha { get; set; }
    }
}
