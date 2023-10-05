using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Principal;
using Newtonsoft.Json;

namespace AppMarciusMagazine.Services.Cobranca
{
    public class APIClientes
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

        public APIClientes()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 30) };
        }

        public async Task<ClientesClass> BuscaInfoClientes(long codCliente)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Clientes/busca-cliente?codcliente=" + codCliente + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<ClientesClass>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<List<InfoHistoricoCliente>> HistoricoPedidosCliente(long codCliente, CancellationToken cancellationToken)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Pedidos/historico-pedidos?codcliente=" + codCliente + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri, cancellationToken);

                // Verificar se o cancelamento foi solicitado
                cancellationToken.ThrowIfCancellationRequested();

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync(cancellationToken); // Passar o token aqui também, se possível.

                    return JsonConvert.DeserializeObject<List<InfoHistoricoCliente>>(responseContent);
                }

                return null;
            }
            catch (OperationCanceledException)
            {
                // O método foi cancelado, não precisa registrar erro
                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }


        public async Task<string> BuscaFotoCliente(long cod, string tipo)
        {
            try
            {
                string directoryPath = cod + "-" + tipo + ".jpg";
                string url = InfoGlobal.apiCobranca + $"/Clientes/foto-cliente?arquivo={directoryPath}";

                using (var httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync($"{url}");

                    if (response.IsSuccessStatusCode)
                    {
                        byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                        return await SalvaImgLocalAsync(imageBytes, directoryPath);
                    }
                }

                return "figura.svg";
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return "figura.svg";
            }
        }


        private async Task<string> SalvaImgLocalAsync(byte[] imageBytes, string itemName)
        {
            try
            {
                string localAppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string localImagePath = System.IO.Path.Combine(localAppDataFolder, itemName);

                using (FileStream destinationStream = new FileStream(localImagePath, FileMode.Create, FileAccess.Write))
                {
                    await destinationStream.WriteAsync(imageBytes, 0, imageBytes.Length);
                }

                return localImagePath;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return "figura.svg";
            }
        }


        #endregion

    }
}
