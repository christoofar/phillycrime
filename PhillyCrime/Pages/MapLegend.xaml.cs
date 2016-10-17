using System;
using System.Collections.Generic;
using PhillyBlotter.Models;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class MapLegend : ContentPage
	{
		CrimeType _type;

		public MapLegend()
		{
			InitializeComponent();
		}

		public MapLegend(CrimeType type)
		{
			InitializeComponent();
			_type = type;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			switch (_type)
			{
				case CrimeType.Homicide:
					Scroll.ScrollToAsync(0, Homicide.Y, true);
					break;
				case CrimeType.Assault:
					Scroll.ScrollToAsync(0, Assault.Y, true);
					break;
				case CrimeType.Burglary:
					Scroll.ScrollToAsync(0, Burglary.Y, true);
					break;
				case CrimeType.Robbery:
					Scroll.ScrollToAsync(0, Robbery.Y, true);
					break;
				case CrimeType.Theft:
					Scroll.ScrollToAsync(0, Theft.Y, true);
					break;
				case CrimeType.Rape:
					Scroll.ScrollToAsync(0, Rape.Y, true);
					break;
				case CrimeType.VehicleRecovery:
					Scroll.ScrollToAsync(0, VehicleRecovery.Y, true);
					break;
				case CrimeType.TheftFromAuto:
					Scroll.ScrollToAsync(0, TheftFromAuto.Y, true);
					break;
				case CrimeType.DUI:
					Scroll.ScrollToAsync(0, DUI.Y, true);
					break;
				case CrimeType.Gun:
					Scroll.ScrollToAsync(0, Gun.Y, true);
					break;
				case CrimeType.Prostitution:
					Scroll.ScrollToAsync(0, Prostitution.Y, true);
					break;
				case CrimeType.Narcotics:
					Scroll.ScrollToAsync(0, Narcotics.Y, true);
					break;
				case CrimeType.CriminalMischief:
					Scroll.ScrollToAsync(0, CriminalMischief.Y, true);
					break;
				case CrimeType.OtherSexAssault:
					Scroll.ScrollToAsync(0, OtherSexAssault.Y, true);
					break;
				case CrimeType.Fraud:
					Scroll.ScrollToAsync(0, Other.Y, true);
					break;
				case CrimeType.Other:
					Scroll.ScrollToAsync(0, Other.Y, true);
					break;
			}

		}
	}
}
