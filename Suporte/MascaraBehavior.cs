using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Suporte
{
    public class MascaraBehavior : Behavior<Entry>
    {
        private string mascara;
        private bool isFormatting; // Adiciona uma flag para controlar a formatação


        public MascaraBehavior(string mascara)
        {
            this.mascara = mascara;
        }

        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (e.OldTextValue != null && e.NewTextValue != null && !string.IsNullOrWhiteSpace(mascara) && !isFormatting)
                {
                    var entry = (Entry)sender;
                    isFormatting = true;

                    var textoSemMascara = RemoverMascara(e.NewTextValue);
                    var textoComMascara = AplicarMascara(textoSemMascara);

                    // Verifica se o texto foi apagado
                    if (e.NewTextValue.Length < e.OldTextValue.Length)
                    {
                        textoComMascara = textoComMascara.Substring(0, e.NewTextValue.Length);
                    }

                    entry.Text = textoComMascara;
                    entry.CursorPosition = textoComMascara.Length;

                    isFormatting = false;
                }
            }
            catch (Exception ex)
            {
                string erro = ex.Message;
                throw;
            }
           
        }

        private string RemoverMascara(string texto)
        {
            return new string(texto.Where(char.IsDigit).ToArray());
        }

        private string AplicarMascara(string texto)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return string.Empty;

            // Remove todos os caracteres não numéricos
            texto = new string(texto.Where(char.IsDigit).ToArray());

            // Aplica a máscara de CPF: "000.000.000-00"
            if (mascara == "CPF")
            {
                if (texto.Length >= 3)
                    texto = texto.Insert(3, ".");
                if (texto.Length >= 7)
                    texto = texto.Insert(7, ".");
                if (texto.Length >= 11)
                    texto = texto.Insert(11, "-");
            }

            // Aplica a máscara de CNPJ: "00.000.000/0000-00"
            else if (mascara == "CNPJ")
            {
                if (texto.Length >= 2)
                    texto = texto.Insert(2, ".");
                if (texto.Length >= 6)
                    texto = texto.Insert(6, ".");
                if (texto.Length >= 10)
                    texto = texto.Insert(10, "/");
                if (texto.Length >= 15)
                    texto = texto.Insert(15, "-");
            }

            // Aplica a máscara de data: "dd/MM/yyyy"
            else if (mascara == "data")
            {
                if (texto.Length >= 2)
                    texto = texto.Insert(2, "/");
                if (texto.Length >= 5)
                    texto = texto.Insert(5, "/");
            }

            return texto;
        }
    }
}
