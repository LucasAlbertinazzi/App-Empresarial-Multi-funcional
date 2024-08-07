using AppEmpresa.Services.Auditoria;
using AppEmpresa.Suporte;
using static AppEmpresa.Classes.API.Auditoria.CarregamentoClass;
using Color = Microsoft.Maui.Graphics.Color;

namespace AppEmpresa.Views.Auditoria;

public partial class VCarregamentoEc : ContentPage
{
    #region 0- LOG

    #endregion

    #region 1- VARIÁVEIS
    AudioColetor _audioColetor = new AudioColetor();
    APICarregamento apiCarregamento = new APICarregamento();
    private CancellationTokenSource _cancellationTokenSource;
    List<ListaCarregamento> listaCarregamentos = new List<ListaCarregamento>();
    List<ListaCarregamento> produtosBipados = new List<ListaCarregamento>();
    #endregion

    #region 2- CONSTRUTORES
    public VCarregamentoEc()
    {
        InitializeComponent();
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        LoadingIndicator.IsVisible = true;
        dpData.Date = DateTime.Now;
        await CarregaTransportadora();
    }
    #endregion

    #region 3- MÉTODOS
    private async Task CarregaTransportadora()
    {
        List<Veiculos> veiculos = await apiCarregamento.BuscaTransportadoras();
        cmbTransportadora.ItemsSource = veiculos;
        LoadingIndicator.IsVisible = false;
    }

    private async Task ListaCarregamento()
    {
        _cancellationTokenSource?.Cancel(); // cancela qualquer operação em andamento
        _cancellationTokenSource = new CancellationTokenSource();

        string data = dpData.Date.ToString();

        Veiculos selecionado = (Veiculos)cmbTransportadora.SelectedItem;
        listaCarregamentos = new List<ListaCarregamento>();

        if (selecionado != null)
        {
            listaCarregamentos = await apiCarregamento.ListaDeCarregamento(data, selecionado.Codigo, _cancellationTokenSource.Token);

            lblVolume.Text = listaCarregamentos.Sum(x => x.Volume).ToString();
            var distinctProducts = listaCarregamentos.Select(x => x.CodProduto).Distinct();
            lblProduto.Text = distinctProducts.Count().ToString();
        }
    }

    private async Task PreencheCampos(string codigo)
    {
        if (listaCarregamentos.Count > 0)
        {
            if (codigo.Length == 12)
            {
                if (!produtosBipados.Any(x => x.CodProduto == codigo))
                {
                    // Encontrar o índice do primeiro elemento que corresponde ao código
                    int index = listaCarregamentos.FindIndex(x => x.CodProduto == codigo);

                    lblConferido1.Text = listaCarregamentos[index].Volume.ToString();
                    lblConferido2.Text = listaCarregamentos[index].Quantidade.ToString();
                    lblCodProduto.Text = listaCarregamentos[index].CodProduto;
                    lblCodProduto.TextColor = Color.FromArgb("#021036");
                    lblDescricaoProd.Text = listaCarregamentos[index].Descricao;
                    lblDescricaoProd.TextColor = Color.FromArgb("#021036");

                    produtosBipados.Add(listaCarregamentos[index]);

                    await Task.Delay(1000);
                    txbCodigo.Text = string.Empty;
                    txbCodigo.Focus();
                    await _audioColetor.SomAcerto();

                }
                else
                {
                    lblVolume.Text = "0";
                    lblProduto.Text = "0";
                    lblCodProduto.Text = codigo;
                    lblCodProduto.TextColor = Colors.Red;
                    lblDescricaoProd.Text = "PRODUTO JÁ BIPADO!";
                    lblDescricaoProd.TextColor = Colors.Red;

                    await Task.Delay(500);
                    txbCodigo.Text = string.Empty;
                    txbCodigo.Focus();
                    await _audioColetor.SomErro();
                }
            }
        }
    }
    #endregion

    #region 4- EVENTOS DE CONTROLE
    private async void btnProdBipado_Clicked(object sender, EventArgs e)
    {
        LoadingIndicator.IsVisible = true;
        await Application.Current.MainPage.Navigation.PushAsync(new VProdutosBipados(produtosBipados));
        LoadingIndicator.IsVisible = false;
    }

    private async void txbCodigo_TextChanged(object sender, TextChangedEventArgs e)
    {
        await PreencheCampos(txbCodigo.Text);
    }

    private async void cmbTransportadora_SelectedIndexChanged(object sender, EventArgs e)
    {
        await ListaCarregamento();
    }
    #endregion
}