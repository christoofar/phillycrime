﻿using System;
using ObjCRuntime;
using UIKit;
using Foundation;
using PhillyBlotter;

[assembly: Xamarin.Forms.Dependency(typeof(PhillyBlotter.iOS.PlatformSpecific_iOS))]
namespace PhillyBlotter.iOS
{
	public class PlatformSpecific_iOS : PlatformSpecificInterface
	{
		public bool CheckIfSimulator()
		{
			if (Runtime.Arch == Arch.SIMULATOR)
				return true;
			return false;
		}

		public void ClearBadges()
		{
			var notification = new UILocalNotification();
			var fireDate = NSDate.FromTimeIntervalSinceNow(0);
			notification.FireDate = fireDate;
			notification.ApplicationIconBadgeNumber = -1;
			UIApplication.SharedApplication.ScheduleLocalNotification(notification);
		}

		public void BringToForeground()
		{
		}
	}
}

