#if __ANDROID__
using Android.Content;
using AppEmpresa.Platforms.Android;
#endif

namespace AppEmpresa.Suporte
{
    public static class SupAndroid
    {
        public static void NotificacaoOcorrencia()
        {
            #if __ANDROID__
            var intent = new Intent(Android.App.Application.Context, typeof(OcorrenciaService));
            Android.App.Application.Context.StartService(intent);
            #endif
        }
    }
}
