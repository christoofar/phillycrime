using System;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public interface PlatformSpecificInterface
	{
		bool CheckIfSimulator();

		void ClearBadges();

		void BringToForeground();

		bool IsInForeground();
	}
}