using Xamarin.Forms;

namespace PhillyCrime
{
	public partial class App : Application
	{

		public static Page GetMainPage()
		{
			// Replace the ExamplePage with whatever page is appropriate to start off your app
			//  - Like your login page, or home screen, or whatever

			// The root page allows navigation to work on Android.
			NavigationPage root = new NavigationPage();
			root.Title = "Blotter";
			NavigationPage.SetHasNavigationBar(root, false);

			// This is really the main page
			TabbedPage page = new TabbedPage();
			page.Title = "Blotter";
			NavigationPage.SetHasNavigationBar(page, false);


			// This beta period will expire 12/30/2016
			if (System.DateTime.Now > System.DateTime.Parse("2016-12-30"))
			{
				page.Children.Add(new BetaWelcome() { Icon = "info.png" });
			}
			else {
				page.Children.Add(new BetaWelcome() { Icon = "info.png" });
				page.Children.Add(new CrimesNearMeView() { Icon = "gun.png" });
				page.Children.Add(new SettingsPage() { Icon = "info.png" });
			}

			//page.Children.Add(new BetaWelcome() {  });
			//page.Children.Add(new CrimesNearMeView() {  });

			root.PushAsync(page);

			return root;
		}

		public App()
		{
			InitializeComponent();

			MainPage = GetMainPage();
		}

		protected override void OnStart()
		{
			DependencyService.Get<PlatformSpecificInterface>().ClearBadges();
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
			MessagingCenter.Send(this, "GoingToSleep");

		}

		protected override void OnResume()
		{
			// Handle when your app resumes
			MessagingCenter.Send(this, "WakingUp");
		}
	}

}

