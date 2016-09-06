using System;
using ObjCRuntime;
[assembly: Xamarin.Forms.Dependency(typeof(PhillyCrime.iOS.PlatformSpecific_iOS))]
namespace PhillyCrime.iOS
{
	public class PlatformSpecific_iOS : PlatformSpecificInterface
	{
		public bool CheckIfSimulator()
		{
			if (Runtime.Arch == Arch.SIMULATOR)
				return true;
			return false;
		}

	}
}

