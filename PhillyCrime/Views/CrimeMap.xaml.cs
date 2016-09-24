using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace PhillyBlotter
{
	public partial class CrimeMap : ContentPage
	{
		public CrimeMap()
		{
			InitializeComponent();
		}

		public CrimeMap(double longitude, double latitude, string title)
		{
			InitializeComponent();

			var span = MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromMiles(0.35));
			var pin = new Pin();
			pin.Label = title;
			pin.Position = span.Center;
			MyMap.MoveToRegion(span);
			MyMap.Pins.Add(pin);
		}
	}
}

