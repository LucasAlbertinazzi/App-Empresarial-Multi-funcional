﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Classes.API.Principal
{
    public class ErrorLogClass
    {
        public int Id { get; set; }
        public string Metodo { get; set; }
        public string Erro { get; set; }
        public string TelaClasse { get; set; }
        public string Dispositivo { get; set; }
        public DateTime? Data { get; set; }
        public string Plataforma { get; set; }
        public string Versao { get; set; }
    }
}
