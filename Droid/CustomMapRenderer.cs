using System;
using System.Collections.Generic;
using System.ComponentModel;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using PhillyBlotter;
using PhillyBlotter.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly:ExportRenderer (typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PhillyBlotter.Droid
{
	public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
	{
        bool _isdrawn = false;
		List<CustomPin> customPins;

		public override void OnViewRemoved(Android.Views.View child)
		{
			base.OnViewRemoved(child);
            if (NativeMap != null)
			{
				customPins.Clear();
				NativeMap.SetInfoWindowAdapter(null);
			}

            _isdrawn = false;
            MessagingCenter.Unsubscribe<CrimesNearMeView>(this, "Clear");
            MessagingCenter.Unsubscribe<CrimesNearMeView, CustomPin>(this, "DroidPin");
            MessagingCenter.Unsubscribe<SearchResultsMap, CustomPin>(this, "DroidPin");

		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				NativeMap.InfoWindowClick -= OnInfoWindowClick;
			}

			if (e.NewElement != null)
			{
				var formsMap = (CustomMap)e.NewElement;
				customPins = formsMap.CustomPins;


				// Register a command to clear the pins.
				MessagingCenter.Subscribe<CrimesNearMeView>(this, "Clear", (sender) =>
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						if (customPins != null)
							customPins.Clear();
						NativeMap.Clear();
					});
				});
				// Register a command to add a droid pin to the map
				MessagingCenter.Subscribe<CrimesNearMeView, CustomPin>(this, "DroidPin", (sender, pin) =>
				{
					// do something whenever the "DroidPin" message is sent
					Device.BeginInvokeOnMainThread(() =>
					{
						AddDroidPin(pin);
					});
				});
				MessagingCenter.Subscribe<SearchResultsMap, CustomPin>(this, "DroidPin", (sender, pin) =>
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						AddDroidPin(pin);
					});
				});

				((MapView)Control).GetMapAsync(this);
			}
		}

		private void AddDroidPin(CustomPin pin)
		{
			var marker = new MarkerOptions();
			marker.SetPosition(new LatLng(pin.Pin.Position.Latitude, pin.Pin.Position.Longitude));
			marker.SetTitle(pin.Pin.Label);
			marker.SetSnippet(pin.Pin.Address);

			switch (pin.CrimeType)
			{
				case Models.CrimeType.Homicide:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.h_pin));
					break;
				case Models.CrimeType.Robbery:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ro_pin));
					break;
				case Models.CrimeType.Assault:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.a_pin));
					break;
				case Models.CrimeType.Burglary:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.b_pin));
					break;
				case Models.CrimeType.Rape:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ra_pin));
					break;
				case Models.CrimeType.Theft:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.t_pin));
					break;
				case Models.CrimeType.Prostition:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.p_pin));
					break;
				case Models.CrimeType.TheftFromAuto:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.ta_pin));
					break;
				case Models.CrimeType.StolenVehicle:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.vt_pin));
					break;
				case Models.CrimeType.VehicleRecovery:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.rv_pin));
					break;
				case Models.CrimeType.Gun:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.g_pin));
					break;
				case Models.CrimeType.Other:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.o_pin));
					break;
				case Models.CrimeType.CriminalMischief:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.m_pin));
					break;
				case Models.CrimeType.DUI:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.d_pin));
					break;
				case Models.CrimeType.OtherSexAssault:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.s_pin));
					break;
				case Models.CrimeType.Narcotics:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.n_pin));
					break;
				default:
					marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.o_pin));
					break;
			}

            if (NativeMap != null)
			    NativeMap.AddMarker(marker);
		}

		//public void OnMapReady(GoogleMap googleMap)
		//{
		//	map = googleMap;
		//	map.InfoWindowClick += OnInfoWindowClick;
		//	map.SetInfoWindowAdapter(this);
		//}

		protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName.Equals("VisibleRegion") && !_isdrawn)
			{
                _isdrawn = true;
				//PopulateMap();
				OnGoogleMapReady();
			}
		}

		private void OnGoogleMapReady()
		{
            //if (_mapReady) return;
			NativeMap.InfoWindowClick += OnInfoWindowClick;
			NativeMap.SetInfoWindowAdapter(this);

			//_mapReady = true;
		}

		void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
		{
			var customPin = GetCustomPin(e.Marker);
			if (customPin == null)
			{
				throw new Exception("Custom pin not found");
			}

			MessagingCenter.Send<CustomPin>(customPin, "ShowCrimeReport");

			//if (!string.IsNullOrWhiteSpace (customPin.Url)) {
			//	var url = Android.Net.Uri.Parse (customPin.Url);
			//	var intent = new Intent (Intent.ActionView, url);
			//	intent.AddFlags (ActivityFlags.NewTask);
			//	Android.App.Application.Context.StartActivity (intent);
			//}
		}

		public Android.Views.View GetInfoContents(Marker marker)
		{
			var inflater = Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as Android.Views.LayoutInflater;
			if (inflater != null)
			{
				Android.Views.View view;

				var customPin = GetCustomPin(marker);
				if (customPin == null)
				{
					//throw new Exception("Custom pin not found");
				}

				if (customPin.Id == "Xamarin")
				{
					view = inflater.Inflate(Resource.Layout.XamarinMapInfoWindow, null);

				}
				else {
					view = inflater.Inflate(Resource.Layout.MapInfoWindow, null);
				}

				var infoTitle = view.FindViewById<TextView>(Resource.Id.InfoWindowTitle);
				var infoSubtitle = view.FindViewById<TextView>(Resource.Id.InfoWindowSubtitle);
				var infoTime = view.FindViewById<TextView>(Resource.Id.InfoWindowTime);
				var infoIcon = view.FindViewById<ImageView>(Resource.Id.InfoWindowIcon);

				infoIcon.SetImageResource(Resource.Drawable.h_on);

				int icon;
				switch (customPin.CrimeType)
				{
					case Models.CrimeType.Homicide:
						icon = Resource.Drawable.h_on;
						break;
					case Models.CrimeType.Robbery:
						icon = Resource.Drawable.ro_on;
						break;
					case Models.CrimeType.Assault:
						icon = Resource.Drawable.a_on;
						break;
					case Models.CrimeType.Burglary:
						icon = Resource.Drawable.b_on;
						break;
					case Models.CrimeType.Rape:
						icon = Resource.Drawable.ra_on;
						break;
					case Models.CrimeType.Theft:
						icon = Resource.Drawable.t_on;
						break;
					case Models.CrimeType.Prostition:
						icon = Resource.Drawable.p_on;
						break;
					case Models.CrimeType.TheftFromAuto:
						icon = Resource.Drawable.ta_on;
						break;
					case Models.CrimeType.StolenVehicle:
						icon = Resource.Drawable.vt_on;
						break;
					case Models.CrimeType.VehicleRecovery:
						icon = Resource.Drawable.rv_on;
						break;
					case Models.CrimeType.Gun:
						icon = Resource.Drawable.g_on;
						break;
					case Models.CrimeType.Other:
						icon = Resource.Drawable.o_on;
						break;
					case Models.CrimeType.CriminalMischief:
						icon = Resource.Drawable.m_on;
						break;
					case Models.CrimeType.DUI:
						icon = Resource.Drawable.d_on;
						break;
					case Models.CrimeType.OtherSexAssault:
						icon = Resource.Drawable.s_on;
						break;
					case Models.CrimeType.Narcotics:
						icon = Resource.Drawable.n_on;
						break;
					default:
						icon = Resource.Drawable.o_on;
						break;
				}

				infoIcon.SetImageResource(icon);

				if (infoTitle != null)
				{
					infoTitle.Text = marker.Title;
				}
				if (infoSubtitle != null)
				{
					infoSubtitle.Text = marker.Snippet;
				}
				if (infoTime != null)
				{
					infoTime.Text = customPin.Occurred;
				}

				return view;
			}
			return null;
		}

		public Android.Views.View GetInfoWindow(Marker marker)
		{
			return null;
		}

		CustomPin GetCustomPin(Marker annotation)
		{
			var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
			foreach (var pin in customPins)
			{
				if (pin.Pin.Position == position)
				{
					return pin;
				}
			}
			return null;
		}
	}
}

