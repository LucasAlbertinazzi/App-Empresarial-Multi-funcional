using static AppEmpresa.Classes.Geral.DepositoClass;

namespace AppEmpresa.Views.Auditoria;

public partial class VAuditEstoqueInfos : ContentPage
{
    #region 0- LOG

    #endregion

    #region 1- VARIÁVEIS
    AuditEstoqueClass parametroPrincipal = new AuditEstoqueClass();
    #endregion

    #region 2- CONSTRUTORES
    public VAuditEstoqueInfos(AuditEstoqueClass selecionado)
    {
        InitializeComponent();
        parametroPrincipal = selecionado;
    }

    private void ContentPage_Loaded(object sender, EventArgs e)
    {
        PreencheDados();
    }
    #endregion

    #region 3- MÉTODOS
    private void PreencheDados()
    {
        lblTituloPage.Text = parametroPrincipal.NumContagem.ToString();
        lblLocal.Text = parametroPrincipal.Local;
        lblDivisao.Text = parametroPrincipal.Divisao;
        lblDep.Text = parametroPrincipal.Departamento;
        lblCodigo.Text = "000.000.0000";
        lblEstoqueInicio.Text = "0";
        lblEntrega.Text = "0";
        lblEstoqueContado.Text = "0";
        lblEstoqueAtual.Text = "0";
    }
    #endregion

    #region 4- EVENTOS DE CONTROLE

    #endregion
}