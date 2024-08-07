using AppEmpresa.Classes.API.Cobranca;
using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Services.Cobranca;
using AppEmpresa.Services.Principal;
using System.Globalization;

namespace AppEmpresa.Views.Cobranca;

public partial class VInfoClienteHistorico : ContentPage
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

    #region 1- VARIAVEIS
    APIClientes apiCliente = new APIClientes();
    ClientesClass listasuporte = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();

    bool conjuge = false;
    int tipoPagina = 0;

    private CancellationTokenSource _cancellationTokenSource;
    #endregion

    #region 2- CLASSES

    #endregion

    #region 3- METODOS CONSTRUTORES
    public VInfoClienteHistorico(ClientesClass clientesClasses, OcorrenciaClass _ocorrencia, bool _conjuge, int _tipoPagina)
    {
        try
        {
            InitializeComponent();
            listasuporte = clientesClasses;
            ocorrencia = _ocorrencia;
            conjuge = _conjuge;
            tipoPagina = _tipoPagina;

            refreshView.Command = new Command(async () => await AtualizarPagina());
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
            if (tipoPagina == 1)
            {
                linha0.Height = 120;
                cmbPeriodo.SelectedIndex = 0;
                frPeriodo.IsVisible = true;
                btnProximo.IsVisible = false;
            }
            else
            {
                await CarregaListas();
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 4- METODOS

    private async Task AtualizarPagina()
    {
        try
        {
            await CarregaListas();

            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
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

            string tipoCliente = cmbPeriodo.SelectedItem.ToString();

            List<InfoHistoricoCliente> infoHistorico = new List<InfoHistoricoCliente>();

            if (!string.IsNullOrEmpty(tipoCliente))
            {
                infoHistorico = await apiCliente.HistoricoPedidosClientePeriodo(listasuporte.Cliente.Codcliente, tipoCliente, _cancellationTokenSource.Token);
            }
            else
            {
                infoHistorico = await apiCliente.HistoricoPedidosCliente(listasuporte.Cliente.Codcliente, _cancellationTokenSource.Token);
            }

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
                        valorpago = item.valorpago,
                        nomecliente = item.nomecliente,
                        prepedido = item.prepedido
                    });
                }

                if (conjuge)
                {
                    List<InfoHistoricoCliente> infoHistoricoConj = new List<InfoHistoricoCliente>();

                    infoHistoricoConj.AddRange(infoHistorico.Where(x => x.nomecliente == listasuporte.Cliente.Conjuge));

                    if (infoHistoricoConj != null && infoHistoricoConj.Count > 0)
                    {
                        cardInfoHistorico.ItemsSource = infoHistoricoConj;

                        Decimal? valorgasto = 0;
                        int qtd = 0;
                        contVencidas = 0;

                        foreach (var item in infoHistoricoConj)
                        {
                            valorgasto += item.valorgasto;
                            qtd++;

                            if (item.pago == "N" && item.vencimento <= DateOnly.FromDateTime(DateTime.Now.Date))
                            {
                                contVencidas++;
                            }
                        }

                        lblValorGasto.Text = "R$ 0,00";

                        if (valorgasto.HasValue)
                        {
                            lblValorGasto.Text = valorgasto.Value.ToString("C", new CultureInfo("pt-BR"));
                        }

                        lblQtdPedido.Text = qtd.ToString();
                        lblVencidos.Text = contVencidas.ToString();
                    }
                    else
                    {
                        LoadingIndicator.IsVisible = false;
                        LoadingIndicator.IsRunning = false;
                        GridPrincipal.IsVisible = false;
                        GridSecundario.IsVisible = true;
                    }
                }
                else
                {
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
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 5- EVENTOS DE CONTROLE

    private async void cmbPeriodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        await CarregaListas();
    }

    private async void OnFrameTapped(object sender, TappedEventArgs e)
    {
        if (tipoPagina == 1)
        {
            var frameSelecionado = sender as Frame;
            InfoHistoricoCliente selecionado = frameSelecionado?.BindingContext as InfoHistoricoCliente;

            if (selecionado != null)
            {
                await Navigation.PushAsync(new VInfoClienteDois(listasuporte, ocorrencia, 1, selecionado.prepedido));
            }
        }
    }
    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (tipoPagina != 1)
            {
                _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
                await Navigation.PushAsync(new VInfoCliente(ocorrencia));
            }
            else
            {
                _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
                await Navigation.PushAsync(new VInfoCliente(Convert.ToInt32(listasuporte.Cliente.Codcliente), 1, listasuporte.TipoCliente));
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void btnProximo_Clicked(object sender, EventArgs e)
    {
        try
        {
            _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
            await Navigation.PushAsync(new VInfoClienteDois(listasuporte, ocorrencia, 0, 0));
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion
}