using Android.Content;
using AppMarciusMagazine.Platforms.Android;
using AppMarciusMagazine.Suporte;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[assembly: Dependency(typeof(FileService))]
namespace AppMarciusMagazine.Platforms.Android
{
    public class FileService : IFileService
    {
        public string GetDownloadFolderPath()
        {
            return global::Android.OS.Environment.ExternalStorageDirectory + "/Download";
        }

        public bool OpenPdfFile(string filePath)
        {
            try
            {
                // Abrir o arquivo PDF após salvá-lo
                var pdfFile = new Java.IO.File(filePath);

                // Use FileProvider para obter o Uri
                var pdfUri = FileProvider.GetUriForFile(Platform.CurrentActivity.ApplicationContext, Platform.CurrentActivity.PackageName + ".fileprovider", pdfFile);

                var intent = new Intent(Intent.ActionView);
                intent.SetDataAndType(pdfUri, "application/pdf");
                intent.SetFlags(ActivityFlags.ClearTop);
                intent.SetFlags(ActivityFlags.NewTask);

                // Adicione esta linha para dar permissão de leitura ao URI
                intent.AddFlags(ActivityFlags.GrantReadUriPermission);

                Platform.CurrentActivity.StartActivity(intent);
                return true;
            }
            catch (ActivityNotFoundException rc)
            {
                return false;
            }
        }
    }
}
