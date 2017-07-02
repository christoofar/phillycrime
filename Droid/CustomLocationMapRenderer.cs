using System.ComponentModel;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.Widget;
using PhillyBlotter;
using PhillyBlotter.Droid;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;

[assembly: ExportRenderer(typeof(LocationMap), typeof(CustomLocationMapRenderer))]
namespace PhillyBlotter.Droid
{
	public class CustomLocationMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter, IOnMapReadyCallback
	{
		bool _isdrawn = false;
		CustomCircle _circle;

		public override void OnViewRemoved(Android.Views.View child)
		{
			base.OnViewRemoved(child);
			if (NativeMap != null)
			{
				NativeMap.SetInfoWindowAdapter(null);
			}

            _isdrawn = false;
            MessagingCenter.Unsubscribe<CustomCircle>(this, "CircleChanged");

		}

		protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
                if (NativeMap != null)
                {
                    NativeMap.InfoWindowClick -= OnInfoWindowClick;
                }
			}

			if (e.NewElement != null)
			{
				var formsMap = (LocationMap)e.NewElement;
				_circle = formsMap.Circle;
				((MapView)Control).GetMapAsync(this);
			}
		}

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

		public void OnGoogleMapReady()
		{
			NativeMap.InfoWindowClick += OnInfoWindowClick;
			NativeMap.SetInfoWindowAdapter(this);

			var circleOpt = new CircleOptions();
			circleOpt.InvokeCenter(new LatLng(_circle.Position.Latitude, _circle.Position.Longitude));
			circleOpt.InvokeRadius(_circle.Radius);
			circleOpt.InvokeFillColor(0X66FF0000);
			circleOpt.InvokeStrokeColor(0X66FF0000);
			circleOpt.InvokeStrokeWidth(0);
			NativeMap.AddCircle(circleOpt);

			/* Listen for circle change events */
			MessagingCenter.Subscribe<CustomCircle>(this, "CircleChanged", (obj) =>
			{
				NativeMap.Clear();

				var circleOptions = new CircleOptions();
				circleOptions.InvokeCenter(new LatLng(obj.Position.Latitude, obj.Position.Longitude));
				circleOptions.InvokeRadius(obj.Radius);
				circleOptions.InvokeFillColor(0X66FFFF00);
				circleOptions.InvokeStrokeColor(0X66FFFF00);
				circleOptions.InvokeStrokeWidth(0);
				NativeMap.AddCircle(circleOptions);
			});
		}

		void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
		{

		}

		public Android.Views.View GetInfoContents(Marker marker)
		{
			return null;
		}

		public Android.Views.View GetInfoWindow(Marker marker)
		{
			return null;
		}

	}
}