using AppEmpresa.Classes.API.Cobranca;
using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Services.Cobranca;
using AppEmpresa.Services.Principal;
using AppEmpresa.Suporte;

namespace AppEmpresa.Views.Cobranca;

public partial class VInfoClienteDois : ContentPage
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
    ClientesClass listasuporte = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();
    APIPedidos apiPedidos = new APIPedidos();

    int tipopagina;
    #endregion

    #region 2- CLASSES

    #endregion

    #region 3- METODOS CONSTRUTORES
    public VInfoClienteDois(ClientesClass lista, OcorrenciaClass _ocorrencia, int tipo_pagina, long? prepedido)
    {
        try
        {
            InitializeComponent();

            if(tipo_pagina != 1)
            {
                listasuporte = lista;
                ocorrencia = _ocorrencia;
            }
            else
            {
                tipopagina = tipo_pagina;
                listasuporte = lista;
                ocorrencia = new OcorrenciaClass();
                ocorrencia.Codigo = prepedido.ToString();
                btnProximo.IsVisible = false;
            }
            

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
            await Inicializa();
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }

    }
    #endregion

    #region 4- METODOS
    private async Task Inicializa()
    {
        try
        {
            GridPrincipal.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            await CarregaListas();
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
            if (!string.IsNullOrEmpty(ocorrencia.Codigo))
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

                    if (infoParcelas != null && infoParcelas.Count > 0)
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
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task AtualizarPagina()
    {
        try
        {
            await Inicializa();

            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 5- EVENTOS DE CONTROLE
    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if(tipopagina != 1)
            {
                await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia, false, 0));
            }
            else
            {
                if(listasuporte.TipoCliente == "TITULAR")
                {
                    await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia, false, 1));
                }
                else
                {
                    await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia, true, 1));
                }
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
            await Navigation.PushAsync(new VInfoClienteTres(listasuporte, ocorrencia));
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }
    #endregion
}