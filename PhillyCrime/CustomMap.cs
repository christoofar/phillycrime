using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace PhillyCrime
{
	public class CustomMap : Map
	{
		public List<CustomPin> CustomPins { get; set; }
	}
}
