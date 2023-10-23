using AppEmpresarialMultFuncional.Classes.API.Principal;
using AppEmpresarialMultFuncional.Classes.Globais;
using AppEmpresarialMultFuncional.Services.Principal;
using System.Diagnostics;

namespace AppEmpresarialMultFuncional;

public partial class App : Application
{
    APIErroLog error = new APIErroLog();
    APIVersaoApp api_versao = new APIVersaoApp();

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

        Task.Run(async () => await VerificaInicial());
        Task.Run(async () => await VerificarConexaoInternet());
        Task.Run(async () => await VerificaVersaoAPP());
    }

    private async Task VerificarConexaoInternet()
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
                await Task.Delay(15000); // Verificar a cada 15 segundos (você pode ajustar o intervalo conforme necessário)
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task VerificaVersaoAPP()
    {
        try
        {
            while (true)
            {
                await api_versao.VerificaIp();

                if (!await api_versao.VerificarVersaoInstalada())
                {
                    if (Debugger.IsAttached)
                    {
                        await api_versao.SalvaVersao();
                    }
                    else
                    {
                        // Exibir mensagem de atualização
                        await Application.Current.MainPage.DisplayAlert("Atualização Disponível", "Uma nova versão do aplicativo está disponível. Por favor, atualize para continuar.", "OK");

                        // Abrir a URL de atualização
                        await Launcher.OpenAsync(InfoGlobal.apk);
                    }
                }

                // Aguarda um intervalo de tempo antes de verificar novamente
                await Task.Delay(100000); // Verificar a cada 1 min (você pode ajustar o intervalo conforme necessário)
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task VerificaInicial()
    {
        // Verifica o estado da conectividade
        var current = Connectivity.NetworkAccess;

        if (current != NetworkAccess.Internet)
        {
            // Não há conexão com a internet, exiba uma mensagem ao usuário
            await Application.Current.MainPage.DisplayAlert("Sem internet", "Reconecte a internet para continuar usando o APP", "OK");
        }

        await api_versao.VerificaIp();

        if (!await api_versao.VerificarVersaoInstalada())
        {
            if (Debugger.IsAttached)
            {
                await api_versao.SalvaVersao();
            }
            else
            {
                // Exibir mensagem de atualização
                await Application.Current.MainPage.DisplayAlert("Atualização Disponível", "Uma nova versão do aplicativo está disponível. Por favor, atualize para continuar.", "OK");

                // Abrir a URL de atualização
                await Launcher.OpenAsync(InfoGlobal.apk);
            }
        }
    }

    #region 4- EVENTOS DE CONTROLE
    #endregion
}
