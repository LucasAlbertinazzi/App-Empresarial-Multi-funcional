using static AppEmpresa.Classes.Geral.DepositoClass;

namespace AppEmpresa.Views.Auditoria;

public partial class VAuditEstoque : ContentPage
{
    #region 0- LOG

    #endregion

    #region 1- PERMISSÕES

    #endregion

    #region 2- VARIÁVEIS

    #endregion

    #region 3- CONSTRUTORES
    public VAuditEstoque()
    {
        InitializeComponent();
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        CarregaAudEstoque();
    }
    #endregion

    #region 4- MÉTODOS
    private void CarregaAudEstoque()
    {
        // Cria uma lista de AuditEstoqueClass
        List<AuditEstoqueClass> listaAuditoria = new List<AuditEstoqueClass>();

        // Adiciona alguns itens à lista
        listaAuditoria.Add(new AuditEstoqueClass
        {
            NumContagem = 1,
            IdLocal = 101,
            Local = "Armazém A",
            IdDivisao = 201,
            Divisao = "Divisão 1",
            IdDepartamento = 301,
            Departamento = "Departamento X",
            DataHora = DateTime.Now
        });

        listaAuditoria.Add(new AuditEstoqueClass
        {
            NumContagem = 2,
            IdLocal = 102,
            Local = "Armazém B",
            IdDivisao = 202,
            Divisao = "Divisão 2",
            IdDepartamento = 302,
            Departamento = "Departamento Y",
            DataHora = DateTime.Now
        });

        listaAuditoria.Add(new AuditEstoqueClass
        {
            NumContagem = 3,
            IdLocal = 103,
            Local = "Armazém C",
            IdDivisao = 203,
            Divisao = "Divisão 3",
            IdDepartamento = 303,
            Departamento = "Departamento Z",
            DataHora = DateTime.Now
        });

        listaAuditoria.Add(new AuditEstoqueClass
        {
            NumContagem = 4,
            IdLocal = 104,
            Local = "Armazém D",
            IdDivisao = 204,
            Divisao = "Divisão 4",
            IdDepartamento = 304,
            Departamento = "Departamento W",
            DataHora = DateTime.Now
        });

        listaAuditoria.Add(new AuditEstoqueClass
        {
            NumContagem = 5,
            IdLocal = 105,
            Local = "Armazém E",
            IdDivisao = 205,
            Divisao = "Divisão 5",
            IdDepartamento = 305,
            Departamento = "Departamento V",
            DataHora = DateTime.Now
        });

        listaAuditoria.Add(new AuditEstoqueClass
        {
            NumContagem = 6,
            IdLocal = 106,
            Local = "Armazém F",
            IdDivisao = 206,
            Divisao = "Divisão 6",
            IdDepartamento = 306,
            Departamento = "Departamento U",
            DataHora = DateTime.Now
        });

        cardInfoHistorico.ItemsSource = listaAuditoria;
    }
    #endregion

    #region 5- EVENTOS DE CONTROLE
    private async void OnFrameTapped(object sender, TappedEventArgs e)
    {
        var frameSelecionado = sender as Frame;
        AuditEstoqueClass selecionado = frameSelecionado?.BindingContext as AuditEstoqueClass;

        if (selecionado != null)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new VAuditEstoqueInfos(selecionado));
        }
    }
    #endregion
}