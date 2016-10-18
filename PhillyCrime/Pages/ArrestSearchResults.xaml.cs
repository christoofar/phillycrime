using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PhillyBlotter.Models;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public partial class ArrestSearchResults : ContentPage
	{
		public ArrestSearchResults()
		{
			InitializeComponent();
		}

		public ArrestSearchResults(ArrestSearchCriteria criteria)
		{
			InitializeComponent();
			Search(criteria);
		}

		private async void Search(ArrestSearchCriteria criteria)
		{
			var data = await Data.SearchArrests(criteria);

		}
	}
}
