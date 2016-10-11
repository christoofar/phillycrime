using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class MainPage : MasterDetailPage
	{

		static MainPage _instance;

		public MainPage()
		{
			// Start listener for pushpin events
			MessagingCenter.Subscribe<string>(this, "JumpToBlotter", (obj) =>
			{
				MainPage.JumpToBlotter();
				MessagingCenter.Unsubscribe<string>(this, "JumpToBlotter");
			});

			InitializeComponent();

			_instance = this;

			masterPage.ListView.ItemSelected += OnItemSelected;
		}

		public static void JumpToBlotter()
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				_instance.Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(BlotterPage)));
				_instance.masterPage.ListView.SelectedItem = null;
				_instance.IsPresented = false;
			});
		}

		void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var item = e.SelectedItem as MasterPageItem;
			if (item != null)
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(item.TargetType));
				masterPage.ListView.SelectedItem = null;
				IsPresented = false;
			}
		}

	}
}
