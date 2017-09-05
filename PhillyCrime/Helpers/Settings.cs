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
		private static readonly int NeighborhoodIDKeyDefault = -1;

		private const string FilterKey = "filter";
		private static readonly Filter FilterKeyDefault = Filter.Homicide | Filter.Robbery | Filter.Rape | Filter.Burglary | Filter.Assault;

        private const string StatsMultipleHoodsWarningKey = "statsMultipleHoods";
        private static readonly bool StatsMultipleHoodsWarningDefault = false;

		#endregion

        public static bool StatsMultipleHoodsWarning
        {
            get
            {
                return AppSettings.GetValueOrDefault(StatsMultipleHoodsWarningKey, StatsMultipleHoodsWarningDefault);    
            }
            set
            {
                AppSettings.AddOrUpdateValue(StatsMultipleHoodsWarningKey, value);
            }
        }

		public static string GeneralSettings
		{
			get
			{
				return AppSettings.GetValueOrDefault(SettingsKey, SettingsDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(SettingsKey, value);
			}
		}

		public static DateTime LastPositionAsk
		{
			get
			{
				return AppSettings.GetValueOrDefault(LastPositionAskKey, LastPositionAskDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(LastPositionAskKey, value);
			}

		}

		public static double PrimaryLat
		{
			get
			{
				return AppSettings.GetValueOrDefault(PrimaryLatKey, PrimaryLatKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(PrimaryLatKey, value);
			}
		}

		public static double PrimaryLong
		{
			get
			{
				return AppSettings.GetValueOrDefault(PrimaryLongKey, PrimaryLongKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(PrimaryLongKey, value);
			}
		}

		public static double CrimeRadius
		{
			get
			{
				return AppSettings.GetValueOrDefault(CrimeRadiusKey, CrimeRadiusKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(CrimeRadiusKey, value);
			}
		}

		public static string PrimaryDistrict
		{
			get
			{
				return AppSettings.GetValueOrDefault(PrimaryDistrictKey, PrimaryDistrictKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(PrimaryDistrictKey, value);
			}
		}

		public static string PrimaryPSA
		{
			get
			{
				return AppSettings.GetValueOrDefault(PrimaryPSAKey, PrimaryPSAKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(PrimaryPSAKey, value);
			}
		}

		public static string Neighborhood
		{
			get
			{
				return AppSettings.GetValueOrDefault(NeighborhoodKey, NeighborhoodKeyDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue(NeighborhoodKey, value);
			}
		}

		public static int? NeighborhoodID
		{
			get
			{
				int hood = AppSettings.GetValueOrDefault(NeighborhoodIDKey, NeighborhoodIDKeyDefault);
                if (hood == -1) return null;
                return hood;
			}
			set
			{
                if (value != null)
                {
                    AppSettings.AddOrUpdateValue(NeighborhoodIDKey, (int)value);
                }
			}
		}

		public static Filter Filter
		{
			get
			{
                var filter = AppSettings.GetValueOrDefault(FilterKey, Convert.ToInt32(FilterKeyDefault));

                return (Filter)filter;
			}
			set
			{                
				AppSettings.AddOrUpdateValue(FilterKey, Convert.ToInt64(value));
			}
		}
	}
}