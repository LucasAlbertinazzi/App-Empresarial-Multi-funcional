using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEmpresarialMultFuncional.Classes.Globais
{
    public static class InfoGlobal
    {
        public static int CodOcorrencia;

        public static string LastVersao;

        public static string usuario = string.Empty;

        public static int codusuario;

        public static string senha;

        public static int funcao;

        public static int departamento;

        public static bool statusCode;

        public static bool isMenuOpen;

        public static int IdItemCamera = 0;

        public static List<string> listaImagens = new List<string>();

        public static string apiApp = "http://187.63.82.117:61620/api";
        public static string apiCobranca = "http://187.63.82.117:61630/api";
        public static string apk = "https://AppEmpresarialMultFuncional";


        public static void ClearData()
        {
            usuario = string.Empty;
            senha = string.Empty;
            funcao = 0;
            statusCode = false;
        }
    }
}
