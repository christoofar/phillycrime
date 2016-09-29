using System;
using System.Collections.Generic;
using CoreGraphics;
using PhillyBlotter;
using PhillyBlotter.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(LocationMap), typeof(CustomLocationMapRenderer))]
namespace PhillyBlotter.iOS
{
	public class CustomLocationMapRenderer : MapRenderer
	{
		LocationMap formsMap;
		MKMapView nativeMap;
		MKCircle _circle;

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				nativeMap.GetViewForAnnotation = null;
				nativeMap.OverlayRenderer = null;
				nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
			}

			if (e.NewElement != null)
			{
				formsMap = (LocationMap)e.NewElement;
				nativeMap = Control as MKMapView;

				var circle = formsMap.Circle;
				nativeMap.OverlayRenderer = GetOverlayRenderer;


				if (circle.Position.Latitude != 0.0)
				{
					_circle = MKCircle.Circle(new CoreLocation.CLLocationCoordinate2D(circle.Position.Latitude, circle.Position.Longitude), circle.Radius);
					nativeMap.AddOverlay(_circle);
				}

				/* Listen for circle change events */
				MessagingCenter.Subscribe<CustomCircle>(this, "CircleChanged", (obj) =>
				{
					if (_circle == null)
					{
						_circle = MKCircle.Circle(new CoreLocation.CLLocationCoordinate2D(obj.Position.Latitude, obj.Position.Longitude), obj.Radius);
						nativeMap.AddOverlay(_circle);
					}
					else
					{
						nativeMap.RemoveOverlay(_circle);
						_circle.Dispose();
						_circle = MKCircle.Circle(new CoreLocation.CLLocationCoordinate2D(obj.Position.Latitude, obj.Position.Longitude), obj.Radius);
						nativeMap.AddOverlay(_circle);
					}
				});

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
			}
		}

		MKOverlayRenderer GetOverlayRenderer(MKMapView mapView, IMKOverlay overlay)
		{
			
			var circleRenderer = new MKCircleRenderer(overlay as MKCircle);
			circleRenderer.FillColor = UIColor.Yellow;
			circleRenderer.Alpha = 0.2f;

			return circleRenderer;
		}

		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			// User's location on map has been identified.  Snap map viewer to that point.
			if (annotation is MKUserLocation)
			{
				return null;					
			}

			return annotationView;
		}

		void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
		{
			// No op.
		}

		void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			// No op.
		}

		void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			// No op.
		}

		CustomPin GetCustomPin(MKPointAnnotation annotation)
		{
			return null;
		}
	}
}
