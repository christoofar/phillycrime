﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using Foundation;
using PushNotification.Plugin;
using UIKit;
using HockeyApp.iOS;

namespace PhillyBlotter.iOS
{

	[Register("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		const string TAG = "PushNotification-APN";

		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
			ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

			global::Xamarin.Forms.Forms.Init();

			var manager = BITHockeyManager.SharedHockeyManager;
			manager.Configure("1bc1155d4570492ca45008d451abaafb");
			manager.StartManager();

			CrossPushNotification.Initialize<CrossPushNotificationListener>();

			// I want notifications!
			PushNotification.Plugin.CrossPushNotification.Current.Register();

			//PushNotification.Plugin.CrossPushNotification.Current.Unregister();

			LoadApplication(new App());

			Xamarin.FormsMaps.Init();

			return base.FinishedLaunching(app, options);
		}

		public override void FailedToRegisterForRemoteNotifications(UIApplication application, NSError error)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler)
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnErrorReceived(error);
			}
		}

		public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
		{
			if (CrossPushNotification.Current is IPushNotificationHandler)
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnRegisteredSuccess(deviceToken);

			}

		}

		public override void DidRegisterUserNotificationSettings(UIApplication application, UIUserNotificationSettings notificationSettings)
		{
			application.RegisterForRemoteNotifications();
		}

		// Uncomment if using remote background notifications. To support this background mode, enable the Remote notifications 
		// option from the Background modes section of iOS project properties. 
		// (You can also enable this support by including the UIBackgroundModes key with the remote-notification 
		//  value in your appís Info.plist file.)
        public override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
			if (CrossPushNotification.Current is IPushNotificationHandler) 
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);
			}
        }        

		public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
		{

			if (CrossPushNotification.Current is IPushNotificationHandler)
			{
				((IPushNotificationHandler)CrossPushNotification.Current).OnMessageReceived(userInfo);

			}
		}
	}


}

