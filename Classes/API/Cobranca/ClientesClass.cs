using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Classes.API.Cobranca
{
    public class Cliente
    {
        public int Codcliente { get; set; }

        public string? Nome { get; set; }

        public DateTime? Datacadastro { get; set; }

        public string? Cpf { get; set; }

        public string? Rg { get; set; }

        public string? Inscest { get; set; }

        public string? Cnpj { get; set; }

        public string? Oe { get; set; }

        public string? Naturalidade { get; set; }

        public string? Pai { get; set; }

        public string? Mae { get; set; }

        public DateTime? Nascimento { get; set; }

        public string? Ctps { get; set; }

        public string? Serie { get; set; }

        public string? Estadocivil { get; set; }

        public string? Endereco { get; set; }

        public string? Bairro { get; set; }

        public string? Cidade { get; set; }

        public string? Uf { get; set; }

        public string? Cep { get; set; }

        public string? Tiporesid { get; set; }

        public string? Temporesid { get; set; }

        public string? Fone { get; set; }

        public string? Tipofone { get; set; }

        public string? Email { get; set; }

        public string? Empresa { get; set; }

        public DateOnly? Admissao { get; set; }

        public string? EmpEndereco { get; set; }

        public string? EmpBairro { get; set; }

        public string? EmpCidade { get; set; }

        public string? EmpUf { get; set; }

        public string? EmpCep { get; set; }

        public string? Cargo { get; set; }

        public string? EmpFone { get; set; }

        public string? Confirmadocom { get; set; }

        public string? Celular { get; set; }

        public string? Conjuge { get; set; }

        public DateTime? ConjNascimento { get; set; }

        public string? ConjCpf { get; set; }

        public string? ConjRg { get; set; }

        public string? ConjOe { get; set; }

        public string? ConjNaturalidade { get; set; }

        public string? ConjPai { get; set; }

        public string? ConjMae { get; set; }

        public string? ConjEmpresa { get; set; }

        public DateOnly? ConjAdmissao { get; set; }

        public string? ConjEmpEndereco { get; set; }

        public string? ConjEmpBairro { get; set; }

        public string? ConjEmpCidade { get; set; }

        public string? ConjEmpUf { get; set; }

        public string? ConjEmpCep { get; set; }

        public string? ConjCargo { get; set; }

        public decimal? ConjRendaliquida { get; set; }

        public string? ConjEmpFone { get; set; }

        public string? ConjConfirmadocom { get; set; }

        public string? ConjCelular { get; set; }

        public int? Codfiador { get; set; }

        public string? Classe { get; set; }

        public decimal? Limitecredito { get; set; }

        public DateTime? Atualizado { get; set; }

        public DateTime? Dataaprovacao { get; set; }

        public string? Observacao { get; set; }

        public string? Situacao { get; set; }

        public string? Vip { get; set; }

        public long Codusuario { get; set; }

        public decimal? Rendaliquida { get; set; }

        public string? Aprovadopor { get; set; }

        public char? Tipocadastro { get; set; }

        public DateTime? Consspc { get; set; }

        public char? Negativado { get; set; }

        public DateTime? ConjConsspc { get; set; }

        public char? ConjNegativado { get; set; }

        public DateTime? PriAprovacao { get; set; }

        public string? PriUsuario { get; set; }

        public string? PriSituacao { get; set; }

        public char? ClienteSpc { get; set; }

        public DateOnly? Pgtopara { get; set; }

        public char? Pricontato { get; set; }

        public char? Segcontato { get; set; }

        public DateOnly? Agendacobranca { get; set; }

        public char? Carta1 { get; set; }

        public char? Carta2 { get; set; }

        public char? Carta3 { get; set; }

        public char? Carta4 { get; set; }

        public char? Carta5 { get; set; }

        public char? Protesto { get; set; }

        public char? Cadassinado { get; set; }

        public string? Tipoemprego { get; set; }

        public string? Tipoempregoconj { get; set; }

        public decimal? Rendaadd { get; set; }

        public decimal? RendaaddAutor { get; set; }

        public string? Userrendaadd { get; set; }

        public string? Origemrendaadd { get; set; }

        public string? Tiporendaadd { get; set; }

        public int? Rotacobranca { get; set; }

        public char? Reaprovar { get; set; }

        public string? Confreaprovacao { get; set; }

        public string? Obsreaprovar { get; set; }

        public string? ContatoQuitado { get; set; }

        public string? ObsQuitado { get; set; }

        public DateTime? DatacontatoQuitado { get; set; }

        public char? ConjugeSpc { get; set; }

        public string? EnderecoC { get; set; }

        public string? BairroC { get; set; }

        public string? CidadeC { get; set; }

        public string? UfC { get; set; }

        public string? CepC { get; set; }

        public char? MesmoEnd { get; set; }

        public string? Beneficio { get; set; }

        public DateOnly? Expedicaorg { get; set; }

        public DateOnly? Temporeside { get; set; }

        public string? Obs2 { get; set; }

        public int? Contatoniver { get; set; }

        public DateTime? Datacontatoniver { get; set; }

        public string? ConjBeneficio { get; set; }

        public int? Codbairro { get; set; }

        public int? EmpreCodbairro { get; set; }

        public int? CodbairroC { get; set; }

        public int? Codcidade { get; set; }

        public int? EmpreCodcidade { get; set; }

        public int? CodcidadeC { get; set; }

        public string? EnderecoNum { get; set; }

        public string? EmpreEnderecoNum { get; set; }

        public string? EnderecoNumC { get; set; }

        public string? EnderecoNumEmpConj { get; set; }

        public int? CodbairroEmpConj { get; set; }

        public int? CodcidadeEmpConj { get; set; }

        public string? ComplemEnd { get; set; }

        public string? RecadoCom { get; set; }

        public char? Carta22 { get; set; }

        public char? Carta23 { get; set; }

        public string? Genero { get; set; }

        public bool? CadEmp { get; set; }
    }
    public class ClientesClass
    {
        public Cliente Cliente { get; set; }
        public string Usuario { get; set; }
        public string BairroCliente { get; set; }
        public string CidadeCliente { get; set; }
        public string BCidadeEmpresa { get; set; }
        public string BairroConjuje { get; set; }
        public string CidadeConjuje { get; set; }
        public string BairroCorrespondencia { get; set; }
        public string CidadeCorrespondencia { get; set; }
    }

    public class InfoHistoricoCliente
    {
        public Color CorBack { get; set; }
        public Color CorBorder { get; set; }
        public string? codpedido { get; set; }
        public DateOnly? vencimento { get; set; }
        public Decimal? valor { get; set; }
        public string? pago { get; set; }
        public Double? atraso { get; set; }
        public int? qtdpedido { get; set; }
        public Decimal? valorgasto { get; set; }
        public Decimal? valorpago { get; set; }
        public string? nomecliente { get; set; }
    }
}
