using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using AppEmpresa.Services.Cobranca;

namespace AppEmpresa.Platforms.Android
{
    [Service]
    public class OcorrenciaService : Service
    {
        public override IBinder OnBind(Intent intent)
        {
            return null;
        }

        public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
        {
            var notification = CreateForegroundNotification(this);
            StartForeground(1, notification);
            StartService(new Intent(this, typeof(InnerService)));

            Task.Run(async () =>
            {
                var api = new APIOcorrencia();
                await api.ListenForOcorrencias().ConfigureAwait(false);
            });

            return StartCommandResult.Sticky;
        }

        [Service]
        public class InnerService : Service
        {
            public override IBinder OnBind(Intent intent)
            {
                return null;
            }

            public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
            {
                var notification = OcorrenciaService.CreateForegroundNotification(this);
                StartForeground(1, notification);
                StopForeground(true);
                StopSelf();
                return StartCommandResult.Sticky;
            }
        }

        public static Notification CreateForegroundNotification(Context context)
        {
            var notificationChannelId = "servico_sincronizacao_app";
            var notificationChannelName = "Sincronização de Notificação APP";

            using (var notificationManager = (NotificationManager)context.GetSystemService(NotificationService))
            {
                if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
                {
                    var channel = new NotificationChannel(notificationChannelId, notificationChannelName, NotificationImportance.Low)
                    {
                        Description = "Sincronização do serviço de ocorrências do App Empresa"
                    };

                    notificationManager.CreateNotificationChannel(channel);
                }
            }

            var notificationBuilder = new NotificationCompat.Builder(context, notificationChannelId)
                .SetContentTitle("Serviço de sincronização")
                .SetContentText("O App Empresa está ativo em segunda plano.")
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetPriority(NotificationCompat.PriorityLow)
                .SetOngoing(false);

            return notificationBuilder.Build();
        }
    }
}
