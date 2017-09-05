using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PhillyBlotter.Models;
using PhillyBlotter.Helpers;

using Xamarin.Forms;
using System.Globalization;

namespace PhillyBlotter
{
    public partial class StatsPage : ContentPage
    {
        List<Models.YTDStat> _stats = new List<YTDStat>();
        int _hood = 0;

        public StatsPage()
        {
            InitializeComponent();
            LabelYears();
            this.Title = "Neighborhood Statistics";
            string month = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
            this.labelDate.Text = $"As of {month} {DateTime.Now.Day}";
            this.ToolbarItems.Add(new ToolbarItem("buttonJump", "Images/searchhouse.png", SelectANeighborhood, ToolbarItemOrder.Primary));
        }

		public StatsPage(int hood)
		{
			InitializeComponent();
            LabelYears();
			this.Title = "Neighborhood Statistics";
			string month = DateTime.Now.ToString("MMM", CultureInfo.InvariantCulture);
			this.labelDate.Text = $"As of {month} {DateTime.Now.Day}";
            _hood = hood;
		}

        public void LabelYears()
        {
            labelCurrentYear.Text = DateTime.Now.Year.ToString();
            labelLastYear.Text = $"'{DateTime.Now.AddYears(-1).ToString("yy")}";
            labelLastYear2.Text = $"'{DateTime.Now.AddYears(-2).ToString("yy")}";
            labelLastYear3.Text = $"'{DateTime.Now.AddYears(-3).ToString("yy")}";
            labelLastYear4.Text = $"'{DateTime.Now.AddYears(-4).ToString("yy")}";
            labelLastYear5.Text = $"'{DateTime.Now.AddYears(-5).ToString("yy")}";
        }

		void SelectANeighborhood()
		{
			var selector = new SelectNeighborhood(true);
			Navigation.PushAsync(selector);
		}

        async protected override void OnAppearing()
        {
            base.OnAppearing();

            var ids = Global.Neighborhoods.Where(p => p.Selected).Select(p => p.ID);

            if (_hood == 0)
            {
                if (ids.Count() > 1)
                {
                    // Has user gotten warning about having multiple neighborhoods selected?
                    if (!Settings.StatsMultipleHoodsWarning)
                    {
                        await DisplayActionSheet("Multiple Neighborhoods Selected", "OK", null, new string[] { "This feature analizes one neighborhood at a time", "To switch between neighborhoods, use the neighborhood selector button on the top right of this screen." });
                        Settings.StatsMultipleHoodsWarning = true;
                    }
                    _hood = ids.First();
                    this.Title = Global.Neighborhoods.Where(p => p.ID == ids.ToArray()[0]).First().Name;
                }

                if (ids.Count() == 1)
                {
                    _hood = ids.First();
                    this.Title = Global.Neighborhoods.Where(p => p.ID == ids.ToArray()[0]).First().Name;
                }
            } 
            else 
            {
                this.Title = Global.Neighborhoods.Where(p => p.ID == _hood).First().Name;
            }

            warningPanel.IsVisible = (ids.Count() == 0);			

            await UpdateStats();
        }

        public async void OnSettingsButtonClicked(object sender, System.EventArgs e)
        {
            await Navigation.PushAsync(new SettingsPage());
        }

        public Task<bool> UpdateStats()
        {
            return Task.Run(async () =>
            {
                try
                {
                    var b = await Data.GetYTD(_hood);
                    foreach (YTD stat in b)
                    {
                        // We don't want the unknown! :-)
                        if (stat.CrimeText != "Unknown")
                        {
                            _stats.Add(stat.ConverToStat());
                        }
                    }

                    Device.BeginInvokeOnMainThread(()=> {
                        ytdListView.ItemsSource = _stats;
						activity.IsRunning = false;
						activity.IsVisible = false;
                    });

                    System.Diagnostics.Debug.WriteLine(b.ToString());
                }
                catch (Exception ex)
                {
                    HockeyApp.MetricsManager.TrackEvent(
                        "Stats Request Failure",
                         new Dictionary<string, string> { { "exception", ex.Message } },
                         new Dictionary<string, double> { }
                     );
                }
                return true;
            });
        }
    }
}
