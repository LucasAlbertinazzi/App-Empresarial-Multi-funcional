using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using Newtonsoft.Json;
using System.Text;

namespace AppEmpresa.Services.Principal
{
    public class APIErroLog
    {
        private HttpClient _httpClient;

        public APIErroLog()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
        }

        public async Task<bool> LogErro(ErrorLogClass erro)
        {
            try
            {
                await Application.Current.MainPage.DisplayAlert("Erro", $"{ObterTipoErro(erro.Erro)}", "OK");

                // Serialize o objeto versionInfo para JSON
                string json = JsonConvert.SerializeObject(erro);
                HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

                string url = InfoGlobal.apiApp + "/Log/erro";
                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                // Verifique se a resposta foi bem-sucedida
                response.EnsureSuccessStatusCode();

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string ObterTipoErro(string ex)
        {
            // Verifica o tipo da exceção para determinar o tipo de erro
            if (ex.Contains("Timeout"))
            {
                return "A conexão com a API pode estar com lentidão, agurade e tente novamente mais tarde";
            }
            else
            {
                return "Ocorreu um erro ao executar está ação, por favor, tente novamente mais tarde!";
            }
        }
    }
}
