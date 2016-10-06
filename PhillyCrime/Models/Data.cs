using System;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms.Maps;

namespace PhillyBlotter.Models
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
		private static string NEIGHBORHOOD = string.Format("{0}Neighborhood", APIBASE);
		private static string BLOTTER = string.Format("{0}Blotter", APIBASE);

		public Data()
		{
		}


		public async static Task<bool> RegisterPushNotifications(string token,
					PushNotification.Plugin.Abstractions.DeviceType deviceType,
					double longitude, double latitude, double radius)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = PUSH;

			var notification = new crime_notification();
			notification.DeviceToken = token;
			notification.DeviceType = Convert.ToInt32(deviceType);
			notification.Longitude = longitude;
			notification.Latitude = latitude;
			notification.Radius = radius;
			notification.Version = Global.VERSION;

			Debug.WriteLine($"About to post registration to server... LONG: {longitude}  LAT: {latitude}");
			await cli.DoSilentPost(getUri, JsonConvert.SerializeObject(notification));
			return true;
		}

		public async static Task<bool> RegisterPushNotifications(string token, 
		                    PushNotification.Plugin.Abstractions.DeviceType deviceType, 
		                    double longitude, double latitude)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = PUSH;

			var notification = new crime_notification();
			notification.DeviceToken = token;
			notification.DeviceType = Convert.ToInt32(deviceType);
			notification.Longitude = longitude;
			notification.Latitude = latitude;
			notification.Version = Global.VERSION;

			Debug.WriteLine($"About to post registration to server... LONG: {longitude}  LAT: {latitude}");
			await cli.DoSilentPost(getUri, JsonConvert.SerializeObject(notification));
			return true;
		}

		/// <summary>
		/// Gets the 10-day blotter given an array of neighborhood IDs. The Neighborhoods are found using the call to Neighborhoods().
		/// </summary>
		/// <returns>The blotter.</returns>
		/// <param name="neighborhoods">Neighborhoods.</param>
		public async static Task<CrimeReport[]> GetBlotter(int[] neighborhoods)
		{
			JsonWebClient cli = new JsonWebClient();

			string hoods = "";
			for (int i = 0; i < neighborhoods.Length; i++)
			{
				hoods += neighborhoods[i].ToString();
				if (i != neighborhoods.Length - 1)
				{
					hoods += ",";
				}
			}

			string getUri = BLOTTER + string.Format("/{0}/", hoods);

			var resp = await cli.DoRequestJsonAsync<CrimeReport[]>(getUri);
			return resp;
		}

		/// <summary>
		/// This pulls a hyper-local 10 day blotter based on a position and a radius distance.
		/// </summary>
		/// <returns>The local blotter.</returns>
		/// <param name="latitutde">Latitutde.</param>
		/// <param name="longitude">Longitude.</param>
		/// <param name="distance">Distance.</param>
		public async static Task<CrimeReport[]> GetLocalBlotter(double latitutde, double longitude, double distance)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = BLOTTER + string.Format($"/{latitutde}/{longitude}/{distance}/");

			var resp = await cli.DoRequestJsonAsync<CrimeReport[]>(getUri);
			return resp;
		}

		/// <summary>
		/// Returns the neighborhoods of Philly
		/// </summary>
		public async static Task<Neighborhood[]> Neighborhoods()
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = NEIGHBORHOOD;
			var resp = await cli.DoRequestJsonAsync<Neighborhood[]>(getUri);
			return resp;
		}

		/// <summary>
		/// Returns specific details (neighborhood, local police district) using the specified longitude and latitude.
		/// It's used to determine where someone is in Philadelphia in relation to community resources and which
		/// neighborhood they're in.
		/// </summary>
		/// <param name="longitude">Longitude.</param>
		/// <param name="latitude">Latitude.</param>
		public async static Task<Area> Area(double longitude, double latitude)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = PHILLY + string.Format("/{0}/{1}/", longitude, latitude);

			var resp = await cli.DoRequestJsonAsync<Area>(getUri);
			return resp;
		}

		/// <summary>
		/// Gets the full crime report using the District Control Number identifier from police.
		/// </summary>
		/// <returns>The full crime report.</returns>
		/// <param name="DCN">Dcn.</param>
		public async static Task<FullCrimeReport> GetFullCrimeReport(string DCN)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = FULLREPORT + string.Format("/{0}/",
													   DCN);

			var resp = await cli.DoRequestJsonAsync<FullCrimeReport>(getUri);
			return resp;
		}

		public async static Task<CrimeReport[]> Get30DayCrimeData(double longitude, double latitude, double longDelta, double latDelta, Filter currentFilter)
		{
			MapSpan span = new MapSpan(new Position(latitude, longitude), latDelta, longDelta);
			return await Get30DayCrimeData(span, currentFilter);
		}

		/// <summary>
		/// Returns the 10 day crime blotter.
		/// TODO: Rename this to 10 Day.
		/// </summary>
		/// <returns>The day crime data.</returns>
		/// <param name="span">A MapSpan object describing the viewport of the device the user can see.</param>
		/// <param name="currentFilter">The crime filter to use to limit what crime data is retured.</param>
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

