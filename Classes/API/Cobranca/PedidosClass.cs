using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEmpresarialMultFuncional.Classes.API.Cobranca
{
    public class PedidosClass
    {
        public string? Nome { get; set; }
        public int? CodCliente { get; set; }
        public DateOnly? DataVenda { get; set; }
        public string? Fone { get; set; }
        public string? Cpf { get; set; }
        public string? Rg { get; set; }
        public string? TipoCadastro { get; set; }
        public string? Endereco { get; set; }
        public string? Numero { get; set; }
        public string? Bairro { get; set; }
        public string? Cidade { get; set; }
        public string? Cep { get; set; }
        public string? Uf { get; set; }
        public string? Comprador { get; set; }
        public int? CodUsuario { get; set; }
        public decimal? AcrescimoPMedio { get; set; }
        public int? CodLoja { get; set; }
        public decimal? TotalProduto { get; set; }
        public decimal? Desconto { get; set; }
        public decimal? Acrescimo { get; set; }
        public decimal? TotalVenda { get; set; }
        public decimal? TotalPagar { get; set; }
        public decimal? ValorFrete { get; set; }
        public string? Email { get; set; }
        public DateOnly? DataVenda1 { get; set; }
        public TimeOnly? Hora { get; set; }
        public DateTime? DataAbertura { get; set; }
        public string? TipoVenda { get; set; }
        public string? Usuario { get; set; }
        public string? Loja { get; set; }
        public decimal? Taxa { get; set; }
        public int? Parcelas { get; set; }
        public string? InfAddNFe { get; set; }
        public bool? VendaEcManual { get; set; }
    }

    public class InfoProdutosPedido
    {
        public int? Codigo { get; set; }
        public string? CodProdutoIndex { get; set; }
        public string? Descricao { get; set; }
        public decimal? QuantVendido { get; set; }
        public decimal? ValorVenda { get; set; }
        public decimal? ValorTotal { get; set; }
        public int? CodDeposito { get; set; }
        public string? TipoEntrega { get; set; }
        public int? Nfe { get; set; }
        public long? Nfce { get; set; }
        public string? CodProduto { get; set; }
        public string? JuroEspec { get; set; }
        public string? Promocao { get; set; }
        public string? BotaFora { get; set; }
        public int? Restou { get; set; }
        public string? Loja { get; set; }
        public string? CodFabrica { get; set; }
        public int? EstoqueAnterior { get; set; }
        public int? TempoMontagem { get; set; }
        public string? Montar { get; set; }
        public decimal? Aliquota { get; set; }
        public bool? Exclusivo22 { get; set; }
        public int? UserF { get; set; }
        public DateTime? NF { get; set; }
        public string? NumF { get; set; }
        public int? UserCF { get; set; }
        public DateTime? CF { get; set; }
        public int? NumCF { get; set; }
        public int? Romaneio { get; set; }
        public string? LiberaCupom { get; set; }
        public decimal? PrecoVendaT1 { get; set; }
        public int? Tipo_Entrega { get; set; }
        public int? TaxaProduto { get; set; }
        public string? Imei { get; set; }
        public int? Ecf { get; set; }
        public string? Bilhete { get; set; }
        public int? IdSeguroSabemi { get; set; }
    }

    public class InfoParcelasPedido
    {
        public string? Parc { get; set; }
        public DateOnly? Vencimento { get; set; }
        public int? CupomTef { get; set; }
        public decimal? Valor { get; set; }
        public string? Forma { get; set; }
        public string? FormaRecebido { get; set; }
        public string? Documento { get; set; }
        public string? Pago { get; set; }
        public decimal? ValorPago { get; set; }
        public DateTime? DataPgto { get; set; }
        public int? Caixa { get; set; }
        public string? Usuario { get; set; }
        public string? Estornada { get; set; }
        public int? NumParcela { get; set; }
        public long? Codigo { get; set; }
        public string? Parcela { get; set; }
    }


}
