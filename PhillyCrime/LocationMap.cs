using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public class CustomCircle
	{
		Position _position;
		double _radius;

		public Position Position
		{ 
			get { return _position; }
			set
			{
				if (_position != value)
				{
					_position = value;
					MessagingCenter.Send<CustomCircle>(this, "CircleChanged");
				}
			}
		}

		public double Radius
		{ 
			get { return _radius; }
			set
			{
				if (_radius != value)				
				{
					_radius = value;
					MessagingCenter.Send<CustomCircle>(this, "CircleChanged");
				}
			}
		}
	}

	public class LocationMap : Map
	{
		public CustomCircle Circle { get; set; } = new CustomCircle();
	}
}
