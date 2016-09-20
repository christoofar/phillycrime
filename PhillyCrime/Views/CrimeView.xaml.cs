using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Geolocator;
using System.Diagnostics;
using PhillyCrime;
using PhillyCrime.Models;

namespace PhillyCrime
{
	public partial class CrimeView : ContentView
	{

		FullCrimeReport _report;

		public CrimeView()
		{
			InitializeComponent();

			buttonMap.Clicked += async (sender, args) =>
			{
				double lo;
				double la;

				lo = double.Parse(_report.FullCrimeDetail.POINT_X);
				la = double.Parse(_report.FullCrimeDetail.POINT_Y);

				await Navigation.PushAsync(new CrimeMap(lo, la, string.Format(
					"{0} {1}", _report.FullCrimeDetail.TEXT_GENERAL_CODE, 
					_report.FullCrimeDetail.DISPATCH_DATE_TIME.ToString())), true);
			};
		}

		public void UpdateData(FullCrimeReport report)
		{

			Device.BeginInvokeOnMainThread(() =>
			{
				_report = report;

				Description.Text = string.Format("{0}", report.FullCrimeDetail.TEXT_GENERAL_CODE);
				Address.Text = string.Format("Location: {0}", report.FullCrimeDetail.LOCATION_BLOCK);
				Time.Text = string.Format("Time: {0}", report.FullCrimeDetail.DISPATCH_DATE_TIME);
				District.Text = string.Format("Police District: {0}", report.FullCrimeDetail.DC_DIST);
				PSA.Text = string.Format("PSA: {0}", report.FullCrimeDetail.SECTOR);
				DCN.Text = String.Format("DCN: {0}-{1}-{2}",
										 report.DCN.Substring(2, 2),
										 report.DCN.Substring(4, 2),
										 report.DCN.Substring(6, report.DCN.Length - 6));

				this.IsVisible = true;
			});
		}
	}
}

