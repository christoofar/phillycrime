using System;
using System.Collections.Generic;
using PhillyBlotter.Helpers;
using Plugin.Geolocator;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PhillyBlotter
{
	public partial class LocationPickerPage : ContentPage
	{
		LocationMap MyMap = new LocationMap();
		Image MapMarker = new Image();

		public LocationPickerPage()
		{
			InitializeComponent();

			MapMarker.Source = "Images/locatorpin.png";
			MapMarker.HeightRequest = 20;
			MapMarker.WidthRequest = 20;
			MapMarker.VerticalOptions = LayoutOptions.Center;
			MapMarker.HorizontalOptions = LayoutOptions.Center;
			MyMap.MapType = MapType.Hybrid;

			if (Math.Abs(Settings.PrimaryLat) > Double.Epsilon)
			{
				MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(Settings.PrimaryLat,
				                                                            Settings.PrimaryLong), Distance.FromMiles(0.5)));
			}
			else
			{
				MyMap.MoveToRegion(
						MapSpan.FromCenterAndRadius(
						new Position(39.952062, -75.163543), Distance.FromMiles(0.5)));
			}

			MyMap.Circle.Radius = Settings.CrimeRadius * 0.3048;
			CrimeSlider.Value = Settings.CrimeRadius;

		}


		protected override void OnAppearing()
		{
			base.OnAppearing();

			MapControl.Children.Add(MyMap);
			MapControl.Children.Add(MapMarker);


			if (Math.Abs(Settings.PrimaryLat) < Double.Epsilon)
			{
				JumpToWhereIAm(null, null);

			}
			MyMap.Circle.Radius = Settings.CrimeRadius * 0.3048;
			CrimeSlider.Value = Settings.CrimeRadius;


			MyMap.PropertyChanged += (sender, e) =>
			{
				if (MyMap.VisibleRegion != null)
				{
					MyMap.Circle.Position = MyMap.VisibleRegion.Center;
				}
			};

		}

		async void OnSaveButtonClicked(object sender, System.EventArgs e)
		{
			// Save this stuff
			Location.Fresh();
			try
			{
				await Location.SavePrimaryLocation(new Position(MyMap.VisibleRegion.Center.Latitude, MyMap.VisibleRegion.Center.Longitude), CrimeSlider.Value);
				await Navigation.PopAsync(true);
			}
			catch { } 
		}

		void SliderChangedValue(object sender, Xamarin.Forms.ValueChangedEventArgs e)
		{
			// Convert the new value to meters
			double meters = e.NewValue * 0.3048;

			// The circle is now "meters" in distance
			MyMap.Circle.Radius = meters;

			// Display the distance in feet on the label.
			Device.BeginInvokeOnMainThread(() =>
			{
				if (e.NewValue == 5280)
				{
					CrimeRadiusText.Text = "1 mile.";
				}
				else
				{
					CrimeRadiusText.Text = $"{Math.Floor(e.NewValue):n0} feet.";
				}

			});
		}

		async void JumpToWhereIAm(object sender, System.EventArgs e)
		{
			// Jump to where user is.
			MyMap.IsShowingUser = true;
			var locator = CrossGeolocator.Current;
			locator.DesiredAccuracy = 50;
			if (!locator.IsListening)
			{
				await locator.StartListeningAsync(5000, 50, false, new Plugin.Geolocator.Abstractions.ListenerSettings
				{
					ListenForSignificantChanges = true
				});
			}
			//locator.AllowsBackgroundUpdates = true;

			Plugin.Geolocator.Abstractions.Position position = new Plugin.Geolocator.Abstractions.Position();
			try
			{
				position = await locator.GetPositionAsync(timeoutMilliseconds: 10000);
			}
			catch
			{
				// For whatever reason getting GPS is blowing up on us. :-(
				position = new Plugin.Geolocator.Abstractions.Position();
				position.Latitude = 39.952062;
				position.Longitude = -75.163543;
			}

			//Shut off GPS, we're done with it
			//locator.AllowsBackgroundUpdates = false;
			await locator.StopListeningAsync();
			Position ps = new Position(position.Latitude, position.Longitude);

			MyMap.Circle = new CustomCircle();
			MyMap.Circle.Position = ps;
			MyMap.Circle.Radius = 152.4; // 500 ft. default
			MessagingCenter.Send<CustomCircle>(MyMap.Circle, "CircleChanged");

			MyMap.MoveToRegion(MapSpan.FromCenterAndRadius(ps, Distance.FromMiles(0.25)));
			MyMap.IsShowingUser = false;
		}
	}
}
