using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Services.Cobranca;
using AppMarciusMagazine.Suporte;

namespace AppMarciusMagazine.Views.Cobranca;

public partial class VInfoClienteDois : ContentPage
{
    ClientesClass listasuporte = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();
    APIOcorrencia apiOcorrencia = new APIOcorrencia();
    APIPedidos apiPedidos = new APIPedidos();


    public VInfoClienteDois(ClientesClass lista, OcorrenciaClass _ocorrencia)
	{
		InitializeComponent();
        listasuporte = lista;
        ocorrencia = _ocorrencia;

        refreshView.Command = new Command(async () => await AtualizarPagina());
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia, false));
    }
    
    private async void btnProximo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VInfoClienteTres(listasuporte, ocorrencia));
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await Inicializa();
    }

    private async Task Inicializa()
    {
        GridPrincipal.IsVisible = false;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        await CarregaListas();

       
    }

    private async Task CarregaListas()
    {
        if(!string.IsNullOrEmpty(ocorrencia.Codigo))
        {
            GridPrincipal.IsVisible = true;
            GridSecundario.IsVisible = false;

            List<PedidosClass> infoPedidos = await apiPedidos.BuscaInfoPedido(ocorrencia.Codigo);

            if (infoPedidos != null && infoPedidos.Count > 0)
            {
                lblNPedido.Text = ocorrencia.Codigo.ToString();
                lblTotalPedido.Text = string.Format("R$ {0:N2}", infoPedidos[0].TotalPagar);

                Decimal? totalProd = infoPedidos[0].TotalProduto;
                Decimal? desconto = infoPedidos[0].Desconto;
                Decimal? porcentagemDesconto = (desconto / totalProd) * 100;
                lblDescPorcent.Text = string.Format("{0:N2}%", porcentagemDesconto);
                lblDescValor.Text = string.Format("R$ {0:N2}", infoPedidos[0].Desconto);

                lblQtdParcelas.Text = $"Parcelas : {infoPedidos[0].Parcelas}";

                List<InfoParcelasPedido> infoParcelas = await apiPedidos.BuscaInfoParcelasPedido(ocorrencia.Codigo);

                if(infoParcelas != null && infoParcelas.Count > 0)
                {
                    cardInfoPedidos.ItemsSource = infoParcelas;
                }
            }

            List<InfoProdutosPedido> infoProdutos = await apiPedidos.BuscaInfoProdutos(ocorrencia.Codigo);

            if (infoProdutos != null && infoProdutos.Count > 0)
            {
                cardInfoProdutos.ItemsSource = infoProdutos;
            }

            listadeprodutos.HeightRequest = ResponsiveAuto.Height(3);

            GridPrincipal.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
        else
        {
            GridPrincipal.IsVisible = false;
            GridSecundario.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
    }

    private async Task AtualizarPagina()
    {
        await Inicializa();

        refreshView.IsRefreshing = false;
    }
}