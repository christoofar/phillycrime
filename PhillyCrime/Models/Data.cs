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

namespace PhillyCrime.Models
{
	public class Data
	{
		private static string APIBASE = "http://192.168.1.30/phillycrime/api/";
		private static string GET_30DAY = string.Format("{0}Values", APIBASE);

		public Data()
		{
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

