using System.ComponentModel;

namespace AppEmpresa.Classes.Geral
{
    public class GraficosInfos : INotifyPropertyChanged
    {
        private Thickness margingrafico;
        public Thickness marginGrafico
        {
            get { return margingrafico; }
            set
            {
                if (margingrafico != value)
                {
                    margingrafico = value;
                    OnPropertyChanged(nameof(marginGrafico));
                }
            }
        }

        private double largurabarra;
        public double LarguraBarra
        {
            get { return largurabarra; }
            set
            {
                if (largurabarra != value)
                {
                    largurabarra = value;
                    OnPropertyChanged(nameof(LarguraBarra));
                }
            }
        }

        private double tamanho;
        public double Tamanho
        {
            get { return tamanho; }
            set
            {
                if (tamanho != value)
                {
                    tamanho = value;
                    OnPropertyChanged(nameof(Tamanho));
                }
            }
        }

        private Decimal valor;
        public Decimal Valor
        {
            get { return valor; }
            set
            {
                if (valor != value)
                {
                    valor = value;
                    OnPropertyChanged(nameof(Valor));
                }
            }
        }

        private string identificacao;
        public string Identificacao
        {
            get { return identificacao; }
            set
            {
                if (identificacao != value)
                {
                    identificacao = value;
                    OnPropertyChanged(nameof(Identificacao));
                }
            }
        }

        private string tipoGrafico;
        public string TipoGrafico
        {
            get { return tipoGrafico; }
            set
            {
                if (tipoGrafico != value)
                {
                    tipoGrafico = value;
                    OnPropertyChanged(nameof(TipoGrafico));
                }
            }
        }

        private string corGrafico;
        public string CorGrafico
        {
            get { return corGrafico; }
            set
            {
                if (corGrafico != value)
                {
                    corGrafico = value;
                    OnPropertyChanged(nameof(CorGrafico));
                }
            }
        }

        private string valorFormatado;
        public string ValorFormatado
        {
            get { return valorFormatado; }
            set
            {
                if (valorFormatado != value)
                {
                    valorFormatado = value;
                    OnPropertyChanged(nameof(ValorFormatado));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
