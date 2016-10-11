using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PhillyBlotter
{
	public interface PlatformSpecificInterface
	{
		bool CheckIfSimulator();

		void ClearBadges();

		void BringToForeground();

		bool IsInForeground();

		string ResolveHostEntry(string ipAddress);

		Task<string> ResolveIPAddress(string host);
	}
}