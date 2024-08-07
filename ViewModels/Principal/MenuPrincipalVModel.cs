using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Principal;
using AppEmpresa.Views.Auditoria;
using AppEmpresa.Views.Cobranca;
using AppEmpresa.Views.Diretoria;
using System.Reflection;

namespace AppEmpresa.ViewModels.Principal
{
    public class MenuPrincipalVModel
    {
        #region 1- VARIAVEIS
        APIErroLog error = new();

        public delegate Task ExecutaMetodo();
        #endregion

        #region 2 - METODOS CONSTRUTORES

        #endregion

        #region 3- METODOS
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

        public async Task RedirecionaFuncao(string nomeMetodo)
        {
            try
            {
                // Obtém informações do método usando reflexão
                MethodInfo metodoInfo = this.GetType().GetMethod(nomeMetodo);

                if (metodoInfo != null)
                {
                    // Cria uma instância do delegate usando as informações do método
                    ExecutaMetodo metodoDelegate = (ExecutaMetodo)Delegate.CreateDelegate(typeof(ExecutaMetodo), this, metodoInfo);

                    // Chama o método usando o delegate
                    await metodoDelegate.Invoke();
                }


            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }
        #endregion

        #region 4- EVENTOS DE CONTROLE

        #region 4.1 - COBRANÇA

        public async Task BuscaContatoCobranca()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VBscClientes(1));
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        public async Task ConsultaClientes()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VBscClientes(0));
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        public async Task BuscaOcorrencias()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VOcorrencia());
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        #endregion

        #region 4.2 - DIRETORIA
        public async Task AnaliseVendas()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VGraficosVendas());
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        public async Task ComparativoVendas()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VComparativoVendas());
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }
        #endregion

        #region 4.3 - DEPÓSITO
        public async Task ExpEcomm()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VExpedicaoEcommerce());
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        public async Task CarregEcomm()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VCarregamentoEc());
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }

        public async Task DepositoAudEstoque()
        {
            try
            {
                InfoGlobal.isMenuOpen = true;
                await Application.Current.MainPage.Navigation.PushAsync(new VAuditEstoque());
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }
        #endregion
        #endregion
    }
}
