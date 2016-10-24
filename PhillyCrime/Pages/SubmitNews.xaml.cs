using System;
using System.Collections.Generic;
using Plugin.Share;
using Xamarin.Forms;
using PhillyBlotter.Models;

namespace PhillyBlotter
{
	public partial class SubmitNews : ContentPage
	{
		string _dcn = "";

		public SubmitNews()
		{
			InitializeComponent();
		}

		public SubmitNews(string dcn)
		{
			InitializeComponent();
			_dcn = dcn;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			string cliptext = DependencyService.Get<PlatformSpecificInterface>().GetClipboardData();
			if (cliptext != null && cliptext.Length > 0)
			{
				if (cliptext.StartsWith("http", StringComparison.CurrentCultureIgnoreCase) || 
				    cliptext.StartsWith("www", StringComparison.CurrentCultureIgnoreCase))
				{
					URL.Text = cliptext;
				}
			}
		}

		async void PostNews(object sender, System.EventArgs e)
		{
			var post = new NewsContribution();
			post.DCN = _dcn;
			post.URL = this.URL.Text;

			// Submit the story
			var response = await Data.SubmitNewsStory(post);

			// It's safe to close the page now.
			Navigation.PopAsync();

			if (response.Accepted)
			{
				await DisplayAlert("News Submission", "Thank you! The news story was accepted and has been posted.", "OK");
			}
			else
			{
				await DisplayAlert("Problem with your news story", $"We couldn't accept this news submission.  {response.ResponseMessage}", "OK");
			}

		}
	}
}