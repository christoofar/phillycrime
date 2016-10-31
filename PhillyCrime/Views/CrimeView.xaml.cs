using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;
using Plugin.Geolocator;
using System.Diagnostics;
using PhillyBlotter;
using PhillyBlotter.Models;
using Xamarin.Forms.Xaml;

namespace PhillyBlotter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CrimeView : ContentView
	{

		FullCrimeReport _report;
		CrimeType _type;

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

		void CrimeShieldTouched(object sender, System.EventArgs e)
		{
			if (_type != CrimeType.Nothing)
			{
				var mapLegend = new MapLegend(_type);
				Navigation.PushAsync(mapLegend);
			}
		}

		void PostNewsClicked(object sender, System.EventArgs e)
		{
			var news = new SubmitNews(_report.DCN);
			Navigation.PushAsync(news);
		}

		void ShareCrimeClicked(object sender, System.EventArgs e)
		{
			Plugin.Share.CrossShare.Current.ShareLink("http://www.google.com", 
			                                          $"{_report.FullCrimeDetail.TEXT_GENERAL_CODE}", 
			                                          $"{_report.FullCrimeDetail.TEXT_GENERAL_CODE} at {_report.FullCrimeDetail.LOCATION_BLOCK}");
		}

		public void UpdateData(FullCrimeReport report, CrimeType type)
		{
			_type = type;

			Device.BeginInvokeOnMainThread(() =>
			{
				_report = report;

				if (report.FullCrimeDetail != null)
				{
					Description.Text = string.Format("{0}", report.FullCrimeDetail.TEXT_GENERAL_CODE);
					Address.Text = string.Format("Location: {0}", report.FullCrimeDetail.LOCATION_BLOCK);
					if (DateTime.Today.Year == report.FullCrimeDetail.DISPATCH_DATE_TIME.Value.Year)
					{
						Time.Text = string.Format("Time: {0}", report.FullCrimeDetail.DISPATCH_DATE_TIME.Value.ToString("dddd MMMM d h:mm tt"));
					}
					else 
					{
						Time.Text = string.Format("Time: {0}", report.FullCrimeDetail.DISPATCH_DATE_TIME.Value.ToString("dddd MMMM d yyyy h:mm tt"));
					}
					District.Text = string.Format("Police District: {0}", report.FullCrimeDetail.DC_DIST);
					CrimeImage.Source = Global.ImageSource(type);

					if (_report.FullCrimeDetail.POINT_X == null || _report.FullCrimeDetail.POINT_Y == null)
					{
						buttonMap.IsVisible = false;
					}

					PSA.Text = string.Format("PSA: {0}", report.FullCrimeDetail.SECTOR);
					DCN.Text = String.Format("DCN: {0}-{1}-{2}",
											 report.DCN.Substring(2, 2),
											 report.DCN.Substring(4, 2),
											 report.DCN.Substring(6, report.DCN.Length - 6));
				}
				else
				{
					CrimeImage.Source = Global.ImageSource(type);
					Description.Text = "Crime report not yet available";
					Address.Text = "The Philadelphia Police Department has not yet published information about this crime.";
					Time.IsVisible = false;
					District.Text = $"Police District: {report.FullArrestDetails[0].PoliceDistrict}";
					buttonMap.IsVisible = false;
					PSA.IsVisible = false;
					DCN.Text = String.Format("DCN: {0}-{1}-{2}",
											 report.DCN.Substring(2, 2),
											 report.DCN.Substring(4, 2),
											 report.DCN.Substring(6, report.DCN.Length - 6));
				}

				this.IsVisible = true;
			});
		}
	}
}

