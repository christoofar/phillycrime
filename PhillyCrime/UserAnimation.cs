using System;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Interfaces.Animations;
using Rg.Plugins.Popup.Pages;
using Xamarin.Forms;

namespace PhillyBlotter
{
	// User animation
	class UserAnimation : IPopupAnimation
	{
		// Call Before OnAppering
		public void Preparing(View content, PopupPage page)
		{
			// Preparing content and page
			content.Opacity = 0;
		}

		// Call After OnDisappering
		public void Disposing(View content, PopupPage page)
		{
			// Dispose Unmanaged Code
		}

		// Call After OnAppering
		public async Task Appearing(View content, PopupPage page)
		{
			// Show animation
			await content.FadeTo(1);
		}

		// Call Before OnDisappering
		public async Task Disappearing(View content, PopupPage page)
		{
			// Hide animation
			await content.FadeTo(0);
		}
	}
}
