using System;
using System.Collections.Generic;
using Xamarin.Forms;
using PhillyCrime.Models;

namespace PhillyCrime
{
	public partial class ArrestView : ContentView
	{
		Arrest _report;
		string _url = "";


		public ArrestView()
		{
			InitializeComponent();
			buttonDocket.Clicked += ButtonDocket_Clicked;
		}

		void OnLivesInTapped(object sender, EventArgs args)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Device.OpenUri(new Uri(string.Format("https://www.google.com/maps/place/{0}", _report.DefendantZip)));
			});
		}

		public void UpdateData(Arrest arrest)
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				// This includes the defendant's age.
				_report = arrest;
				var today = DateTime.Today;

				if (_report.DateOfBirth != null && _report.DateOfBirth.Length > 5)
				{
					DateTime birthdate;
					DateTime.TryParse(_report.DateOfBirth, out birthdate);
					if (birthdate > DateTime.Parse("1/1/1900"))
					{
						var age = today.Year - birthdate.Year;
						if (birthdate > today.AddYears(-age)) age--;

						textDefendantName.Text = string.Format("{0}, {1}", _report.Defendant,
															   age);
					}
					else {
						textDefendantName.Text = string.Format("{0}", _report.Defendant);
					}

				}
				else {
					textDefendantName.Text = string.Format("{0}", _report.Defendant);
				}

				// See if this is a valid arrest date
				if (_report.DateArrested != null)
				{
					if (_report.DateArrested.GetValueOrDefault().Year > 2000)
					{
						textArrestDate.Text = string.Format("{0}", _report.DateArrested.GetValueOrDefault().ToString("d"));
					}
				}

				// See if bail is set
				if (_report.Bail != null && _report.Bail.Length > 2)
				{
					textBail.Text = string.Format("Bail Amount: {0}", _report.Bail);
				}

				textOffenseTitle.Text = string.Format("{0}", _report.PrimaryCharge);
				textArrestingOfficer.Text = string.Format("{0}", _report.ArrestingOfficer);

				textSuspectLivesIn.Text = string.Format("{0}", _report.DefendantZip);
				_url = string.Format("https://docs.google.com/viewer?url={0}", _report.Url);

				this.IsVisible = true;
			});
		}

		void ButtonDocket_Clicked(object sender, EventArgs e)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Device.OpenUri(new Uri(_url));
			});
		}
	}
}

