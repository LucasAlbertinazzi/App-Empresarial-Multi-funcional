#if __ANDROID__
using Android.Content.PM;
using AppEmpresa.Platforms.Android;
#endif
using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Geral;
using AppEmpresa.Services.Diretoria;
using AppEmpresa.Services.Principal;
using System.Globalization;

namespace AppEmpresa.Views.Diretoria;

public partial class VGraficosVendas : ContentPage
{
    #region 1 - VARIÁVEIS
    List<GraficosInfos> graficos = new List<GraficosInfos>();
    APIDiretoria aPIDiretoria = new APIDiretoria();
    #endregion

    #region 2 - CONSTRUTORES
    public VGraficosVendas()
    {
        InitializeComponent();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Rotacionar();
        DefineData();
        CriaGraficos();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        RestaurarRotacao();
    }
    #endregion

    #region 3 - MÉTODOS

    #region 3.1 - ANDROID
    private void Rotacionar()
    {
#if __ANDROID__
        try
        {
            MainActivity activity = Xamarin.Essentials.Platform.CurrentActivity as MainActivity;
            if (activity != null)
            {
                activity.RequestedOrientation = ScreenOrientation.Landscape;
            }
        }
        catch (Exception ex)
        {
            string e = ex.Message;
            throw;
        }
#endif
    }

    private void RestaurarRotacao()
    {
#if __ANDROID__
        try
        {
            MainActivity activity = Xamarin.Essentials.Platform.CurrentActivity as MainActivity;
            if (activity != null)
            {
                activity.RequestedOrientation = ScreenOrientation.Portrait;
            }
        }
        catch (Exception ex)
        {
            string e = ex.Message;
            throw;
        }
#endif
    }

    #endregion

    #region 3.2 - GERAL

    private async void DefineData()
    {
        try
        {
            dpInicial.MaximumDate = DateTime.Now;
            dpInicial.Date = DateTime.Now;
            dpFinal.Date = DateTime.Now;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task<double> DefineAlturaBarras(decimal valor, decimal valorMax, double tamanhoMaximo)
    {
        try
        {
            // Calcula a proporção do valor em relação ao valor máximo
            double proporcao = (double)(valor / valorMax);

            // Multiplica a proporção pelo tamanho máximo para obter a altura da barra
            return proporcao * tamanhoMaximo;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return 0;
        }
    }

    private async Task AbasteceGraficos()
    {
        try
        {
            listGrafico.ItemsSource = null;
            graficos.Clear();
            graficos = null;
            graficos = new List<GraficosInfos>();

            graficos.AddRange(await aPIDiretoria.CarregaGraficos(dpInicial.Date.ToString(), dpFinal.Date.ToString()));

            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var screenDensity = displayInfo.Density;
            var screenHeightInDp = displayInfo.Height / screenDensity;
            double alturaMax = screenHeightInDp - 130;

            decimal valorMaior = graficos.Max(z => z.Valor);

            foreach (var item in graficos)
            {
                item.Tamanho = await DefineAlturaBarras(item.Valor, valorMaior, alturaMax);
            }

            listGrafico.HeightRequest = screenHeightInDp - 40;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task CriaGraficos()
    {
        try
        {
            infoAvisoCarregando.IsVisible = true;

            await AbasteceGraficos();

            int marginCount = 0;

            // Loop através dos dados e crie as barras
            foreach (var item in graficos)
            {
                marginCount++;

                if (marginCount <= 2)
                {
                    item.marginGrafico = new Thickness(0, 0, 5, 0);
                }

                else if (marginCount == 3)
                {
                    item.marginGrafico = new Thickness(0, 0, 30, 0);
                    marginCount = 0;
                }

                CultureInfo cultura = new CultureInfo("pt-BR");
                string valorFormatado = item.Valor.ToString("C", cultura);

                string corGrafico = "#0d2b6b";

                if (item.TipoGrafico == "Fluxo Caixa")
                {
                    corGrafico = "#fcb441";
                }
                else if (item.TipoGrafico == "Ecommerce")
                {
                    corGrafico = "#e0695c";
                }

                item.LarguraBarra = 70;
                item.CorGrafico = corGrafico;
                item.ValorFormatado = valorFormatado;
            }

            listGrafico.ItemsSource = graficos.OrderBy(g => int.Parse(g.Identificacao.Replace("Loja", "")));

            infoAvisoCarregando.IsVisible = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void MostrarAlerta(List<GraficosInfos> lista, GraficosInfos item)
    {
        try
        {
            var displayInfo = DeviceDisplay.MainDisplayInfo;
            var screenDensity = displayInfo.Density;
            var altura = displayInfo.Height / screenDensity;
            var largura = displayInfo.Width / screenDensity;

            framePopup.HeightRequest = altura * 0.75;
            framePopup.WidthRequest = largura * 0.55;
            framePopup.BorderColor = Color.Parse(item.CorGrafico);

            CultureInfo cultura = new CultureInfo("pt-BR");

            foreach (var itemCard in lista)
            {
                lblTitulo.Text = itemCard.Identificacao.ToUpper();

                if (itemCard.TipoGrafico == "Pedidos")
                {
                    lblDescricaoPedidos.Text = itemCard.TipoGrafico;
                    lblValorPedidos.Text = itemCard.Valor.ToString("C", cultura);
                }

                if (itemCard.TipoGrafico == "Fluxo Caixa")
                {
                    lblDescricaoFluxo.Text = itemCard.TipoGrafico;
                    lblValorFluxo.Text = itemCard.Valor.ToString("C", cultura);
                }

                if (itemCard.TipoGrafico == "Ecommerce")
                {
                    lblDescricaoEco.Text = itemCard.TipoGrafico;
                    lblValorEco.Text = itemCard.Valor.ToString("C", cultura);
                }
            }

            framePopup.IsVisible = true;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task<List<GraficosInfos>> AgrupaDadosGrafico(GraficosInfos garficosInfos)
    {
        try
        {
            List<GraficosInfos> agrupados = new List<GraficosInfos>();

            agrupados.AddRange(graficos.Where(x => x.Identificacao == garficosInfos.Identificacao));

            return agrupados;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return new List<GraficosInfos>();
        }
    }

    #endregion

    #region 3.3 - LOG ERRO
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
            Data = DateTime.Now
        };

        await error.LogErro(erroLog);
    }
    #endregion

    #endregion

    #region 4 - EVENTOS DE CONTROLE
    private void OnFrameTapped(object sender, TappedEventArgs e)
    {
        framePopup.IsVisible = false;
    }

    private async void MostrarPopupGraficos(object sender, TappedEventArgs e)
    {
        try
        {
            var frame = (Frame)sender;
            var graficosInfos = (GraficosInfos)frame.BindingContext;

            List<GraficosInfos> item = await AgrupaDadosGrafico(graficosInfos);

            if (item.Count > 0)
            {
                MostrarAlerta(item, graficosInfos);
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void btnConsultar_Clicked(object sender, EventArgs e)
    {
        await CriaGraficos();
    }

    private void dpInicial_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (dpFinal.Date < dpInicial.Date)
        {
            dpFinal.Date = dpInicial.Date.AddDays(1);
        }
    }

    private void dpFinal_DateSelected(object sender, DateChangedEventArgs e)
    {
        if (dpFinal.Date < dpInicial.Date)
        {
            dpFinal.Date = dpInicial.Date.AddDays(1);
        }
    }
    #endregion
}