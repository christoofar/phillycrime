using Xamarin.Forms;
using PCLStorage;
using System.Threading.Tasks;
using System.Diagnostics;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Collections.ObjectModel;

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
				page.Children.Add(new BlotterPage() { Icon = "gun.png" });
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

			// Load neighborhood listing.
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
			LoadNeighborhoods();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed

			MainPage = new MainPage();
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

		public static async Task<bool> UpdateNeighborhood(int hoodID, bool selected)
		{
			// This is a real drag but we gotta update the global vars and the local file.
			var hood = Global.Neighborhoods.Where(p => p.ID == hoodID).FirstOrDefault();
			if (hood != null)
			{
				hood.Selected = selected;
				IFile file;
				var exists = await FileSystem.Current.LocalStorage.CheckExistsAsync("Neighborhoods.json");
				if (exists == ExistenceCheckResult.FileExists)
				{
					try
					{
						file = await FileSystem.Current.LocalStorage.GetFileAsync("Neighborhoods.json");
						await file.DeleteAsync();
					}
					catch { }
				}

				try
				{
					file = await FileSystem.Current.LocalStorage.CreateFileAsync("Neighborhoods.json", CreationCollisionOption.ReplaceExisting);

					if (file != null)
					{
						await file.WriteAllTextAsync(JsonConvert.SerializeObject(Global.Neighborhoods.ToList()));
					}
				}
				catch { }

			}

			return true;
		}

		public static async Task<bool> LoadNeighborhoods()
		{

			// First, do we need to actually do this?
			if (Global.Neighborhoods.Count > 0)
			{
				return true;
			}

			// Is there anything on the disk we can use?
			var exists = await FileSystem.Current.LocalStorage.CheckExistsAsync("Neighborhoods.json");

			// Ugh.  Nope.  We need to run to the server and get a neighborhood list.  We shall do
			// this silent-but-deadly so nobody will notice.  Hopefully it will return before user
			// makes their way to the setup panel.
			if (exists == ExistenceCheckResult.NotFound)
			{
				try
				{
					var hoods = await Models.Data.Neighborhoods();
					if (hoods != null)
					{
						foreach (var p in hoods)
						{
							Global.Neighborhoods.Add(p);
						}
					}

					// Serialize this to disk so we don't need to fetch it again.
					var file = await FileSystem.Current.LocalStorage.CreateFileAsync("Neighborhoods.json", CreationCollisionOption.ReplaceExisting);
					await file.WriteAllTextAsync(JsonConvert.SerializeObject(hoods));
				}
				catch (Exception ex)
				{
					Debug.WriteLine("Loading Neighborhoods Exception: {0}\r\n{1}", ex.Message, ex.StackTrace);
					// This likely bombed because of no Internet.  Don't care; we can survive without it.
				}
			}
			else
			{
				var file = await FileSystem.Current.LocalStorage.GetFileAsync("Neighborhoods.json");
				string data = await file.ReadAllTextAsync();

				if (data == "") // No data in the file due to a bad earlier return
				{
					var hoods = await Models.Data.Neighborhoods();
					if (hoods != null)
					{
						foreach (var p in hoods)
						{
							Global.Neighborhoods.Add(p);
						}
					}

					await file.WriteAllTextAsync(JsonConvert.SerializeObject(hoods));
				}
				else {
					if (Global.Neighborhoods != null)
					{
						lock (Global.Neighborhoods)
						{
							Global.Neighborhoods.Clear();
							var hoods = JsonConvert.DeserializeObject<Models.Neighborhood[]>(data);
							foreach (var p in hoods)
							{
								Global.Neighborhoods.Add(p);
							}
						}
					}
				}
			}

			return true;
		}
	}

}

