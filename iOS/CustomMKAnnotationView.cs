using MapKit;
using PhillyBlotter;

namespace PhillyBlotter.iOS
{
	public class CustomMKAnnotationView : MKAnnotationView
	{
		public string Id { get; set; }

		public string Url { get; set; }

		public string Occurred { get; set;}

		public CustomPin Pin { get; set;}

		public CustomMKAnnotationView(IMKAnnotation annotation, string id)
			: base(annotation, id)
		{
		}
	}
}
