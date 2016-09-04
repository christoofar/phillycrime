using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;
using System.Diagnostics;
using PhillyCrime;

namespace PhillyCrime
{
	public class Data
	{
		private static string APIBASE = "http://192.168.1.30/phillycrime/api/";
		private static string GET_30DAY = string.Format("{0}/Values", APIBASE);

		public Data()
		{
		}

		public async static void Get30DayCrimeData(object sender)
		{

			JsonWebClient cli = new JsonWebClient();
			var resp = await cli.DoRequestJsonAsync<PhillyCrime.CrimeReport[]>("http://192.168.1.30/phillycrime/api/Values");

#if DEBUG
			Debug.WriteLine("The following DCNs were located for the 30 day crime report...");
			foreach (PhillyCrime.CrimeReport report in resp)
			{
				Debug.WriteLine(report.DCKEY);
			}
#endif

			if (sender != null && sender is CrimesNearMeView)
			{
				((CrimesNearMeView)sender).DataFill(resp);
			}
		}
	}
}

