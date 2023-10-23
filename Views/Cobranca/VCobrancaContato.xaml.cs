using AppEmpresarialMultFuncional.Classes.API.Cobranca;
using AppEmpresarialMultFuncional.Classes.API.Principal;
using AppEmpresarialMultFuncional.Classes.Globais;
using AppEmpresarialMultFuncional.Services.Cobranca;
using AppEmpresarialMultFuncional.Services.Principal;
using AppEmpresarialMultFuncional.Suporte;

namespace AppEmpresarialMultFuncional.Views.Cobranca;

public partial class VCobrancaContato : ContentPage
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
    APICobrancaClientes apiCCliente = new();

    private readonly int codcliente;
    #endregion

    #region 3- CLASSES

    #endregion

    #region 4- METODOS CONSTRUTORES
    public VCobrancaContato(int _codcliente)
    {
        try
        {
            InitializeComponent();
            codcliente = _codcliente;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
        }
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        try
        {
            Prncipal.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            cmbProcesso.SelectedIndex = 0;
            await DefineMascara();
            await CarregaAtraso();
            await CarregaVencer();
            await CarregaHistorico();

            Prncipal.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }

    }
    #endregion

    #region 5- METODOS
    private async Task DefineMascara()
    {
        try
        {
            txbDataPrometido.Placeholder = "dd/MM/yyyy";
            txbDataPrometido.MaxLength = 10;
            txbDataPrometido.Keyboard = Keyboard.Numeric;
            txbDataPrometido.Behaviors.Add(new MascaraBehavior("data"));
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task CarregaAtraso()
    {
        try
        {
            await Task.Run(async () =>
            {
                GridAtrasoPrinc.IsVisible = true;
                GridAtrasoSec.IsVisible = false;

                CobrancaClientesClass infoPreenchida = new();

                infoPreenchida.Codcliente = codcliente;
                infoPreenchida.Vencimento = DateOnly.FromDateTime(DateTime.Now);
                infoPreenchida.Pago = 'N';
                infoPreenchida.Tipovenda = 2;
                infoPreenchida.Cancelado = 'N';

                List<CobrancaClientesClass> infoAtrasos = await apiCCliente.BuscaAtraso(infoPreenchida);

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (infoAtrasos != null && infoAtrasos.Count > 0)
                    {
                        DateTime? atual = DateTime.Now.Date;

                        foreach (var item in infoAtrasos)
                        {
                            item.Dias = (atual.Value - DateTime.Parse(item.Vencimento.ToString())).Days;
                        }

                        cardAtrasos.ItemsSource = infoAtrasos;

                        lblAtraso.Text = infoAtrasos.Count.ToString();
                    }
                    else
                    {
                        GridAtrasoPrinc.IsVisible = false;
                        GridAtrasoSec.IsVisible = true;
                    }
                });
            });
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task CarregaVencer()
    {
        try
        {
            await Task.Run(async () =>
            {
                GridVencerPrinc.IsVisible = true;
                GridVencerSec.IsVisible = false;

                CobrancaClientesClass infoPreenchida = new();

                infoPreenchida.Codcliente = codcliente;
                infoPreenchida.Vencimento = DateOnly.FromDateTime(DateTime.Now);
                infoPreenchida.Tipovenda = 2;
                infoPreenchida.Cancelado = 'N';

                List<CobrancaClientesClass> infoVencer = await apiCCliente.BuscaVencer(infoPreenchida);

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (infoVencer != null && infoVencer.Count > 0)
                    {
                        DateTime? atual = DateTime.Now.Date;

                        foreach (var item in infoVencer)
                        {
                            item.Dias = (atual.Value - DateTime.Parse(item.Vencimento.ToString())).Days;
                        }

                        cardVencer.ItemsSource = infoVencer;

                        lblVencer.Text = infoVencer.Count.ToString();
                    }
                    else
                    {
                        GridVencerPrinc.IsVisible = false;
                        GridVencerSec.IsVisible = true;
                    }
                });
            });
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task CarregaHistorico()
    {
        try
        {
            await Task.Run(async () =>
            {
                GridHistContPrinc.IsVisible = true;
                GridHistContSec.IsVisible = false;

                CobrancaClientesClass infoPreenchida = new();

                infoPreenchida.Codcliente = codcliente;

                List<CobrancaClientesClass> infoHist = await apiCCliente.BuscaHistoricoCobranca(infoPreenchida);

                Device.BeginInvokeOnMainThread(() =>
                {
                    if (infoHist != null && infoHist.Count > 0)
                    {
                        foreach (var item in infoHist)
                        {
                            if (item.ClienteProcessado == true)
                            {
                                item.Process = "SIM";
                            }
                            else
                            {
                                item.Process = "NÃO";
                            }
                        }

                        cardHist.ItemsSource = infoHist;
                    }
                    else
                    {
                        GridHistContPrinc.IsVisible = false;
                        GridHistContSec.IsVisible = true;
                    }
                });
            });
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task GravarContato()
    {
        try
        {
            CobrancaClientesClass cobranca = new CobrancaClientesClass();
            cobranca.Codcliente = codcliente;
            cobranca.Codusuario = InfoGlobal.codusuario;
            cobranca.Datacontato = DateTime.Now.Date;
            cobranca.Descricao = txbDescricao.Text.ToUpper();

            bool process = false;
            if (cmbProcesso.SelectedIndex == 1)
            {
                process = true;
            }

            cobranca.ClienteProcessado = process;

            if (await apiCCliente.NovaCobranca(cobranca))
            {
                await DisplayAlert("AVISO", "Contato salvo com sucesso!", "OK");
                await AtualizaPagina();
            }
            else
            {
                await DisplayAlert("AVISO", "Erro ao gravar o contato. Tente novamente mais tarde!", "OK");
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task AgendaCliente()
    {
        try
        {
            if (!string.IsNullOrEmpty(txbDataPrometido.Text))
            {
                CobrancaClientesClass cobranca = new CobrancaClientesClass();
                cobranca.Codcliente = codcliente;
                cobranca.Pgtopara = DateOnly.Parse(txbDataPrometido.Text);
                cobranca.Agendacobranca = DateOnly.Parse(txbDataPrometido.Text).AddDays(1);

                if (!await apiCCliente.AgendamentoCobranca(cobranca))
                {
                    await DisplayAlert("AVISO", "Erro ao gravar o agendamento. Tente novamente mais tarde!", "OK");
                }
            }
            else
            {
                await DisplayAlert("AVISO", "Preencha a data!", "OK");
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async Task AtualizaPagina()
    {
        try
        {
            Prncipal.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            LimpaCampos();
            await CarregaAtraso();
            await CarregaVencer();
            await CarregaHistorico();

            Prncipal.IsVisible = true;
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void LimpaCampos()
    {
        try
        {
            txbDescricao.Text = string.Empty;
            txbDataPrometido.Text = string.Empty;
            cmbProcesso.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }


    #endregion

    #region 6- EVENTOS DE CONTROLE
    private async void ClickHistDescricao(object sender, TappedEventArgs e)
    {
        try
        {
            var swipeView = sender as StackLayout;
            CobrancaClientesClass selecionado = swipeView?.BindingContext as CobrancaClientesClass;

            if (selecionado != null)
            {
                lblViewDescricao.Text = selecionado.Descricao.ToUpper();
                ComentarioPopupFrame.HeightRequest = ResponsiveAuto.Height(3);
                ComentarioPopupFrame.WidthRequest = ResponsiveAuto.Width(1.4);
                ComentarioPopupGrid.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void OnOkClicked(object sender, EventArgs e)
    {
        try
        {
            ComentarioPopupGrid.IsVisible = false;
            lblViewDescricao.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void OnBackgroundTapped(object sender, TappedEventArgs e)
    {
        try
        {
            ComentarioPopupGrid.IsVisible = false;
            lblViewDescricao.Text = string.Empty;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    private async void btnSalvar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (string.IsNullOrEmpty(txbDataPrometido.Text))
            {
                await AgendaCliente();
                await GravarContato();
            }
            else
            {
                DateTime agendado = DateTime.Parse(txbDataPrometido.Text);
                TimeSpan dias = agendado - DateTime.Now.Date;

                if (dias.Days >= 0 && dias.Days < 61)
                {
                    await AgendaCliente();
                    await GravarContato();
                }
                else
                {
                    await DisplayAlert("AVISO", "Agendamento no máximo para 60 dias", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
        }
    }

    protected override bool OnBackButtonPressed()
    {
        try
        {
            Application.Current.MainPage.Navigation.PushAsync(new VBscClientes(1));

            return true;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return true;
        }
    }
    #endregion
}