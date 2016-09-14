using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using PhillyCrime;
using Xamarin.Forms.Maps;
using PhillyCrime.Models;
using Newtonsoft.Json;

namespace PhillyCrime.Models
{
	public class Data
	{
		//private static string APIBASE = "http://192.168.1.30/phillycrime/api/";
		private static string APIBASE = "https://philadelinquency.com/phillycrime/api/";
		//private static string APIBASE = "http://homeserver.local/phillycrime/api/";
		private static string GET_30DAY = string.Format("{0}Values", APIBASE);
		private static string FULLREPORT = string.Format("{0}Crime", APIBASE);
		private static string PHILLY = string.Format("{0}Philly", APIBASE);
		private static string PUSH = string.Format("{0}Push", APIBASE);

		public Data()
		{
		}

		public async static Task<bool> RegisterPushNotifications(string token, PushNotification.Plugin.Abstractions.DeviceType deviceType, double longitude, double latitude)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = PUSH;

			var notification = new crime_notification();
			notification.DeviceToken = token;
			notification.DeviceType = Convert.ToInt32(deviceType);
			notification.Longitude = longitude;
			notification.Latitude = latitude;

			Debug.WriteLine("About to post registration to server...");
			await cli.DoSilentPost(getUri, JsonConvert.SerializeObject(notification));
			return true;
		}

		public async static Task<PoliceDistrict> GetPoliceDistrict(double longitude, double latitude)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = PHILLY + string.Format("/{0}/{1}/", longitude, latitude);

			var resp = await cli.DoRequestJsonAsync<PoliceDistrict>(getUri);
			return resp;
		}

		public async static Task<FullCrimeReport> GetFullCrimeReport(string DCN)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = FULLREPORT + string.Format("/{0}/",
													   DCN);

			var resp = await cli.DoRequestJsonAsync<FullCrimeReport>(getUri);
			return resp;
		}

		public async static Task<CrimeReport[]> Get30DayCrimeData(MapSpan span, Filter currentFilter)
		{

			// If the span is ridiculous we're not going to the server for data.
			if (span != null && span.LongitudeDegrees < 5)
			{

				JsonWebClient cli = new JsonWebClient();
				Debug.WriteLine("Span Center: {0},{1}   Longitude Degrees: {2}  Latitude Degrees: {3}",
								span.Center.Longitude, span.Center.Latitude, span.LongitudeDegrees, span.LatitudeDegrees);
				string getUri = GET_30DAY + string.Format("/{0}/{1}/{2}/{3}/{4}/",
														  span.Center.Longitude,
														  span.Center.Latitude,
														  span.LongitudeDegrees,
														  span.LatitudeDegrees,
				                                          Convert.ToInt32(currentFilter));


				var resp = await cli.DoRequestJsonAsync<CrimeReport[]>(getUri);
				return resp;

			} 
			else
			{
				return new CrimeReport[0];
			}

		}
	}
}

