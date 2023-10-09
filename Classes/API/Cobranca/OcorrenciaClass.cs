using System;
using System.ComponentModel;

namespace AppMarciusMagazine.Classes.API.Cobranca
{
    public class OcorrenciaClass : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private int _codsolicitacao;
        private string? _loja;
        private string _nomeCliente;
        private string? _tipo;
        private string? _codigo;
        private DateTime? _datasolicitacao;
        private DateTime? _datasenha;
        private int? _analista;
        private string? _usuario;
        private string _obs;
        private long _codcliente;
        private long? _codLoja;
        private short? _tiposenha;
        private int? _solicitante;
        private bool? _cancelado;
        private bool? _finalizado;
        private string _nomeExibicao;

        public int Codsolicitacao
        {
            get { return _codsolicitacao; }
            set
            {
                if (_codsolicitacao != value)
                {
                    _codsolicitacao = value;
                    OnPropertyChanged(nameof(Codsolicitacao));
                }
            }
        }

        public string? Loja
        {
            get { return _loja; }
            set
            {
                if (_loja != value)
                {
                    _loja = value;
                    OnPropertyChanged(nameof(Loja));
                }
            }
        }

        public string NomeCliente
        {
            get { return _nomeCliente; }
            set
            {
                if (_nomeCliente != value)
                {
                    _nomeCliente = value;
                    OnPropertyChanged(nameof(NomeCliente));
                }
            }
        }

        public string Tipo
        {
            get { return _tipo; }
            set
            {
                if (_tipo != value)
                {
                    _tipo = value;
                    OnPropertyChanged(nameof(Tipo));
                }
            }
        }

        public string Codigo
        {
            get { return _codigo; }
            set
            {
                if (_codigo != value)
                {
                    _codigo = value;
                    OnPropertyChanged(nameof(Codigo));
                }
            }
        }

        public DateTime? Datasolicitacao
        {
            get { return _datasolicitacao; }
            set
            {
                if (_datasolicitacao != value)
                {
                    _datasolicitacao = value;
                    OnPropertyChanged(nameof(Datasolicitacao));
                }
            }
        }

        public DateTime? Datasenha
        {
            get { return _datasenha; }
            set
            {
                if (_datasenha != value)
                {
                    _datasenha = value;
                    OnPropertyChanged(nameof(Datasenha));
                }
            }
        }

        public int? Analista
        {
            get { return _analista; }
            set
            {
                if (_analista != value)
                {
                    _analista = value;
                    OnPropertyChanged(nameof(Analista));
                }
            }
        }

        public string? Usuario
        {
            get { return _usuario; }
            set
            {
                if (_usuario != value)
                {
                    _usuario = value;
                    OnPropertyChanged(nameof(Usuario));
                }
            }
        }

        public string Obs
        {
            get { return _obs; }
            set
            {
                if (_obs != value)
                {
                    _obs = value;
                    OnPropertyChanged(nameof(Obs));
                }
            }
        }

        public long Codcliente
        {
            get { return _codcliente; }
            set
            {
                if (_codcliente != value)
                {
                    _codcliente = value;
                    OnPropertyChanged(nameof(Codcliente));
                }
            }
        }

        public long? CodLoja
        {
            get { return _codLoja; }
            set
            {
                if (_codLoja != value)
                {
                    _codLoja = value;
                    OnPropertyChanged(nameof(CodLoja));
                }
            }
        }

        public short? Tiposenha
        {
            get { return _tiposenha; }
            set
            {
                if (_tiposenha != value)
                {
                    _tiposenha = value;
                    OnPropertyChanged(nameof(Tiposenha));
                }
            }
        }

        public int? Solicitante
        {
            get { return _solicitante; }
            set
            {
                if (_solicitante != value)
                {
                    _solicitante = value;
                    OnPropertyChanged(nameof(Solicitante));
                }
            }
        }

        public bool? Cancelado
        {
            get { return _cancelado; }
            set
            {
                if (_cancelado != value)
                {
                    _cancelado = value;
                    OnPropertyChanged(nameof(Cancelado));
                }
            }
        }

        public bool? Finalizado
        {
            get { return _finalizado; }
            set
            {
                if (_finalizado != value)
                {
                    _finalizado = value;
                    OnPropertyChanged(nameof(Finalizado));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public class RestricaoOcorrencias 
    {
        public string? Motivo { get; set; }
        public string? Detalhe { get; set; }
        public int CodOcorrencia { get; set; }
        public int? CodPrePedido { get; set; }

        public Color Cor { get; set; }
    }

    public class SenhaNegada
    {
        public string? Motivo { get; set; }
        public string? Usuario { get; set; }
        public DateTime? Data { get; set; }
        public int? Senha { get; set; }
    }

    public class ScoreHist
    {
        public string? Atenuentes { get; set; }
        public string? Agravantes { get; set; }
    }

    public class SenhaPedido
    {
        public string Senha2 { get; set; }
        public DateTime Horasenha2 { get; set; }
        public string Senha2obs { get; set; }
        public int Codprepedido { get; set; }
        public int Codcliente { get; set; }
        public int Codusuario { get; set; }
        public bool Identifica { get; set; }
        public int Codsolicitacao { get; set; }
    }

}
