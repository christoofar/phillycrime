using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Plugin.Geolocator;
using PhillyCrime.Models;

namespace PhillyCrime
{
	public partial class CrimesNearMeView : ContentPage
	{
		// Testpoint: {Xamarin.Forms.Maps.Position}
		//	Latitude: 39.9785737
		//	Longitude: -75.1221699

		MapSpan lastPosition = null;
		Position currentPosition = new Position();
		Filter currentFilter = Filter.Homicide |
									 Filter.Robbery |
									 Filter.Rape |
									 Filter.Burglary |
									 Filter.Assault;

		IList<string> pins = new List<string>();
		public CrimesNearMeView()
		{
			InitializeComponent();
			MyMap.CustomPins = new List<CustomPin>();

			MyMap.PropertyChanged += (sender, e) =>
			{
				try
				{

					if (!lastPosition.Equals(MyMap.VisibleRegion))
					{
						lastPosition = MyMap.VisibleRegion;
						// User moved the map.  Recycle the pins.
						//Data.Get30DayCrimeData(MyMap.VisibleRegion, this);
						UpdateMap();
					}
				}
				catch { }
			};

			this.Appearing += (sender, e) =>
			{
				CenterTheMap();
				UpdateMap();
			};

		}

		public Task<bool> UpdateMap()
		{
			return (Task<bool>)Task.Run(async () =>
		   {
			   var crime = await Data.Get30DayCrimeData(MyMap.VisibleRegion, currentFilter);
			   DataFill(crime);
			   return true;
		   });
		}

		public void DataFill(CrimeReport[] reports)
		{
			foreach (var report in reports)
			{
				List<CustomPin> toAdd = new List<CustomPin>();

				if (pins.IndexOf(report.DCN) == -1)
				{
					var pin = new CustomPin
					{
						Pin = new Pin
						{
							Type = PinType.Place,
							Position = new Position(report.Latitutde, report.Longitude),
							Label = report.Title + report.Code,
							Address = report.Address
						},
						Id = "Xamarin",
						Url = "http://xamarin.com/about/",
						CrimeType = report.Type
					};

					pins.Add(report.DCN);
					toAdd.Add(pin);
				}
				else
				{
					int b = 0;
				}


				Device.BeginInvokeOnMainThread(() =>
				{
					//Throw all the new pins on at once.
					foreach (CustomPin pin in toAdd)
					{
						MyMap.CustomPins.Add(pin);
						MyMap.Pins.Add(pin.Pin);
					}
				});
			}
		}

		// We're going to find the centerpoint region best for displaying crime data.
		public async void CenterTheMap()
		{

			if (DependencyService.Get<PlatformSpecificInterface>().CheckIfSimulator() && Device.OS == TargetPlatform.iOS)
			{
				Debug.WriteLine("We're running inside of an iOS similar, return fakey coordinates");
				currentPosition = new Position(39.9785737, -75.122699);
			}

			try
			{

				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;
				locator.AllowsBackgroundUpdates = true;

				var position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);

				currentPosition = new Position(position.Latitude, position.Longitude);

#if DEBUG
				Debug.WriteLine("Position Status: {0}", position.Timestamp);
				Debug.WriteLine("Position Latitude: {0}", position.Latitude);
				Debug.WriteLine("Position Longitude: {0}", position.Longitude);
#endif

			}
			catch (Exception ex)
			{
				Debug.WriteLine("Unable to get location, may need to increase timeout: " + ex);
			}

			var mapspan = MapSpan.FromCenterAndRadius(currentPosition, Distance.FromMiles(0.25));
			lastPosition = mapspan;
			MyMap.MoveToRegion(mapspan);
		}

	}
}

