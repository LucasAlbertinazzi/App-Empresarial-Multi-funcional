using static AppEmpresa.Classes.API.Auditoria.CarregamentoClass;

namespace AppEmpresa.Views.Auditoria;

public partial class VExpedicaoEcommerce : ContentPage
{
    #region 0- LOG

    #endregion

    #region 1- VARIAVEIS

    #endregion

    #region 2- CONSTRUTORES
    public VExpedicaoEcommerce()
    {
        InitializeComponent();
    }
    #endregion

    #region 3- METODOS

    #endregion

    #region 4- EVENTOS DE CONTROLE
    private async void btnConferirNotas_Clicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new VConfereNotas());
    }

    private async void btnProdBipado_Clicked(object sender, EventArgs e)
    {
        await Application.Current.MainPage.Navigation.PushAsync(new VProdutosBipados(new List<ListaCarregamento>()));
    }
    #endregion
}