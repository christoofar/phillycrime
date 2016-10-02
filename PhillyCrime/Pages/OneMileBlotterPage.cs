using System;
namespace PhillyBlotter
{
	/// <summary>
	/// One mile blotter.
	/// The logic for distance blotting is actually in BlotterPage.xaml.cs, this just forces
	/// the correct constructor call to be made to activate that logic.
	/// </summary>
	public class OneMileBlotterPage : BlotterPage
	{
		public OneMileBlotterPage() : base(true)
		{
		}

		new public void Handle_ItemSelected(object sender, Xamarin.Forms.SelectedItemChangedEventArgs e)
		{
			base.Handle_ItemSelected(sender, e);
		}

		new public void Handle_Refreshing(object sender, System.EventArgs e)
		{
			base.Handle_Refreshing(sender, e);
		}

		new public void OnSettingsButtonClicked(object sender, System.EventArgs e)
		{
			base.OnSettingsButtonClicked(sender, e);
		}
	}
}
