using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Geral;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Principal;
using Newtonsoft.Json;

namespace AppEmpresa.Services.Diretoria
{
    public class APIDiretoria
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
                Data = DateTime.Now
            };

            await error.LogErro(erroLog);
        }
        #endregion

        #region 2- API
        private HttpClient _httpClient;

        public APIDiretoria()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 300) };
        }

        public async Task<List<GraficosInfos>> CarregaGraficos(string dataInicio, string dataFinal)
        {
            try
            {
                string uri = $"{InfoGlobal.apiDiretoria}/Diretoria/total-vendido?dataInicio={Uri.EscapeDataString(dataInicio)}&dataFim={Uri.EscapeDataString(dataFinal)}";

                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var graficosInfos = JsonConvert.DeserializeObject<List<GraficosInfos>>(content);
                    return graficosInfos;
                }

                return new List<GraficosInfos>();
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return new List<GraficosInfos>();
            }
        }

        public async Task<InfoComparativo> CompVendMesAntes(string dataInicio, string dataFinal, string loja)
        {
            try
            {
                string uri = $"{InfoGlobal.apiDiretoria}/Diretoria/comp-vendido-mes-antes?dataInicio={Uri.EscapeDataString(dataInicio)}&dataFim={Uri.EscapeDataString(dataFinal)}&loja={loja}";

                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var graficosInfos = JsonConvert.DeserializeObject<InfoComparativo>(content);
                    return graficosInfos;
                }

                return new InfoComparativo();
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return new InfoComparativo();
            }
        }

        public async Task<InfoComparativo> CompVendAnoAntes(string dataInicio, string dataFinal, string loja)
        {
            try
            {
                string uri = $"{InfoGlobal.apiDiretoria}/Diretoria/comp-vendido-ano-antes?dataInicio={Uri.EscapeDataString(dataInicio)}&dataFim={Uri.EscapeDataString(dataFinal)}&loja={loja}";

                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var graficosInfos = JsonConvert.DeserializeObject<InfoComparativo>(content);
                    return graficosInfos;
                }

                return new InfoComparativo();
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return new InfoComparativo();
            }
        }

        public async Task<InfoComparativo> CompVendPeridoSelect(string dataInicio, string dataFinal, string loja)
        {
            try
            {
                string uri = $"{InfoGlobal.apiDiretoria}/Diretoria/comp-vendido-periodo-select?dataInicio={Uri.EscapeDataString(dataInicio)}&dataFim={Uri.EscapeDataString(dataFinal)}&loja={loja}";

                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var graficosInfos = JsonConvert.DeserializeObject<InfoComparativo>(content);
                    return graficosInfos;
                }

                return new InfoComparativo();
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return new InfoComparativo();
            }
        }
        #endregion
    }
}
