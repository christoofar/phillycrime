using System;
using System.Collections.Generic;
using Xamarin.Forms;
using PhillyBlotter.Models;
using System.Diagnostics;
using System.Linq;
using PhillyBlotter.Helpers;
using Xamarin.Forms.Xaml;

namespace PhillyBlotter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SearchPage : ContentPage
	{

		Filter currentFilter;

		public SearchPage()
		{
			InitializeComponent();

			DateTime presetDate = DateTime.Now;
			// If we're over the hump we'll set the start point at the first of this month.
			if (presetDate.Day >= 15)
			{
				presetDate = presetDate.AddDays((presetDate.Day - 1) * -1);
			}
			else
			{
				// We'll set the start at the 15th of the last month.
				// (except if this is January)
				if (presetDate.Month == 1)
				{
					presetDate = new DateTime(presetDate.Year - 1, 12, 15);
				}
				else
				{
					presetDate = new DateTime(presetDate.Year, presetDate.Month - 1, 15);
				}
			}

			// Pre-set the date search for the user
			DateStart.Date = presetDate;

			// Do we have filters set?		
			Models.Filter setFilter = Settings.Filter;
			currentFilter = setFilter;
			LoadFilters();

			/* Prime neighborhoods */
			foreach (Neighborhood hood in Global.Neighborhoods)
			{
				Neighborhood.Items.Add(hood.Name);
			}

			// If user has one primary neighborhood then we'll autofill the neighborhood search to
			// make life easier for them.
			if (Global.Neighborhoods.Where(p => p.Selected).Count() == 1)
			{
				int index = Neighborhood.Items.IndexOf(Global.Neighborhoods.Where(p => p.Selected).FirstOrDefault().Name);
				Neighborhood.SelectedIndex = index;
			}

			UpdateFilters();
		}

		void NeighborhoodChanged(object sender, System.EventArgs e)
		{
			if (Neighborhood.SelectedIndex != -1)
			{
				PoliceDistrictID.SelectedIndex = -1;
				PSA.SelectedIndex = -1;
			}
		}

		void PoliceDistrictChanged(object sender, System.EventArgs e)
		{
			if (PoliceDistrictID.SelectedIndex != -1)
			{
				Neighborhood.SelectedIndex = -1;
			}
		}

		void DCNChanged(object sender, Xamarin.Forms.TextChangedEventArgs e)
		{
			if (e.OldTextValue == null) return;

			if (e.NewTextValue.Length > e.OldTextValue.Length)
			{

				// How many dashes are there
				int newDashes = e.NewTextValue.Split('-').Length - 1;
				int oldDashes = e.OldTextValue.Split('-').Length - 1;

				// if this is just 1 increase in dashes then fuck it
				if (newDashes - oldDashes == 1)
				{
					DCN.Text = e.NewTextValue;
					return;
				}

				string s = e.NewTextValue;
				while (s.Contains("-"))
				{
					s = s.Remove(s.IndexOf('-'), 1);				
				}
				int len = s.Length;
				string v = "";

				switch (len)
				{
					case 0:
						break;
					case 1:
						v += s[0];
						break;
					case 2:
						v += s[0];
						v += s[1];
						break;
					case 3:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						break;
					case 4:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						break;
					case 5:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						break;
					case 6:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						v += s[5];
						break;
					case 7:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						v += s[5];
						v += s[6];
						break;
					case 8:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += s[4];
						v += s[5];
						v += s[6];
						v += s[7];
						break;
					case 9:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += s[6];
						v += s[7];
						v += s[8];
						break;
					case 10:
						v += s[0];
						v += s[1];
						v += '-';
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						break;
					case 11:
						v += s[0];
						v += s[1];
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += '-';
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						v += s[10];
						break;
					case 12:
						v += s[0];
						v += s[1];
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += '-';
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						v += s[10];
						v += s[11];
						break;
					default:
						v += s[0];
						v += s[1];
						v += s[2];
						v += s[3];
						v += '-';
						v += s[4];
						v += s[5];
						v += '-';
						v += s[6];
						v += s[7];
						v += s[8];
						v += s[9];
						v += s[10];
						v += s[11];
						break;
				}

				DCN.Text = v;
			}
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

		void Search_Clicked(object sender, System.EventArgs e)
		{
			CrimeSearchCriteria criteria = new CrimeSearchCriteria();

			// If there's a DCN we don't need to bother with rest
			if (DCN.Text != null && DCN.Text.Length > 3)
			{
				criteria.DCN = DCN.Text;
			}
			else
			{
				// Was a neighborhood selected?
				if (Neighborhood.SelectedIndex > -1)
				{
					string selectedhood = Neighborhood.Items[Neighborhood.SelectedIndex];
					var hood = Global.Neighborhoods.Where(p => p.Name == selectedhood).FirstOrDefault();
					if (hood != null)
					{
						criteria.NeighborhoodID = hood.ID.ToString();
					}

					// Special case if user picked "Entire City"
					if (Neighborhood.SelectedIndex == 0)
					{
						criteria.NeighborhoodID = "-1";
					}
				}

				// What about a police district?
				if (PoliceDistrictID.SelectedIndex > -1)
				{
					criteria.PoliceDistrict = PoliceDistrictID.Items[PoliceDistrictID.SelectedIndex];

					// And what about a PSA?
					if (PSA.SelectedIndex > 0)
					{
						criteria.PSA = PSA.Items[PSA.SelectedIndex];
					}
				}

				criteria.StartDate = DateStart.Date + TimeStart.Time;
				criteria.EndDate = DateEnd.Date + TimeEnd.Time;
				criteria.Filter = currentFilter;
				criteria.OnlyArrests = SwitchArrests.IsToggled;
			}

			// Do we have any selection criteria now?  If not then this query make no sense
			// and we need to train the user.
			if ((criteria.NeighborhoodID == null || criteria.NeighborhoodID == "") &&
				(criteria.PoliceDistrict == null || criteria.PoliceDistrict == "") &&
				(criteria.DCN == null || criteria.DCN == ""))
			{
				DisplayAlert("Incomplete Search", "Please provide a neighborhood or a police district to look for crimes in--or, a District Control Number to " +
							 "retrieve a specific crime", "OK");
				return;
			}

			Navigation.PushAsync(new BlotterPage(criteria));

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
