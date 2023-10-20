using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Principal;
using Newtonsoft.Json;
using System.Text;

namespace AppMarciusMagazine.Services.Cobranca
{
    public class APICobrancaClientes
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

        private HttpClient _httpClient;

        public APICobrancaClientes()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 40) };
        }

        #region 1 - GET
        public async Task<List<CobrancaClientesClass>> BuscaAtraso(CobrancaClientesClass item)
        {
            try
            {
                string dataFormatada = item.Vencimento.Value.Year + "-" + item.Vencimento.Value.Month.ToString("D2") + "-" + item.Vencimento.Value.Day.ToString("D2");

                string uri = InfoGlobal.apiCobranca +
                             "/CobrancaClientes/cobranca-atraso?" +
                             $"Codcliente={item.Codcliente}&" +
                             $"Vencimento={dataFormatada}&" +
                             $"Pago={item.Pago}&" +
                             $"Tipovenda={item.Tipovenda}&" +
                             $"Cancelado={item.Cancelado}";


                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CobrancaClientesClass>>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<List<CobrancaClientesClass>> BuscaVencer(CobrancaClientesClass item)
        {
            try
            {
                string dataFormatada = item.Vencimento.Value.Year + "-" + item.Vencimento.Value.Month.ToString("D2") + "-" + item.Vencimento.Value.Day.ToString("D2");

                string uri = InfoGlobal.apiCobranca +
                            "/CobrancaClientes/cobranca-vencer?" +
                            $"Codcliente={item.Codcliente}&" +
                            $"Vencimento={dataFormatada}&" +
                            $"Tipovenda={item.Tipovenda}&" +
                            $"Cancelado={item.Cancelado}";

                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CobrancaClientesClass>>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<List<CobrancaClientesClass>> BuscaHistoricoCobranca(CobrancaClientesClass item)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca +
                            "/CobrancaClientes/cobranca-historico?" +
                            $"Codcliente={item.Codcliente}";

                var response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<List<CobrancaClientesClass>>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }
        #endregion

        #region 2 - POST
        public async Task<bool> NovaCobranca(CobrancaClientesClass item)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/CobrancaClientes/cobranca-new";

                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }

        public async Task<bool> AgendamentoCobranca(CobrancaClientesClass item)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/CobrancaClientes/cobranca-agendamento";

                // Serializa o objeto `item` para JSON e o envia como conteúdo da requisição
                var content = new StringContent(JsonConvert.SerializeObject(item), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }
        #endregion
    }
}
