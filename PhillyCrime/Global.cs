using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using PhillyCrime.Models;
using Xamarin.Forms;

namespace PhillyCrime
{
	/// <summary>
	/// This holds data that needs to be visible, usable everywhere.
	/// Use very sparingly.
	/// </summary>
	public class Global
	{
		public static ObservableCollection<Neighborhood> Neighborhoods = new ObservableCollection<Neighborhood>();

		public async static Task<bool> NeighborhoodSyncSettings()
		{
			await App.LoadNeighborhoods();
			return true;
		}

		public Global()
		{
		}

	}
}
