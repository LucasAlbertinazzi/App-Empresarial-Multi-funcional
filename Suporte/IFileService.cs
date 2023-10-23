using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppEmpresarialMultFuncional.Suporte
{
    public interface IFileService
    {
        string GetDownloadFolderPath();

        bool OpenPdfFile(string file);
    }
}
