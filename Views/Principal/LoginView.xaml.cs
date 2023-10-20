using Android.Content;
using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Platforms.Android;
using AppMarciusMagazine.Services.Principal;
using AppMarciusMagazine.Suporte;
using System.Diagnostics;

namespace AppMarciusMagazine.Views.Principal;

public partial class LoginView : ContentPage
{
    #region 1- LOG
    APIErroLog error = new();

    private async Task MetodoErroLog(Exception ex)
    {
        var erroLog = new ErrorLogClass
        {
            Erro = ex.Message, // Obt�m a mensagem de erro
            Metodo = ex.TargetSite.Name, // Obt�m o nome do m�todo que gerou o erro
            Dispositivo = DeviceInfo.Model, // Obt�m o nome do dispositivo em execu��o
            Versao = DeviceInfo.Version.ToString(), // Obt�m a vers�o do dispostivo
            Plataforma = DeviceInfo.Platform.ToString(), // Obt�m o sistema operacional do dispostivo
            TelaClasse = GetType().FullName, // Obt�m o nome da tela/classe
            Data = DateTime.Now,
        };

        await error.LogErro(erroLog);
    }
    #endregion

    #region 2- VARIAVEIS
    APIUser aPIUser = new APIUser();
    APIVersaoApp versaoApp = new APIVersaoApp();
    AppShell appShell = (AppShell)Application.Current.MainPage;
    #endregion

    #region 3- CLASSES

    #endregion

    #region 4- METODOS CONSTRUTORES
    public LoginView()
    {
        try
        {
            InitializeComponent();
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            await Inicializa();
            await MetodosIniciais();
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 5- METODOS
    private async Task MetodosIniciais()
    {
        try
        {
            //Redimensionamento de logo
            logoSize.WidthRequest = ResponsiveAuto.Width(1.4);

            //Exibe label com vers�o do APP
            await ExibeVersao();
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task AuthenticateSavedCredentials()
    {
        try
        {
            btnEntrar.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var savedUsername = await SecureStorage.GetAsync("Username");
            var savedPassword = await SecureStorage.GetAsync("Password");

            UsernameEntry.Text = savedUsername;
            PasswordEntry.Text = savedPassword;

            var user = new Login
            {
                usuario = savedUsername,
                senha = savedPassword
            };

            if (await aPIUser.ValidaUser(user))
            {
                var intent = new Intent(Android.App.Application.Context, typeof(OcorrenciaService));
                Android.App.Application.Context.StartService(intent);

                appShell.Permissao();

                // Definir a nova p�gina principal ap�s o login
                await Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
            }

            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            btnEntrar.IsVisible = true;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task<bool> CheckSavedCredentials()
    {
        try
        {
            return !string.IsNullOrEmpty(SecureStorage.GetAsync("Username").Result) &&
               !string.IsNullOrEmpty(SecureStorage.GetAsync("Password").Result);
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return false;
        }
    }

    private async Task ExibeVersao()
    {
        try
        {
            lblInfoDev.Text = "Marciu's Magazine @Todos os direitos reservados";
            lblInfoDevVersao.Text = $"Vers�o {AppInfo.Version}";
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task Inicializa()
    {
        try
        {

            // Desabilita o menu lateral
            App.Current.MainPage.SetValue(Shell.FlyoutBehaviorProperty, FlyoutBehavior.Disabled);
            ShowPasswordButton.Source = "eyeclose.svg";

            // Verifica se as credenciais est�o salvas
            if (await CheckSavedCredentials())
            {
                // Autentica automaticamente
                await AuthenticateSavedCredentials();
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 6- EVENTOS DE CONTROLE
    private async void OnLoginClicked(object sender, EventArgs e)
    {
        try
        {
            btnEntrar.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            var user = new Login
            {
                usuario = UsernameEntry.Text,
                senha = PasswordEntry.Text
            };

            if (CredentialsSwitch.IsToggled)
            {
                // Limpar usu�rio e senha armazenados
                SecureStorage.Remove("Username");
                SecureStorage.Remove("Password");
                SecureStorage.RemoveAll();

                InfoGlobal.ClearData();

                if (await aPIUser.ValidaUser(user))
                {
                    var intent = new Intent(Android.App.Application.Context, typeof(OcorrenciaService));
                    Android.App.Application.Context.StartService(intent);

                    appShell.Permissao();

                    // Definir a nova p�gina principal ap�s o login
                    await Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
                }
                else
                {
                    await DisplayAlert("Erro", "Credenciais inv�lidas", "OK");
                }
            }
            else
            {
                if (await aPIUser.ValidaUser(user))
                {
                    var intent = new Intent(Android.App.Application.Context, typeof(OcorrenciaService));
                    Android.App.Application.Context.StartService(intent);

                    // Armazenar usu�rio e senha
                    await SecureStorage.SetAsync("Username", user.usuario);
                    await SecureStorage.SetAsync("Password", user.senha);

                    appShell.Permissao();

                    // Definir a nova p�gina principal ap�s o login
                    await Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
                }
                else
                {
                    await DisplayAlert("Erro", "Credenciais inv�lidas", "OK");
                }
            }

            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            btnEntrar.IsVisible = true;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void OnShowPasswordButtonClicked(object sender, EventArgs e)
    {
        try
        {
            if (PasswordEntry.IsPassword)
            {
                PasswordEntry.IsPassword = false;
                ShowPasswordButton.Source = "eyeopen.svg";
            }
            else
            {
                PasswordEntry.IsPassword = true;
                ShowPasswordButton.Source = "eyeclose.svg";
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