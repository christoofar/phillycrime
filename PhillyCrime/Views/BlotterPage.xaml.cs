using System;
using System.Collections.Generic;
using System.Linq;
using PhillyCrime.Models;

using Xamarin.Forms;

namespace PhillyCrime
{
	public partial class BlotterPage : ContentPage
	{
		public BlotterPage()
		{
			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			//Look for neighborhoods user subscribes to.
			var ids = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.ID);
			var hoods = Data.GetBlotter(ids.ToArray());


		}
	}
}
