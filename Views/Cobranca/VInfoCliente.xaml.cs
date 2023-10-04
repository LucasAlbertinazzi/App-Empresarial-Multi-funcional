using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Cobranca;

namespace AppMarciusMagazine.Views.Cobranca;

public partial class VInfoCliente : ContentPage
{
    long codCliente;
    long codFiador;
    string tipo;
    APIClientes apiClientes = new APIClientes();

    ClientesClass listasuporte = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();

    public VInfoCliente(OcorrenciaClass _ocorrencia)
    {
        InitializeComponent();
        codCliente = _ocorrencia.Codcliente;
        ocorrencia = _ocorrencia;
        InfoGlobal.CodOcorrencia = ocorrencia.Codsolicitacao;
    }

    public VInfoCliente(OcorrenciaClass _ocorrencia, string _tipo, long _codFiador)
    {
        InitializeComponent();
        ocorrencia = _ocorrencia;
        InfoGlobal.CodOcorrencia = ocorrencia.Codsolicitacao;
        tipo = _tipo;
        codFiador = _codFiador;
        swPrincipal.IsEnabled = false;
    }

    private async Task BuscaCodigoTipo(string tipo)
    {
        if(tipo == "FIADOR")
        {
            codCliente = codFiador;
        }
        else
        {

        }
    }

    private async Task Inicializa()
    {
        PrincipalView.IsVisible = false;
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;

        var lista = await BuscaInfo();

        if (lista != null)
        {
            await PreencheInfo(lista);
        }

        LoadingIndicator.IsVisible = false;
        LoadingIndicator.IsRunning = false;
        PrincipalView.IsVisible = true;
    }

    private async Task<ClientesClass> BuscaInfo()
    {
        if (codCliente > 0)
        {
            ClientesClass lista = await apiClientes.BuscaInfoClientes(codCliente);

            listasuporte = new ClientesClass();

            if (lista != null)
            {
                listasuporte = lista;
                return lista;
            }
        }
        return null;
    }

    private async Task PreencheInfo(ClientesClass lista)
    {
        try
        {
            
                lblNome.Text = lista.Cliente.Nome ?? lblNome.Text;
                lblNascimento.Text = lista.Cliente.Nascimento.ToString("dd/MM/yyyy") ?? lblNascimento.Text;
                lblIdade.Text = (lista.Cliente.Nascimento != null) ? CalcularIdade(lista.Cliente.Nascimento, DateTime.Today).ToString() : lblIdade.Text;
                lblCPF.Text = lista.Cliente.Cpf ?? lblCPF.Text;
                lblRG.Text = FormatarRG(lista.Cliente.Rg) ?? lblRG.Text;
                lblTelefone.Text = lista.Cliente.Celular ?? lblTelefone.Text;
                lblEndereco.Text = lista.Cliente.Endereco ?? lblEndereco.Text;
                lblBairro.Text = lista.BairroCliente ?? lblBairro.Text;
                lblNumEnd.Text = lista.Cliente.EnderecoNum ?? lblNumEnd.Text;
            

            FotoCliente.Source = await apiClientes.BuscaFotoCliente(Convert.ToInt64(lista.Cliente.Codcliente), "TITULAR");
        }
        catch (Exception ex)
        {
            string erro = ex.Message;
            return;
        }
        
    }

    private int CalcularIdade(DateTime dataNascimento, DateTime dataAtual)
    {
        int idade = dataAtual.Year - dataNascimento.Year;

        if (dataNascimento.Date > dataAtual.AddYears(-idade))
        {
            idade--;
        }

        return idade;
    }

    private string FormatarRG(string rg)
    {
        string newrg = rg.Replace(".", "");
        newrg = newrg.Replace("-", "");

        if (newrg.Length == 8)
        {
            string part0 = newrg.Substring(0, 2);
            string part1 = newrg.Substring(2, 3);
            string part2 = newrg.Substring(5, 3);

            string RG = $"{newrg.Substring(0, 2)}.{newrg.Substring(2, 3)}.{newrg.Substring(5, 3)}";
            return RG;
        }
        else if (newrg.Length == 9)
        {
            string part0 = newrg.Substring(0, 2);
            string part1 = newrg.Substring(2, 3);
            string part2 = newrg.Substring(5, 3);
            string part3 = newrg.Substring(8, 1);

            string RG = $"{newrg.Substring(0, 2)}.{newrg.Substring(2, 3)}.{newrg.Substring(5, 3)}-{newrg.Substring(8, 1)}";
            return RG;
        }
        else if (newrg.Length == 10)
        {
            string part0 = newrg.Substring(0, 2);
            string part1 = newrg.Substring(2, 3);
            string part2 = newrg.Substring(5, 3);
            string part3 = newrg.Substring(8, 2);

            string RG = $"{newrg.Substring(0, 2)}.{newrg.Substring(2, 3)}.{newrg.Substring(5, 3)}-{newrg.Substring(8, 2)}";
            return RG;
        }

        return string.Empty;
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (!string.IsNullOrEmpty(tipo))
        {
            BuscaCodigoTipo(tipo);
        }
        await Inicializa();
    }

    private async void FiadorInvoked(object sender, EventArgs e)
    {
        if(listasuporte.Cliente.Codfiador > 0)
        {
            await Application.Current.MainPage.Navigation.PushAsync(new VInfoCliente(ocorrencia, "FIADOR",Convert.ToInt64(listasuporte.Cliente.Codfiador)));
        }
        else
        {
            await DisplayAlert("AVISO", "O cliente não possui fiador", "OK");
        }
    }

    private async void ConjugeInvoked(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(listasuporte.Cliente.Conjuge))
        {
            await Application.Current.MainPage.Navigation.PushAsync(new VInfoCliente(ocorrencia, "CONJUGE",0));
        }
        else
        {
            await DisplayAlert("AVISO", "O cliente não possui cônjuge", "OK");
        }
    }

    private async void btnProximo_Clicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia));
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VOcorrencia());
    }
}