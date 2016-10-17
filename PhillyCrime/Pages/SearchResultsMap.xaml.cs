using System;
using System.Collections.Generic;
using PhillyBlotter.Models;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PhillyBlotter
{
	public partial class SearchResultsMap : ContentPage
	{
		CrimeReport[] _crimes = null;
		IList<string> pins = new List<string>();
		bool _initialized = false;

		public SearchResultsMap()
		{
			InitializeComponent();
			MyMap.MoveToRegion(
				MapSpan.FromCenterAndRadius(
				new Position(39.952062, -75.163543), Distance.FromMiles(0.5)));
		}

		public SearchResultsMap(CrimeReport[] crimes)
		{
			InitializeComponent();
			MyMap.MoveToRegion(
				MapSpan.FromCenterAndRadius(
				new Position(39.952062, -75.163543), Distance.FromMiles(15)));
			MyMap.CustomPins = new List<CustomPin>();
			_crimes = crimes;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();


			Device.OnPlatform(() =>
			{
				// Start listener for pushpin events
				MessagingCenter.Subscribe<CustomPin>(this, "ShowCrimeReport", (obj) =>
				{
					var crimeDetailPage = new CrimeDetailPage(obj);
					Navigation.PushAsync(crimeDetailPage);
				});

				DataFill(_crimes);
				_initialized = true;
			}, () => {
				MyMap.PropertyChanged += (sender, e) =>
				{
					if (!_initialized)
					{
									// Start listener for pushpin events
									MessagingCenter.Subscribe<CustomPin>(this, "ShowCrimeReport", (obj) =>
						{
							var crimeDetailPage = new CrimeDetailPage(obj);
							Navigation.PushAsync(crimeDetailPage);
						});

						DataFill(_crimes);
						_initialized = true;
					}
				};
			}, null);
		}

		public void DataFill(CrimeReport[] reports)
		{
			foreach (var report in reports)
			{

				// Some shit is geocoded badly or not at all by police.  I have no choice here
				// but to skip those records for map display
				if (report.Latitutde.Value == 0.00 || report.Longitude.Value == 0.00)
				{
					continue;
				}

				// Calculate maximum and minimum latitude of data
				var maxLat = _crimes.Max(p => p.Latitutde);
				var minLat = _crimes.Min(p => p.Latitutde);
				var maxLong = _crimes.Max(p => p.Longitude);
				var minLong = _crimes.Min(p => p.Longitude);

				// The center is going to be the halfway point between x1<->x2 and y1<->y2
				var latC = minLat + (Math.Abs(maxLat.Value - minLat.Value) / 2);
				var longC = minLong + (Math.Abs(maxLong.Value - minLong.Value) / 2);

				//MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latC.Value, longC.Value), Distance.FromMiles(2)));

				MapSpan span = new MapSpan(new Position(latC.Value, longC.Value), Math.Abs(maxLat.Value - minLat.Value) + 0.01,
										   Math.Abs(maxLong.Value - minLong.Value) + 0.01);
				MyMap.MoveToRegion(span);


				var toAdd = new List<CustomPin>();

				if (pins.IndexOf(report.DCN) == -1)
				{
					var pin = new CustomPin
					{
						Pin = new Pin
						{
							Type = PinType.Place,
							Position = new Position(report.Latitutde.Value, report.Longitude.Value),
							Label = report.Title,
							Address = report.Address
						},
						Id = report.DCN,
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
#pragma warning disable IDE0004 // Remove Unnecessary Cast
							MessagingCenter.Send<SearchResultsMap, CustomPin>((SearchResultsMap)this, "DroidPin", pin);
#pragma warning restore IDE0004 // Remove Unnecessary Cast
						}, null, null);

					}

				});
			}
		}
	}
}
