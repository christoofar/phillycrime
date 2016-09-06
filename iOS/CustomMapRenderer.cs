﻿using System;
using System.Collections.Generic;
using CoreGraphics;
using PhillyCrime;
using PhillyCrime.iOS;
using MapKit;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.iOS;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PhillyCrime.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		UIView customPinView;
		List<CustomPin> customPins;

		protected override void OnElementChanged(ElementChangedEventArgs<View> e)
		{
			base.OnElementChanged(e);

			if (e.OldElement != null)
			{
				var nativeMap = Control as MKMapView;
				nativeMap.GetViewForAnnotation = null;
				nativeMap.CalloutAccessoryControlTapped -= OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView -= OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView -= OnDidDeselectAnnotationView;
			}

			if (e.NewElement != null)
			{
				var formsMap = (CustomMap)e.NewElement;
				var nativeMap = Control as MKMapView;
				customPins = formsMap.CustomPins;

				nativeMap.GetViewForAnnotation = GetViewForAnnotation;
				nativeMap.CalloutAccessoryControlTapped += OnCalloutAccessoryControlTapped;
				nativeMap.DidSelectAnnotationView += OnDidSelectAnnotationView;
				nativeMap.DidDeselectAnnotationView += OnDidDeselectAnnotationView;
			}
		}

		MKAnnotationView GetViewForAnnotation(MKMapView mapView, IMKAnnotation annotation)
		{
			MKAnnotationView annotationView = null;

			if (annotation is MKUserLocation)
				return null;

			var anno = annotation as MKPointAnnotation;
			var customPin = GetCustomPin(anno);
			if (customPin == null)
			{
				return null;
				//throw new Exception("Custom pin not found");
			}

			annotationView = mapView.DequeueReusableAnnotation(customPin.Id);
			if (annotationView == null)
			{
				annotationView = new CustomMKAnnotationView(annotation, customPin.Id);

				string s = "Images/";
				switch (customPin.CrimeType)
				{
					case Models.CrimeType.Homicide:
						s += "h_pin.png";
					break;
					case Models.CrimeType.Robbery:
						s += "ro_pin.png";
					break;
					case Models.CrimeType.Assault:
						s += "a_pin.png";
					break;
					case Models.CrimeType.Burglary:
						s += "b_pin.png";
					break;;
					case Models.CrimeType.Rape:
						s += "ra_pin.png";
					break;
				}
				annotationView.Image = UIImage.FromFile(s);
				annotationView.CalloutOffset = new CGPoint(0, 0);
				//annotationView.LeftCalloutAccessoryView = new UIImageView(UIImage.FromFile("monkey.png"));
				annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
				((CustomMKAnnotationView)annotationView).Id = customPin.Id;
				((CustomMKAnnotationView)annotationView).Url = customPin.Url;
			}
			annotationView.CanShowCallout = true;

			return annotationView;
		}

		void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
		{
			var customView = e.View as CustomMKAnnotationView;
			if (!string.IsNullOrWhiteSpace(customView.Url))
			{
				UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
			}
		}

		void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			var customView = e.View as CustomMKAnnotationView;
			customPinView = new UIView();

			if (customView.Id == "Xamarin")
			{
				customPinView.Frame = new CGRect(0, 0, 200, 84);
				var image = new UIImageView(new CGRect(0, 0, 200, 84));
				image.Image = UIImage.FromFile("Images/car_on.png");
				customPinView.AddSubview(image);
				customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
				e.View.AddSubview(customPinView);
			}
		}

		void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			if (!e.View.Selected)
			{
				customPinView.RemoveFromSuperview();
				customPinView.Dispose();
				customPinView = null;
			}
		}

		CustomPin GetCustomPin(MKPointAnnotation annotation)
		{
			if (annotation == null)
				return null;
			
			var position = new Position(annotation.Coordinate.Latitude, annotation.Coordinate.Longitude);
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