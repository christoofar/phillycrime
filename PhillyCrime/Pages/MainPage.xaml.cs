using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Polly;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PhillyBlotter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : MasterDetailPage
	{

		static MainPage _instance;

		public MainPage()
		{
			// Start listener for pushpin events
			MessagingCenter.Subscribe<string>(this, "JumpToBlotter", (obj) =>
			{
				_instance.JumpToBlotter();
				MessagingCenter.Unsubscribe<string>(this, "JumpToBlotter");
			});

			MessagingCenter.Subscribe<object>(this, "NewCrimePush", (obj) =>
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(BlotterPage), "NewCrimePush"));
			});

			InitializeComponent();

			_instance = this;

			masterPage.ListView.ItemSelected += OnItemSelected;

			if (Global.ReceivedCrimeAlert)
			{
				Global.ReceivedCrimeAlert = false;
				Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(BlotterPage), "NewCrimePush"));
			}
			else
			{
				Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(CrimesNearMeView)));
			}
		}

		public void JumpToBlotter()
		{
			_instance.Detail = new NavigationPage((Page)Activator.CreateInstance(typeof(BlotterPage)));
			_instance.masterPage.ListView.SelectedItem = null;
			_instance.IsPresented = false;
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
