using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using AppEmpresarialMultFuncional.Services.Cobranca;

namespace AppEmpresarialMultFuncional.Platforms.Android
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
                await api.ListenForOcorrencias();
            });

            return StartCommandResult.Sticky;
        }

        [Service]
        public class InnerService : Service
        {
            public override IBinder OnBind(Intent intent) => null;

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
            var notificationChannelId = "foreground_service_channel";
            var notificationChannelName = "Foreground Service Notification";

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channel = new NotificationChannel(notificationChannelId, notificationChannelName, NotificationImportance.Low)
                {
                    Description = "Foreground Service Channel"
                };

                var notificationManager = (NotificationManager)context.GetSystemService(NotificationService);
                notificationManager.CreateNotificationChannel(channel);
            }

            var notificationBuilder = new NotificationCompat.Builder(context, notificationChannelId)
                .SetContentTitle("Serviço de sincronização")
                .SetContentText("O App Empresarial Mult-Funcional está sendo executado")
                .SetSmallIcon(Resource.Drawable.notification_icon)
                .SetPriority(NotificationCompat.PriorityLow)
                .SetOngoing(false);

            return notificationBuilder.Build();
        }
    }
}
