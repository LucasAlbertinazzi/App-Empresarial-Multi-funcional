using AppMarciusMagazine.Classes.API.Principal;
using AppMarciusMagazine.Services.Principal;
using AppMarciusMagazine.Views.Cobranca;
using AppMarciusMagazine.Views.Principal;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;
using Plugin.LocalNotification.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppMarciusMagazine.Suporte
{
    public class Notificacao
    {
        #region 1- LOG
        APIErroLog error = new();

        private async Task MetodoErroLog(Exception ex)
        {
            var erroLog = new ErrorLogClass
            {
                Erro = ex.Message, // Obtém a mensagem de erro
                Metodo = ex.TargetSite.Name, // Obtém o nome do método que gerou o erro
                Dispositivo = DeviceInfo.Model, // Obtém o nome do dispositivo em execução
                Versao = DeviceInfo.Version.ToString(), // Obtém a versão do dispostivo
                Plataforma = DeviceInfo.Platform.ToString(), // Obtém o sistema operacional do dispostivo
                TelaClasse = GetType().FullName, // Obtém o nome da tela/classe
                Data = DateTime.Now,
            };

            await error.LogErro(erroLog);
        }
        #endregion

        #region 2- CLASSE

        public Notificacao()
        {
            // Assine o evento de toque na notificação
            LocalNotificationCenter.Current.NotificationActionTapped += OnNotificationTapped;
        }

        private void OnNotificationTapped(NotificationActionEventArgs e)
        {
            var tela = e.Request.ReturningData;

            if (string.IsNullOrEmpty(tela))
            {
                // Navegue para a tela VMenuPrincipal se não houver nome de tela especificado
                Application.Current.MainPage.Navigation.PushAsync(new VMenuPrincipal());
            }
            else if(tela == "VOcorrencia")
            {
                // Navegue para a tela especificada usando o valor de 'tela'
                Application.Current.MainPage.Navigation.PushAsync(new VOcorrencia());
            }
        }

        public async Task EnviaNotificacao(string sub, string desc, string nomeTela)
        {
            try
            {
                var request = new NotificationRequest
                {
                    NotificationId = 2,
                    Title = "Marciu's Magazine",
                    Subtitle = sub,
                    Description = desc,
                    BadgeNumber = 10,
                    CategoryType = NotificationCategoryType.Status,
                    Schedule = new NotificationRequestSchedule
                    {
                        NotifyTime = DateTime.Now.AddSeconds(5),
                    },
                    Android = new AndroidOptions
                    {
                        Priority = AndroidPriority.Max,
                        Ongoing = false,
                        VibrationPattern = new long[] { 0, 200, 100, 200, 100, 200 },
                    },
                    ReturningData = nomeTela
                };

                await LocalNotificationCenter.Current.Show(request);
            }
            catch (Exception ex)
            {
                await MetodoErroLog(ex);
                return;
            }
        }
        #endregion
    }
}
