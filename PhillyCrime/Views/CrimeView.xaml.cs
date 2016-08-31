using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace PhillyCrime
{
	public partial class CrimeView : ContentView
	{
		public CrimeView()
		{
			InitializeComponent();

			buttonMap.Clicked += async (sender, args) =>
			{
				await Navigation.PushAsync(new CrimeMap(), true);
			};
		}
	}
}

