using Android.Content;
using AppEmpresarialMultFuncional.Classes.API.Cobranca;
using AppEmpresarialMultFuncional.Classes.API.Principal;
using AppEmpresarialMultFuncional.Classes.Globais;
using AppEmpresarialMultFuncional.Platforms.Android;
using AppEmpresarialMultFuncional.Services.Cobranca;
using AppEmpresarialMultFuncional.Services.Principal;
using AppEmpresarialMultFuncional.Suporte;
using AppEmpresarialMultFuncional.Views.Cobranca;
using AppEmpresarialMultFuncional.Views.Principal;

namespace AppEmpresarialMultFuncional;

public partial class AppShell : Shell
{
    #region 1- VARIAVEIS
    APIErroLog error = new();
    #endregion

    #region 2- METODOS CONSTRUTORES
    public AppShell()
    {
        InitializeComponent();
    }

   

    private async void Shell_Loaded(object sender, EventArgs e)
    {
        try
        {
            menuLateral.HeightRequest = ResponsiveAuto.Height(1.1);
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

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

    public void Permissao()
    {
        if (InfoGlobal.departamento != 25)
        {
            btnOcorrencia.IsVisible = true;
        }
        if (InfoGlobal.departamento == 1)
        {
            btnConfig.IsVisible = true;
        }
    }

    #endregion

    #region 4- EVENTOS DE CONTROLE

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        try
        {
            // Limpar usuário e senha armazenados
            SecureStorage.Remove("Username");
            SecureStorage.Remove("Password");
            SecureStorage.RemoveAll();

            var intent = new Intent(Android.App.Application.Context, typeof(OcorrenciaService));
            Android.App.Application.Context.StopService(intent);

            await Application.Current.MainPage.Navigation.PushAsync(new LoginView());
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void OnHomeClicked(object sender, EventArgs e)
    {
        try
        {
            if (InfoGlobal.isMenuOpen)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
            }
            else
            {
                Shell.Current.FlyoutIsPresented = false;
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void OnOcorrenciaClicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new VOcorrencia());
    }

    private async void OnSettingsCliked(object sender, EventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    #endregion
}
