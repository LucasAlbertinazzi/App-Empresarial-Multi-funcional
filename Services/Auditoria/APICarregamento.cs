using AppEmpresa.Classes.Globais;
using Newtonsoft.Json;
using static AppEmpresa.Classes.API.Auditoria.CarregamentoClass;

namespace AppEmpresa.Services.Auditoria
{
    public class APICarregamento
    {
        private HttpClient _httpClient;

        public APICarregamento()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 20) };
        }

        public async Task<List<ListaCarregamento>> ListaDeCarregamento(string dataString, int codVeiculo, CancellationToken cancellationToken)
        {
            try
            {
                if (!string.IsNullOrEmpty(dataString))
                {
                    DateTime data;

                    if (DateTime.TryParse(dataString, out data))
                    {
                        string formattedDate = data.ToString("dd/MM/yyyy");
                        string uri = InfoGlobal.apiAuditoria + $"/Carregamento/listar-carregamento?dataString={formattedDate}&codVeiculo={codVeiculo}";

                        HttpResponseMessage response = await _httpClient.GetAsync(uri, cancellationToken);

                        // Verificar se o cancelamento foi solicitado
                        cancellationToken.ThrowIfCancellationRequested();

                        if (response.IsSuccessStatusCode)
                        {
                            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

                            return JsonConvert.DeserializeObject<List<ListaCarregamento>>(responseContent);
                        }
                    }
                }

                return null;
            }
            catch (OperationCanceledException)
            {
                // O método foi cancelado, não precisa registrar erro
                return null;
            }
            catch (Exception)
            {
                // Registrar o erro (caso necessário)
                //await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<List<Veiculos>> BuscaTransportadoras()
        {
            try

            {
                string url = InfoGlobal.apiAuditoria + "/Carregamento/carrega-veiculos";

                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                List<Veiculos> listaVeiculos = await response.Content.ReadAsAsync<List<Veiculos>>();

                if (listaVeiculos.Count > 0 && listaVeiculos != null)
                {
                    return listaVeiculos;
                }

                return new List<Veiculos>();
            }
            catch (Exception)
            {
                //await MetodoErroLog(ex);
                return new List<Veiculos>();
            }
        }

    }
}
