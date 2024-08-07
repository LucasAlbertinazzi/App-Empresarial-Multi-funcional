using AppEmpresa.Classes.API.Cobranca;
using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Services.Cobranca;
using AppEmpresa.Services.Principal;
using AppEmpresa.Suporte;

namespace AppEmpresa.Views.Cobranca;

public partial class VInfoClienteQuatro : ContentPage
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

    ClientesClass listacliente = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();

    APIOcorrencia apiOcorrencia = new APIOcorrencia();
    APIClientes apiCliente = new APIClientes();
    #endregion

    #region 2- CLASSES

    #endregion

    #region 3- METODOS CONSTRUTORES
    public VInfoClienteQuatro(ClientesClass lista, OcorrenciaClass _ocorrencia)
    {
        try
        {
            InitializeComponent();
            listacliente = lista;
            ocorrencia = _ocorrencia;
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
            await Metodos();
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 4- METODOS
    private async Task Metodos()
    {
        try
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
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 5- EVENTOS DE CONTROLE
    private async void btnInicio_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new VInfoCliente(ocorrencia));
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        try
        {
            await Navigation.PushAsync(new VInfoClienteTres(listacliente, ocorrencia));
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void btnSolicitaContrato_Clicked(object sender, EventArgs e)
    {
        try
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
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion
}