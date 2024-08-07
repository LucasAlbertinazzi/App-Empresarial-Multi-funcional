using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Principal;
using AppEmpresa.Suporte;
using System.Diagnostics;

namespace AppEmpresa.Views.Principal;

public partial class LoginView : ContentPage
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
        InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            await Inicializa();
            await MetodosIniciais();

            if (Debugger.IsAttached)
            {
                UsernameEntry.Text = "lucas852";
                PasswordEntry.Text = "01234567L@";
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 5- METODOS

    private void RedmensionaElementos()
    {
        double screenHeight = DeviceDisplay.MainDisplayInfo.Height;
        double screenWidth = DeviceDisplay.MainDisplayInfo.Width;

        if (screenHeight <= 800 && screenWidth <= 480)
        {
            //Redimensionamento de logo
            logoSize.WidthRequest = ResponsiveAuto.Width(1.3);

            ShowPasswordButton.WidthRequest = ResponsiveAuto.Width(15);
            ShowPasswordButton.HeightRequest = ResponsiveAuto.Height(15);

            //Infos App
            lblInfoDev.FontSize = ResponsiveAuto.FontSize(13);
            lblInfoDevVersao.FontSize = ResponsiveAuto.FontSize(13);

            stackLembrar.IsVisible = false;
            CredentialsSwitch.IsToggled = true;
            UsernameEntry.Text = string.Empty;
            PasswordEntry.Text = string.Empty;
        }
        else
        {
            //Redimensionamento de logo
            logoSize.WidthRequest = ResponsiveAuto.Width(1.3);

            //Entry login
            UsernameEntry.HeightRequest = ResponsiveAuto.Height(15);

            //Entry senha
            PasswordEntry.HeightRequest = ResponsiveAuto.Height(15);
            ShowPasswordButton.WidthRequest = ResponsiveAuto.Width(14);
            ShowPasswordButton.HeightRequest = ResponsiveAuto.Height(16);

            //Salva credenciais
            lblLembrar.FontSize = ResponsiveAuto.FontSize(12);

            //Botão entrar
            btnEntrar.FontSize = ResponsiveAuto.FontSize(12);
            btnEntrar.HeightRequest = ResponsiveAuto.Height(16);

            //Infos App
            lblInfoDev.FontSize = ResponsiveAuto.FontSize(9);
            lblInfoDevVersao.FontSize = ResponsiveAuto.FontSize(9);
        }
    }

    private async Task MetodosIniciais()
    {
        try
        {
            RedmensionaElementos();

            //Exibe label com versão do APP
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

            var savedUsername = await Xamarin.Essentials.SecureStorage.GetAsync("Username");
            var savedPassword = await Xamarin.Essentials.SecureStorage.GetAsync("Password");

            UsernameEntry.Text = savedUsername;
            PasswordEntry.Text = savedPassword;

            var user = new Login
            {
                usuario = savedUsername,
                senha = savedPassword
            };

            if (await aPIUser.ValidaUser(user))
            {
                SupAndroid.NotificacaoOcorrencia();
                appShell.Permissao();

                // Definir a nova página principal após o login
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
            return !string.IsNullOrEmpty(Xamarin.Essentials.SecureStorage.GetAsync("Username").Result) &&
               !string.IsNullOrEmpty(Xamarin.Essentials.SecureStorage.GetAsync("Password").Result);
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
            lblInfoDev.Text = "Empresa @Todos os direitos reservados";
            lblInfoDevVersao.Text = $"Versão {AppInfo.Version}";
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

            // Verifica se as credenciais estão salvas
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
                // Limpar usuário e senha armazenados
                SecureStorage.Remove("Username");
                SecureStorage.Remove("Password");
                SecureStorage.RemoveAll();

                InfoGlobal.ClearData();

                if (await aPIUser.ValidaUser(user))
                {
                    if (InfoGlobal.departamento == 8 || InfoGlobal.departamento == 5 || InfoGlobal.departamento == 1)
                    {
                        SupAndroid.NotificacaoOcorrencia();
                    }

                    appShell.Permissao();

                    // Definir a nova página principal após o login
                    await Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
                }
                else
                {
                    await DisplayAlert("Erro", "Credenciais inválidas", "OK");
                }
            }
            else
            {
                if (await aPIUser.ValidaUser(user))
                {
                    SupAndroid.NotificacaoOcorrencia();

                    // Armazenar usuário e senha
                    await Xamarin.Essentials.SecureStorage.SetAsync("Username", user.usuario);
                    await Xamarin.Essentials.SecureStorage.SetAsync("Password", user.senha);

                    appShell.Permissao();

                    // Definir a nova página principal após o login
                    await Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
                }
                else
                {
                    await DisplayAlert("Erro", "Credenciais inválidas", "OK");
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