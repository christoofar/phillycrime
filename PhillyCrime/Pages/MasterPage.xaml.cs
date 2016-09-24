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
				Title = "Local Blotter",
				IconSource = "Images/cop.png",
				TargetType = typeof(BlotterPage)
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
