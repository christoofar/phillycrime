using System;
using Android.OS;
[assembly: Xamarin.Forms.Dependency (typeof (PhillyCrime.Droid.PlatformSpecific_Droid))]
namespace PhillyCrime.Droid
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
	}
}