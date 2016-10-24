using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using PhillyBlotter;
using PhillyBlotter.Models;

using Xamarin.Forms;
using System.Diagnostics;

namespace PhillyBlotter
{
	public partial class CrimeDetailPage : ContentPage
	{
		CustomPin _pin;
		CrimeType _type;

		public CrimeDetailPage(CustomPin pin)
		{
			_pin = pin;
			_type = pin.CrimeType;
			InitializeComponent();

			Appearing += (object sender, EventArgs e) =>
			{
				SetupMessaging();
			};
			Disappearing += (object sender, EventArgs e) =>
			{
				TeardownMessaging();
			};

			GetCrimeDetails();
		}

		public CrimeDetailPage(CrimeReport report)
		{
			_pin = new CustomPin();
			_pin.Id = report.DCN;
			_type = report.Type;
			InitializeComponent();

			Appearing += (object sender, EventArgs e) =>
			{
				SetupMessaging();
			};
			Disappearing += (object sender, EventArgs e) =>
			{
				TeardownMessaging();
			};

			GetCrimeDetails();
		}

		public CrimeDetailPage(string dcn, CrimeType crimeType)
		{
			_pin = new CustomPin();
			_pin.Id = dcn;
			_type = crimeType;
			InitializeComponent();

			Appearing += (object sender, EventArgs e) =>
			{
				SetupMessaging();
			};
			Disappearing += (object sender, EventArgs e) =>
			{
				TeardownMessaging();
			};

			GetCrimeDetails();
		}

		~CrimeDetailPage()
		{
			// Destruct/Finalize needs to unsubscribe from the MessageCenter so it's not polluted.
			TeardownMessaging();
		}

		void TeardownMessaging()
		{
			MessagingCenter.Unsubscribe<object, NewsFlagConcern>(this, "FlagArticle");
			MessagingCenter.Unsubscribe<object, string>(this, "FlagFailed");
		}

		void SetupMessaging()
		{
			// Is is for when the user flags a crime news article.  Since DisplayActionSheet is only accessible
			// from page classes, this message will be sent up by a NewsCard that wants to ask the user why
			// a news article should be flagged.
#pragma warning disable RECS0165 // Asynchronous methods should return a Task instead of void
			MessagingCenter.Subscribe<object, NewsFlagConcern>(this, "FlagArticle", async (sender, arg) =>
#pragma warning restore RECS0165 // Asynchronous methods should return a Task instead of void
			{
				var action = await DisplayActionSheet("What's wrong with the article?", "Cancel", null, "Story is not about this crime", "Spam or advertising", "Duplicate", "Opinion column or blog");
				if (action != "Cancel")
				{
					var concern = new NewsFlagConcern();
					concern.Reason = action;
					concern.URL = arg.URL;
					concern.DCN = arg.DCN;
					// Send a message back to the listening news card.
					MessagingCenter.Send<object, NewsFlagConcern>(sender, "SubmitFlaggedArticle", concern);
				}
			});

			// This is in case a user flags a news article but that fails.
			MessagingCenter.Subscribe<object, string>(this, "FlagFailed", (sender, arg) =>
			{
				DisplayAlert("Problem flagging this article", arg, "OK");
			});
		}

		// Initialize view with data
		public Task<bool> GetCrimeDetails()
		{
			return Task.Run(async () =>
			  {
				  string dcn = _pin.Id;
				  var crime = await Data.GetFullCrimeReport(dcn);
				  DataFill(crime, _type);
				  return true;
			  });
		}

		public void DataFill(FullCrimeReport report, CrimeType type)
		{
			Device.BeginInvokeOnMainThread(() =>
			{
				Indicator.IsRunning = false;
				MainLayout.Children.Remove(Indicator);
			});
			CrimeView.UpdateData(report, type);

			// Was a single person arrested for this?
			if (report.FullArrestDetails.Length == 1)
			{
				SingleArrestView.UpdateData(report.FullArrestDetails[0]);
			}
			else
			{
				foreach (var arrest in report.FullArrestDetails)
				{
					Device.BeginInvokeOnMainThread(() =>
					{
						MultipleArrestsView view = new MultipleArrestsView();
						view.UpdateData(arrest);
						MultipleNotice.IsVisible = true;
						MultipleArrestContent.Children.Add(view);
						MultipleArrestContent.IsVisible = true;
					});
				}
			}

			// Add a news card for each piece of news found
			foreach (var news in report.News)
			{
				Device.BeginInvokeOnMainThread(() =>
				{
					var newsCard = new NewsCard(news);
					NewsView.Children.Add(newsCard);
				});
			}
		}
	}
}