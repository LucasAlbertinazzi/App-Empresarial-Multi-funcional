using AppMarciusMagazine.Services.Cobranca;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui;
using System.Globalization;

namespace AppMarciusMagazine.Suporte
{
    public static class ScoreOptions
    {

        public static string RemoveAllCharactersAndSpaces(string input)
        {
            string newInput = input.Replace(".", "");
            newInput = newInput.Replace("-", "");
            newInput = newInput.Replace(" ", "");

            return newInput;
        }

        public static string FormatDate(string input)
        {
            try
            {
                // Tentar analisar a data com formatos comuns
                DateTime date = DateTime.ParseExact(input,
                    new string[] { "dd-MM-yyyy", "dd-MM-yyyy HH:mm:ss", "dd/MM/yyyy", "dd/MM/yyyy HH:mm:ss" },
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None);

                // Retorna a data formatada
                return date.ToString("dd/MM/yyyy");
            }
            catch (FormatException)
            {
                // Caso não seja possível analisar a data, retorna a entrada original
                return input;
            }
        }

        public static async Task<bool> CarregaPDF(string resultadoScore, string codcliente)
        {
            APIScore aPIScore = new APIScore();

            var downloadPath = DependencyService.Get<IFileService>().GetDownloadFolderPath();
            string formattedDate = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var filePath = Path.Combine(downloadPath, $"SCORE_{codcliente}_{formattedDate}.pdf");

            using (var fileStream = File.Create(filePath))
            {
                var pdfStream = await aPIScore.CarregaPdfScore(resultadoScore);
                pdfStream.CopyTo(fileStream);
            }

            return DependencyService.Get<IFileService>().OpenPdfFile(filePath);
        }
    }
}
