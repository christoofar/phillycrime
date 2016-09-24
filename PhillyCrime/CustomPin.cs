using Xamarin.Forms.Maps;
using PhillyBlotter.Models;

namespace PhillyBlotter
{
	public class CustomPin
	{
		public Pin Pin { get; set; }

		public string Id { get; set; }

		public string Url { get; set; }

		public string Occurred { get; set;}

		public CrimeType CrimeType { get; set; }
	}
}
