using System;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using PhillyBlotter;
using PhillyBlotter.Droid;
using Plugin.CurrentActivity;
using PushNotification.Plugin;
using Xamarin.Forms;

namespace PhillyCrime.Droid
{
	//You can specify additional application information in this attribute
    [Application]
    public class MainApplication : Android.App.Application, Android.App.Application.IActivityLifecycleCallbacks
    {
		public static Context AppContext;
		public static bool Foreground = false;

        public MainApplication(IntPtr handle, JniHandleOwnership transer)
          :base(handle, transer)
        {
        }

        public override void OnCreate()
        {
            base.OnCreate();
			AppContext = this.ApplicationContext;

			CrossPushNotification.Initialize<CrossPushNotificationListener>("623924830057");
			CrossPushNotification.NotificationContentTitleKey = "title";
			CrossPushNotification.NotificationContentTextKey = "text";
			CrossPushNotification.NotificationContentDataKey = "info";
			CrossPushNotification.IconResource = Resource.Drawable.police;

            RegisterActivityLifecycleCallbacks(this);
			//A great place to initialize Xamarin.Insights and Dependency Services!

			//This service will keep your app receiving push even when closed.             
			StartPushService();
			Foreground = true;
        }

		public static void StartPushService()
		{
			AppContext.StartService(new Intent(AppContext, typeof(PushNotificationService)));

			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
			{
				PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
				AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
				alarm.Cancel(pintent);
			}
		}

		public static void StopPushService()
		{
			AppContext.StopService(new Intent(AppContext, typeof(PushNotificationService)));
			if (Android.OS.Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.Kitkat)
			{
				PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
				AlarmManager alarm = (AlarmManager)AppContext.GetSystemService(Context.AlarmService);
				alarm.Cancel(pintent);
			}
		}

        public override void OnTerminate()
        {
            base.OnTerminate();
			Foreground = false;
            UnregisterActivityLifecycleCallbacks(this);
        }

        public void OnActivityCreated(Activity activity, Bundle savedInstanceState)
        {
            CrossCurrentActivity.Current.Activity = activity;
			PendingIntent pintent = PendingIntent.GetService(AppContext, 0, new Intent(AppContext, typeof(PushNotificationService)), 0);
			Foreground = true;
        }

        public void OnActivityDestroyed(Activity activity)
        {
        }

        public void OnActivityPaused(Activity activity)
        {
			Foreground = false;
        }

        public void OnActivityResumed(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
			Foreground = true;
        }

        public void OnActivitySaveInstanceState(Activity activity, Bundle outState)
        {
        }

        public void OnActivityStarted(Activity activity)
        {
            CrossCurrentActivity.Current.Activity = activity;
        }

        public void OnActivityStopped(Activity activity)
        {
			Foreground = false;
        }
    }
}