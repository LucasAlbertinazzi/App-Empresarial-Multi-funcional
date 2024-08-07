namespace AppEmpresa.Classes.Geral
{
    public class DepositoClass
    {
        public class AuditEstoqueClass
        {
            public int NumContagem { get; set; }
            public int IdLocal { get; set; }
            public string Local { get; set; }
            public int IdDivisao { get; set; }
            public string Divisao { get; set; }
            public int IdDepartamento { get; set; }
            public string Departamento { get; set; }
            public DateTime DataHora { get; set; }
        }
    }
}
