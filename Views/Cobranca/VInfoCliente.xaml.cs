using AppEmpresarialMultFuncional.Classes.API.Cobranca;
using AppEmpresarialMultFuncional.Classes.API.Principal;
using AppEmpresarialMultFuncional.Classes.Globais;
using AppEmpresarialMultFuncional.Services.Cobranca;
using AppEmpresarialMultFuncional.Services.Principal;

namespace AppEmpresarialMultFuncional.Views.Cobranca;

public partial class VInfoCliente : ContentPage
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
    long codCliente;
    long codFiador;
    string tipo;
    int tipoPagina = 0;
    APIClientes apiClientes = new APIClientes();
    ClientesClass listasuporte = new ClientesClass();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();
    #endregion

    #region 2- CLASSES

    #endregion

    #region 3- METODOS CONSTRUTORES
    public VInfoCliente(OcorrenciaClass _ocorrencia)
    {
        try
        {
            InitializeComponent();
            codCliente = _ocorrencia.Codcliente;
            ocorrencia = _ocorrencia;
            InfoGlobal.CodOcorrencia = ocorrencia.Codsolicitacao;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    public VInfoCliente(int codcliente, int _tipo, string tipoCliente)
    {
        try
        {
            InitializeComponent();
            codCliente = codcliente;
            tipoPagina = _tipo;
            tipo = tipoCliente;
            ocorrencia = null;
            swPrincipal.IsEnabled = false;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    public VInfoCliente(OcorrenciaClass _ocorrencia, string _tipo, long _codFiador)
    {
        try
        {
            InitializeComponent();
            ocorrencia = _ocorrencia;
            InfoGlobal.CodOcorrencia = ocorrencia.Codsolicitacao;
            tipo = _tipo;
            codFiador = _codFiador;
            swPrincipal.IsEnabled = false;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }

    protected override async void OnAppearing()
    {
        try
        {
            base.OnAppearing();

            if (!string.IsNullOrEmpty(tipo))
            {
                BuscaCodigoTipo(tipo);
            }
            await Inicializa();
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return;
        }
    }
    #endregion

    #region 4- METODOS
    private async Task BuscaCodigoTipo(string tipo)
    {
        try
        {
            if (tipo == "FIADOR")
            {
                codCliente = codFiador;
            }
            else
            {
                codCliente = ocorrencia.Codcliente;
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task Inicializa()
    {
        try
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
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async Task<ClientesClass> BuscaInfo()
    {
        try
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
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return null;
        }
    }

    private async Task PreencheInfo(ClientesClass lista)
    {
        try
        {
            gridProcessado.IsVisible = false;
            gridPromessa.IsVisible = false;

            if (await apiClientes.ClienteProcessado(codCliente))
            {
                gridProcessado.IsVisible = true;
                lblProcesso.Text = "SIM";
            }

            if (lista.Cliente.Pgtopara != null)
            {
                gridPromessa.IsVisible = true;
                lblPromessaPara.Text = lista.Cliente.Pgtopara.Value.ToString("dd/MM/yyyy");
            }

            if (tipo == "CONJUGE" || tipo == "CONJUGÊ")
            {
                lblNome.Text = lista.Cliente.Conjuge ?? lblNome.Text;
                lblNascimento.Text = (lista.Cliente.ConjNascimento != null) ? lista.Cliente.ConjNascimento.Value.ToString("dd/MM/yyyy") : lblNascimento.Text;
                lblIdade.Text = (lista.Cliente.ConjNascimento != null) ? CalcularIdade(lista.Cliente.ConjNascimento.Value, DateTime.Today).ToString() : lblIdade.Text;
                lblCPF.Text = lista.Cliente.ConjCpf ?? lblCPF.Text;
                lblRG.Text = FormatarRG(lista.Cliente.ConjRg) ?? lblRG.Text;
                lblTelefone.Text = lista.Cliente.ConjCelular ?? lblTelefone.Text;
                lblEndereco.Text = lista.Cliente.Endereco ?? lblEndereco.Text;
                lblBairro.Text = lista.BairroCliente ?? lblBairro.Text;
                lblNumEnd.Text = lista.Cliente.EnderecoNum ?? lblNumEnd.Text;

                lblEmpresa.Text = lista.Cliente.ConjEmpresa ?? lblEmpresa.Text;
                lblFoneEmp.Text = lista.Cliente.ConjEmpFone ?? lblEmpresa.Text;
                lblTipoEmp.Text = lista.Cliente.Tipoempregoconj ?? lblEmpresa.Text;
                lblAdmissaoEmp.Text = (lista.Cliente.ConjAdmissao != null) ? lista.Cliente.ConjAdmissao.Value.ToString("dd/MM/yyyy") : lblEmpresa.Text;
                lblFuncEmp.Text = lista.Cliente.ConjCargo ?? lblEmpresa.Text;

                int codCidade = lista.Cliente.CodcidadeC ?? 0;
                lblCidadeEmp.Text = await apiClientes.BuscaCidade(codCidade);

                decimal renda = lista.Cliente.ConjRendaliquida ?? 0;
                string rendaFormatada = renda.ToString("C");
                lblRendaEmp.Text = rendaFormatada;

                FotoCliente.Source = await apiClientes.BuscaFotoCliente(Convert.ToInt64(lista.Cliente.Codcliente), "CONJUGE");
            }
            else
            {
                lblNome.Text = lista.Cliente.Nome ?? lblNome.Text;
                lblNascimento.Text = (lista.Cliente.Nascimento != null) ? lista.Cliente.Nascimento.Value.ToString("dd/MM/yyyy") : lblNascimento.Text;
                lblIdade.Text = (lista.Cliente.Nascimento != null) ? CalcularIdade(lista.Cliente.Nascimento.Value, DateTime.Today).ToString() : lblIdade.Text;
                lblCPF.Text = lista.Cliente.Cpf ?? lblCPF.Text;
                lblRG.Text = FormatarRG(lista.Cliente.Rg) ?? lblRG.Text;
                lblTelefone.Text = lista.Cliente.Celular ?? lblTelefone.Text;
                lblEndereco.Text = lista.Cliente.Endereco ?? lblEndereco.Text;
                lblBairro.Text = lista.BairroCliente ?? lblBairro.Text;
                lblNumEnd.Text = lista.Cliente.EnderecoNum ?? lblNumEnd.Text;

                lblEmpresa.Text = lista.Cliente.Empresa ?? lblEmpresa.Text;
                lblFoneEmp.Text = lista.Cliente.EmpFone ?? lblEmpresa.Text;
                lblTipoEmp.Text = lista.Cliente.Tipoemprego ?? lblEmpresa.Text;
                lblAdmissaoEmp.Text = (lista.Cliente.Admissao != null) ? lista.Cliente.Admissao.Value.ToString("dd/MM/yyyy") : lblEmpresa.Text;
                lblFuncEmp.Text = lista.Cliente.Cargo ?? lblEmpresa.Text;

                int codCidade = lista.Cliente.EmpreCodcidade ?? 0;
                lblCidadeEmp.Text = await apiClientes.BuscaCidade(codCidade);

                decimal renda = lista.Cliente.Rendaliquida ?? 0;
                string rendaFormatada = renda.ToString("C");
                lblRendaEmp.Text = rendaFormatada;

                FotoCliente.Source = await apiClientes.BuscaFotoCliente(Convert.ToInt64(lista.Cliente.Codcliente), "TITULAR");
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private int CalcularIdade(DateTime dataNascimento, DateTime dataAtual)
    {
        try
        {
            int idade = dataAtual.Year - dataNascimento.Year;

            if (dataNascimento.Date > dataAtual.AddYears(-idade))
            {
                idade--;
            }

            return idade;
        }
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return 0;
        }
    }

    private string FormatarRG(string rg)
    {
        try
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
        catch (Exception ex)
        {
            MetodoErroLog(ex);
            return string.Empty;
        }
    }

    protected override bool OnBackButtonPressed()
    {
        return true;
    }
    #endregion

    #region 5- EVENTOS DE CONTROLE
    private async void FiadorInvoked(object sender, EventArgs e)
    {
        try
        {
            if (listasuporte.Cliente.Codfiador > 0)
            {
                await Application.Current.MainPage.Navigation.PushAsync(new VInfoCliente(ocorrencia, "FIADOR", Convert.ToInt64(listasuporte.Cliente.Codfiador)));
            }
            else
            {
                await DisplayAlert("AVISO", "O cliente não possui fiador", "OK");
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void ConjugeInvoked(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(listasuporte.Cliente.Conjuge))
            {
                await Application.Current.MainPage.Navigation.PushAsync(new VInfoCliente(ocorrencia, "CONJUGE", 0));
            }
            else
            {
                await DisplayAlert("AVISO", "O cliente não possui cônjuge", "OK");
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
            if (tipo == "CONJUGE" || tipo == "CONJUGÊ")
            {
                listasuporte.TipoCliente = "CONJUGE";
                await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia, true, tipoPagina));
            }
            else
            {
                listasuporte.TipoCliente = "TITULAR";
                await Navigation.PushAsync(new VInfoClienteHistorico(listasuporte, ocorrencia, false, tipoPagina));
            }
        }
        catch (Exception ex)
        {
            await MetodoErroLog(ex);
            return;
        }
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        try
        {
            if (tipoPagina != 1)
            {
                if (tipo == "CONJUGE" || tipo == "FIADOR")
                {
                    await Navigation.PushAsync(new VInfoCliente(ocorrencia));
                }
                else
                {
                    await Navigation.PushAsync(new VOcorrencia());
                }
            }
            else
            {
                await Navigation.PushAsync(new VBscClientes(0));
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