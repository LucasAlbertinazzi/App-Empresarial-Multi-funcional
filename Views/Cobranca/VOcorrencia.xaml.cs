using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Cobranca;
using AppMarciusMagazine.Services.Principal;
using AppMarciusMagazine.Suporte;
using AppMarciusMagazine.Views.Principal;
using System.Collections.ObjectModel;

namespace AppMarciusMagazine.Views.Cobranca;

public partial class VOcorrencia : ContentPage
{
    #region 0- LOG
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

    #region 1- PERMISSOES

    #endregion

    #region 2- VARIAVEIS
    ObservableCollection<OcorrenciaClass> infoOcorrencias = new ObservableCollection<OcorrenciaClass>();
    APIOcorrencia api_ocorrencia = new APIOcorrencia();
    SenhaPedido senhaPedido = new SenhaPedido();
    #endregion

    #region 3- CLASSES

    #endregion

    #region 4- METODOS CONSTRUTORES
    [Obsolete]
    public VOcorrencia()
    {
        try
        {
            InitializeComponent();

            InfoGlobal.CodOcorrencia = 0;
            api_ocorrencia.NewOcorrenciasReceived += OnNewOcorrenciaReceived;
            api_ocorrencia.OcorrenciasAltered += OnOcorrenciasAltered;
            cardOcorrencias.ItemsSource = infoOcorrencias.Where(x => x.Tiposenha == 1);

            refreshView.Command = new Command(async () => await AtualizarLista());
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

    #region 5- METODOS
    private async Task Inicializa()
    {
        try
        {
            GridPrincipal.IsVisible = false;
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;

            await CarregaOcorrenciasIniciais();

            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            GridPrincipal.IsVisible = true;

            await Task.Run(api_ocorrencia.ListenForOcorrencias);
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    [Obsolete]
    private void OnNewOcorrenciaReceived(List<OcorrenciaClass> novasOcorrencias)
    {
        try
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                foreach (var item in novasOcorrencias)
                {
                    if (!infoOcorrencias.Any(x => x.Codsolicitacao == item.Codsolicitacao))
                    {
                        string nomeClienteFormatado = item.NomeCliente.TrimStart().ToUpper();

                        infoOcorrencias.Add(new OcorrenciaClass
                        {
                            Codsolicitacao = item.Codsolicitacao,
                            Loja = item.Loja,
                            NomeCliente = nomeClienteFormatado,
                            Tipo = item.Tipo,
                            Codigo = item.Codigo,
                            Datasolicitacao = item.Datasolicitacao,
                            Datasenha = item.Datasenha,
                            Analista = item.Analista,
                            Usuario = item.Usuario,
                            Obs = item.Obs,
                            Codcliente = item.Codcliente,
                            CodLoja = item.CodLoja,
                            Tiposenha = item.Tiposenha,
                            Finalizado = item.Finalizado,
                            Solicitante = item.Solicitante,
                            Cancelado = item.Cancelado,
                        });
                    }
                }

                if (infoOcorrencias.Count == 1)
                {
                    Task.Run(async () => await ReloadPage());
                }
            });
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    [Obsolete]
    private void OnOcorrenciasAltered(List<OcorrenciaClass> ocorrenciasAlteradas)
    {
        try
        {
            Device.InvokeOnMainThreadAsync(() =>
            {
                foreach (var ocorrenciaAlterada in ocorrenciasAlteradas)
                {
                    var itemToRemove = infoOcorrencias.FirstOrDefault(x => x.Codsolicitacao == ocorrenciaAlterada.Codsolicitacao);
                    if (itemToRemove != null)
                    {
                        infoOcorrencias.Remove(itemToRemove);
                    }
                }

                if (infoOcorrencias.Count <= 0)
                {
                    Task.Run(async () => await ReloadPage());
                }
            });
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    private async Task CarregaOcorrenciasIniciais()
    {
        try
        {
            var lista = await api_ocorrencia.BuscaOcorrencias();
            await UpdateOcorrencias(lista);
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task UpdateOcorrencias(List<OcorrenciaClass> lista)
    {
        try
        {
            cardOcorrencias.ItemsSource = null;
            infoOcorrencias.Clear();

            if (lista != null && lista.Count > 0)
            {
                foreach (var item in lista)
                {
                    string nomeClienteFormatado = item.NomeCliente.TrimStart().ToUpper();

                    infoOcorrencias.Add(new OcorrenciaClass
                    {
                        Codsolicitacao = item.Codsolicitacao,
                        Loja = item.Loja,
                        NomeCliente = nomeClienteFormatado,
                        Tipo = item.Tipo,
                        Codigo = item.Codigo,
                        Datasolicitacao = item.Datasolicitacao,
                        Datasenha = item.Datasenha,
                        Analista = item.Analista,
                        Usuario = item.Usuario,
                        Obs = item.Obs,
                        Codcliente = item.Codcliente,
                        CodLoja = item.CodLoja,
                        Tiposenha = item.Tiposenha,
                        Finalizado = item.Finalizado,
                        Solicitante = item.Solicitante,
                        Cancelado = item.Cancelado,
                    });
                }

                cardOcorrencias.ItemsSource = infoOcorrencias.Where(x => x.Tiposenha == 1);
                GridPrincipal.IsVisible = true;
            }
            else
            {
                GridPrincipal.IsVisible = false;
                GridSecundario.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        try
        {
            if (InfoGlobal.isMenuOpen)
            {
                Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
            }
            else
            {
                Shell.Current.FlyoutIsPresented = false;
            }

            return true;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return true;
        }
    }

    [Obsolete]
    private async Task ReloadPage()
    {
        try
        {
            // Se necessário, obtenha novos dados antes de atualizar o layout.
            List<OcorrenciaClass> novosItens = await ObterNovosItens();

            // Atualize na Thread da UI para evitar problemas de acesso cruzado a threads.
            await Device.InvokeOnMainThreadAsync(() =>
            {
                // Limpe e atualize a CollectionView com novos dados.
                infoOcorrencias.Clear();

                foreach (var item in novosItens)
                {
                    string nomeClienteFormatado = item.NomeCliente.TrimStart().ToUpper();

                    infoOcorrencias.Add(new OcorrenciaClass
                    {
                        Codsolicitacao = item.Codsolicitacao,
                        Loja = item.Loja,
                        NomeCliente = nomeClienteFormatado,
                        Tipo = item.Tipo,
                        Codigo = item.Codigo,
                        Datasolicitacao = item.Datasolicitacao,
                        Datasenha = item.Datasenha,
                        Analista = item.Analista,
                        Usuario = item.Usuario,
                        Obs = item.Obs,
                        Codcliente = item.Codcliente,
                        CodLoja = item.CodLoja,
                        Tiposenha = item.Tiposenha,
                        Finalizado = item.Finalizado,
                        Solicitante = item.Solicitante,
                        Cancelado = item.Cancelado,
                    });
                }

                cardOcorrencias.ItemsSource = infoOcorrencias.Where(x => x.Tiposenha == 1);

            });

            if (novosItens != null)
            {
                GridPrincipal.IsVisible = true;
                GridSecundario.IsVisible = false;
            }
            else
            {
                GridSecundario.IsVisible = true;
                GridPrincipal.IsVisible = false;
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task<List<OcorrenciaClass>> ObterNovosItens()
    {
        try
        {
            return await api_ocorrencia.BuscaOcorrencias();

        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return null;
        }
    }

    private async Task AtualizarLista()
    {
        try
        {
            await CarregaOcorrenciasIniciais();
            refreshView.IsRefreshing = false;
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    protected override void OnDisappearing()
    {
        try
        {

            base.OnDisappearing();
            api_ocorrencia.NewOcorrenciasReceived -= OnNewOcorrenciaReceived;
            api_ocorrencia.OcorrenciasAltered -= OnOcorrenciasAltered;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    #endregion

    #region 6- EVENTOS DE CONTROLE

    /// <summary>
    /// Este método realiza ações diferentes baseadas no tipo de senha da ocorrência selecionada.
    /// 
    /// Tipos de senha:
    /// 1 - PEDIDO: Realiza a ação XYZ e navega para a página VInfoCliente.
    /// 2 - RENEGOCIAÇÃO: Exibe um alerta informando para continuar a operação pelo sistema.
    /// 3 - CANCELAMENTO: Exibe um alerta informando para continuar a operação pelo sistema.
    /// 4 - CADASTRO: Exibe um alerta informando para continuar a operação pelo sistema.
    /// </summary>
    private async void OnFrameTapped(object sender, TappedEventArgs e)
    {
        try
        {
            var swipeView = sender as StackLayout;
            OcorrenciaClass selecionado = swipeView?.BindingContext as OcorrenciaClass;

            if (selecionado != null)
            {
                // Navega para a página VInfoCliente se o tipo de senha for 'PEDIDO'.
                if (selecionado.Tiposenha == 1)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new VInfoCliente(selecionado));
                }
                else // Exibe um alerta para os outros tipos de senha.
                {
                    await DisplayAlert("AVISO", $"Para essa ocorrência ({selecionado.Tipo}) continue a operação pelo sistema!", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void swNegar_Clicked(object sender, EventArgs e)
    {
        try
        {
            senhaPedido = new SenhaPedido();

            var swipeItem = sender as SwipeItemView;
            var ocorrencia = swipeItem.CommandParameter as OcorrenciaClass;

            if (ocorrencia != null)
            {
                senhaPedido.Codcliente = Convert.ToInt32(ocorrencia.Codcliente);
                senhaPedido.Codprepedido = Convert.ToInt32(ocorrencia.Codigo);
                senhaPedido.Codusuario = InfoGlobal.codusuario;
                senhaPedido.Identifica = false;
                senhaPedido.Horasenha2 = DateTime.Now;
                senhaPedido.Codsolicitacao = ocorrencia.Codsolicitacao;

                ComentarioPopupFrame.HeightRequest = ResponsiveAuto.Height(3);
                ComentarioPopupFrame.WidthRequest = ResponsiveAuto.Width(1.4);
                ComentarioPopupGrid.IsVisible = true;
            }
            else
            {
                await DisplayAlert("ERRO", "Ocorreu um erro ao executar essa ação, reinicei o APP e tente novamente!", "OK");
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void swValidar_Clicked(object sender, EventArgs e)
    {
        try
        {
            senhaPedido = new SenhaPedido();

            var swipeItem = sender as SwipeItemView;
            var ocorrencia = swipeItem.CommandParameter as OcorrenciaClass;

            if (ocorrencia != null)
            {
                senhaPedido.Codcliente = Convert.ToInt32(ocorrencia.Codcliente);
                senhaPedido.Codprepedido = Convert.ToInt32(ocorrencia.Codigo);
                senhaPedido.Codusuario = InfoGlobal.codusuario;
                senhaPedido.Identifica = true;
                senhaPedido.Horasenha2 = DateTime.Now;
                senhaPedido.Codsolicitacao = ocorrencia.Codsolicitacao;

                ComentarioPopupFrame.HeightRequest = ResponsiveAuto.Height(3);
                ComentarioPopupFrame.WidthRequest = ResponsiveAuto.Width(1.4);
                ComentarioPopupGrid.IsVisible = true;
            }
            else
            {
                await DisplayAlert("ERRO", "Ocorreu um erro ao executar essa ação, reinicie o APP e tente novamente!", "OK");
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private void OnCancelarClicked(object sender, EventArgs e)
    {
        ComentarioPopupGrid.IsVisible = false;
    }

    private void OnBackgroundTapped(object sender, TappedEventArgs e)
    {
        ComentarioPopupGrid.IsVisible = false;
    }

    private async void OnSalvarClicked(object sender, EventArgs e)
    {
        try
        {
            if (!await api_ocorrencia.VerificaOcorrencia(senhaPedido.Codsolicitacao))
            {
                if (!string.IsNullOrEmpty(ComentarioEntry.Text))
                {
                    senhaPedido.Senha2obs = ComentarioEntry.Text;

                    if (senhaPedido.Identifica)
                    {
                        if (await api_ocorrencia.AprovaPedido(senhaPedido))
                        {
                            await DisplayAlert("AVISO", "Pedido APROVADO com sucesso!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("ERRO", "Ocorreu um erro ao salvar as informações, reinicie o APP e tente novamente!", "OK");
                        }

                        await CarregaOcorrenciasIniciais();
                        ComentarioEntry.Text = string.Empty;
                    }
                    else
                    {
                        if (await api_ocorrencia.NegarPedido(senhaPedido))
                        {
                            await DisplayAlert("AVISO", "Pedido NEGADO com sucesso!", "OK");
                        }
                        else
                        {
                            await DisplayAlert("ERRO", "Ocorreu um erro ao salvar as informações, reinicie o APP e tente novamente!", "OK");
                        }

                        await CarregaOcorrenciasIniciais();
                        ComentarioEntry.Text = string.Empty;
                    }

                    ComentarioPopupGrid.IsVisible = false;
                    ComentarioEntry.Text = string.Empty;
                }
                else
                {
                    await DisplayAlert("AVISO", "Observação necessária!", "OK");
                }
            }
            else
            {
                await DisplayAlert("AVISO", "Essa ocorrência ja foi finalizada!", "OK");
                return;
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