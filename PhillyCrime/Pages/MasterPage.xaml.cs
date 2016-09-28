using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class MasterPage : ContentPage
	{

		public ListView ListView
		{
			get { return listView; }
			set { listView = value; }
		}


		public MasterPage()
		{
			InitializeComponent();

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
				Title = "Settings",
				IconSource = "Images/config.png",
				TargetType = typeof(SettingsPage)
			});

			listView.ItemsSource = masterPageItems;
		}


	}
}
