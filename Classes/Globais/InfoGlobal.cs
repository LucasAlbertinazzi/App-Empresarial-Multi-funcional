using System.Diagnostics;

namespace AppEmpresa.Classes.Globais
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

        public static string apiApp = "http://192.168.00.00:61620/api";
        public static string apiCobranca = "http://192.168.00.00:61630/api";
        public static string apiDiretoria = "http://192.168.00.00:61640/api";
        public static string apiAuditoria = "http://192.168.00.00:61650/api";

        public static string apiAppDev = "http://192.168.00.01:5050/api";
        public static string apiCobrancaDev = "http://192.168.00.01:5060/api";
        public static string apiDiretoriaDev = "http://192.168.00.01:5070/api";
        public static string apiAuditoriaDev = "http://192.168.00.01:5080/api";

        public static string apk = "https://linkdoapk";

        // Método para ajustar as URLs das APIs com base no ambiente de execução
        public static void AjustarUrlsParaDebug()
        {
            // Verifica se o aplicativo está em modo de depuração
            if (Debugger.IsAttached)
            {
                // Altera as URLs das APIs para as URLs de desenvolvimento
                apiApp = apiAppDev;
                apiCobranca = apiCobrancaDev;
                apiDiretoria = apiDiretoriaDev;
                apiAuditoria = apiAuditoriaDev;
            }
        }

        public static void ClearData()
        {
            usuario = string.Empty;
            senha = string.Empty;
            funcao = 0;
            statusCode = false;
        }
    }
}
