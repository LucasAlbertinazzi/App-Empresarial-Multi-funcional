using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Principal;
using AppEmpresa.Suporte;
using AppEmpresa.ViewModels.Principal;
using static MenuPrincipalClass;
using static Microsoft.Maui.Controls.Button;
using static Microsoft.Maui.Controls.Button.ButtonContentLayout;

namespace AppEmpresa.Views.Principal;

public partial class VMenuPrincipal : ContentPage
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

    #region 1- VARIAVEIS

    MenuPrincipalVModel menuPrincipalModels = new MenuPrincipalVModel();
    APIMenuPrincipal aPIMenuPrincipal = new APIMenuPrincipal();

    #endregion

    #region 2- METODOS CONSTRUTORES

    public VMenuPrincipal()
    {
        InitializeComponent();
        Inicializa();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            LoadingIndicator.IsVisible = true;
            await CreateMenu();
            LoadingIndicator.IsVisible = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            throw;
        }
    }

    #endregion

    #region 3- METODOS
    private async Task Inicializa()
    {
        try
        {
            App.Current.MainPage.SetValue(Shell.FlyoutBehaviorProperty, FlyoutBehavior.Flyout);
            NavigationPage.SetHasNavigationBar(this, false);
            InfoGlobal.isMenuOpen = true;


        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }

    }

    private async Task CreateMenu()
    {
        try
        {
            // Crie os bot�es dinamicamente
            int rowIndex = 0;
            int columnIndex = 0;

            List<MenuItemModel> lista = await CarregaMenu();

            if (lista != null)
            {
                foreach (var item in lista)
                {
                    Button button = new Button();

                    button.Text = item.TextoBtn.ToUpper();
                    button.CommandParameter = item.NomeMetodo;
                    button.Clicked += MenuItem_Clicked;
                    button.Style = (Style)Application.Current.Resources["btnMenuPadrao"];

                    // Centralize horizontalmente e verticalmente o conte�do do bot�o
                    button.HorizontalOptions = LayoutOptions.CenterAndExpand;
                    button.VerticalOptions = LayoutOptions.CenterAndExpand;

                    button.ImageSource = item.CodIcone;


                    double screenHeight = DeviceDisplay.MainDisplayInfo.Height;
                    double screenWidth = DeviceDisplay.MainDisplayInfo.Width;

                    if (screenHeight <= 800 && screenWidth <= 480)
                    {
                        button.FontSize = ResponsiveAuto.FontSize(24);
                        button.Padding = new Thickness(6, 12);
                    }
                    else
                    {
                        button.FontSize = ResponsiveAuto.FontSize(16);
                        button.Padding = new Thickness(6, 25);
                    }
                    // Defina o tamanho da fonte do texto do bot�o
                    button.LineBreakMode = LineBreakMode.WordWrap;

                    // Adicione margens aos bot�es
                    if (columnIndex == 0)
                    {
                        button.Margin = new Thickness(10, 10, 5, 10); // Margem dos bot�es na coluna 0
                    }
                    else if (columnIndex == 1)
                    {
                        button.Margin = new Thickness(5, 10, 10, 10); // Margem dos bot�es na coluna 1
                    }

                    button.ContentLayout = new ButtonContentLayout(ImagePosition.Bottom, 0);

                    Grid.SetRow(button, rowIndex);
                    Grid.SetColumn(button, columnIndex);

                    mainGrid.Children.Add(button);

                    // Atualize a posi��o da linha e coluna
                    columnIndex++;
                    if (columnIndex > 1)
                    {
                        columnIndex = 0;
                        rowIndex++;
                    }

                    // Verifique se h� uma nova linha a ser criada
                    if (columnIndex == 0)
                    {
                        mainGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
                    }
                }

                // Chame o m�todo mainGrid_SizeChanged para atualizar as dimens�es dos bot�es
                mainGrid_SizeChanged(null, EventArgs.Empty);
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task<List<MenuItemModel>> CarregaMenu()
    {
        try
        {
            return await aPIMenuPrincipal.ListaMenuPrincipal(InfoGlobal.departamento);
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return null;
        }
    }

    private async Task<double> GetButtonPaddingForDevice()
    {
        try
        {
            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

            // Ajuste o valor de acordo com suas prefer�ncias
            double buttonPadding = screenWidth / 20;

            return buttonPadding;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return 0;
        }
    }

    private async Task<double> GetFontSizeForDevice()
    {
        try
        {
            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

            // Ajuste o valor de acordo com suas prefer�ncias
            double fontSize = screenWidth / 26;

            return fontSize;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return 0;
        }

    }

    protected override bool OnBackButtonPressed()
    {
        // Impedir que a a��o padr�o do bot�o "Voltar" seja executada
        return true;
    }

    #endregion

    #region 4- EVENTOS DE CONTROLE
    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
#if __ANDROID__

            LoadingIndicator.IsVisible = true;
#endif

            Button button = (Button)sender;
            string value = button.CommandParameter.ToString();

            await menuPrincipalModels.RedirecionaFuncao(value);

#if __ANDROID__
            LoadingIndicator.IsVisible = false;
#endif
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void mainGrid_SizeChanged(object sender, EventArgs e)
    {
        try
        {
            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;
            double buttonWidth = screenWidth / 2.3;

            double screenHeight = DeviceDisplay.MainDisplayInfo.Height / DeviceDisplay.MainDisplayInfo.Density;
            double buttonHeight = screenHeight / 5;

            foreach (View child in mainGrid.Children)
            {
                if (child is Button button)
                {
                    button.WidthRequest = buttonWidth;
                    button.HeightRequest = buttonHeight;
                }
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