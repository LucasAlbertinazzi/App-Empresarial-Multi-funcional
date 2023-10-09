using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Services.Cobranca;
using AppMarciusMagazine.Suporte;
using System.Net.Sockets;

namespace AppMarciusMagazine.Views.Cobranca;

public partial class VInfoClienteQuatro : ContentPage
{
    ClientesClass listacliente = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();

    APIOcorrencia apiOcorrencia = new APIOcorrencia();
    APIClientes apiCliente = new APIClientes();

    public VInfoClienteQuatro(ClientesClass lista, OcorrenciaClass _ocorrencia)
    {
        InitializeComponent();
        listacliente = lista;
        ocorrencia = _ocorrencia;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await Metodos();
    }

    private async Task Metodos()
    {
        frameAtenuentes.HeightRequest = ResponsiveAuto.Height(3);
        frameAgravantes.HeightRequest = ResponsiveAuto.Height(3);

        ScoreHist scoreHist = await apiOcorrencia.BuscaScoreHist(ocorrencia.Codigo);

        lblAgravantes.Text = "Não existe agravantes.";
        lblAtenuentes.Text = "Não existe atenuentes.";

        if (scoreHist != null)
        {
            lblAgravantes.Text = scoreHist.Agravantes.ToUpper();
            lblAtenuentes.Text = scoreHist.Atenuentes.ToUpper();
        }
    }

    private async void btnInicio_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VInfoCliente(ocorrencia));
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VInfoClienteTres(listacliente, ocorrencia));
    }

    private async void btnSolicitaContrato_Clicked(object sender, EventArgs e)
    {
        bool resposta = await DisplayAlert("AVISO", "Deseja solicitar a impressão do contrato de venda desse pedido?", "SIM", "NÃO");

        if (resposta)
        {
            int iRetorno = await apiCliente.SolicitaContrato(Convert.ToInt32(ocorrencia.Codigo));

            if (iRetorno > 0)
            {
                await DisplayAlert("AVISO", "Contrato solicitado com sucesso!", "OK");
            }
            else
            {
                await DisplayAlert("AVISO", "Erro ao solicitar contrato!", "OK");
            }
        }
    }
}