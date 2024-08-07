using AppEmpresa.Classes.API.Cobranca;
using AppEmpresa.Classes.API.Principal;
using AppEmpresa.Classes.Globais;
using AppEmpresa.Services.Cobranca;
using AppEmpresa.Services.Principal;
using AppEmpresa.Suporte;
using AppEmpresa.Views.Principal;

namespace AppEmpresa.Views.Cobranca;

public partial class VBscClientes : ContentPage
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
    APIClientes apiClientes = new APIClientes();
    BuscaCliente buscaCliente = new BuscaCliente();
    #endregion

    #region 3- CLASSES

    #endregion

    #region 4- METODOS CONSTRUTORES
    public VBscClientes(int pagina)
    {
        try
        {
            InitializeComponent();
            cmbTipoCliente.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;

            if (pagina == 1)
            {
                btnBuscar.IsVisible = false;
                btnBuscarCobranca.IsVisible = true;
            }
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    protected override void OnAppearing()
    {
        try
        {
            base.OnAppearing();
            cmbTipoCliente.SelectedIndex = 0;
            cmbTipo.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 5- METODOS
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
    #endregion

    #region 6- EVENTOS DE CONTROLE
    private void cmbTipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {

            txbBusca.Text = string.Empty;
            buscaCliente = new BuscaCliente();

            string tipoSelecionado = cmbTipo.SelectedItem.ToString();

            // Atualize o Placeholder e o tipo de teclado do Entry
            if (tipoSelecionado == "CÓDIGO")
            {
                txbBusca.Placeholder = "000000";
                txbBusca.MaxLength = 10;
                txbBusca.Keyboard = Keyboard.Numeric;
                txbBusca.Behaviors.Clear();

                buscaCliente.tipo = "codcliente";
            }
            else if (tipoSelecionado == "CPF")
            {
                txbBusca.Placeholder = "000.000.000-00";
                txbBusca.MaxLength = 14;
                txbBusca.Keyboard = Keyboard.Numeric;
                txbBusca.Behaviors.Clear(); // Limpa os behaviors anteriores
                txbBusca.Behaviors.Add(new MascaraBehavior("CPF")); // Adiciona o behavior para CPF

                buscaCliente.tipo = "cpf";

            }
            else if (tipoSelecionado == "CNPJ")
            {
                txbBusca.Placeholder = "00.000.000/0000-00";
                txbBusca.MaxLength = 18;
                txbBusca.Keyboard = Keyboard.Numeric;
                txbBusca.Behaviors.Clear(); // Limpa os behaviors anteriores
                txbBusca.Behaviors.Add(new MascaraBehavior("CNPJ")); // Adiciona o behavior para CNPJ

                buscaCliente.tipo = "cnpj";
            }
            else
            {
                txbBusca.Placeholder = "Nome do cliente";
                txbBusca.Keyboard = Keyboard.Default;
                txbBusca.Behaviors.Clear();

                buscaCliente.tipo = "cliente";
            }
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    private async void btnBuscar_Clicked(object sender, EventArgs e)
    {
        try
        {
            string tipoCliente = cmbTipoCliente.SelectedItem.ToString();

            buscaCliente.cliente = txbBusca.Text;

            if (buscaCliente != null && !string.IsNullOrEmpty(tipoCliente))
            {
                var result = await apiClientes.BuscaInfoClientesTipo(buscaCliente.cliente, buscaCliente.tipo, tipoCliente);

                if (result != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new VInfoCliente(result.Cliente.Codcliente, 1, tipoCliente));
                }
                else
                {
                    await DisplayAlert("AVISO", "Cliente não encontrado!", "OK");
                }
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void btnBuscarCobranca_Clicked(object sender, EventArgs e)
    {
        try
        {
            string tipoCliente = cmbTipoCliente.SelectedItem.ToString();

            buscaCliente.cliente = txbBusca.Text;

            if (buscaCliente != null && !string.IsNullOrEmpty(tipoCliente))
            {
                var result = await apiClientes.BuscaInfoClientesTipo(buscaCliente.cliente, buscaCliente.tipo, tipoCliente);

                if (result != null)
                {
                    await Application.Current.MainPage.Navigation.PushAsync(new VCobrancaContato(result.Cliente.Codcliente));
                }
                else
                {
                    await DisplayAlert("AVISO", "Cliente não encontrado!", "OK");
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