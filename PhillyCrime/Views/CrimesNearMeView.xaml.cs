using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms.Maps;
using Xamarin.Forms;
using Plugin.Geolocator;

namespace PhillyCrime
{
	public partial class CrimesNearMeView : ContentPage
	{
		// Testpoint: {Xamarin.Forms.Maps.Position}
		//	Latitude: 39.9785737
		//	Longitude: -75.1221699

		Position currentPosition = new Position();

		public CrimesNearMeView()
		{
			InitializeComponent();

			this.Appearing += (sender, e) =>
			{
				CenterTheMap();
				Data.Get30DayCrimeData(this);
			};

		}

		public void DataFill(PhillyCrime.CrimeReport[] reports)
		{
			MyMap.Pins.Clear();

			foreach (var report in reports)
			{
				Pin pin = new Pin();
				pin.Position = new Position(double.Parse(report.POINTY), double.Parse(report.POINTX));
				pin.Address = report.LOCATIONBLOCK;
				pin.Label = report.TEXTGENERALCODE;
				MyMap.Pins.Add(pin);
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
			MyMap.MoveToRegion(mapspan);
		}

	}
}

