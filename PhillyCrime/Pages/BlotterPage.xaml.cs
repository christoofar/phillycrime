using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PhillyBlotter.Helpers;
using PhillyBlotter.Models;

using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class BlotterPage : ContentPage
	{
		bool _distanceFormat = false;  // Signifies that we're going to show a distance blotter
		bool _searchResults = false;
		CrimeSearchCriteria _crimeSearchCriteria = null;
		CrimeReport[] _data = new CrimeReport[0];

		bool _querycomplete = false;

		public BlotterPage()
		{
			InitializeComponent();
		}

		public BlotterPage(bool distanceFormat = false)
		{
			InitializeComponent();

			_distanceFormat = distanceFormat;
		}

		void JumpToMap()
		{
			// Don't let user to go the map until we have some data.
			if (_data.Count() == 0) return;

			var map = new SearchResultsMap(_data);
			Navigation.PushAsync(map);
		}

		// For now we're overloading the blotter view to show
		// crime results.  That means (again, FOR NOW) we have to change the configuration of the
		// list view.   If we deviate too far then we'll have to build a new viewer for this.
		public BlotterPage(CrimeSearchCriteria criteria)
		{
			InitializeComponent();
			this.ToolbarItems.Add(new ToolbarItem("buttonJump", "Images/map.png", JumpToMap, ToolbarItemOrder.Primary));

			// No panels needed
			warningPanel.IsVisible = false;
			activity.IsVisible = true;

			// Disable grouping and pull-to-refresh
			blotterListView.IsPullToRefreshEnabled = false;
			blotterListView.IsGroupingEnabled = false;

			// Data used to execute search in the model.
			_searchResults = true;
			_crimeSearchCriteria = criteria;
		}

		public async void Handle_Refreshing(object sender, System.EventArgs e)
		{
			_querycomplete = false;
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
			if (_querycomplete) return true;

			if (_distanceFormat)
			{
				// If there's no lat/long, don't load this screen but change the warning.
				if (Math.Abs(Settings.PrimaryLat) < Double.Epsilon)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						warningPanel.IsVisible = true;
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


			// Set the title based on blotter/search result type
			if (_distanceFormat)
			{
				Title = "1 Mile Blotter";
			}
			else if (_searchResults)
			{
				Title = "Search Results";
			}
			else 
			{
				if (ids.Count() == 0)
				{
					// You need to select a primary neighborhood.
					warningPanel.IsVisible = true;
					blotterListView.IsVisible = false;
					activity.IsRunning = false;
					activity.IsVisible = false;
					return true;
				}
				else
				{
					var nameList = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.Name);
					string hoodNames = string.Join("/", nameList);
					Title = hoodNames;
				}
			}

			// Populate the blotter data
			if (_distanceFormat)
			{
				var crimes = await Data.GetLocalBlotter(Settings.PrimaryLat, Settings.PrimaryLong, 5280.00);
				var proximitygroup = crimes.GroupBy(item => item.Proximity, (key, group) => new Group(key, group.ToArray()));
				blotterListView.ItemsSource = proximitygroup;
			}
			else if (_searchResults)
			{
				var crimes = await Data.SearchCrimes(_crimeSearchCriteria);
				// if there's only 1 crime result, jump straight to it!
				if (crimes.Length == 1)
				{
					var crimeDetailPage = new CrimeDetailPage((CrimeReport)crimes[0]);
					await Navigation.PushAsync(crimeDetailPage, true);
				}

				// Nothing came back in the search
				if (crimes.Length == 0)
				{
					labelNoRecords.Text = "Nothing came back for your search.";
					warningPanel.IsVisible = true;
					buttonSettings.IsVisible = false;
					activity.IsRunning = false;
					activity.IsVisible = false;
					return true;
				}
				else
				{
					_data = crimes;
					blotterListView.ItemsSource = crimes;
				}
			}
			else
			{
				var hoods = await Data.GetBlotter(ids.ToArray());
				var dategroup = hoods.GroupBy(item => item.OccurredDateType, (key, group) => new Group(key, group.ToArray()));
				blotterListView.ItemsSource = dategroup;
			}

			warningPanel.IsVisible = false;
			blotterListView.IsVisible = true;
			activity.IsRunning = false;
			activity.IsVisible = false;

			blotterListView.EndRefresh();

			_querycomplete = true;

			return true;
		}
	}
}
