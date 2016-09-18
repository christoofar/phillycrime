using System;
using System.Collections.ObjectModel;
using PhillyCrime.Models;

namespace PhillyCrime
{
	/// <summary>
	/// This holds data that needs to be visible, usable everywhere.
	/// Use very sparingly.
	/// </summary>
	public class Global
	{
		public static ObservableCollection<Neighborhood> Neighborhoods = new ObservableCollection<Neighborhood>();

		public Global()
		{
		}

	}
}
