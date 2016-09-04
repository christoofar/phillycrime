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
			root.Title = "PhillyCrime";
			NavigationPage.SetHasNavigationBar(root, false);

			// This is really the main page
			TabbedPage page = new TabbedPage();
			page.Title = "PhillyCrime";
			NavigationPage.SetHasNavigationBar(page, false);


			page.Children.Add(new CrimesNearMeView());
			page.Children.Add(new ArrestPage());

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
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}

}

