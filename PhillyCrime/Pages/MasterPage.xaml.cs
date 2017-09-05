using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhillyBlotter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : ContentPage
	{

		object loc = new object();
		bool _visible = false;

		public ListView ListView
		{
			get { return listView; }
			set { listView = value; }
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			lock(loc)
			{
				_visible = true;
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			lock(loc)
			{
				_visible = false;
			}
		}


		public MasterPage()
		{
			InitializeComponent();

			MessagingCenter.Subscribe<object, string>(Global.MessagingInstance, "Notification", (arg1, arg2) =>
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					try
					{
						lock(loc)
						{
							if (_visible)
							{
								this.DisplayAlert("Crime Notification", arg2, "OK");
								MessagingCenter.Send<object>(Global.MessagingInstance, "NewCrimePush");
							}
						}
					}
					catch { }
				});
			});

			var masterPageItems = new List<MasterPageItem>();
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Crime Map",
				IconSource = "Images/map.png",
				TargetType = typeof(CrimesNearMeView)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Neighborhood Blotter",
				IconSource = "Images/checklist.png",
				TargetType = typeof(BlotterPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "1 Mile Blotter",
				IconSource = "Images/radar.png",
				TargetType = typeof(OneMileBlotterPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Neighborhood Statistics",
				IconSource = "Images/chart.png",
				TargetType = typeof(StatsPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Search Crimes",
				IconSource = "Images/search.png",
				TargetType = typeof(SearchPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Search Arrest Records",
				IconSource = "Images/justice.png",
				TargetType = typeof(ArrestSearchPage)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Map Legend",
				IconSource = "Images/legend.png",
				TargetType = typeof(MapLegend)
			});
			masterPageItems.Add(new MasterPageItem
			{
				Title = "Settings",
				IconSource = "Images/config.png",
				TargetType = typeof(SettingsPage)
			});

			listView.ItemsSource = masterPageItems;

		}


	}
}
