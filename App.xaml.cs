using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Principal;
using System.Net;
using System.Net.NetworkInformation;

namespace AppEmpresa;

public partial class App : Application
{
    #region 1 - VARIÁVEIS
    private APIErroLog error = new APIErroLog();
    private APIVersaoApp api_versao = new APIVersaoApp();
    private Timer internetTimer;
    private Timer versionCheckTimer;
    private CancellationTokenSource cancellationTokenSource;
    #endregion

    #region 2 - CONSTRUTORES
    public App()
    {
        InitializeComponent();
        MainPage = new AppShell();
    }
    #endregion

    #region 3 - MÉTODOS

    private void DefineTemaApp()
    {
        var theme = App.Current.RequestedTheme;
        if (theme != AppTheme.Light)
        {
            App.Current.UserAppTheme = AppTheme.Light;
        }
    }

    private void VerificaIpExterno()
    {
        string ip = null;
        IPAddress deviceIp = null;

        // Obtém todas as interfaces de rede
        var interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (var interfac in interfaces)
        {
            if (interfac.OperationalStatus == OperationalStatus.Up)
            {
                var properties = interfac.GetIPProperties();

                foreach (var address in properties.UnicastAddresses)
                {
                    if (address.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        ip = address.Address.ToString();
                        deviceIp = IPAddress.Parse(ip);

                        // Verifica se o endereço IP está dentro do intervalo especificado
                        if (IsInRange(deviceIp, "192.168.10.1", "192.168.10.254"))
                        {
                            InfoGlobal.apiApp = "http://192.168.10.3:6162/api";
                            InfoGlobal.apiCobranca = "http://192.168.10.3:6163/api";
                            InfoGlobal.apiDiretoria = "http://192.168.10.3:6164/api";
                        }
                    }
                }
            }
        }
    }

    private bool IsInRange(IPAddress ip, string startIpString, string endIpString)
    {
        IPAddress startIp = IPAddress.Parse(startIpString);
        IPAddress endIp = IPAddress.Parse(endIpString);

        byte[] startBytes = startIp.GetAddressBytes();
        byte[] endBytes = endIp.GetAddressBytes();
        byte[] ipBytes = ip.GetAddressBytes();

        bool greaterOrEqualStart = true;
        bool lessOrEqualEnd = true;

        for (int i = 0; i < startBytes.Length; i++)
        {
            if (ipBytes[i] < startBytes[i])
            {
                greaterOrEqualStart = false;
                break;
            }
        }

        for (int i = 0; i < endBytes.Length; i++)
        {
            if (ipBytes[i] > endBytes[i])
            {
                lessOrEqualEnd = false;
                break;
            }
        }

        return greaterOrEqualStart && lessOrEqualEnd;
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
        DefineTemaApp();
        VerificaIpExterno();
        InfoGlobal.AjustarUrlsParaDebug();
        StartTimers();
    }

    protected override void OnSleep()
    {
        base.OnSleep();
        StopTimers();
    }

    protected override void OnResume()
    {
        base.OnResume();
        StartTimers();
    }

    private async void StartTimers()
    {
        cancellationTokenSource = new CancellationTokenSource();

        internetTimer = new Timer(async (state) =>
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            await VerificarConexaoInternet();
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(30));

        versionCheckTimer = new Timer(async (state) =>
        {
            if (cancellationTokenSource.IsCancellationRequested)
                return;

            await VerificaVersaoAPP();
        }, null, TimeSpan.Zero, TimeSpan.FromMinutes(5));
    }

    private void StopTimers()
    {
        cancellationTokenSource.Cancel();
        internetTimer.Dispose();
        versionCheckTimer.Dispose();
    }

    private async Task VerificarConexaoInternet()
    {
        try
        {
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                await Application.Current.MainPage.DisplayAlert("Sem internet", "Reconecte a internet para continuar usando o APP", "OK");
            }
            else
            {
                await VerificaVersaoAPP();
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task VerificaVersaoAPP()
    {
        try
        {
            await api_versao.VerificaIp();

            if (!await api_versao.VerificarVersaoInstalada())
            {
                await Application.Current.MainPage.DisplayAlert("Atualização Disponível", "Uma nova versão do aplicativo está disponível. Por favor, atualize para continuar.", "OK");
                await Launcher.OpenAsync(InfoGlobal.apk);
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    #endregion
}
