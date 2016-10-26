using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using PhillyBlotter.Models;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class ArrestSearchResults : ContentPage
	{
		public ArrestSearchResults()
		{
			InitializeComponent();
		}

		public ArrestSearchResults(ArrestSearchCriteria criteria)
		{
			InitializeComponent();
			Search(criteria);
		}

		public ArrestSearchResults(string name, DateTime DateOfBirth)
		{
			InitializeComponent();
			MultipleNotice.IsVisible = true;
			Title = "Arrests Matching '" + name + "'";
			ArrestSearchCriteria criteria = new ArrestSearchCriteria();
			criteria.FirstName = name;
			criteria.Birthday = DateOfBirth;
			criteria.ArrestStart = DateTime.Today.AddYears(-10);
			criteria.ArrestEnd = DateTime.Today;
			Search(criteria);
		}

		public void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			ArrestReport report = (ArrestReport)e.SelectedItem;
			var crimeDetailPage = new CrimeDetailPage(report.DCN, report.CrimeType);
			Navigation.PushAsync(crimeDetailPage);
		}

		private async void Search(ArrestSearchCriteria criteria)
		{
			var data = await Data.SearchArrests(criteria);

			blotterListView.ItemsSource = data;

			blotterListView.IsVisible = true;
			activity.IsRunning = false;
			activity.IsVisible = false;
		}
	}
}
