using System;
using System.Net;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PhillyBlotter.Models
{
	public class Data
	{
		private static string HTTP_TYPE = "https://";
		private static string API_BASE = "/phillycrime/api/";
		private static string HOST = "www.philadelinquency.com";

		private static string GET_30DAY = "Values";
		private static string FULLREPORT = "Crime";
		private static string PHILLY = "Philly";
		private static string PUSH = "Push";
		private static string NEIGHBORHOOD = "Neighborhood";
		private static string BLOTTER = "Blotter";
		private static string CRIMESEARCH = "CrimeSearch";
		private static string ARRESTSEARCH = "ArrestSearch";
		private static string NEWS = "News";

		public Data()
		{
			
		}

		public static void ClearDNS()
		{
			HOST = "www.philadelinquency.com";
			Application.Current.Properties["DNSExpires"] = DateTime.Parse("1970-01-01");
			Application.Current.SavePropertiesAsync();
		}

		private static async Task<string> GetRoot()
		{
			string dns = HOST;

			try
			{

				// Do we already know what the DNS entry is?
				if (Application.Current.Properties.ContainsKey("DNS"))
				{
					// Has it been less than 2 days since the last time we 
					// queried this value?
					if (DateTime.Now < (DateTime)Application.Current.Properties["DNSExpires"])
					{
						// Yes.  The cache value is still good.
						dns = (string)Application.Current.Properties["DNS"];
					}
				}

				// If we got an IP address instead of the host domain, we'll cache this.
				// This can be an IPv6 or IPv4 address (both work).
				if (dns.Contains("philadelinquency"))
				{
					string address = await DependencyService.Get<PlatformSpecificInterface>().ResolveIPAddress(dns);
					Application.Current.Properties["DNS"] = address;
					Application.Current.Properties["DNSExpires"] = DateTime.Now.AddDays(2);
					await Application.Current.SavePropertiesAsync(); //save
					return address;
				}

			}
			catch (Exception ex)
			{
				Debug.WriteLine($"DNS resolve failed:{ex.Message}\r\n{ex.StackTrace}");
			}

			return dns;
		}

		public static string GetUri(string verb, string parameters)
		{
			string host = Task.Run(async () => await GetRoot()).Result;

			return $"{HTTP_TYPE}{host}{API_BASE}{verb}{parameters}";
		}

		public static string GetUri(string verb)
		{
			string host = Task.Run(async () => await GetRoot()).Result;

			return $"{HTTP_TYPE}{host}{API_BASE}{verb}";
		}

		public async static Task<bool> RegisterPushNotifications(string token,
					PushNotification.Plugin.Abstractions.DeviceType deviceType,
					double longitude, double latitude, double radius)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = GetUri(PUSH);

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

			string getUri = GetUri(PUSH);

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

			string getUri = GetUri(BLOTTER, string.Format("/{0}/", hoods));

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

			string getUri = GetUri(BLOTTER, string.Format($"/{latitutde}/{longitude}/{distance}/"));
			var resp = await cli.DoRequestJsonAsync<CrimeReport[]>(getUri);
			return resp;
		}

		/// <summary>
		/// Submits a news story contribution to a crime report.
		/// </summary>
		/// <returns>A NewsContributionResponse which informs the client whether the 
		/// submission was accepted or turned down</returns>
		/// <param name="contribution">Contribution.</param>
		public async static Task<NewsContributionResponse> SubmitNewsStory(NewsContribution contribution)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = GetUri(NEWS);
			var resp = await cli.DoPostJson<NewsContributionResponse>(getUri, JsonConvert.SerializeObject(contribution));
			return resp;
		}

		public async static Task<NewsContributionResponse> FlagNewsStory(NewsFlagConcern reason)
		{
			JsonWebClient cli = new JsonWebClient();

			// This is fucked up but this API stack will not allow a message body to go through in this case.
			// And % sign is verboten in URL string, so we have to replace that with $ signs.  The server will
			// reverse this process on the other end.

			string url = reason.URL;
			url = System.Net.WebUtility.UrlEncode(url);
			url = url.Replace('%', '$');

			string getUri = GetUri(NEWS, $"/{url}/{reason.Reason}/{reason.DCN}/");

			var resp = await cli.DoDeleteJson<NewsContributionResponse>(getUri);
			return resp;
		}

		/// <summary>
		/// Returns the neighborhoods of Philly
		/// </summary>
		public async static Task<Neighborhood[]> Neighborhoods()
		{
			JsonWebClient cli = new JsonWebClient();
			var resp = await cli.DoRequestJsonAsync<Neighborhood[]>(GetUri(NEIGHBORHOOD, ""));
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
			var resp = await cli.DoRequestJsonAsync<Area>(GetUri(PHILLY, $"/{longitude}/{latitude}/"));
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

			var resp = await cli.DoRequestJsonAsync<FullCrimeReport>(GetUri(FULLREPORT, 
				                                                            $"/{DCN}/"));
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
				
					string getUri = GetUri(GET_30DAY, string.Format("/{0}/{1}/{2}/{3}/{4}/",
														  span.Center.Longitude,
														  span.Center.Latitude,
														  span.LongitudeDegrees,
														  span.LatitudeDegrees,
					                                            Convert.ToInt32(currentFilter)));

				var resp = await cli.DoRequestJsonAsync<CrimeReport[]>(getUri);
				return resp;

			}
			else
			{
				return new CrimeReport[0];
			}

		}

		/// <summary>
		/// Searchs the crimes.
		/// </summary>
		/// <returns>The crimes.</returns>
		/// <param name="criteria">Criteria.</param>
		public async static Task<CrimeReport[]> SearchCrimes(CrimeSearchCriteria criteria)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = GetUri(CRIMESEARCH);

			Debug.WriteLine($"Sending search request...");
			return await cli.DoPostJson<CrimeReport[]>(getUri, JsonConvert.SerializeObject(criteria));
		}

		public async static Task<ArrestReport[]> SearchArrests(ArrestSearchCriteria criteria)
		{
			JsonWebClient cli = new JsonWebClient();

			string getUri = GetUri(ARRESTSEARCH);

			Debug.WriteLine($"Sending search request...");
			return await cli.DoPostJson<ArrestReport[]>(getUri, JsonConvert.SerializeObject(criteria));
		}

	}
}

