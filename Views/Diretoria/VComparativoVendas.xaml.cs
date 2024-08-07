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

public partial class VComparativoVendas : ContentPage
{
    #region 1 - VARIÁVEIS
    APIDiretoria api = new APIDiretoria();
    #endregion

    #region 2 - CONSTRUTORES
    public VComparativoVendas()
    {
        InitializeComponent();
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();
        Rotacionar();
        DefineData();
        CarregaLojas();
        DefineFontSize();
        await CarregaInfos();
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

    private async Task<double> FontResponsiva(int tamanho)
    {
        try
        {
            double screenWidth = DeviceDisplay.MainDisplayInfo.Width / DeviceDisplay.MainDisplayInfo.Density;

            // Ajuste o valor de acordo com suas preferências
            double fontSize = screenWidth / tamanho;

            return fontSize;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return 0;
        }
    }

    private async void DefineFontSize()
    {
        try
        {
            lblFluxoAno.FontSize = await FontResponsiva(26);
            lblQtdAno.FontSize = await FontResponsiva(26);
            lblTotalAno.FontSize = await FontResponsiva(26);
            lblCanceladoAno.FontSize = await FontResponsiva(26);
            lblSaldoAno.FontSize = await FontResponsiva(26);
            lblFluxoMes.FontSize = await FontResponsiva(26);
            lblQtdMes.FontSize = await FontResponsiva(26);
            lblTotalMes.FontSize = await FontResponsiva(26);
            lblCanceladoMes.FontSize = await FontResponsiva(26);
            lblSaldoMes.FontSize = await FontResponsiva(26);
            lblFluxoPeriodo.FontSize = await FontResponsiva(26);
            lblQtdPeriodo.FontSize = await FontResponsiva(26);
            lblTotalPeriodo.FontSize = await FontResponsiva(26);
            lblCanceladoPeriodo.FontSize = await FontResponsiva(26);
            lblSaldoPeriodo.FontSize = await FontResponsiva(26);
            lblCrecAno.FontSize = await FontResponsiva(28);
            lblCrecPorcentAno.FontSize = await FontResponsiva(28);
            lblCrecMes.FontSize = await FontResponsiva(28);
            lblCrecPorcentMes.FontSize = await FontResponsiva(28);
            lblCrecFluxoAno.FontSize = await FontResponsiva(28);
            lblCrecFluxoPorcentAno.FontSize = await FontResponsiva(28);
            lblCrecFluxoMes.FontSize = await FontResponsiva(28);
            lblCrecFluxoPorcentMes.FontSize = await FontResponsiva(28);
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void CarregaLojas()
    {
        try
        {
            List<infoLojas> infoLojas = new List<infoLojas>();

            infoLojas.Add(new infoLojas { Loja = 0, NomeLoja = "TODAS" });
            infoLojas.Add(new infoLojas { Loja = 2, NomeLoja = "LOJA 2" });
            infoLojas.Add(new infoLojas { Loja = 3, NomeLoja = "LOJA 3" });
            infoLojas.Add(new infoLojas { Loja = 4, NomeLoja = "LOJA 4" });
            infoLojas.Add(new infoLojas { Loja = 5, NomeLoja = "LOJA 5" });
            infoLojas.Add(new infoLojas { Loja = 6, NomeLoja = "LOJA 6" });
            infoLojas.Add(new infoLojas { Loja = 7, NomeLoja = "LOJA 7" });
            infoLojas.Add(new infoLojas { Loja = 8, NomeLoja = "LOJA 8" });
            infoLojas.Add(new infoLojas { Loja = 9, NomeLoja = "LOJA 9" });
            infoLojas.Add(new infoLojas { Loja = 10, NomeLoja = "LOJA 10" });
            infoLojas.Add(new infoLojas { Loja = 20, NomeLoja = "LOJA 20" });
            infoLojas.Add(new infoLojas { Loja = 21, NomeLoja = "LOJA 21" });
            infoLojas.Add(new infoLojas { Loja = 22, NomeLoja = "LOJA 22" });

            cmbLojas.ItemsSource = infoLojas;
            cmbLojas.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task CarregaInfos()
    {
        try
        {
            frameCarregando.IsVisible = true;
            stackCardsUm.IsVisible = false;
            stackCardsDois.IsVisible = false;

            infoLojas lojaSelecionada = (infoLojas)cmbLojas.SelectedItem;

            string listaSeparadaPorVirgulas = string.Empty;

            if (lojaSelecionada.Loja == 0)
            {
                List<string> numeros = new List<string>();

                for (int i = 1; i <= 10; i++)
                {
                    numeros.Add(i.ToString());
                }

                numeros.Add("20");
                numeros.Add("21");
                numeros.Add("22");

                listaSeparadaPorVirgulas = string.Join(",", numeros);
            }
            else
            {
                listaSeparadaPorVirgulas = lojaSelecionada.Loja.ToString();
            }

            InfoComparativo ano = await api.CompVendAnoAntes(dpInicial.Date.ToString(), dpFinal.Date.ToString(), listaSeparadaPorVirgulas);
            InfoComparativo mes = await api.CompVendMesAntes(dpInicial.Date.ToString(), dpFinal.Date.ToString(), listaSeparadaPorVirgulas);
            InfoComparativo periodo = await api.CompVendPeridoSelect(dpInicial.Date.ToString(), dpFinal.Date.ToString(), listaSeparadaPorVirgulas);

            PreencheCards(ano, mes, periodo);

            frameCarregando.IsVisible = false;
            stackCardsUm.IsVisible = true;
            stackCardsDois.IsVisible = true;

        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void PreencheCards(InfoComparativo ano, InfoComparativo mes, InfoComparativo periodo)
    {
        try
        {
            CultureInfo cultura = new CultureInfo("pt-BR");

            //Ano
            lblFluxoAno.Text = ano.FluxoCaixa.ToString("C", cultura);
            lblQtdAno.Text = ano.QtdPedidos.ToString();
            lblTotalAno.Text = ano.TotalPedidos.ToString("C", cultura);
            lblCanceladoAno.Text = ano.Cancelado.ToString("C", cultura);
            lblSaldoAno.Text = ano.SaldoPedidos.ToString("C", cultura);

            //Mês
            lblFluxoMes.Text = mes.FluxoCaixa.ToString("C", cultura);
            lblQtdMes.Text = mes.QtdPedidos.ToString();
            lblTotalMes.Text = mes.TotalPedidos.ToString("C", cultura);
            lblCanceladoMes.Text = mes.Cancelado.ToString("C", cultura);
            lblSaldoMes.Text = mes.SaldoPedidos.ToString("C", cultura);

            //Periodo
            lblFluxoPeriodo.Text = periodo.FluxoCaixa.ToString("C", cultura);
            lblQtdPeriodo.Text = periodo.QtdPedidos.ToString();
            lblTotalPeriodo.Text = periodo.TotalPedidos.ToString("C", cultura);
            lblCanceladoPeriodo.Text = periodo.Cancelado.ToString("C", cultura);
            lblSaldoPeriodo.Text = periodo.SaldoPedidos.ToString("C", cultura);

            // Crescimento de Pedidos
            Decimal CrecAno = periodo.SaldoPedidos - ano.SaldoPedidos;
            lblCrecAno.Text = CrecAno.ToString("C", cultura);
            double CrecPorcentAno = ano.SaldoPedidos != 0 ? ((double)(CrecAno / ano.SaldoPedidos) * 100) : 0;
            lblCrecPorcentAno.Text = CrecPorcentAno.ToString("F2") + "%";

            Decimal CrecMes = periodo.SaldoPedidos - mes.SaldoPedidos;
            lblCrecMes.Text = CrecMes.ToString("C", cultura);
            double CrecPorcentMes = mes.SaldoPedidos != 0 ? ((double)(CrecMes / mes.SaldoPedidos) * 100) : 0;
            lblCrecPorcentMes.Text = CrecPorcentMes.ToString("F2") + "%";

            // Crescimento de Fluxo de Caixa
            Decimal CrecFluxoAno = periodo.FluxoCaixa - ano.FluxoCaixa;
            lblCrecFluxoAno.Text = CrecFluxoAno.ToString("C", cultura);
            double CrecFluxoPorcentAno = ano.FluxoCaixa != 0 ? ((double)(CrecFluxoAno / ano.FluxoCaixa) * 100) : 0;
            lblCrecFluxoPorcentAno.Text = CrecFluxoPorcentAno.ToString("F2") + "%";

            Decimal CrecFluxoMes = periodo.FluxoCaixa - mes.FluxoCaixa;
            lblCrecFluxoMes.Text = CrecFluxoMes.ToString("C", cultura);
            double CrecFluxoPorcentMes = mes.FluxoCaixa != 0 ? ((double)(CrecFluxoMes / mes.FluxoCaixa) * 100) : 0;
            lblCrecFluxoPorcentMes.Text = CrecFluxoPorcentMes.ToString("F2") + "%";

        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

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
    #endregion

    #region 3.2 - LOG
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
    private async void dpInicial_DateSelected(object sender, DateChangedEventArgs e)
    {
        try
        {
            if (dpFinal.Date < dpInicial.Date)
            {
                dpFinal.Date = dpInicial.Date.AddDays(1);
            }

        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void dpFinal_DateSelected(object sender, DateChangedEventArgs e)
    {
        try
        {
            if (dpFinal.Date < dpInicial.Date)
            {
                dpFinal.Date = dpInicial.Date.AddDays(1);
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void btnConsultar_Clicked(object sender, EventArgs e)
    {
        try
        {
            CarregaInfos();
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }
    #endregion
}