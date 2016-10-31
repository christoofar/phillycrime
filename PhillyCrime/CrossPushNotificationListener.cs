using System;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using PushNotification.Plugin;
using PushNotification.Plugin.Abstractions;
using Xamarin.Forms;
using Newtonsoft.Json;

namespace PhillyBlotter
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
			Debug.WriteLine($"Message Arrived: {values.ToString()}");
			if (deviceType == PushNotification.Plugin.Abstractions.DeviceType.Android)
			{
				var p = JObject.Parse(values.ToString());
				if (p != null && p["info"] != null && p["info"].ToString() == "blotter")
				{
					Global.ReceivedCrimeAlert = true;
					MessagingCenter.Send<object, string>(Global.MessagingInstance, "Notification", p["text"].ToString());
				}
			}

			if (deviceType == PushNotification.Plugin.Abstractions.DeviceType.iOS)
			{
				var p = JObject.Parse(values.ToString());
				if (p != null && p["alert"] != null)
				{
					Global.ReceivedCrimeAlert = true;
					MessagingCenter.Send<object, string>(Global.MessagingInstance, "Notification", p["alert"].ToString());
				}
			}
		}

		public void OnRegistered(string token, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			Debug.WriteLine(string.Format("Push Notification - Device Registered - Token : {0}", token));

			Location.PostToken(token, deviceType);
		}

		public void OnUnregistered(PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			Debug.WriteLine("Push Notification - Device Unnregistered");
		}

		public bool ShouldShowNotification()
		{
			if (Device.OS == TargetPlatform.Android)
				return true;
			
			return true;
		}

	}
}
