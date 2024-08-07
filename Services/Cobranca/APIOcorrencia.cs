using AppEmpresa.Classes.API.Cobranca;
using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Principal;
using AppEmpresa.Suporte;
using Newtonsoft.Json;
using System.Text;

namespace AppEmpresa.Services.Cobranca
{
    public class APIOcorrencia
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
        public delegate void NewOcorrenciasReceivedHandler(List<OcorrenciaClass> ocorrencias);
        public event NewOcorrenciasReceivedHandler NewOcorrenciasReceived;

        public delegate void OcorrenciasAlteredHandler(List<OcorrenciaClass> ocorrenciasAlteradas);
        public event OcorrenciasAlteredHandler OcorrenciasAltered;

        private HttpClient _httpClient;
        private int _reconnectionAttempts = 0;

        public APIOcorrencia()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
        }

        public async Task<List<OcorrenciaClass>> BuscaOcorrencias()
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/Ocorrencias/busca-ocorrencias";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                List<OcorrenciaClass> listaocorrencias = await response.Content.ReadAsAsync<List<OcorrenciaClass>>();

                if (listaocorrencias.Count > 0 && listaocorrencias != null)
                {
                    return listaocorrencias;
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<List<RestricaoOcorrencias>> BuscaRestricoesOcorrencia(string codPrepedido)
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/Ocorrencias/busca-restricoes-ocorrencias?codPrepedido=" + codPrepedido + "";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                List<RestricaoOcorrencias> listaocorrencias = await response.Content.ReadAsAsync<List<RestricaoOcorrencias>>();

                if (listaocorrencias.Count > 0 && listaocorrencias != null)
                {
                    return listaocorrencias;
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<SenhaNegada> BuscaSenhaNegada(long codcliente)
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/Ocorrencias/busca-senha-negada?codcliente=" + codcliente + "";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                SenhaNegada senhanegada = await response.Content.ReadAsAsync<SenhaNegada>();

                if (senhanegada != null)
                {
                    return senhanegada;
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<ScoreHist> BuscaScoreHist(string codprepedido)
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/Ocorrencias/busca-score-historico?codprepedido=" + codprepedido + "";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                ScoreHist score = await response.Content.ReadAsAsync<ScoreHist>();

                if (score != null)
                {
                    return score;
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<int?> BuscaVendedor(string codigo)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Ocorrencias/busca-vendedor?codigo=" + codigo + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<int?>(responseContent);
                }

                return 0;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return 0;
            }
        }

        public async Task<bool> AprovaPedido(SenhaPedido senhaPedido)
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/Ocorrencias/aprova-pedido";

                // Converte o objeto para JSON
                var jsonContent = JsonConvert.SerializeObject(senhaPedido);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(responseContent);
                }

                return false;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }

        public async Task<bool> NegarPedido(SenhaPedido senhaPedido)
        {
            try
            {
                string url = InfoGlobal.apiCobranca + "/Ocorrencias/negar-pedido";

                // Converte o objeto para JSON
                var jsonContent = JsonConvert.SerializeObject(senhaPedido);
                HttpContent content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<bool>(responseContent);
                }

                return false;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }

        public async Task<bool> VerificaOcorrencia(int codocorrencia)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Ocorrencias/verifica-ocorrencia?codocorrencia=" + codocorrencia + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<bool>(responseContent);
                }

                return false;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return false;
            }
        }

        public async Task ListenForOcorrencias()
        {
            int maxReconnectionAttempts = 5; // Ou obtenha de um arquivo de configuração
            string url = InfoGlobal.apiCobranca + "/Ocorrencias/busca-ocorrencias-auto";

            while (_reconnectionAttempts < maxReconnectionAttempts)
            {
                try
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, url);
                    var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);
                    response.EnsureSuccessStatusCode();

                    await using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = new StreamReader(stream))
                    {
                        while (!reader.EndOfStream)
                        {
                            var line = await reader.ReadLineAsync();
                            if (line.StartsWith("data: "))
                            {
                                var data = line.Substring(6);
                                var ocorrenciaData = JsonConvert.DeserializeObject<OcorrenciaData>(data);

                                if (ocorrenciaData.EventType == "Update")
                                    await HandleAlteredOcorrencias(ocorrenciaData.Changes);
                                else
                                    await HandleNewOcorrencias(ocorrenciaData.Changes);
                            }
                        }
                    }

                    _reconnectionAttempts = 0; // Reset das tentativas de reconexão após sucesso
                    break;
                }
                catch (HttpRequestException) // Trate exceções específicas primeiro
                {
                    await HandleErrorAndReconnect(url);
                }
                catch (Exception ex) // Trate exceções gerais por último
                {
                    await MetodoErroLog(ex);
                    await HandleErrorAndReconnect(url);
                }
            }
        }

        public class OcorrenciaData
        {
            public string EventType { get; set; }
            public List<OcorrenciaClass> Changes { get; set; }
        }

        private async Task HandleNewOcorrencias(List<OcorrenciaClass> ocorrenciaList)
        {
            try
            {
                if (ocorrenciaList != null && ocorrenciaList.Count > 0)
                {
                    var notificacao = new Notificacao();
                    await notificacao.EnviaNotificacao("Nova Ocorrência", "Uma nova ocorrência foi recebida.", "VOcorrencia");
                    NewOcorrenciasReceived?.Invoke(ocorrenciaList);
                }
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        private async Task HandleErrorAndReconnect(string url)
        {
            try
            {
                await Task.Delay(5000); // Aguarde um pouco antes de tentar reconectar
                if (_reconnectionAttempts < 5)
                {
                    _reconnectionAttempts++;
                    await ListenForOcorrencias(); // Reconecta após um erro
                }
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                await ListenForOcorrencias();
                return;
            }
        }

        private async Task HandleAlteredOcorrencias(List<OcorrenciaClass> ocorrenciaList)
        {
            try
            {
                if (ocorrenciaList != null && ocorrenciaList.Count > 0)
                {
                    OcorrenciasAltered?.Invoke(ocorrenciaList);
                }
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        #endregion
    }
}
