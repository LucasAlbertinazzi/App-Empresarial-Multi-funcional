using AppEmpresarialMultFuncional.Classes.API.Cobranca;
using AppEmpresarialMultFuncional.Classes.API.Principal;
using AppEmpresarialMultFuncional.Classes.Globais;
using AppEmpresarialMultFuncional.Services.Principal;
using Newtonsoft.Json;

namespace AppEmpresarialMultFuncional.Services.Cobranca
{
    public class APIPedidos
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

        public APIPedidos()
        {
            _httpClient = new HttpClient() { Timeout = new TimeSpan(0, 0, 60) };
        }

        public async Task<List<PedidosClass>> BuscaInfoPedido(string codigo)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Pedidos/busca-info-pedido?codigo=" + codigo + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<PedidosClass>>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<List<InfoProdutosPedido>> BuscaInfoProdutos(string codigo)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Pedidos/busca-info-produtos-pedido?codigo=" + codigo + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<InfoProdutosPedido>>(responseContent);
                }

                return null;
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return null;
            }
        }

        public async Task<string> BuscaCodPedido(string codprepedido)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Pedidos/busca-codigo-pedido?codprepedido=" + codprepedido + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<string?>(responseContent);
                }

                return "0";
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return "0";
            }
        }

        public async Task<List<InfoParcelasPedido>> BuscaInfoParcelasPedido(string codigoPrePedido)
        {
            try
            {
                string uri = InfoGlobal.apiCobranca + "/Pedidos/busca-info-parcelas-pedido?codigoPrePedido=" + Convert.ToInt32(codigoPrePedido) + "";

                HttpResponseMessage response = await _httpClient.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return JsonConvert.DeserializeObject<List<InfoParcelasPedido>>(responseContent);
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
    }
}
