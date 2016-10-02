﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PhillyBlotter.Models;

using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class BlotterPage : ContentPage
	{
		bool _distanceFormat = false;  // Signifies that we're going to show a distance blotter

		public BlotterPage()
		{
			InitializeComponent();
		}


		public BlotterPage(bool distanceFormat = false)
		{
			InitializeComponent();

			_distanceFormat = distanceFormat;
		}

		public async void Handle_Refreshing(object sender, System.EventArgs e)
		{
			await Refresh();
		}

		async protected override void OnAppearing()
		{
			base.OnAppearing();
			await Refresh();
		}

		public async void OnSettingsButtonClicked(object sender, System.EventArgs e)
		{
			if (_distanceFormat)
			{
				await Navigation.PushAsync(new LocationPickerPage());
			}
			else
			{
				await Navigation.PushAsync(new SettingsPage());
			}
		}

		public void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			var crimeDetailPage = new CrimeDetailPage((CrimeReport)e.SelectedItem);
			Navigation.PushAsync(crimeDetailPage);
		}

		async Task<bool> Refresh()
		{

			if (_distanceFormat)
			{
				// If there's no lat/long, don't load this screen but change the warning.
				if (!Application.Current.Properties.ContainsKey("PrimaryLat"))
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						labelNoRecords.Text = "You need to set a primary location in order to use this feature.";
						buttonSettings.Text = "Set Primary Location...";
						activity.IsRunning = false;
						activity.IsVisible = false;
					});
					return true;
				}
			}

			//Look for neighborhoods user subscribes to.
			var ids = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.ID);

			if (ids.Count() == 0)
			{
				// You need to select a primary neighborhood.
				warningPanel.IsVisible = true;
				blotterListView.IsVisible = false;
				activity.IsRunning = false;
				activity.IsVisible = false;
			}
			else
			{
				if (!_distanceFormat)
				{
					var nameList = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.Name);
					string hoodNames = string.Join("/", nameList);
					Title = hoodNames;
				}
				else
				{
					Title = "1 Mile Blotter";
				}

				warningPanel.IsVisible = false;
				blotterListView.IsVisible = true;
				activity.IsRunning = false;
				activity.IsVisible = false;

				if (!_distanceFormat)
				{
					var hoods = await Data.GetBlotter(ids.ToArray());
					var dategroup = hoods.GroupBy(item => item.OccurredDateType, (key, group) => new Group(key, group.ToArray()));
					blotterListView.ItemsSource = dategroup;
				}
				else
				{
					var crimes = await Data.GetLocalBlotter((double)Application.Current.Properties["PrimaryLat"], (double)Application.Current.Properties["PrimaryLong"], 5280.00);
					var proximitygroup = crimes.GroupBy(item => item.Proximity, (key, group) => new Group(key, group.ToArray()));
					blotterListView.ItemsSource = proximitygroup;
				}
			}

			blotterListView.EndRefresh();

			return true;
		}
	}
}
