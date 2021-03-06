﻿using System;
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

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace PhillyBlotter.iOS
{
	public class CustomMapRenderer : MapRenderer
	{
		UIView customPinView;
		List<CustomPin> customPins;

		public static UIImage Pin_Assault = UIImage.FromFile("Images/a_pin.png");
		public static UIImage Pin_Homicide = UIImage.FromFile("Images/h_pin.png");
		public static UIImage Pin_Burglary = UIImage.FromFile("Images/b_pin.png");
		public static UIImage Pin_Robbery = UIImage.FromFile("Images/ro_pin.png");
		public static UIImage Pin_Rape = UIImage.FromFile("Images/ra_pin.png");
		public static UIImage Pin_Theft = UIImage.FromFile("Images/t_pin.png");
		public static UIImage Pin_VehicleRecovery = UIImage.FromFile("Images/rv_pin.png");
		public static UIImage Pin_Prostition = UIImage.FromFile("Images/p_pin.png");
		public static UIImage Pin_Gun = UIImage.FromFile("Images/g_pin.png");
		public static UIImage Pin_Other = UIImage.FromFile("Images/o_pin.png");
		public static UIImage Pin_StolenVehicle = UIImage.FromFile("Images/vt_pin.png");
		public static UIImage Pin_TheftFromAuto = UIImage.FromFile("Images/ta_pin.png");
		public static UIImage Pin_CriminalMischief = UIImage.FromFile("Images/m_pin.png");
		public static UIImage Pin_Narcotics = UIImage.FromFile("Images/n_pin.png");
		public static UIImage Pin_DUI = UIImage.FromFile("Images/d_pin.png");
		public static UIImage Pin_Sex = UIImage.FromFile("Images/s_pin.png");
		public static UIImage FS_Assault = UIImage.FromFile("Images/a_on.png");
		public static UIImage FS_Homicide = UIImage.FromFile("Images/h_on.png");
		public static UIImage FS_Burglary = UIImage.FromFile("Images/b_on.png");
		public static UIImage FS_Robbery = UIImage.FromFile("Images/ro_on.png");
		public static UIImage FS_Rape = UIImage.FromFile("Images/ra_on.png");
		public static UIImage FS_Theft = UIImage.FromFile("Images/t_on.png");
		public static UIImage FS_VehicleRecovery = UIImage.FromFile("Images/rv_on.png");
		public static UIImage FS_Prostition = UIImage.FromFile("Images/p_on.png");
		public static UIImage FS_Gun = UIImage.FromFile("Images/g_on.png");
		public static UIImage FS_Other = UIImage.FromFile("Images/o_on.png");
		public static UIImage FS_StolenVehicle = UIImage.FromFile("Images/vt_on.png");
		public static UIImage FS_TheftFromAuto = UIImage.FromFile("Images/ta_on.png");
		public static UIImage FS_CriminalMischief = UIImage.FromFile("Images/m_on.png");
		public static UIImage FS_Narcotics = UIImage.FromFile("Images/n_on.png");
		public static UIImage FS_DUI = UIImage.FromFile("Images/d_on.png");
		public static UIImage FS_Sex = UIImage.FromFile("Images/s_on.png");

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
			}
			else
			{
				// OK we really need to clean the hell out of this pin, because it's
				// got garbage on it.
				annotationView.Image = null;
				annotationView.LeftCalloutAccessoryView.Dispose();
				annotationView.RightCalloutAccessoryView.Dispose();
				if (annotationView.DetailCalloutAccessoryView != null)
				{
					annotationView.DetailCalloutAccessoryView.Dispose();
				}

			}

			switch (customPin.CrimeType)
			{
				case Models.CrimeType.Homicide:
					annotationView.Image = Pin_Homicide;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Homicide);
					break;
				case Models.CrimeType.Robbery:
					annotationView.Image = Pin_Robbery;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Robbery);
					break;
				case Models.CrimeType.Assault:
					annotationView.Image = Pin_Assault;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Assault);
					break;
				case Models.CrimeType.Burglary:
					annotationView.Image = Pin_Burglary;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Burglary);
					break;
				case Models.CrimeType.Rape:
					annotationView.Image = Pin_Rape;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Rape);
					break;
				case Models.CrimeType.Theft:
					annotationView.Image = Pin_Theft;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Theft);
					break;
				case Models.CrimeType.Prostition:
					annotationView.Image = Pin_Prostition;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Prostition);
					break;
				case Models.CrimeType.TheftFromAuto:
					annotationView.Image = Pin_TheftFromAuto;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_TheftFromAuto);
					break;
				case Models.CrimeType.StolenVehicle:
					annotationView.Image = Pin_StolenVehicle;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_StolenVehicle);
					break;
				case Models.CrimeType.VehicleRecovery:
					annotationView.Image = Pin_VehicleRecovery;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_VehicleRecovery);
					break;
				case Models.CrimeType.Gun:
					annotationView.Image = Pin_Gun;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Gun);
					break;
				case Models.CrimeType.CriminalMischief:
					annotationView.Image = Pin_CriminalMischief;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_CriminalMischief);
					break;
				case Models.CrimeType.DUI:
					annotationView.Image = Pin_DUI;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_DUI);
					break;
				case Models.CrimeType.Narcotics:
					annotationView.Image = Pin_Narcotics;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Narcotics);
					break;
				case Models.CrimeType.Other:
					annotationView.Image = Pin_Other;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Other);
					break;
				case Models.CrimeType.OtherSexAssault:
					annotationView.Image = Pin_Sex;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Sex);
					break;
				default:
					annotationView.Image = Pin_Other;
					annotationView.LeftCalloutAccessoryView = new UIImageView(FS_Other);
					break;

			}

			annotationView.CalloutOffset = new CGPoint(0, 0);

			annotationView.RightCalloutAccessoryView = UIButton.FromType(UIButtonType.DetailDisclosure);
			UIStackView stackview = new UIStackView();
			stackview.Axis = UILayoutConstraintAxis.Vertical;
			stackview.AddArrangedSubview(new UILabel() { Text = customPin.Occurred });
			stackview.AddArrangedSubview(new UILabel() { Text = customPin.Pin.Address });
			annotationView.DetailCalloutAccessoryView = stackview;
			((CustomMKAnnotationView)annotationView).Id = customPin.Id;
			((CustomMKAnnotationView)annotationView).Url = customPin.Url;
			((CustomMKAnnotationView)annotationView).Occurred = customPin.Occurred;
			((CustomMKAnnotationView)annotationView).Pin = customPin;
			annotationView.CanShowCallout = true;

			return annotationView;
		}

		void OnCalloutAccessoryControlTapped(object sender, MKMapViewAccessoryTappedEventArgs e)
		{
			if (e.View == null) return;
			if (!(e.View is CustomMKAnnotationView)) return;

			var customView = e.View as CustomMKAnnotationView;
			MessagingCenter.Send<CustomPin>(customView.Pin, "ShowCrimeReport");
			//if (!string.IsNullOrWhiteSpace(customView.Url))
			//{
			//	UIApplication.SharedApplication.OpenUrl(new Foundation.NSUrl(customView.Url));
			//}
		}

		void OnDidSelectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			if (e.View == null) return;
			if (!(e.View is CustomMKAnnotationView)) return;

			var customView = e.View as CustomMKAnnotationView;
			customPinView = new UIView();

			if (customView.Id == "Xamarin")
			{
				customPinView.Frame = new CGRect(0, 0, 200, 84);
				var image = new UIImageView(new CGRect(0, 0, 200, 84));
				image.Image = UIImage.FromFile("Images/car_on.png");
				customPinView.AddSubview(image);
				customPinView.Center = new CGPoint(0, -(e.View.Frame.Height + 75));
				//e.View.AddSubview(customPinView);
			}
		}

		void OnDidDeselectAnnotationView(object sender, MKAnnotationViewEventArgs e)
		{
			if (e.View == null) return;
			if (!(e.View is CustomMKAnnotationView)) return;

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
