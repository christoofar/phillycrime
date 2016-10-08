using System;
using System.Collections.Generic;
using Xamarin.Forms;
using PhillyBlotter.Models;

namespace PhillyBlotter
{
	public partial class SearchPage : ContentPage
	{

		Filter currentFilter;

		public SearchPage()
		{
			InitializeComponent();

			// Do we have filters set?
			if (Application.Current.Properties.ContainsKey("Filter"))
			{
				/* Yes! We need to reflect what we know */
				Models.Filter setFilter = (Models.Filter)((int)Application.Current.Properties["Filter"]);
				currentFilter = setFilter;
				LoadFilters();
			}

			UpdateFilters();
		}

		void OnTapGestureRecognizerTapped(object sender, System.EventArgs e)
		{
			var imageSender = (Image)sender;

			// Flip the switch to off
			string imagesource = ((FileImageSource)imageSender.Source).File;
			if (imagesource.Contains("off"))
			{
				imageSender.Source = imagesource.Replace("off", "on");
			}
			else
			{
				imageSender.Source = imagesource.Replace("on", "off");
			}

			UpdateFilters();
		}

		public void LoadFilters()
		{
			if (currentFilter.HasFlag(Filter.Homicide))
				filterHomicide.Source = (((FileImageSource)filterHomicide.Source).File).Replace("off", "on");
			else
				filterHomicide.Source = (((FileImageSource)filterHomicide.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Assault))
				filterAssault.Source = (((FileImageSource)filterAssault.Source).File).Replace("off", "on");
			else
				filterAssault.Source = (((FileImageSource)filterAssault.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Burglary))
				filterBurglary.Source = (((FileImageSource)filterBurglary.Source).File).Replace("off", "on");
			else
				filterBurglary.Source = (((FileImageSource)filterBurglary.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Robbery))
				filterRobbery.Source = (((FileImageSource)filterRobbery.Source).File).Replace("off", "on");
			else
				filterRobbery.Source = (((FileImageSource)filterRobbery.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Theft))
				filterTheft.Source = (((FileImageSource)filterTheft.Source).File).Replace("off", "on");
			else
				filterTheft.Source = (((FileImageSource)filterTheft.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Rape))
				filterRape.Source = (((FileImageSource)filterRape.Source).File).Replace("off", "on");
			else
				filterRape.Source = (((FileImageSource)filterRape.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Vehicle))
				filterAuto.Source = (((FileImageSource)filterAuto.Source).File).Replace("off", "on");
			else
				filterAuto.Source = (((FileImageSource)filterAuto.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Gun))
				filterGuns.Source = (((FileImageSource)filterGuns.Source).File).Replace("off", "on");
			else
				filterGuns.Source = (((FileImageSource)filterGuns.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Prostition))
				filterProstitution.Source = (((FileImageSource)filterProstitution.Source).File).Replace("off", "on");
			else
				filterProstitution.Source = (((FileImageSource)filterProstitution.Source).File).Replace("on", "off");

			if (currentFilter.HasFlag(Filter.Other))
				filterOther.Source = (((FileImageSource)filterOther.Source).File).Replace("off", "on");
			else
				filterOther.Source = (((FileImageSource)filterOther.Source).File).Replace("on", "off");
		}

		public void UpdateFilters()
		{
			var types = new List<string>{ "filterHomicide",
				"filterAssault",
				"filterBurglary",
				"filterRobbery",
				"filterTheft",
				"filterRape",
				"filterAuto",
				"filterGuns",
				"filterProstitution",
				"filterOther"
			};

			foreach (string filter in types)
			{
				Image p = Content.FindByName<Image>(filter);
				string imagesource = ((FileImageSource)p.Source).File;
				if (imagesource.Contains("on"))
				{
					switch (filter)
					{
						case "filterHomicide":
							currentFilter = currentFilter | Filter.Homicide;
							break;
						case "filterAssault":
							currentFilter = currentFilter | Filter.Assault;
							break;
						case "filterBurglary":
							currentFilter = currentFilter | Filter.Burglary;
							break;
						case "filterRobbery":
							currentFilter = currentFilter | Filter.Robbery;
							break;
						case "filterTheft":
							currentFilter = currentFilter | Filter.Theft;
							break;
						case "filterRape":
							currentFilter = currentFilter | Filter.Rape;
							break;
						case "filterAuto":
							currentFilter = currentFilter | Filter.Vehicle;
							break;
						case "filterGuns":
							currentFilter = currentFilter | Filter.Gun;
							break;
						case "filterProstitution":
							currentFilter = currentFilter | Filter.Prostition;
							break;
						case "filterOther":
							currentFilter = currentFilter | Filter.Other;
							break;
					}
				}
				else {
					switch (filter)
					{
						case "filterHomicide":
							currentFilter = currentFilter & ~Filter.Homicide;
							break;
						case "filterAssault":
							currentFilter = currentFilter & ~Filter.Assault;
							break;
						case "filterBurglary":
							currentFilter = currentFilter & ~Filter.Burglary;
							break;
						case "filterRobbery":
							currentFilter = currentFilter & ~Filter.Robbery;
							break;
						case "filterTheft":
							currentFilter = currentFilter & ~Filter.Theft;
							break;
						case "filterRape":
							currentFilter = currentFilter & ~Filter.Rape;
							break;
						case "filterAuto":
							currentFilter = currentFilter & ~Filter.Vehicle;
							break;
						case "filterGuns":
							currentFilter = currentFilter & ~Filter.Gun;
							break;
						case "filterProstitution":
							currentFilter = currentFilter & ~Filter.Prostition;
							break;
						case "filterOther":
							currentFilter = currentFilter & ~Filter.Other;
							break;
					}
				}
			}
		}
	}
}
