using System;
using System.Collections.Generic;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class BetaWelcome : PopupPage
	{
		public BetaWelcome()
		{
			InitializeComponent();

		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			if (CarouselFeatures != null)
			{
			}
		}

		void ClosePopup(object sender, System.EventArgs e)
		{
			Application.Current.Properties["WELCOME"] = Global.VERSION;
			Application.Current.SavePropertiesAsync();
			PopupNavigation.PopAllAsync(true);
		}
	}
}

