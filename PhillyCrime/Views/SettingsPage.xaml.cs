using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using PhillyCrime.Models;
using System.Diagnostics;

namespace PhillyCrime
{
	public partial class SettingsPage : ContentPage
	{
		ObservableCollection<Neighborhood> hoods = new ObservableCollection<Neighborhood>();

		public SettingsPage()
		{
			InitializeComponent();
		}

		void OnTapGestureRecognizerTapped(object sender, EventArgs args)
		{
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			hoods.Add(new Neighborhood { Name = "Olde Richmond", ID = 34 });
			hoods.Add(new Neighborhood { Name = "Fishtown", ID = 25 });

			NeighborhoodListView.ItemsSource = hoods;

		}

		void NeighborhoodSelected(object sender, Xamarin.Forms.ToggledEventArgs e)
		{
			if (sender is SwitchCell)			
			{
				
				Neighborhood hood = (Neighborhood)((SwitchCell)sender).BindingContext;
				hood.Selected = false;
			}
		}
	}
}

