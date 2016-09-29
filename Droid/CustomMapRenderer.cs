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

[assembly:ExportRenderer (typeof(LocationMap), typeof(CustomLocationMapRenderer))]
namespace PhillyBlotter.Droid
{
	public class CustomLocationMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
	{
		static GoogleMap map;
		CustomCircle _circle;

		public override void OnViewRemoved(Android.Views.View child)
		{
			base.OnViewRemoved(child);
			if (map != null)
			{
				map.SetInfoWindowAdapter(null);
			}
		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				map.InfoWindowClick -= OnInfoWindowClick;
			}

			if (e.NewElement != null)
			{
				var formsMap = (LocationMap)e.NewElement;
				_circle = formsMap.Circle;
				((MapView)Control).GetMapAsync(this);
			}
		}

		public void OnMapReady (GoogleMap googleMap)
		{
			map = googleMap;
			map.InfoWindowClick += OnInfoWindowClick;
			map.SetInfoWindowAdapter (this);

			var circleOpt = new CircleOptions();
			circleOpt.InvokeCenter(new LatLng(_circle.Position.Latitude, _circle.Position.Longitude));
			circleOpt.InvokeRadius(_circle.Radius);
			circleOpt.InvokeFillColor(0X66FF0000);
			circleOpt.InvokeStrokeColor(0X66FF0000);
			circleOpt.InvokeStrokeWidth(0);
			map.AddCircle(circleOpt);

			/* Listen for circle change events */
			MessagingCenter.Subscribe<CustomCircle>(this, "CircleChanged", (obj) =>
			{
				map.Clear();

				var circleOptions = new CircleOptions();
				circleOptions.InvokeCenter(new LatLng(obj.Position.Latitude, obj.Position.Longitude));
				circleOptions.InvokeRadius(obj.Radius);
				circleOptions.InvokeFillColor(0X66FFFF00);
				circleOptions.InvokeStrokeColor(0X66FFFF00);
				circleOptions.InvokeStrokeWidth(0);
				map.AddCircle(circleOptions);
			});
		}

		void OnInfoWindowClick (object sender, GoogleMap.InfoWindowClickEventArgs e)
		{

		}

		public Android.Views.View GetInfoContents (Marker marker)
		{
			return null;
		}

		public Android.Views.View GetInfoWindow (Marker marker)
		{
			return null;
		}

	}
}

