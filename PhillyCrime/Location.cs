using System;
using System.Threading.Tasks;
using PhillyBlotter.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using System.Linq;

namespace PhillyBlotter
{
	public class Location
	{

		static string _deviceToken = "";
		static PushNotification.Plugin.Abstractions.DeviceType _deviceType;
		static double _primarylat = 0.00;
		static double _primarylong = 0.00;
		static double _radius = 0.00;
		static bool _locationRegistered = false;

		public Location()
		{

		}

		public static async void PostToken(string token, PushNotification.Plugin.Abstractions.DeviceType deviceType)
		{
			_deviceToken = token;
			_deviceType = deviceType;
			await RegisterPushNotifications();
		}

		public static async void PostLocation(double longitude, double latitude)
		{
			_primarylat = latitude;
			_primarylong = longitude;
			// We have a radius?
			if (Application.Current.Properties.ContainsKey("CrimeRadius"))
			{
				_radius = (double)Application.Current.Properties["CrimeRadius"];
			}
			else
			{
				_radius = 750;
			}
			await RegisterPushNotifications();
		}

		// Sets the Location handler so the next change to location will trigger a POST to the server
		public static void Fresh()
		{
			_locationRegistered = false;
		}

		public static async void PostLocation(double longitude, double latitude, double radius)
		{
			_primarylat = latitude;
			_primarylong = longitude;
			_radius = radius;
			await RegisterPushNotifications();
		}

		/// <summary>
		/// Different parts of the app locate where the device's primary location is and tries to connect to GMS/APNS
		/// to subscribe to the push channel.  Once both have been set, we need to tell Philadelinquency so we can start
		/// getting pushes.
		/// </summary>
		/// <returns>The push notifications.</returns>
		private async static Task<bool> RegisterPushNotifications()
		{
			if (!_locationRegistered && _deviceToken != "" && Math.Abs(_primarylong) > 0.00)
			{
				// We only need to do this once.  It will happen every time app is started fresh.
				// If network doesn't make it through, oh well.
				_locationRegistered = true;
				await Data.RegisterPushNotifications(_deviceToken, _deviceType, _primarylong, _primarylat, _radius);
			}
			return true;
		}

		public static async Task<bool> SavePrimaryLocation(Position currentPosition, double radius)
		{
			// OK, now we need to see if we're located in Philly.  We do this by asking the server
			// what PPD district the device is in right now.
			var area = await Data.Area(currentPosition.Longitude, currentPosition.Latitude);

			if (area != null)
			{
				/* if we got back nothing from the server then we were fooled */
				if (area.PoliceDistrict == null || area.Neighborhood == null)
				{
					return true;
				}

				Application.Current.Properties["LastPositionAsk"] = DateTime.Now;
				Application.Current.Properties["PrimaryLat"] = currentPosition.Latitude;
				Application.Current.Properties["PrimaryLong"] = currentPosition.Longitude;
				Application.Current.Properties["CrimeRadius"] = radius; //Default starting value.  User can change it.
				Application.Current.Properties["PrimaryDistrict"] = area.PoliceDistrict.District;
				Application.Current.Properties["PrimaryPSA"] = area.PoliceDistrict.PSA;
				Application.Current.Properties["Neighborhood"] = area.Neighborhood.Name;
				Application.Current.Properties["NeighborhoodID"] = area.Neighborhood.ID;

				var matchingHood = Global.Neighborhoods.Where((arg) => arg.ID == area.Neighborhood.ID).FirstOrDefault();
				// Do we have this neighborhood already selected?
				if (matchingHood != null && !matchingHood.Selected)
				{
					// Clear all the selected neighborhoods
					await App.ClearNeighborhoods();

					// This new primary location is what we'll use as the primary neighborhood.
					await App.UpdateNeighborhood(area.Neighborhood.ID, true);
				}

				await Application.Current.SavePropertiesAsync();
				PostLocation(currentPosition.Longitude, currentPosition.Latitude, radius);
			}

			return true;
		}

#pragma warning disable CS1998 
		// Async method lacks 'await' operators and will run synchronously
		/// <summary>
		/// Attemps to take a position found by GPS and set it as the app's primary locaiton.
		/// If the user is in NJ or in outer space with no primary location set, we'll set them at City Hall.
		/// </summary>
		/// <returns>The primary location.</returns>
		/// <param name="askingPage">Asking page.</param>
		/// <param name="currentPosition">Current position.</param>
		public static async Task<Position> ConfigurePrimaryLocation(Page askingPage, Position currentPosition)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
		{			
			// Check to see if the user has a primary location set and if not, offer to set it for her
			if (!Application.Current.Properties.ContainsKey("PrimaryLat"))
			{
				// Have we asked the user before?  (If we have, let's not bug them but every 10days)
				if (!Application.Current.Properties.ContainsKey("LastPositionAsk") ||
					((DateTime)(Application.Current.Properties["LastPositionAsk"])).AddDays(10) < DateTime.Now)
				{

					Area area = null;

					try
					{
						// OK, now we need to see if we're located in Philly.  We do this by asking the server
						// what PPD district the device is in right now.
						area = await Data.Area(currentPosition.Longitude, currentPosition.Latitude);
					}
					catch {}  // If this bombs out we don't know where the user is.

					if (area != null)  // We're in Philly!
					{
						Position positionToUse = new Position();

						// Now let's ask if the user wants to set their primary location.
						if (await askingPage.DisplayAlert("Set Primary Location",
											  "Would you like to use where you are right now as your primary location?", "Yes", "No"))
						{

							/* if we got back nothing from the server then we were fooled */
							if (area.PoliceDistrict == null || area.Neighborhood == null)
							{
								return new Position(39.952062, -75.163543);
							}

							Application.Current.Properties["LastPositionAsk"] = DateTime.Now;
							Application.Current.Properties["PrimaryLat"] = currentPosition.Latitude;
							Application.Current.Properties["PrimaryLong"] = currentPosition.Longitude;
							Application.Current.Properties["CrimeRadius"] = 750.0; //Default starting value.  User can change it.
							Application.Current.Properties["PrimaryDistrict"] = area.PoliceDistrict.District;
							Application.Current.Properties["PrimaryPSA"] = area.PoliceDistrict.PSA;
							Application.Current.Properties["Neighborhood"] = area.Neighborhood.Name;
							Application.Current.Properties["NeighborhoodID"] = area.Neighborhood.ID;

							// We have to store the selected neighborhoods in a file.  This will sync or create that file.
							await App.UpdateNeighborhood(area.Neighborhood.ID, true);

							positionToUse = currentPosition;
							await Application.Current.SavePropertiesAsync();
							PostLocation();
						}
						else
						{
							// WUT??
							// For some stupid reason the scared user answered no to this question.
							positionToUse = new Position(39.952062, -75.163543);

							// This is to mark down when we asked.  Every 10 days we are going
							// to bug the user about this when they're in Philadelphia.
							Application.Current.Properties["LastPositionAsk"] = DateTime.Now;
							await Application.Current.SavePropertiesAsync();
						}

						return positionToUse;
					}
					else
					{
						// OK, the user answered no (see code exit marked WUT??), so City Hall it is.
						// With no primary location and no primary District/PSA there's not gonna be a
						// blotter listing.
						return new Position(39.952062, -75.163543);
					}
				}
				else 
				{
					// The user has already answered no within the last 10 days, so send
					// them to City Hall. *sigh*
					return new Position(39.952062, -75.163543);
				}
			}


			PostLocation();			
			// Return config (the data was actually retrieved!)
			return new Position((double)Application.Current.Properties["PrimaryLat"],
			                    (double)Application.Current.Properties["PrimaryLong"]);			
		}

		static void PostLocation()
		{
			// This is required to get hyper-local crime push notifications to work
			Location.PostLocation((double)Application.Current.Properties["PrimaryLong"],
								  (double)Application.Current.Properties["PrimaryLat"]);
		}
}
}

