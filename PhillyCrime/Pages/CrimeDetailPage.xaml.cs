using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PhillyBlotter;
using PhillyBlotter.Models;

using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class CrimeDetailPage : ContentPage
	{
		CustomPin _pin;
		CrimeType _type;

		public CrimeDetailPage(CustomPin pin)
		{
			_pin = pin;
			_type = pin.CrimeType;
			InitializeComponent();

			GetCrimeDetails();
		}

		public CrimeDetailPage(CrimeReport report)
		{
			_pin = new CustomPin();
			_pin.Id = report.DCN;
			_type = report.Type;
			InitializeComponent();

			GetCrimeDetails();
		}

		public CrimeDetailPage(string dcn, CrimeType crimeType)
		{
			_pin = new CustomPin();
			_pin.Id = dcn;
			_type = crimeType;
			InitializeComponent();

			GetCrimeDetails();
		}

		// Initialize view with data
		public Task<bool> GetCrimeDetails()
		{
			return Task.Run(async () =>
			  {
				  string dcn = _pin.Id;
				  var crime = await Data.GetFullCrimeReport(dcn);
				  DataFill(crime, _type);
				  return true;
			  });
		}

		public void DataFill(FullCrimeReport report, CrimeType type)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Indicator.IsRunning = false;
				MainLayout.Children.Remove(Indicator);
			});
			CrimeView.UpdateData(report, type);

			// Was a single person arrested for this?
			if (report.FullArrestDetails.Length == 1)
			{
				SingleArrestView.UpdateData(report.FullArrestDetails[0]);
			}
			else
			{
				foreach (var arrest in report.FullArrestDetails)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						MultipleArrestsView view = new MultipleArrestsView();
						view.UpdateData(arrest);
						MultipleNotice.IsVisible = true;
						MultipleArrestContent.Children.Add(view);
						MultipleArrestContent.IsVisible = true;
					});
				}
			}
		}
	}
}