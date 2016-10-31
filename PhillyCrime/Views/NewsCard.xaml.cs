using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using PhillyBlotter.Models;
using System.Diagnostics;
using Xamarin.Forms.Xaml;

namespace PhillyBlotter
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewsCard : ContentView
	{
		string _url = "";
		string _dcn = "";

		public NewsCard()
		{
			InitializeComponent();

			// This is the 2nd half of the message-passing for flagged articles.  The parent page would
			// be sending this message after the user answers why the article has been flagged.
			MessagingCenter.Subscribe<object, string>(this, "SubmitFlaggedArticle", (sender, arg) =>
			{
				Debug.WriteLine($"Ready to flag the article:  Arg is {arg}, the sender is {sender.ToString()}");
			});
		}

		public NewsCard(crime_news news)
		{
			InitializeComponent();
			labelTitle.Text = news.Headline;
			labelLede.Text = news.Lede;
			labelNewsOutlet.Text = news.crime_newsorganization.NewsOrganization;
			imageStory.Source = ImageSource.FromUri(new Uri(news.SlugImageURL));
			_dcn = news.DC_KEY;
			_url = news.NewsURL;

			// This is the 2nd half of the message-passing for flagged articles.  The parent page would
			// be sending this message after the user answers why the article has been flagged.
			MessagingCenter.Subscribe<object, NewsFlagConcern>(this, "SubmitFlaggedArticle", async (sender, arg) =>
			{
				
				// If more than one news card is on the page they'll all hear this message.  Only
				// the news card that sent the flag is the one that should respond to this
				if (sender == this)
				{
					try
					{
						var response = await Data.FlagNewsStory(arg);
						if (response.Accepted)
						{
							this.IsVisible = false;
						}
						else
						{
							MessagingCenter.Send<object, string>(this, "FlagFailed", "Due to technical difficulties we cannot flag this article.");
						}
					}
					catch (Exception ex)
					{
						MessagingCenter.Send<object, string>(this, "FlagFailed", ex.Message);
					}

					// Message has been flagged.  We can now unsubscribe.
					MessagingCenter.Unsubscribe<object, NewsFlagConcern>(this, "SubmitFlaggedArticle");
				}

			});

		}

		void Handle_Tapped(object sender, System.EventArgs e)
		{
			var WebPage = new WebPage(_url);
			Navigation.PushAsync(WebPage);
		}

		void ArticleFlagged(object sender, System.EventArgs e)
		{
			var concern = new NewsFlagConcern();
			concern.DCN = _dcn;
			concern.URL = _url;
			// User has identified this as a bad article.  Ask the parent page to display a dialog asking why.
			MessagingCenter.Send<object, NewsFlagConcern>(this, "FlagArticle", concern);
		}
	}
}
