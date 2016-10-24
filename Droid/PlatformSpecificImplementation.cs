using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using PhillyCrime.Droid;

[assembly: Xamarin.Forms.Dependency(typeof(PhillyBlotter.Droid.PlatformSpecific_Droid))]
namespace PhillyBlotter.Droid
{
	public class PlatformSpecific_Droid : PlatformSpecificInterface
	{
		public bool CheckIfSimulator()
		{
			if (Build.Fingerprint != null)
			{
				if (Build.Fingerprint.Contains("vbox") ||
					Build.Fingerprint.Contains("generic"))
					return true;
			}
			return false;
		}

		public void ClearBadges()
		{
			//No-op.  Android doesn't support this
		}

		public void BringToForeground()
		{
			var intent = new Intent(MainApplication.AppContext, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.NewTask | ActivityFlags.TaskOnHome);
			MainApplication.AppContext.StartActivity(intent);
		}

		public async Task<string> ResolveIPAddress(string host)
		{
			var addresses = await Dns.GetHostAddressesAsync(host);
			if (addresses != null && addresses.Length > 0)
				return addresses[0].ToString();
			return "";
		}

		public string GetClipboardData()
		{
			var app = Application.Context;

			var clipboard = (ClipboardManager)app.GetSystemService(Context.ClipboardService);

			       
			if (clipboard != null && clipboard.HasPrimaryClip)
			{
				return clipboard.Text;
			}

			return "";
		}
	}
}