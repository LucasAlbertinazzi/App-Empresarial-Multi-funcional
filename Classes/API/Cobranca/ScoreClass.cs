using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Classes.API.Cobranca
{
    public class ScoreClass
    {
        public string? codusuario { get; set; }
        public string? codcliente { get; set; }
        public string? tipos { get; set; }
        public string? codigo { get; set; }
        public string? senha { get; set; }
        public string? codconsulta { get; set; }
        public string? cpf { get; set; }
        public string? rg { get; set; }
        public string? nome { get; set; }
        public string? uf { get; set; }
        public string? nascimento { get; set; }
        public string? situacao { get; set; }
        public string? informacao { get; set; }
        public string? informante { get; set; }
        public string? negativado { get; set; }
    }

    public class ScoreLastClass
    {
        public long Codigo { get; set; }
        public string Informante { get; set; }
        public string Situacao { get; set; }
        public string Usuario { get; set; }
        public DateTime Dataconsulta { get; set; }
        public string Pdflast { get; set; }
    }
}
