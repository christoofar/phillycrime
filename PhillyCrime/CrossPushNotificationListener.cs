using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;

namespace PhillyCrime
{
	public enum DeviceType
	{
		Android,
		iOS,
		WindowsPhone,
		Windows
	}

	public class CrossPushNotificationListener : IPushNotificationListener
	{
		public CrossPushNotificationListener()
		{
		}

		public void OnError(string message, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			Debug.WriteLine(string.Format("Push notification error - {0}", message));
		}

		public void OnMessage(JObject values, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			Debug.WriteLine("Message Arrived");
		}

		public void OnRegistered(string token, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", token));
		}

		public void OnUnregistered(PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			Debug.WriteLine("Push Notification - Device Unnregistered");
		}

		public bool ShouldShowNotification()
		{
			return true;
		}
	}
}
