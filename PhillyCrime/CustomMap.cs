using System.Collections.Generic;
using Xamarin.Forms.Maps;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public class CustomMap : Map
	{
		public List<CustomPin> CustomPins { get; set; }

		public void Clear()
		{
			if (CustomPins != null)
			{
				CustomPins.Clear();
			}
		}

	}
}
