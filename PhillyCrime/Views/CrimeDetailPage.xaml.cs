using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PhillyCrime;
using PhillyCrime.Models;

using Xamarin.Forms;

namespace PhillyCrime
{
	public partial class CrimeDetailPage : ContentPage
	{
		CustomPin _pin;

		public CrimeDetailPage(CustomPin pin)
		{
			_pin = pin;
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
				  DataFill(crime);
				  return true;
			  });
		}

		public void DataFill(FullCrimeReport report)
		{
			Indicator.IsVisible = false;
			Indicator.IsRunning = false;
		}
	}
}