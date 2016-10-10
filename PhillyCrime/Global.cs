using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhillyBlotter.Models;
using Xamarin.Forms;

namespace PhillyBlotter
{
	/// <summary>
	/// This holds data that needs to be visible, usable everywhere.
	/// Use very sparingly.
	/// </summary>
	public class Global
	{
		//Change this on every cycle
		public static string VERSION = "APK13";
		public static ObservableCollection<Neighborhood> Neighborhoods = new ObservableCollection<Neighborhood>();

		public static object MessagingInstance { get; set; }

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
