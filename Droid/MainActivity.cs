using System;

using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Common;
using PushNotification.Plugin;
using System.Reflection;
using System.Net;
using HockeyApp.Android;
using HockeyApp.Android.Metrics;

namespace PhillyBlotter.Droid
{
	[Activity(Label = "PhillyBlotter", Icon = "@drawable/icon", Theme = "@style/MyTheme", MainLauncher = false, 
	          ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{

		private const string appId = "c1787e2f3e554b9aa7f7969dc60f3f80";

		protected override void OnCreate(Bundle bundle)
		{

			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

			Xamarin.FormsMaps.Init(this, bundle);

			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;

			base.OnCreate(bundle);

			/* HockeyApp Init */
			CrashManager.Register(this);
			MetricsManager.Register(Application);

			global::Xamarin.Forms.Forms.Init(this, bundle);

			LoadApplication(new App());

			if (IsPlayServicesAvailable())
			{
				PushNotification.Plugin.CrossPushNotification.Current.Register();
			}

		}

		public bool IsPlayServicesAvailable()
		{
			int resultCode = GoogleApiAvailability.Instance.IsGooglePlayServicesAvailable(this);
			if (resultCode != ConnectionResult.Success)
			{
				if (GoogleApiAvailability.Instance.IsUserResolvableError(resultCode))
				{
					//Debug.("Error with Android notifications -> " + GoogleApiAvailability.Instance.GetErrorString(resultCode));
				}
				else
				{
					//Debug.WriteLine("Sorry, this device is not supported for notifications");
					//Finish();
				}
				return false;
			}
			else
			{
				//Debug.WriteLine("Google Play Services is available.");
				return true;
			}
		}
	}
}

