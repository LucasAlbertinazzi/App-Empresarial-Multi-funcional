using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Principal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Services.Cobranca
{
    public class APIScore
    {
        #region 1- LOG
        APIErroLog error = new();

        private async Task MetodoErroLog(Exception ex)
        {
            var erroLog = new ErrorLogClass
            {
                Erro = ex.Message, // Obtém a mensagem de erro
                Metodo = ex.TargetSite.Name, // Obtém o nome do método que gerou o erro
                Dispositivo = DeviceInfo.Model, // Obtém o nome do dispositivo em execução
                Versao = DeviceInfo.Version.ToString(), // Obtém a versão do dispostivo
                Plataforma = DeviceInfo.Platform.ToString(), // Obtém o sistema operacional do dispostivo
                TelaClasse = GetType().FullName, // Obtém o nome da tela/classe
                Data = DateTime.Now,
            };

            await error.LogErro(erroLog);
        }
        #endregion

        #region 2- API
        private HttpClient _httpClient;

        public APIScore()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
        }

        public async Task<Stream> CarregaPdfScore(string resultado)
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/ScoreBoaVista/carrega-pdf-score";
                var requestContent = new StringContent(JsonConvert.SerializeObject(new { resultado }), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, requestContent);
                response.EnsureSuccessStatusCode();

                // Retorna o stream do PDF
                return await response.Content.ReadAsStreamAsync();
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }


        public async Task<string> BuscaScore(ScoreClass scoreClass)
        {
            try
            {
                string url = $"{InfoGlobal.apiCobranca}/ScoreBoaVista/consulta-score" +
                    $"?codusuario={scoreClass.codusuario}" +
                    $"&codcliente={scoreClass.codcliente}" +
                    $"&tipos={scoreClass.tipos}" +
                    $"&cpf={scoreClass.cpf}" +
                    $"&rg={scoreClass.rg}" +
                    $"&nome={Uri.EscapeDataString(scoreClass.nome ?? "")}" + // EscapeDataString usado para nomes com espaços ou caracteres especiais
                    $"&uf={scoreClass.uf}" +
                    $"&nascimento={scoreClass.nascimento}";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var result = await response.Content.ReadAsStringAsync();

                if (!string.IsNullOrEmpty(result))
                {
                    return result;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return string.Empty;
            }
        }

        public async Task<ScoreLastClass> ObterUltimoScore(int codCliente, string tipo)
        {
            try
            {
                string url = $"{InfoGlobal.apiCobranca}/ScoreBoaVista/last-score?codcliente={codCliente}&tipo={tipo}";

                HttpResponseMessage response = await _httpClient.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var resultado = JsonConvert.DeserializeObject<ScoreLastClass>(responseContent);

                    return resultado;
                }

                return null;
               
            }
            catch (Exception ex)
            {
                // Tratar exceção aqui, se necessário
                return null;
            }
        }



        #endregion
    }
}
