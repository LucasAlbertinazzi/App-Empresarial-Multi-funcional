using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEmpresarialMultFuncional.Classes.API.Cobranca
{
    public class CobrancaClientesClass
    {
        public int? Dias {get;set;}
        public decimal? Valor { get; set; }
        public long? Codcliente { get; set; }
        public string? Codpedido { get; set; }
        public int? Codusuario { get; set; }
        public DateOnly? Vencimento { get; set; }
        public char? Pago { get; set; }
        public int? Tipovenda { get; set; }
        public char? Cancelado { get; set; }
        public DateTime? Datacontato { get; set; }
        public string? Descricao { get; set; }
        public bool? ClienteProcessado { get; set; }
        public string? Usuario { get; set; }
        public char? Pricontato { get; set; }
        public char? Segcontato { get; set; }
        public DateOnly? Pgtopara { get; set; }
        public DateOnly? Agendacobranca { get; set; }
        public string Process { get; set; }
    }
}
