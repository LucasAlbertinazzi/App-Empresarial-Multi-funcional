using static AppEmpresa.Classes.API.Auditoria.CarregamentoClass;

namespace AppEmpresa.Views.Auditoria;

public partial class VProdutosBipados : ContentPage
{
    #region 1- LOG

    #endregion

    #region 2- VARIAVEIS
    List<ListaCarregamento> produtosBipados = new List<ListaCarregamento>();

    #endregion

    #region 3- CONSTRUTORES
    public VProdutosBipados(List<ListaCarregamento> listaCarregamentos)
    {
        InitializeComponent();
        produtosBipados.AddRange(listaCarregamentos);
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        CarregaBipados();
    }
    #endregion

    #region 4- METODOS
    private void CarregaBipados()
    {
        LoadingIndicator.IsVisible = true;

        if (produtosBipados.Count > 0)
        {
            cardBipados.ItemsSource = produtosBipados;
        }
        LoadingIndicator.IsVisible = false;
    }

    #endregion

    #region 5- EVENTOS DE CONTROLE

    #endregion
}