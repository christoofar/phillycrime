using Android.OS;

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
	}
}