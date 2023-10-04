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
        List<RestricaoOcorrencias> infoRestricoes = await apiOcorrencia.BuscaRestricoesOcorrencia(ocorrencia.Codigo);
        SenhaNegada infoNegada = await apiOcorrencia.BuscaSenhaNegada(ocorrencia.Codcliente);

        if (infoRestricoes != null && infoRestricoes.Count > 0)
        {
            cardRestricoes.ItemsSource = DefinecaoListaRestricao(infoRestricoes);

            frameCampoNegado.HeightRequest = ResponsiveAuto.Height(8);

            lblCampoNegado.Text = "Não há restrições de senha";

            if (infoNegada != null)
            {
                lblCampoNegado.Text = infoNegada.Usuario + " - " + infoNegada.Motivo + " - " + infoNegada.Data.ToString();
            }
        }
    }

    private async void btnConsultaScore_Clicked(object sender, EventArgs e)
    {
        await DisplayAlert("AVISO", "No momento a função CONSULTA SCORE está desativada, faça a consulta pelo sistema", "OK");

        //int piker = cmbTipo.SelectedIndex;

        //ScoreClass scoreClass = new ScoreClass();

        //if (piker == 0)
        //{
        //    scoreClass = new ScoreClass
        //    {
        //        codusuario = InfoGlobal.codusuario.ToString(),
        //        codcliente = listasuporte[0].Codcliente.ToString(),
        //        tipos = "TITULAR",
        //        cpf = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte[0].Cpf.ToString()),
        //        rg = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte[0].Rg.ToString()),
        //        nome = listasuporte[0].Nome.ToUpper(),
        //        uf = listasuporte[0].Uf.ToUpper(),
        //        nascimento = ScoreOptions.FormatDate(listasuporte[0].Nascimento.ToString()),
        //    };
        //}
        //else
        //{
        //    scoreClass = new ScoreClass
        //    {
        //        codusuario = InfoGlobal.codusuario.ToString(),
        //        codcliente = listasuporte[0].Codcliente.ToString(),
        //        tipos = "CONJUGE",
        //        cpf = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte[0].ConjCpf.ToString()),
        //        rg = ScoreOptions.RemoveAllCharactersAndSpaces(listasuporte[0].ConjRg.ToString()),
        //        nome = listasuporte[0].Conjuge.ToUpper(),
        //        uf = listasuporte[0].UfC.ToUpper(),
        //        nascimento = ScoreOptions.FormatDate(listasuporte[0].ConjNascimento.ToString()),
        //    };
        //}

        //var result = await apiScore.BuscaScore(scoreClass);

        //if (string.IsNullOrEmpty(result))
        //{
        //    await DisplayAlert("Aviso", "Não foi possível encontrar o score do cliente. Verifique as informações ou tente novamente mais tarde.", "OK");
        //    return;
        //}

        //if(!await ScoreOptions.CarregaPDF(result, scoreClass.codcliente))
        //{
        //    await DisplayAlert("Aviso", "Não foi possível visualizar o PDF, tente novamente mais tarde.", "OK");
        //    return;
        //}
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