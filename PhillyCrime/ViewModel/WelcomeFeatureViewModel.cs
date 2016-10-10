using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public class WelcomeFeatureViewModel
	{
		public ObservableCollection<WelcomeFeatureItem> Features { get; set; }

		static string FOLDER = "";

		public WelcomeFeatureViewModel()
		{
			Device.OnPlatform(() => { FOLDER = "Images/"; }, () => { FOLDER = "drawable/"; }, null, null);

			Features = new ObservableCollection<WelcomeFeatureItem>
			{
				new WelcomeFeatureItem
				{
					ImageUrl = $"{WelcomeFeatureViewModel.FOLDER}DistanceBlotterFeature.png",
					Description = "See crimes by distance from you",
					IsNotLastItem = true
				},
				new WelcomeFeatureItem
				{
					ImageUrl =    $"{WelcomeFeatureViewModel.FOLDER}NotifyFeatureImage",
					Description = "Set the area you care about the most.  PhillyBlotter returns the data that's relevant to you",
					IsNotLastItem = true
				},
				new WelcomeFeatureItem
				{
					ImageUrl = "http://content.screencast.com/users/JamesMontemagno/folders/Jing/media/e8179889-8189-4acb-bac5-812611199a03/2016-06-02_1053.png",
					Description = "Phoenix Zoo",
					IsNotLastItem = false
				}
			};
		}
	}
}


