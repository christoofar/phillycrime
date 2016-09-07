﻿using System;
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

		void OnTapGestureRecognizerTapped(object sender, EventArgs args) 
		{ 			
		    var imageSender = (Image)sender;

			// Flip the switch to off
			string imagesource = ((FileImageSource)imageSender.Source).File;
			if (imagesource.Contains("off"))
			{
				imageSender.Source = imagesource.Replace("off", "on");
			}
			else 
			{
				imageSender.Source = imagesource.Replace("on", "off");
			}

			UpdateFilters();
		}

		public void UpdateFilters()
		{
			List<string> types = new List<string>{ "filterHomicide",
				"filterAssault",
				"filterBurglary",
				"filterRobbery",
				"filterTheft",
				"filterRape",
				"filterAuto",
				"filterGuns",
				"filterProstitution",
				"filterOther"
			};

			foreach (string filter in types)
			{
				Image p = this.Content.FindByName<Image>(filter);
				string imagesource = ((FileImageSource)p.Source).File;
				if (imagesource.Contains("on"))
				{
					switch (filter)
					{
						case "filterHomicide":
							currentFilter = currentFilter | Filter.Homicide;
							break;
						case "filterAssault":
							currentFilter = currentFilter | Filter.Assault;
							break;
						case "filterBurglary":
							currentFilter = currentFilter | Filter.Burglary;
							break;
						case "filterRobbery":
							currentFilter = currentFilter | Filter.Robbery;
							break;
						case "filterTheft":
							currentFilter = currentFilter | Filter.Theft;
							break;
						case "filterRape":
							currentFilter = currentFilter | Filter.Rape;
							break;
						case "filterAuto":
							currentFilter = currentFilter | Filter.Vehicle;
							break;
						case "filterGuns":
							currentFilter = currentFilter | Filter.Gun;
							break;
						case "filterProstitution":
							currentFilter = currentFilter | Filter.Prostition;
							break;
						case "filterOther":
							currentFilter = currentFilter | Filter.Other;
							break;
					}
				} else {
					switch (filter)
					{
						case "filterHomicide":
							currentFilter = currentFilter & ~Filter.Homicide;
							break;
						case "filterAssault":
							currentFilter = currentFilter & ~Filter.Assault;
							break;
						case "filterBurglary":
							currentFilter = currentFilter & ~Filter.Burglary;
							break;
						case "filterRobbery":
							currentFilter = currentFilter & ~Filter.Robbery;
							break;
						case "filterTheft":
							currentFilter = currentFilter & ~Filter.Theft;
							break;
						case "filterRape":
							currentFilter = currentFilter & ~Filter.Rape;
							break;
						case "filterAuto":
							currentFilter = currentFilter & ~Filter.Vehicle;
							break;
						case "filterGuns":
							currentFilter = currentFilter & ~Filter.Gun;
							break;
						case "filterProstitution":
							currentFilter = currentFilter & ~Filter.Prostition;
							break;
						case "filterOther":
							currentFilter = currentFilter & ~Filter.Other;
							break;
					}
				}
			}

			// Go get the data again, this time with the updated filters activated
			// This also requires all the pins be cleaned

			pins.Clear();
			MyMap.Pins.Clear();
			MyMap.CustomPins.Clear();
			MyMap.Clear();
			// On Droid it takes more work to clear the friggin' map
			Device.OnPlatform(null, () => {
				MessagingCenter.Send<CrimesNearMeView>(this, "Clear");
			}, null);
			UpdateMap();
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
							Label = report.Title,
							Address = report.Address
						},
						Id = "Xamarin",
						Url = "http://xamarin.com/about/",
						Occurred = report.Occurred.GetValueOrDefault().ToString("dddd M/d h:mm tt"),
						CrimeType = report.Type
					};

					pins.Add(report.DCN);
					toAdd.Add(pin);
				}

				Device.BeginInvokeOnMainThread(() =>
				{		
					MyMap.CustomPins.AddRange(toAdd);

					//Throw all the new pins on at once.
					foreach (CustomPin pin in toAdd)
					{
						// On iOS we're doing the checking of pin
						// type using the renderer.  But on Droid
						// this is next to impossible because
						// we can't get to the pin collection when
						// a pin is added to adjust the icon
						// that Xamarin.Forms.Maps sets by default :-(
						Device.OnPlatform(() =>						
						{
							MyMap.Pins.Add(pin.Pin);
						},
						() =>
						{
							MessagingCenter.Send<CrimesNearMeView, CustomPin>((CrimesNearMeView)this, "DroidPin", pin);
						}, null, null);

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

