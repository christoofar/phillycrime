using System;
using Xamarin.Forms;

namespace PhillyCrime
{
	public class TextSupport
	{

		// Define two values as multiples of font size.
		private static double lineHeight = Device.OnPlatform(1.2, 1.2, 1.3);
		private static double charWidth = 0.5;

		public TextSupport()
		{
		}

		public static int CalcMaxFontSize(string text, VisualElement view)
		{
			int charCount = text.Length;

			int fontSize = (int)Math.Sqrt(view.Width * view.Height /
				(charCount * lineHeight * charWidth));

			return fontSize;
		}
	}
}

