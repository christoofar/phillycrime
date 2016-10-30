// Helpers/Settings.cs
using System;
using PhillyBlotter.Models;
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace PhillyBlotter.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}

		#region Setting Constants

		private const string SettingsKey = "settings_key";
		private static readonly string SettingsDefault = string.Empty;

		private const string LastPositionAskKey = "lastpositionask";
		private static readonly DateTime LastPositionAskDefault = DateTime.MinValue;

		private const string PrimaryLatKey = "primarylat";
		private static readonly double PrimaryLatKeyDefault = 0.00;

		private const string PrimaryLongKey = "primarylong";
		private static readonly double PrimaryLongKeyDefault = 0.00;

		private const string CrimeRadiusKey = "crimeradius";
		private static readonly double CrimeRadiusKeyDefault = 750.00;

		private const string PrimaryDistrictKey = "primarydistrict";
		private static readonly string PrimaryDistrictKeyDefault = null;

		private const string PrimaryPSAKey = "primarypsa";
		private static readonly string PrimaryPSAKeyDefault = null;

		private const string NeighborhoodKey = "neighborhood";
		private static readonly string NeighborhoodKeyDefault = null;

		private const string NeighborhoodIDKey = "neighborhoodid";
		private static readonly int? NeighborhoodIDKeyDefault = null;

		private const string FilterKey = "filter";
		private static readonly Filter FilterKeyDefault = Filter.Homicide | Filter.Robbery | Filter.Rape | Filter.Burglary | Filter.Assault;

		#endregion


		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(SettingsKey, value);
			}
		}

		public static DateTime LastPositionAsk
		{
			get
			{
				return AppSettings.GetValueOrDefault<DateTime>(LastPositionAskKey, LastPositionAskDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<DateTime>(LastPositionAskKey, value);
			}

		}

		public static double PrimaryLat
		{
			get
			{
				return AppSettings.GetValueOrDefault<double>(PrimaryLatKey, PrimaryLatKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<double>(PrimaryLatKey, value);
			}
		}

		public static double PrimaryLong
		{
			get
			{
				return AppSettings.GetValueOrDefault<double>(PrimaryLongKey, PrimaryLongKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<double>(PrimaryLongKey, value);
			}
		}

		public static double CrimeRadius
		{
			get
			{
				return AppSettings.GetValueOrDefault<double>(CrimeRadiusKey, CrimeRadiusKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<double>(CrimeRadiusKey, value);
			}
		}

		public static string PrimaryDistrict
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(PrimaryDistrictKey, PrimaryDistrictKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(PrimaryDistrictKey, value);
			}
		}

		public static string PrimaryPSA
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(PrimaryPSAKey, PrimaryPSAKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(PrimaryPSAKey, value);
			}
		}

		public static string Neighborhood
		{
			get
			{
				return AppSettings.GetValueOrDefault<string>(NeighborhoodKey, NeighborhoodKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<string>(NeighborhoodKey, value);
			}
		}

		public static int? NeighborhoodID
		{
			get
			{
				return AppSettings.GetValueOrDefault<int?>(NeighborhoodIDKey, NeighborhoodIDKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<int?>(NeighborhoodIDKey, value);
			}
		}

		public static Filter Filter
		{
			get
			{
				return AppSettings.GetValueOrDefault<Filter>(FilterKey, FilterKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<Filter>(FilterKey, value);
			}
		}
	}
}