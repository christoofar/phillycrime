using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using PhillyCrime.Models;

using Xamarin.Forms;

namespace PhillyCrime
{
	public partial class BlotterPage : ContentPage
	{
		public BlotterPage()
		{
			InitializeComponent();

		}

		async void Handle_Refreshing(object sender, System.EventArgs e)
		{
			await Refresh();
		}

		async protected override void OnAppearing()
		{
			base.OnAppearing();
			await Refresh();
		}

		void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			int b = 0;
			Debug.WriteLine(e.SelectedItem.ToString());

			var crimeDetailPage = new CrimeDetailPage((CrimeReport)e.SelectedItem);
			Navigation.PushAsync(crimeDetailPage);
		}

		async Task<bool> Refresh()
		{

			//Look for neighborhoods user subscribes to.
			var ids = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.ID);


			if (ids.Count() == 0)
			{
				// You need to select a primary neighborhood.
				labelNoRecords.IsVisible = true;
				blotterListView.IsVisible = false;
				activity.IsRunning = false;
				activity.IsVisible = false;
			}
			else
			{
				var nameList = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.Name);
				string hoodNames = string.Join("/", nameList);
				Title = hoodNames;

				labelNoRecords.IsVisible = false;
				blotterListView.IsVisible = true;
				activity.IsRunning = false;
				activity.IsVisible = false;
				var hoods = await Data.GetBlotter(ids.ToArray());
				blotterListView.ItemsSource = hoods;
			}

			blotterListView.EndRefresh();

			return true;
		}
	}
}
