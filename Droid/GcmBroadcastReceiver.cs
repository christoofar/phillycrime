//using Android.App;
//using Android.Content;
//using Android.Graphics;
//using Android.Media;
//using Android.OS;
//using Android.Support.V4.App;
//using PushNotification.Plugin;
//using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;

//[assembly: Permission(Name = "com.philadelinquency.phillycrime.permission.C2D_MESSAGE")]
//[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
//[assembly: UsesPermission(Name = "com.philadelinquency.permission.C2D_MESSAGE")]
//[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
//[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
//[assembly: UsesPermission(Name = "android.permission.INTERNET")]


//namespace PhillyBlotter.Droid
//{
//	[BroadcastReceiver(Exported = true, Name="com.philadelinquency.phillycrime.GCMBroadcastReciever", Permission = "com.google.android.c2dm.permission.SEND")]
//	[IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "com.philadelinquency.phillycrime" })]
//	[IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "com.philadelinquency.phillycrime" })]
//	[IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "com.philadelinquency.phillycrime" })]
//	public class GCMBroadcastReciever : BroadcastReceiver
//	{
//		public override void OnReceive(Context context, Intent intent)
//		{

//			PendingIntent pintent = PendingIntent.GetService(context, 0, new Intent(context, typeof(PushNotificationService)), 0);

//			PowerManager.WakeLock sWakeLock;
//			var pm = PowerManager.FromContext(context);
//			sWakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "GCM Broadcast Reciever Tag");
//			sWakeLock.Acquire();

//			// I want to use this custom receiver but I think it's doing V4 stuff I don't want.
//			// Will have to come back to it some other time.
//			////handle the notification here
//			//if (intent.Action.Equals("com.google.android.c2dm.intent.RECEIVE"))
//			//{
//			//	if (intent.GetStringExtra("info") == "blotter")
//			//	{

//			//		// We have a blotter notification.
//			//		NotificationCompat.Builder builder = new NotificationCompat.Builder(context)
//			//		  .SetAutoCancel(true) // dismiss the notification from the notification area when the user clicks on it
//			//		  .SetContentIntent(pintent) // start up this activity when the user clicks the intent.
//			//		  .SetContentTitle("PhillyBlotter") // Set the title
//			//		  //.SetNumber(number) // Display the count in the Content Info
//			//		  .SetSmallIcon(Resource.Drawable.police) // This is the icon to display
//			//			//.SetLargeIcon(bitmap)
//			//		  .SetSound(RingtoneManager.GetDefaultUri(RingtoneType.Notification))
//			//			.SetContentText(intent.GetStringExtra("text")); // the message to display.

//			//		// Build the notification:
//			//		Notification notification = builder.Build();

//			//		// Get the notification manager:
//			//		NotificationManager notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager;

//			//		// Publish the notification:
//			//		const int notificationId = 0;
//			//		notificationManager.Notify(notificationId, notification);
//			//	}
//			//}

//			sWakeLock.Release();
//		}

//		private void HandleMessage(Context context, Intent intent)
//		{
			
//			//NotificationCompat.Builder builder = new NotificationCompat.Builder (context)
//			//	.SetContentTitle ("Test")
//			//	.SetSmallIcon (Resource.Drawable.police)
//			//	.SetContentText ("Testing");

//			// Obtain a reference to the NotificationManager
//			//NotificationManager notificationManager = (NotificationManager)GetSystemService(Context.NotificationService);
//			//notificationManager.Notify (1, builder.Build ());

//		}
//	}
//}

