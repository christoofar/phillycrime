using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PhillyBlotter.Models;
using Polly;
using Xamarin.Forms;

namespace PhillyBlotter
{
	/// <summary>
	/// This holds data that needs to be visible, usable everywhere.
	/// Use very sparingly.
	/// </summary>
	public class Global
	{
		public static string VERSION = "PROD01";
		public static ObservableCollection<Neighborhood> Neighborhoods = new ObservableCollection<Neighborhood>();

		public static object MessagingInstance { get; set; }

		public static string ImageSource(CrimeType type)
		{
			switch (type)
			{
				case Models.CrimeType.Homicide:
					return "Images/h_on.png";
				case Models.CrimeType.Robbery:
					return "Images/ro_on.png";
				case Models.CrimeType.Assault:
					return "Images/a_on.png";
				case Models.CrimeType.Burglary:
					return "Images/b_on.png";
				case Models.CrimeType.Rape:
					return "Images/ra_on.png";
				case Models.CrimeType.Theft:
					return "Images/t_on.png";
				case Models.CrimeType.Prostition:
					return "Images/p_on.png";
				case Models.CrimeType.TheftFromAuto:
					return "Images/ta_on.png";
				case Models.CrimeType.StolenVehicle:
					return "Images/vt_on.png";
				case Models.CrimeType.VehicleRecovery:
					return "Images/rv_on.png";
				case Models.CrimeType.Gun:
					return "Images/g_on.png";
				case Models.CrimeType.CriminalMischief:
					return "Images/m_on.png";
				case Models.CrimeType.DUI:
					return "Images/d_on.png";
				case Models.CrimeType.Narcotics:
					return "Images/n_on.png";
				case Models.CrimeType.Other:
					return "Images/o_on.png";
				case Models.CrimeType.OtherSexAssault:
					return "Images/s_on.png";
				case Models.CrimeType.Nothing:
					return "Images/unknown_on.png";					
				default:
					return "Images/o_on.png";
			}
		}


		public async static Task<bool> NeighborhoodSyncSettings()
		{
			await App.LoadNeighborhoods();
			return true;
		}

		static Global()
		{
			MessagingInstance = new object();
		}

	}
}
