using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;
using PhillyCrime.Models;
using System.Diagnostics;
using System.Threading.Tasks;

namespace PhillyCrime
{
	public partial class SettingsPage : ContentPage
	{
		bool _runOnce = false;

		public SettingsPage()
		{
			InitializeComponent();
		}

		void OnTapGestureRecognizerTapped(object sender, EventArgs args)
		{
		}

		async Task<bool> GetNeighborhoodSettings()
		{
			// Sync the selected neighborhood settings with the model.
			bool getSettings = await Global.NeighborhoodSyncSettings();
			if (getSettings)
			{
				Debug.WriteLine("Neighborhood list configured.");
			}

			return true;
		}

		protected async override void OnAppearing()
		{
			base.OnAppearing();

			if (!_runOnce)
			{
				var b = await GetNeighborhoodSettings();
				if (b)
				{

					NeighborhoodListView.ItemsSource = Global.Neighborhoods;

					// Scroll the list to the first selected thing.
					var firstSelected = Global.Neighborhoods.Where(p => p.Selected).FirstOrDefault();
					if (firstSelected != null)
					{
						NeighborhoodListView.ScrollTo(firstSelected, ScrollToPosition.Start, false);
					}
				}

				_runOnce = true;
			}
		}

		async void NeighborhoodSelected(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			if (sender is SwitchCell)			
			{
				Neighborhood hood = (Neighborhood)((SwitchCell)sender).BindingContext;

				// We shouldn't allow more than 3 hoods to be activated
				if (e.Value)
				{
					if (Global.Neighborhoods.Where(p => p.Selected).Count() == 4)
					{
						hood.Selected = false;
						await DisplayAlert("Maximum Neighborhods Reached", "You can only have 3 neighborhood blotters active " +
									 "at once.", "OK");
						return;
					}
				}

				await App.UpdateNeighborhood(hood.ID, e.Value);
			}
		}
	}
}

