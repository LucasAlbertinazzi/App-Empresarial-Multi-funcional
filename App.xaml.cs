using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Cobranca;
using AppMarciusMagazine.Services.Principal;
using AppMarciusMagazine.Views.Cobranca;

namespace AppMarciusMagazine;

public partial class App : Application
{
    APIErroLog error = new APIErroLog();
    APIOcorrencia api_ocorrencia = new APIOcorrencia();

    public App()
    {
        InitializeComponent();

        MainPage = new AppShell();
    }

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

    protected override void OnStart()
    {
        base.OnStart();

        VerificarConexaoInternet();
    }

    public async Task VerificarConexaoInternet()
    {
        try
        {
            while (true)
            {
                // Verifica o estado da conectividade
                var current = Connectivity.NetworkAccess;

                if (current != NetworkAccess.Internet)
                {
                    // Não há conexão com a internet, exiba uma mensagem ao usuário
                    await Application.Current.MainPage.DisplayAlert("Sem internet", "Reconecte a internet para continuar usando o APP", "OK");
                }

                // Aguarda um intervalo de tempo antes de verificar novamente
                await Task.Delay(5000); // Verificar a cada 5 segundos (você pode ajustar o intervalo conforme necessário)
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    #region 4- EVENTOS DE CONTROLE
    #endregion
}
