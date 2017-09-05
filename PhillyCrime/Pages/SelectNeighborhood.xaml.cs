using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PhillyBlotter
{
    public partial class SelectNeighborhood : ContentPage
    {

        bool _isStatPage = false;

        public SelectNeighborhood()
        {
            InitializeComponent();

            neighborhoodListView.ItemsSource = new Models.Neighborhoods();
            neighborhoodListView.IsVisible = true;
        }

        public SelectNeighborhood(bool statPage = false)
		{
			InitializeComponent();

			neighborhoodListView.ItemsSource = new Models.Neighborhoods();
			neighborhoodListView.IsVisible = true;

            _isStatPage = statPage;
		}

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {
            if (!_isStatPage)
            {
                var selectedHood = new BlotterPage(((Models.Hood)e.Item).ID);
                Navigation.PushAsync(selectedHood);
            }
            else
            {
				var selectedHood = new StatsPage(((Models.Hood)e.Item).ID);
				Navigation.PushAsync(selectedHood);
            }
        }

    }
}
