using System;
using System.Collections.Generic;
using Xamarin.Forms;
using PhillyCrime.Models;

namespace PhillyCrime
{
	public partial class ArrestView : ContentView
	{
		Arrest _report;


		public ArrestView()
		{
			InitializeComponent();
		}

		public void UpdateData(Arrest arrest)
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				_report = arrest;

				textDefendantName.Text = string.Format("{0}", _report.Defendant);
				textOffenseTitle.Text = string.Format("{0}", _report.PrimaryCharge);
				textArrestDate.Text = string.Format("{0}", _report.DateArrested);
				textSuspectLivesIn.Text = string.Format("{0}", _report.DefendantZip);

				this.IsVisible = true;
			});
		}
	}
}

