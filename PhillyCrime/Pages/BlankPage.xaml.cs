using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class BlankPage : ContentPage
	{
		public BlankPage()
		{
			InitializeComponent();

			var betaWindow = new BetaWelcome();
			PopupNavigation.PushAsync(betaWindow);
		}
	}
}
