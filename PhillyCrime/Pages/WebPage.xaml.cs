using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class WebPage : ContentPage
	{
		public WebPage()
		{
			InitializeComponent();
		}

		public WebPage(string URL)
		{
			InitializeComponent();
			Web.Source = URL;
		}
	}
}
