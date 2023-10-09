using AppMarciusMagazine.Classes.API.Cobranca;
using AppMarciusMagazine.Classes.Globais;
using AppMarciusMagazine.Services.Cobranca;
using AppMarciusMagazine.Suporte;
using System.Linq;

namespace AppMarciusMagazine.Views.Cobranca;

public partial class VInfoClienteTres : ContentPage
{
    ClientesClass listasuporte = new ClientesClass();
    APIScore apiScore = new APIScore();
    APIOcorrencia apiOcorrencia = new APIOcorrencia();
    OcorrenciaClass ocorrencia = new OcorrenciaClass();

    public VInfoClienteTres(ClientesClass lista, OcorrenciaClass _ocorrencia)
    {
        InitializeComponent();
        listasuporte = lista;
        ocorrencia = _ocorrencia;
    }

    private async void ContentPage_Loaded(object sender, EventArgs e)
    {
        await CarregaRestricoes();
    }

    private List<RestricaoOcorrencias> DefinecaoListaRestricao(List<RestricaoOcorrencias> infoRestricoes)
    {
        cardRestricoes.HeightRequest = ResponsiveAuto.Height(2.3);

        var mapeamentoCores = new Dictionary<int, string>
            {
                { 3, "#ffbf00" }, // Amarelo
                { 28, "#fb7800" }, // Laranja
                { 8, "#fb7800" }, // Laranja
                { 9, "#fb7800" }, // Laranja
                { 10, "#fb7800" }, // Laranja
                { 5, "#fb7800" }, // Laranja
                { 23, "#fb7800" }, // Laranja
                { 24, "#fb7800" }, // Laranja
                { 6, "#fb7800" }, // Laranja
                { 2, "#ff322e" } // Vermelho
            };

        foreach (var item in infoRestricoes)
        {
            if (mapeamentoCores.TryGetValue(item.CodOcorrencia, out var corHex))
            {
                item.Cor = Color.FromHex(corHex);
            }
            else
            {
                item.Cor = Color.FromHex("#ffffff"); // Branco
            }
        }

        var valoresPermitidos = new List<int> { 3, 9, 10, 30, 5, 23, 24, 6, 8, 28, 2 };

        return infoRestricoes.Where(x => valoresPermitidos.Contains(x.CodOcorrencia)).ToList();
    }

    private async Task CarregaRestricoes()
    {
        GridPrincipal.MinimumHeightRequest = ResponsiveAuto.Height(2.4);
        GridSecundario.MinimumHeightRequest = ResponsiveAuto.Height(2.4);

        List<RestricaoOcorrencias> infoRestricoes = await apiOcorrencia.BuscaRestricoesOcorrencia(ocorrencia.Codigo);
        SenhaNegada infoNegada = await apiOcorrencia.BuscaSenhaNegada(ocorrencia.Codcliente);

        if (infoRestricoes != null && infoRestricoes.Count > 0)
        {
            List<RestricaoOcorrencias> lista = DefinecaoListaRestricao(infoRestricoes);

            GridPrincipal.IsVisible = false;
            GridSecundario.IsVisible = true;

            if (lista != null && lista.Count > 0)
            {
                cardRestricoes.ItemsSource = lista;
                GridPrincipal.IsVisible = true;
                GridSecundario.IsVisible = false;
            }

            frameCampoNegado.HeightRequest = ResponsiveAuto.Height(8);

            lblCampoNegado.Text = "Não há restrições de senha";

            if (infoNegada != null)
            {
                lblCampoNegado.Text = infoNegada.Usuario + " - " + infoNegada.Motivo + " - " + infoNegada.Data.ToString();
            }
        }
        else
        {
            lblCampoNegado.Text = "Não há restrições de senha";
            GridPrincipal.IsVisible = false;
            GridSecundario.IsVisible = true;
        }
    }

    private async void btnConsultaScore_Clicked(object sender, EventArgs e)
    {

        int piker = cmbTipo.SelectedIndex;

        ScoreClass scoreClass = new ScoreClass();

        if (piker == 0)
        {
            var last = await apiScore.ObterUltimoScore(listasuporte.Cliente.Codcliente, "TITULAR");

            if (last != null)
            {
                DateTime dataAtual = DateTime.Now;

                DateTime dataConsulta = last.Dataconsulta;

                DateTime dataLimite = dataConsulta.AddDays(60);

                if (dataAtual < dataLimite)
                {

                    await DisplayAlert("AVISO", $"A consulta a ser exibida a seguir, é referente a data {last.Dataconsulta.ToShortDateString()}.", "OK");

                    if (!await ScoreOptions.CarregaPDF(last.Pdflast, listasuporte.Cliente.Codcliente.ToString()))
                    {
                        await DisplayAlert("Aviso", "Não foi possível visualizar o PDF, tente novamente mais tarde.", "OK");
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    scoreClass = new ScoreClass
                    {
                        codusuario = InfoGlobal.codusuario.ToString(),
                        codcliente = listasuporte.Cliente.Codcliente.ToString(),
                        tipos = "TITULAR",
                        cpf = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Cpf.ToString()),
                        rg = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Rg.ToString()),
                        nome = listasuporte.Cliente.Nome.ToUpper(),
                        uf = listasuporte.Cliente.Uf.ToUpper(),
                        nascimento = ScoreOptions.FormatDate(listasuporte.Cliente.Nascimento.ToString()),
                    };
                }
            }
            else
            {
                scoreClass = new ScoreClass
                {
                    codusuario = InfoGlobal.codusuario.ToString(),
                    codcliente = listasuporte.Cliente.Codcliente.ToString(),
                    tipos = "TITULAR",
                    cpf = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Cpf.ToString()),
                    rg = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Rg.ToString()),
                    nome = listasuporte.Cliente.Nome.ToUpper(),
                    uf = listasuporte.Cliente.Uf.ToUpper(),
                    nascimento = ScoreOptions.FormatDate(listasuporte.Cliente.Nascimento.ToString()),
                };
            }
        }
        else if (piker == 1)
        {
            var last = await apiScore.ObterUltimoScore(listasuporte.Cliente.Codcliente, "CONJUGE");

            if (last != null)
            {
                DateTime dataAtual = DateTime.Now;

                DateTime dataConsulta = last.Dataconsulta;

                DateTime dataLimite = dataConsulta.AddDays(60);

                if (dataAtual < dataLimite)
                {
                    await DisplayAlert("AVISO", $"A consulta a ser exibida a seguir, é referente a data {last.Dataconsulta.ToShortDateString()}.", "OK");

                    if (!await ScoreOptions.CarregaPDF(last.Pdflast, listasuporte.Cliente.Codcliente.ToString()))
                    {
                        await DisplayAlert("Aviso", "Não foi possível visualizar o PDF, tente novamente mais tarde.", "OK");
                        return;
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    scoreClass = new ScoreClass
                    {
                        codusuario = InfoGlobal.codusuario.ToString(),
                        codcliente = listasuporte.Cliente.Codcliente.ToString(),
                        tipos = "CONJUGE",
                        cpf = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Cpf.ToString()),
                        rg = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Rg.ToString()),
                        nome = listasuporte.Cliente.Nome.ToUpper(),
                        uf = listasuporte.Cliente.Uf.ToUpper(),
                        nascimento = ScoreOptions.FormatDate(listasuporte.Cliente.Nascimento.ToString()),
                    };
                }
            }
            else
            {
                scoreClass = new ScoreClass
                {
                    codusuario = InfoGlobal.codusuario.ToString(),
                    codcliente = listasuporte.Cliente.Codcliente.ToString(),
                    tipos = "CONJUGE",
                    cpf = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Cpf.ToString()),
                    rg = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte.Cliente.Rg.ToString()),
                    nome = listasuporte.Cliente.Nome.ToUpper(),
                    uf = listasuporte.Cliente.Uf.ToUpper(),
                    nascimento = ScoreOptions.FormatDate(listasuporte.Cliente.Nascimento.ToString()),
                };
            }
        }
        else
        {
            await DisplayAlert("AVISO", "Selecione o tipo de cliente!", "OK");
            return;
        }

        var result = await apiScore.BuscaScore(scoreClass);

        if (string.IsNullOrEmpty(result))
        {
            await DisplayAlert("Aviso", "Não foi possível encontrar o score do cliente. Verifique as informações ou tente novamente mais tarde.", "OK");
            return;
        }

        if (!await ScoreOptions.CarregaPDF(result, scoreClass.codcliente))
        {
            await DisplayAlert("Aviso", "Não foi possível visualizar o PDF, tente novamente mais tarde.", "OK");
            return;
        }
    }

    private async void btnVoltar_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VInfoClienteDois(listasuporte, ocorrencia));
    }

    private async void btnProximo_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new VInfoClienteQuatro(listasuporte, ocorrencia));
    }


}