using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Services.Cobranca;
using System.Globalization;

namespace AppMarciusMagazine.Views.Cobranca;

public partial class VInfoClienteHistorico : ContentPage
{
    APIClientes apiCliente = new APIClientes();
    ClientesClass listasuporte = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();

    private CancellationTokenSource _cancellationTokenSource;

    public VInfoClienteHistorico(ClientesClass clientesClasses, OcorrenciaClass _ocorrencia)
    {
        InitializeComponent();
        listasuporte = clientesClasses;
        ocorrencia = _ocorrencia;

        refreshView.Command = new Command(async () => await AtualizarPagina());
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await CarregaListas();
    }

    private async Task AtualizarPagina()
    {
        await CarregaListas();

        refreshView.IsRefreshing = false;
    }

    private async Task CarregaListas()
    {
        try
        {
            _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
            _cancellationTokenSource = new CancellationTokenSource();

            GridPrincipal.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            List<InfoHistoricoCliente> infoHistorico = await apiCliente.HistoricoPedidosCliente(listasuporte.Cliente.Codcliente, _cancellationTokenSource.Token);
            List<InfoHistoricoCliente> infoHistoricoNovo = new List<InfoHistoricoCliente>();

            int contVencidas = 0;

            if (infoHistorico != null && infoHistorico.Count > 0)
            {
                GridPrincipal.IsVisible = true;
                GridSecundario.IsVisible = false;

                foreach (var item in infoHistorico)
                {
                    Color colorBack = Color.FromHex("#4d4d4d");
                    Color colorBorder = Color.FromHex("#808080");

                    string status = "NORMAL";

                    if (item.pago == "N" && item.vencimento <= DateOnly.FromDateTime(DateTime.Now.Date))
                    {
                        colorBack = Color.FromHex("#e67700");
                        colorBorder = Color.FromHex("#994200");
                        status = "DEVE";
                        contVencidas++;
                    }

                    if (item.pago == "A")
                    {
                        colorBack = Color.FromHex("#338cc3");
                        colorBorder = Color.FromHex("#346d90");
                        status = "ACERTO";
                    }

                    if (item.pago == "S")
                    {
                        colorBack = Color.FromHex("#0a5235");
                        colorBorder = Color.FromHex("#022914");
                        status = "PAGO";
                    }

                    infoHistoricoNovo.Add(new InfoHistoricoCliente
                    {
                        CorBack = colorBack,
                        CorBorder = colorBorder,
                        codpedido = item.codpedido,
                        vencimento = item.vencimento,
                        atraso = item.atraso,
                        valor = item.valor,
                        pago = status,
                        qtdpedido = item.qtdpedido,
                        valorgasto = item.valorgasto,
                        valorpago = item.valorpago
                    });
                }

                cardInfoHistorico.ItemsSource = infoHistoricoNovo;

                Decimal? valorgasto = infoHistoricoNovo[0].valorgasto;

                lblValorGasto.Text = "R$ 0,00";

                if (valorgasto.HasValue)
                {
                    lblValorGasto.Text = valorgasto.Value.ToString("C", new CultureInfo("pt-BR"));
                }

                lblQtdPedido.Text = infoHistoricoNovo[0].qtdpedido.ToString();
                lblVencidos.Text = contVencidas.ToString();

                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                GridPrincipal.IsVisible = true;
            }
            else
            {
                LoadingIndicator.IsVisible = false;
                LoadingIndicator.IsRunning = false;
                GridPrincipal.IsVisible = false;
                GridSecundario.IsVisible = true;
            }
        }
        catch (OperationCanceledException)
        {
            // operação foi cancelada, não faça nada
        }
        catch (Exception ex)
        {
            // trate outros tipos de exceções conforme necessário
            return;
        }
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
        await Navigation.PushAsync(new VInfoCliente(ocorrencia));
    }

    private async void btnProximo_Clicked(object sender, EventArgs e)
    {
        _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
        await Navigation.PushAsync(new VInfoClienteDois(listasuporte, ocorrencia));
    }

    
}