using Xamarin.Forms.Maps;
using PhillyCrime.Models;

namespace PhillyCrime
{
	public class CustomPin
	{
		public Pin Pin { get; set; }

		public string Id { get; set; }

		public string Url { get; set; }

		public CrimeType CrimeType { get; set; }
	}
}
