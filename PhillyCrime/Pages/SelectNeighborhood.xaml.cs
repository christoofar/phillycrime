using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace PhillyBlotter
{
    public partial class SelectNeighborhood : ContentPage
    {
        public SelectNeighborhood()
        {
            InitializeComponent();

            neighborhoodListView.ItemsSource = new Models.Neighborhoods();
            neighborhoodListView.IsVisible = true;
        }

        void Handle_ItemTapped(object sender, Xamarin.Forms.ItemTappedEventArgs e)
        {            
			var selectedHood = new BlotterPage(((Models.Hood)e.Item).ID);
			Navigation.PushAsync(selectedHood);
        }

    }
}
