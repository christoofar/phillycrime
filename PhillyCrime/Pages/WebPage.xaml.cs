using System;
using System.Collections.Generic;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhillyBlotter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
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
