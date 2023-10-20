using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Principal;
using AppMarciusMagazine.Views.Cobranca;

namespace AppMarciusMagazine.ViewModels.Principal
{
    public class MenuPrincipalVModel
    {
        #region 1- VARIAVEIS
        APIErroLog error = new();

        // Definição do delegate
        public delegate void ExecutaMetodo();
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
                // Cria uma instância do delegate com o nome do método
                ExecutaMetodo metodoDelegate = (ExecutaMetodo)Delegate.CreateDelegate(typeof(ExecutaMetodo), this, nomeMetodo);

                // Chama o método usando o delegate
                metodoDelegate.Invoke();
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }
        #endregion

        #region 4- EVENTOS DE CONTROLE

        public async void BuscaContatoCobranca()
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

        public async void ConsultaClientes()
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

        public async void BuscaOcorrencias()
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
    }
}
